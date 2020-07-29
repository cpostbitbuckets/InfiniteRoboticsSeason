using Godot;
using System;

// [Tool]
// Leaving this as a tool node crashes the editor when you rename a node in match, player, robot, etc.
public class Hopper : RobotComponent
{
	#region drawing exports
	[Export]
	Color HopperColor { get; set; } = new Color("FFFFFF");

	[Export]
	float Radius { get; set; } = 19;

	[Export]
	float LineThickness { get; set; } = 2;

	[Export]
	bool AntiAliased { get; set; } = true;

	#endregion

	#region Hopper stats
	private int ballsInHopper = 3;

	[Export]
	public int BallsInHopper
	{
		get => ballsInHopper;
		set { ballsInHopper = value; ShowHopperBalls(); }
	}

	[Export]
	int Capacity { get; set; } = 5;

	#endregion

	private bool feeding = false;
	public bool Feeding
	{
		get => Feeding;
		set
		{
			feeding = value;
			if (animationPlayer != null)
			{
				GD.Print($"Feeding ${feeding} and updating animation player");
				if (feeding)
				{
					animationPlayer.Play();
				}
				else
				{
					animationPlayer.Stop();
				}
			}
		}
	}

	private const int segments = 16;

	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		if (animationPlayer != null)
		{
			animationPlayer.CurrentAnimation = "FeedBalls";
			animationPlayer.Stop();
		}
		ShowHopperBalls();
	}

	public override void _Process(float delta)
	{

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
			if (sprite != null)
			{
				sprite.Visible = BallsInHopper >= i;
			}
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
