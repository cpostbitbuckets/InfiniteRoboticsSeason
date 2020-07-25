using Godot;
using System;

public class Ball : RigidBody2D
{
	public bool ShotByPlayer { get; set; } = false;
	public Robot Shooter { get; set; }

	Sprite Sprite { get; set; }

	Node2D FireBallEffect { get; set; }

	public override void _Ready()
	{
		Sprite = GetNode<Sprite>("Sprite");
		FireBallEffect = GetNode<Node2D>("FireBallEffect");
		if (Shooter != null)
		{
			FireBallEffect.Visible = true;
		}
		
	}

	void OnBallBodyEntered(Node2D body)
	{
		GD.Print($"body collided with ball: {body}");
		if (body == null || body != Shooter)
		{
			CallDeferred("RemoveShotBallGroup");
		}
	}

	void RemoveShotBallGroup()
	{
		ShotByPlayer = false;
		Shooter = null;
		FireBallEffect.Visible = false;
		CollisionLayer = (uint)CollisionLayers.Ground;
		CollisionMask = (uint)CollisionLayers.Ground;
	}

}
