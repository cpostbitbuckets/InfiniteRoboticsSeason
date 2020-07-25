using Godot;
using System;

public class Intake : RobotComponent
{

	[Signal]
	public delegate void CanIntakeBall(RigidBody2D ball);

	private bool _intaking = false;
	public bool Intaking
	{
		get
		{
			return _intaking;
		}
		set
		{
			_intaking = value;
			ChargingParticles.Visible = _intaking;
		}
	}

	Particles2D ChargingParticles { get; set; }

	public override void _Ready()
	{
		ChargingParticles = GetNode<Particles2D>("ChargingParticles");

		Connect("body_entered", this, nameof(OnBodyEntered));

	}

	public void OnBodyEntered(Node body)
	{
		if (body is Ball ball && Intaking)
		{
			Robot.TryIntakeBall(ball);
		}
	}


}
