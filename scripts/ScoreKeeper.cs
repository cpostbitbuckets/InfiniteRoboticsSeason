using Godot;
using System;
using System.Collections.Generic;

public class ScoreKeeper : Node
{
	public Statistics GameStatistics { get; private set; } = new Statistics();

	public Dictionary<Alliance, Statistics> AllianceStatistics { get; private set; } = new Dictionary<Alliance, Statistics>
	{
		{Alliance.Blue, new Statistics()},
		{Alliance.Red, new Statistics()}
	};

	/// <summary>
	/// Robot statistics keyed by robot TeamNumber
	/// </summary>
	/// <typeparam name="String"></typeparam>
	/// <typeparam name="Statistics"></typeparam>
	/// <returns></returns>
	public Dictionary<int, Statistics> RobotStatistics { get; private set; } = new Dictionary<int, Statistics>();

	/// <summary>
	/// Alliance scores keyed by alliance
	/// </summary>
	/// <typeparam name="Alliance"></typeparam>
	/// <typeparam name="int"></typeparam>
	/// <returns></returns>
	private Dictionary<Alliance, int> AllianceScore { get; } = new Dictionary<Alliance, int>()
	{
		{Alliance.Blue, 0},
		{Alliance.Red, 0}
	};

	public int BlueScore => AllianceScore[Alliance.Blue];
	public int RedScore => AllianceScore[Alliance.Red];

	public override void _Ready()
	{
		Signals.RobotJoinedEvent += OnRobotJoined;
		Signals.LowBallScoredEvent += OnLowBallScored;
		Signals.HighBallScoredEvent += OnHighBallScored;
		Signals.InnerBallScoredEvent += OnInnerBallScored;
		Signals.ColorWheelSpunEvent += OnColorWheelSpun;
		Signals.ColorWheelPositionedEvent += OnColorWheelPositioned;

	}

	private void OnColorWheelPositioned(Robot robot)
	{
		GameStatistics.PositionColorWheel = true;
		RobotStatistics[robot.TeamNumber].PositionColorWheel = true;
		AllianceStatistics[robot.Alliance].PositionColorWheel = true;

		// update the alliance score
		AllianceScore[robot.Alliance] = CalculateScore(AllianceStatistics[robot.Alliance]);

		// publish the event
		Signals.PublishScoreUpdatedEvent(this);
	}

	private void OnColorWheelSpun(Robot robot)
	{
		GameStatistics.SpinColorWheel = true;
		RobotStatistics[robot.TeamNumber].SpinColorWheel = true;
		AllianceStatistics[robot.Alliance].SpinColorWheel = true;

		// update the alliance score
		AllianceScore[robot.Alliance] = CalculateScore(AllianceStatistics[robot.Alliance]);

		// publish the event
		Signals.PublishScoreUpdatedEvent(this);
	}

	private void OnLowBallScored(Robot robot)
	{
		GameStatistics.LowBalls++;
		RobotStatistics[robot.TeamNumber].LowBalls++;
		AllianceStatistics[robot.Alliance].LowBalls++;
		// update the alliance score
		AllianceScore[robot.Alliance] = CalculateScore(AllianceStatistics[robot.Alliance]);

		// publish the event
		Signals.PublishScoreUpdatedEvent(this);
	}

	private void OnInnerBallScored(Robot robot)
	{
		GameStatistics.InnerBalls++;
		RobotStatistics[robot.TeamNumber].InnerBalls++;
		AllianceStatistics[robot.Alliance].InnerBalls++;
		// update the alliance score
		AllianceScore[robot.Alliance] = CalculateScore(AllianceStatistics[robot.Alliance]);

		// publish the event
		Signals.PublishScoreUpdatedEvent(this);
	}

	private void OnHighBallScored(Robot robot)
	{
		GameStatistics.HighBalls++;
		RobotStatistics[robot.TeamNumber].HighBalls++;
		AllianceStatistics[robot.Alliance].HighBalls++;
		// update the alliance score
		AllianceScore[robot.Alliance] = CalculateScore(AllianceStatistics[robot.Alliance]);

		// publish the event
		Signals.PublishScoreUpdatedEvent(this);
	}

	private void OnRobotJoined(Robot robot)
	{
		RobotStatistics[robot.TeamNumber] = new Statistics();
	}

	private int CalculateScore(Statistics s)
	{
		int score = 0;

		// auto
		score = (
			s.AutoLowBalls * Constants.LowBallPoints +
			s.AutoHighBalls * Constants.HighBallPoints +
			s.AutoInnerBalls * Constants.InnerBallPoints) * Constants.AutoFactor;

		score += s.AutoLeaveLines * Constants.AutoLeaveLine;

		// balls
		score += s.LowBalls * Constants.LowBallPoints +
				 s.HighBalls * Constants.HighBallPoints +
				 s.InnerBalls * Constants.InnerBallPoints;

		// color wheel
		if (s.SpinColorWheel)
		{
			score += Constants.SpinColorWheel;
		}
		if (s.PositionColorWheel)
		{
			score += Constants.PositionColorWheel;
		}

		return score;
	}
}
