using Godot;
using System;

public class Match : Node2D
{
	[Export]
	public float BallDropForce { get; set; }

	private Timer matchTimer;
	private Field field;
	private Player player;
	private AIRobot redAIRobot1;
	private PackedScene BallScene { get; set; }

	public override void _Ready()
	{
		// build our nodes we need to access from the scene
		matchTimer = GetNode<Timer>("MatchTimer");
		field = GetNode<Field>("Field");
		player = GetNode<Player>("Player");
		redAIRobot1 = GetNode<AIRobot>("RedAIRobot1");

		// we instantiate balls during the match using the Ball scene
		BallScene = (PackedScene)ResourceLoader.Load("res://matches/ball.tscn");

		// setup ai robot
		redAIRobot1.Field = field;
		redAIRobot1.ChangeStateEvent += OnAIRobotChangedState;
		redAIRobot1.ShootEvent += OnAIRobotShoot;

		// subscribe to signals and events
		matchTimer.Connect("timeout", this, nameof(OnMatchTimerTimeout));
		GetNode<Timer>("MatchTimer/MatchTimerUpdateTimer").Connect("timeout", this, nameof(OnMatchTimerUpdateTimerTimeout));
		player.ShootEvent += OnPlayerShoot;
		Signals.BallDropRequestedEvent += OnBallDropRequested;

		// publish any events
		Signals.PublishRobotJoinedEvent(player);
		Signals.PublishRobotJoinedEvent(redAIRobot1);

		// start the match!
		matchTimer.WaitTime = Constants.MatchTime;
		matchTimer.Start();

		var debug = GetNode<Debug>("Debug");
		debug.BlueRobot = player;
		debug.RedRobot = GetNode<Robot>("RedAIRobot1");
	}

	public override void _Draw()
	{
		if (redAIRobot1.Path != null)
		{
			DrawPath(redAIRobot1.Path);
		}
	}
	private Color lineColor = new Color("ffffff");
	private float lineWidth = 2;
	public void DrawPath(Vector2[] path)
	{
		if (path.Length > 1)
		{
			Vector2 start = path[0];
			Vector2 end = path[path.Length - 1];
			Vector2 last = new Vector2(start.x, start.y);

			for (int index = 1; index < path.Length; index++)
			{
				Vector2 current = new Vector2(path[index].x, path[index].y);
				DrawLine(last, current, lineColor, 2, true);
				DrawCircle(current, lineWidth * 2.0f, lineColor);
				last = current;
			}
		}
	}




	/// <summary>
	/// Player specific shot event
	/// </summary>
	/// <param name="robot"></param>
	/// <param name="globalPosition"></param>
	/// <param name="direction"></param>
	/// <param name="force"></param>
	private void OnPlayerShoot(Robot robot, Vector2 globalPosition, Vector2 direction, float force)
	{
		GD.Print($"Player {robot.TeamName} shot ball.");

		BallShot(robot, globalPosition, direction, force);
	}

	private void OnAIRobotShoot(Robot robot, Vector2 globalPosition, Vector2 direction, float force)
	{
		GD.Print($"AI Robot {robot.TeamName} shot ball.");

		BallShot(robot, globalPosition, direction, force);
	}

	private void BallShot(Robot robot, Vector2 globalPosition, Vector2 direction, float force)
	{
		GD.Print("Ball Shot.");
		Ball ball = (Ball)BallScene.Instance();

		// this is a shot that could go in a goal
		ball.ShotByPlayer = true;

		ball.Shooter = robot;

		// add it to our scene at the robot position and send it away with some force
		AddChild(ball);

		// setup the ball shooting
		ball.Position = globalPosition;
		ball.AddToGroup("ShotBall");
		ball.CollisionLayer = (uint)CollisionLayers.HighBall;
		ball.CollisionMask = (uint)CollisionLayers.HighBall;
		ball.ApplyImpulse(ball.GlobalPosition, direction * force);
	}

	//
	// Events
	//

	public void OnBallDropRequested(Robot robot, Position2D position, Vector2 direction)
	{
		GD.Print($"Ball Drop Requested {position}");

		// TODO: Does this need to be a PackedScene Instance(), or is that only for dynamic exported nodes
		Ball ball = (Ball)BallScene.Instance();
		ball.Position = position.GlobalPosition;
		ball.ApplyImpulse(ball.GlobalPosition, direction * BallDropForce);
		AddChild(ball);
	}

	//
	// Signals
	//

	public void OnMatchTimerUpdateTimerTimeout()
	{
		Signals.PublishMatchTimeUpdatedEvent(Constants.MatchTime - matchTimer.TimeLeft);
	}

	async public void OnMatchTimerTimeout()
	{
		GetNode<PopupDialog>("Menus/PopupDialog").PopupCentered();
		await ToSignal(GetTree().CreateTimer(10), "timeout");
		Signals.PublishMatchCompletedEvent();
	}

	void OnAIRobotChangedState(AIRobot robot)
	{
		// update the draw
		Update();
	}
}
