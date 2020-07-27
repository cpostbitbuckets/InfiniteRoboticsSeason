using Godot;
using System;

public class Match : Node2D
{
	[Export]
	public float BallDropForce { get; set; }

	private Timer matchTimer;
	private Player player;
	private PackedScene BallScene { get; set; }

	public override void _Ready()
	{
		// build our nodes we need to access from the scene
		matchTimer = GetNode<Timer>("MatchTimer");
		player = GetNode<Player>("Player");

		// we instantiate balls during the match using the Ball scene
		BallScene = (PackedScene)ResourceLoader.Load("res://matches/ball.tscn");

		// subscribe to signals and events
		matchTimer.Connect("timeout", this, nameof(OnMatchTimerTimeout));
		GetNode<Timer>("MatchTimer/MatchTimerUpdateTimer").Connect("timeout", this, nameof(OnMatchTimerUpdateTimerTimeout));
		player.ShootEvent += OnPlayerShoot;
		Signals.BallDropRequestedEvent += OnBallDropRequested;

		// publish any events
		Signals.PublishRobotJoinedEvent(player);

		// start the match!
		matchTimer.WaitTime = Constants.MatchTime;
		matchTimer.Start();
		
		var debug = GetNode<Debug>("Debug");
		debug.BlueRobot = player;
		debug.RedRobot = GetNode<Robot>("RedAIRobot1");
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
		GD.Print("Player shot ball.");

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
}
