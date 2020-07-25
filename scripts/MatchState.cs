using Godot;
using System;

public class MatchState : Node
{
	public AllianceStage RedStage { get; private set; } = new AllianceStage(Alliance.Red);
	public AllianceStage BlueStage { get; private set; } = new AllianceStage(Alliance.Blue);

	public override void _Ready()
	{
		Signals.LowBallScoredEvent += OnBallScored;
		Signals.HighBallScoredEvent += OnBallScored;
		Signals.InnerBallScoredEvent += OnBallScored;

		Signals.ColorWheelSpunEvent += OnColorWheelSpun;
		Signals.ColorWheelPositionedEvent += OnColorWheelPositioned;

		// setup the stages
		Signals.PublishStageChangedEvent(RedStage);
		Signals.PublishStageChangedEvent(BlueStage);
	}

	private AllianceStage GetAllianceStage(Alliance alliance)
	{
		switch (alliance)
		{
			case Alliance.Blue:
				return BlueStage;
			case Alliance.Red:
				return RedStage;
			default:
				throw new ArgumentException($"Unknown Alliance {alliance}");
		}
	}

	private void OnBallScored(Robot robot)
	{
		AllianceStage stage = GetAllianceStage(robot.Alliance);
		stage.ScoreBall();
	}

	private void OnColorWheelSpun(Robot robot)
	{
		AllianceStage stage = GetAllianceStage(robot.Alliance);
		stage.SpinColorWheel();
	}

	private void OnColorWheelPositioned(Robot robot)
	{
		AllianceStage stage = GetAllianceStage(robot.Alliance);
		stage.PositionColorWheel();
	}


}
