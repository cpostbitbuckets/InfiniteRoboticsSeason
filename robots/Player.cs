using Godot;
using System;

public class Player : Robot
{
	public int RotationDir { get; set; } = 0;

	public int VelocityChange { get; set; } = 0;

	public override void _Ready()
	{
		base._Ready();

		if (GetParent() == GetViewport())
		{
			// put the robot in the center if we launch the Player scene independently
			GlobalPosition = new Vector2(
				GetViewport().GetVisibleRect().Size.x / 2,
				GetViewport().GetVisibleRect().Size.y / 2);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionPressed("rotate_right"))
		{
			RotationDir = 1;
		}
		else if (Input.IsActionPressed("rotate_left"))
		{
			RotationDir = -1;
		}
		else
		{
			RotationDir = 0;
		}

		if (Input.IsActionPressed("forward"))
		{
			VelocityChange = MaxSpeed;
		}
		else if (Input.IsActionPressed("backward"))
		{
			VelocityChange = -MaxSpeed;
		}
		else
		{
			VelocityChange = 0;
		}

		if (Input.IsActionJustPressed("intake"))
		{
			Intaking(true);
		}
		else if (Input.IsActionJustReleased("intake"))
		{
			Intaking(false);
		}

		if (Input.IsActionJustPressed("shoot"))
		{
			Shooting(true);
		}
		else if (Input.IsActionJustReleased("shoot"))
		{
			Shooting(false);
		}
	}

	protected override void Control(float delta)
	{
		Rotation += RotationSpeed * RotationDir * delta;
		Velocity = new Vector2(VelocityChange, 0).Rotated(Rotation);
	}
}
