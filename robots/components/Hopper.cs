using Godot;
using System;

[Tool]
public class Hopper : RobotComponent
{
	#region exports
	[Export]
	Color HopperColor { get; set; } = new Color("FFFFFF");

	[Export]
	float Radius { get; set; } = 19;

	[Export]
	float LineThickness { get; set; } = 2;

	[Export]
	bool AntiAliased { get; set; } = true;

	#endregion

	private int ballsInHopper = 3;
	[Export]
	public int BallsInHopper
	{
		get => ballsInHopper;
		set { ballsInHopper = value; ShowHopperBalls(); }
	}

	[Export]
	int Capacity { get; set; } = 5;

	private const int segments = 16;

	public override void _Ready()
	{
		ShowHopperBalls();
	}

	public override void _Draw()
	{
		DrawArc(Position, Radius, 0, 180, segments, HopperColor, LineThickness, true);
	}

	private void ShowHopperBalls()
	{
		for (int i = 1; i <= 5; i++)
		{
			var sprite = GetNode<Sprite>($"Ball{i}");
			sprite.Visible = BallsInHopper >= i;
		}
	}

	public void RemoveBall()
	{
		if (BallsInHopper > 0)
		{
			BallsInHopper--;
		}
	}

	public void AddBall()
	{
		if (BallsInHopper < Capacity)
		{
			BallsInHopper++;
		}
	}

	public bool HasBalls => BallsInHopper > 0;

	public bool HasRoom => BallsInHopper < Capacity;

}
