using Godot;
using System;

public class Field : StaticBody2D
{
    Position2D BlueBallDropSpawnPoint { get; set; }
    Position2D RedBallDropSpawnPoint { get; set; }

    public override void _Ready()
    {
        var redGoal = GetNode<Area2D>("RedGoal");
        var blueGoal = GetNode<Area2D>("BlueGoal");
        var blueBallDrop = GetNode<Area2D>("BlueBallDrop");
        var redBallDrop = GetNode<Area2D>("RedBallDrop");
        BlueBallDropSpawnPoint = GetNode<Position2D>("BlueBallDrop/SpawnPoint");
        RedBallDropSpawnPoint = GetNode<Position2D>("RedBallDrop/SpawnPoint");

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

}
