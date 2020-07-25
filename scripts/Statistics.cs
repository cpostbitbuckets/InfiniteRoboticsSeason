using Godot;
using System;

/// <summary>
/// The Statistics we track for a match, alliance, or robot
/// </summary>
public class Statistics : Reference
{

	//
	// Auto Scores
	// 
	public int AutoLowBalls { get; set; }
	public int AutoHighBalls { get; set; }
	public int AutoInnerBalls { get; set; }
	public int AutoLeaveLines { get; set; }

	//
	// Scoring Balls 
	//
	public int LowBalls { get; set; }
	public int HighBalls { get; set; }
	public int InnerBalls { get; set; }

	//
	// Color Wheel
	//
	public bool SpinColorWheel { get; set; }
	public bool PositionColorWheel { get; set; }

	//
	// End Game
	// 
	public int Climbs { get; set; }
	public int Parks { get; set; }
	public bool LevelClimb { get; set; }

}
