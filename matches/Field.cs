using Godot;
using System;

public class Field : StaticBody2D
{
	protected Position2D BlueBallDropSpawnPoint { get; private set; }
	protected Position2D RedBallDropSpawnPoint { get; private set; }

	public Position2D RedScorePosition { get; private set; }
	public Position2D RedPickupPosition { get; private set; }
	public Navigation2D RedNavigation { get; private set; }

	public override void _Ready()
	{
		var redGoal = GetNode<Area2D>("RedGoal");
		var blueGoal = GetNode<Area2D>("BlueGoal");
		var blueBallDrop = GetNode<Area2D>("BlueBallDrop");
		var redBallDrop = GetNode<Area2D>("RedBallDrop");
		BlueBallDropSpawnPoint = GetNode<Position2D>("BlueBallDrop/SpawnPoint");
		RedBallDropSpawnPoint = GetNode<Position2D>("RedBallDrop/SpawnPoint");
		RedScorePosition = GetNode<Position2D>("Positions/RedScorePosition");
		RedPickupPosition = GetNode<Position2D>("Positions/RedPickupPosition");
		RedNavigation = GetNode<Navigation2D>("RedNav");

		blueGoal.Connect("body_entered", this, nameof(OnBlueGoalBodyEntered));
		redGoal.Connect("body_entered", this, nameof(OnRedGoalBodyEntered));
		blueBallDrop.Connect("body_entered", this, nameof(OnBlueBallDropBodyEntered));
		redBallDrop.Connect("body_entered", this, nameof(OnRedBallDropBodyEntered));
	}

	public void OnBlueGoalBodyEntered(Node body)
	{
		if (body is Ball ball && ball.ShotByPlayer && ball.Shooter.Alliance == Alliance.Blue)
		{
			Signals.PublishHighBallScoredEvent(ball.Shooter);
			ball.QueueFree();
		}
	}

	public void OnRedGoalBodyEntered(Node body)
	{
		if (body is Ball ball && ball.ShotByPlayer && ball.Shooter.Alliance == Alliance.Red)
		{
			Signals.PublishHighBallScoredEvent(ball.Shooter);
			ball.QueueFree();
		}
	}

	public void OnBlueBallDropBodyEntered(Node body)
	{
		if (body is Robot robot && robot.Alliance == Alliance.Blue)
		{
			Signals.PublishBallDropRequestedEvent(robot, BlueBallDropSpawnPoint, Vector2.Right);
		}
	}

	public void OnRedBallDropBodyEntered(Node body)
	{
		if (body is Robot robot && robot.Alliance == Alliance.Red)
		{
			Signals.PublishBallDropRequestedEvent(robot, RedBallDropSpawnPoint, Vector2.Right);
		}
	}

	public Vector2[] GetScorePath(Robot robot)
	{
		Vector2[] path;
		switch (robot.Alliance)
		{
			case Alliance.Red:
				path = RedNavigation.GetSimplePath(ToLocal(robot.GlobalPosition), RedScorePosition.GlobalPosition);
				break;
			case Alliance.Blue:
			//path = BlueNavigation.GetSimplePath(ToLocal(robot.GlobalPosition), BlueScorePosition.GlobalPosition);
			// break;
			default:
				path = new Vector2[] { };
				break;
		}

		return VectorsToGlobal(path);
	}

	public Vector2[] GetPickupPath(Robot robot)
	{
		Vector2[] path;
		switch (robot.Alliance)
		{
			case Alliance.Red:
				path = RedNavigation.GetSimplePath(ToLocal(robot.GlobalPosition), RedPickupPosition.GlobalPosition);
				break;
			case Alliance.Blue:
			//path = BlueNavigation.GetSimplePath(ToLocal(robot.GlobalPosition), BlueScorePosition.GlobalPosition);
			// break;
			default:
				path = new Vector2[] { };
				break;
		}

		return VectorsToGlobal(path);
	}

	private Vector2[] VectorsToGlobal(Vector2[] path)
	{
		Vector2[] globalPath = new Vector2[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			globalPath[i] = ToGlobal(path[i]);
		}

		return globalPath;
	}
}
