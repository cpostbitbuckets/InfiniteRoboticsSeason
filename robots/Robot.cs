using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// A robot is one of the Player or AI controlled robots on
/// each team
/// </summary>
public class Robot : KinematicBody2D
{

    [Signal]
    public delegate void Shoot(Robot robot, Vector2 globalPosition, Vector2 direction, float force);
    public event Shoot ShootEvent;


    [Export]
    public int TeamNumber { get; set; } = 4183;

    [Export]
    public String TeamName { get; set; } = "Bit Buckets";

    [Export]
    public Alliance Alliance { get; set; } = Alliance.Blue;

    [Export]
    public int MaxSpeed { get; set; } = 200;

    [Export]
    public int RotationSpeed { get; set; } = 5;

    protected Hopper Hopper { get; set; }

    protected DriveBase DriveBase { get; set; }

    protected Shooter Shooter { get; set; }

    protected Intake Intake { get; set; }

    protected List<RobotComponent> Components { get; private set; } = new List<RobotComponent>();

    protected Vector2 Velocity { get; set; }

    public override void _Ready()
    {
        Hopper = GetNode<Hopper>("Hopper");
        DriveBase = GetNode<DriveBase>("DriveBase");
        Shooter = GetNode<Shooter>("Shooter");
        Intake = GetNode<Intake>("Intake");

        Components.Add(Hopper);
        Components.Add(DriveBase);
        Components.Add(Shooter);
        Components.Add(Intake);

        Components.ForEach(c =>
        {
            c.Robot = this;
            c.Alliance = Alliance;
        });

        Shooter.TryShootEvent += OnTryShoot;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!Engine.EditorHint)
        {
            Control(delta);
            MoveAndSlide(Velocity);
        }
    }

    /// <summary>
    /// Control the robot, defaults to a no-op
    /// </summary>
    /// <param name="delta"></param>
    protected virtual void Control(float delta)
    {

    }

    // Intake
    protected void Intaking(bool intaking)
    {
        Intake.Intaking = intaking;
    }

    protected void Shooting(bool shooting)
    {
        Shooter.Shooting = shooting;
    }

    public void TryIntakeBall(Ball ball)
    {
        if (Hopper.HasRoom)
        {
            Hopper.AddBall();
            ball.QueueFree();
        }
    }

    #region Event Subscriptions

    protected virtual void OnTryShoot(Shooter shooter)
    {
        if (Hopper.HasBalls)
        {
            Hopper.RemoveBall();
            PublishShootEvent();
        }
    }

    #endregion

    #region Events
    protected void PublishShootEvent()
    {
        ShootEvent?.Invoke(this, GlobalPosition + Vector2.Right.Rotated(GlobalRotation) * 32, Vector2.Right.Rotated(GlobalRotation), Shooter.Stats.Force);

    }
    #endregion

}
