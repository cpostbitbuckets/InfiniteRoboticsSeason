using Godot;
using Stateless;
using System;

public class AIRobot : Robot
{
    public enum States
    {
        Idle,
        DrivingToScore,
        DrivingToPickup,
        Shoot,
        Pickup,
    }

    public enum Triggers
    {
        Idle,
        Score,
        Shoot,
        PickupBalls,
        Pickup
    }

    public delegate void SelectHighlightedUnits();
    public event SelectHighlightedUnits SelectHighlightedUnitsEvent;


    public delegate void ChangeState(AIRobot robot);
    public event ChangeState ChangeStateEvent;

    /// <summary>
    /// The amount of distance, in pixels, to stop moving towards the path
    /// </summary>
    private const float PathDistanceDeadband = 5;
    public States State { get; private set; } = States.Idle;

    public Field Field { get; set; }

    Area2D BallSensor { get; set; }
    Area2D ScorePickupSensor { get; set; }
    int sensedBallsCount = 0;

    Node2D pathMover;

    public Vector2[] Path { get; private set; }
    Vector2 PathEnd => (Path != null && Path.Length > 0) ? Path[Path.Length - 1] : Vector2.Zero;
    float PathEndDistance => PathEnd == Vector2.Zero ? 0 : GlobalPosition.DistanceTo(PathEnd);


    StateMachine<States, Triggers> machine;

    public override void _Ready()
    {
        base._Ready();

        BallSensor = GetNode<Area2D>("BallSensor");
        ScorePickupSensor = GetNode<Area2D>("ScorePickupSensor");
        ScorePickupSensor.Connect("area_entered", this, nameof(OnScorePickupSensorAreaEntered));
        BallSensor.Connect("body_entered", this, nameof(OnBallSensorBodyEntered));
        BallSensor.Connect("body_exited", this, nameof(OnBallSensorBodyExited));

        pathMover = GetNode<Node2D>("PathMover");
        pathMover.Call("setup");

        // Our AI robot is either idle, trying to score, or trying to pickup balls
        machine = new StateMachine<States, Triggers>(() => State, (s) => State = s);

        machine.Configure(States.Idle)
            .Permit(Triggers.Score, States.DrivingToScore)
            .Permit(Triggers.PickupBalls, States.DrivingToPickup)
            .OnEntry(() => OnIdle());

        machine.Configure(States.DrivingToScore)
            .Permit(Triggers.Idle, States.Idle)
            .Permit(Triggers.Shoot, States.Shoot)
            .OnEntry(() => OnDrivingToScore())
            .OnExit(() => StopDriving());

        machine.Configure(States.DrivingToPickup)
            .Permit(Triggers.Idle, States.Idle)
            .Permit(Triggers.Pickup, States.Pickup)
            .Permit(Triggers.Score, States.DrivingToScore)
            .OnEntry(() => OnDrivingToPickup())
            .OnExit(() => StopDriving());

        machine.Configure(States.Shoot)
            .Permit(Triggers.Idle, States.Idle)
            .Permit(Triggers.PickupBalls, States.DrivingToPickup)
            .OnEntry(() => OnShoot())
            .OnExit(() => OnStopShooting());

        machine.Configure(States.Pickup)
            .Permit(Triggers.Idle, States.Idle)
            .Permit(Triggers.Score, States.DrivingToScore)
            .OnEntry(() => OnPickupBalls())
            .OnExit(() => OnStopPickupBalls());

        machine.OnTransitioned(t =>
        {
            GD.Print($"OnTransitioned: {t.Source} -> {t.Destination} via {t.Trigger}({string.Join(", ", t.Parameters)})");
            PublishChangeStateEvent();
        });
    }

    protected override void Control(float delta)
    {
        if (PathEnd != Vector2.Zero && PathEndDistance > PathDistanceDeadband)
        {
            // update the path mover script
            pathMover.Call("update_acceleration", delta);
            Velocity = (Vector2)(pathMover.Get("velocity"));
            float angularVelocity = (float)(pathMover.Get("angular_velocity"));
            Rotation += angularVelocity * delta;

        }
        else
        {
            Path = new Vector2[] { };
            Velocity = Vector2.Zero;
        }

        if (State == States.Idle)
        {
            if (Hopper.HasBalls)
            {
                machine.Fire(Triggers.Score);
            }
            else
            {
                machine.Fire(Triggers.PickupBalls);
            }
        }
        else if (State == States.DrivingToPickup)
        {
            // we ran out of room, go score
            if (!Hopper.HasRoom)
            {
                machine.Fire(Triggers.Score);
            }
        }
        else if (State == States.Shoot && !Hopper.HasBalls)
        {
            machine.Fire(Triggers.PickupBalls);
        }
        else if (State == States.Pickup && Hopper.HasBalls)
        {
            machine.Fire(Triggers.Score);
        }
    }

    private void OnDrivingToScore()
    {
        // build a path from my location to the score position
        Path = Field.GetScorePath(this);
        pathMover.Call("build_path", Path);
    }

    private void OnDrivingToPickup()
    {
        // build a path from my location to the pickup position
        Path = Field.GetPickupPath(this);
        pathMover.Call("build_path", Path);
    }

    private void StopDriving()
    {
    }


    private void OnIdle()
    {
        Shooting(false);
        Intaking(false);
    }

    private void OnShoot()
    {
        Shooting(true);
    }

    private void OnStopShooting()
    {
        Shooting(false);
    }


    private void OnPickupBalls()
    {
        Intaking(true);
    }

    private void OnStopPickupBalls()
    {
        Intaking(false);
    }

    #region Sensor Triggers

    /// <summary>
    /// This signal is called when our Score/Pickup sensor encounters another area
    /// We check if it's a score or pickup location, and if so we change states
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    void OnScorePickupSensorAreaEntered(Area2D area)
    {
        GD.Print($"ScorePickupSensor encountered area: {area.Name} + {area.GetGroups()}");
        switch (State)
        {
            case States.DrivingToPickup:
            case States.DrivingToScore:
                // if we encountered an area that is part of our alliance, possibly change states
                if (area != null && area.IsInGroup(Alliance.ToString()))
                {
                    GD.Print($"ScorePickupSensor encountered area in our Alliance: {area.Name} + {area.GetGroups()}");
                    if (Hopper.HasBalls && area.IsInGroup("Goal"))
                    {
                        // try and score
                        machine.Fire(Triggers.Shoot);
                    }
                    else if (Hopper.HasRoom && area.IsInGroup("Pickup"))
                    {
                        machine.Fire(Triggers.Pickup);
                    }
                }
                break;
        }

    }

    void OnBallSensorBodyEntered(PhysicsBody2D body)
    {
        if (body != null && body is Ball ball)
        {
            sensedBallsCount++;
            Intaking(true);
        }
    }

    void OnBallSensorBodyExited(PhysicsBody2D body)
    {
        if (body != null && body is Ball ball)
        {
            sensedBallsCount--;
            if (sensedBallsCount <= 0)
            {
                Intaking(false);
            }
        }
    }

    #endregion

    void PublishChangeStateEvent()
    {
        ChangeStateEvent?.Invoke(this);
    }

    void PublishSelectHighlightedUnitsEvent()
    {
        SelectHighlightedUnitsEvent?.Invoke();
    }

}
