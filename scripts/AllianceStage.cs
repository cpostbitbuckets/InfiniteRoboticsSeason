using Godot;
using System;

public class AllianceStage : Reference
{
	public Alliance Alliance { get; } = Alliance.Blue;
	public Stage Stage { get; private set; } = Stage.One;

	public StageState[] StageStates { get; private set; } = new[]{
		StageState.Balls,
		StageState.Locked,
		StageState.Locked
	};

	public int StageBallsRemaining { get; private set; } = Constants.StageOneBalls;

	public bool StageColorWheelComplete { get; private set; } = false;

	public ColorWheelColor StageColorWheelColor { get; private set; } = ColorWheelColor.Unknown;

	/// <summary>
	/// The state of the current stage
	/// </summary>
	/// <returns></returns>
	public StageState StageState => StageStates[(int)Stage];

	private Random random = new Random();
	public AllianceStage()
	{
	}

	public AllianceStage(Alliance alliance)
	{
		this.Alliance = alliance;
	}

	public void ScoreBall()
	{
		if (StageState == StageState.Balls)
		{
			StageBallsRemaining -= 1;
			CheckStageComplete();
			Signals.PublishStageChangedEvent(this);
		}
	}

	public void SpinColorWheel()
	{
		if (StageState == StageState.SpinColorWheel)
		{
			StageColorWheelComplete = true;
			CheckStageComplete();
			Signals.PublishStageChangedEvent(this);
		}
	}

	public void PositionColorWheel()
	{
		if (StageState == StageState.PositionColorWheel)
		{
			StageColorWheelComplete = true;
			CheckStageComplete();
			Signals.PublishStageChangedEvent(this);
		}
	}

	/// <summary>
	/// Check if the current stage is complete
	/// </summary>
	/// <returns>true if the stage is complete, false otherwise</returns>
	private bool CheckStageComplete()
	{
		// check for stage one transition
		if (StageBallsRemaining == 0)
		{
			if (Stage == Stage.One)
			{
				// complete this stage
				StageStates[(int)Stage] = StageState.Complete;

				// go to the next stage
				Stage = Stage.Two;
				StageStates[(int)Stage] = StageState.Balls;
				StageBallsRemaining = Constants.StageTwoBalls;

				// we changed state
				return true;
			}
			else if (Stage == Stage.Two)
			{
				if (StageState == StageState.Balls)
				{
					// if we were on stage two balls, switch to Color Wheel rotation
					StageStates[(int)Stage] = StageState.SpinColorWheel;
					return true;
				}
				else if (StageState == StageState.SpinColorWheel && StageColorWheelComplete)
				{
					// we put in balls and spun the color wheel, complete this stage
					StageStates[(int)Stage] = StageState.Complete;

					// setup the next stage
					Stage = Stage.Three;
					StageStates[(int)Stage] = StageState.Balls;
					StageBallsRemaining = Constants.StageThreeBalls;
					StageColorWheelComplete = false;
					return true;
				}
			}
			else if (Stage == Stage.Three)
			{
				if (StageState == StageState.Balls)
				{
					// if we were on stage two balls, switch to Color Wheel position
					StageStates[(int)Stage] = StageState.PositionColorWheel;
					// pick a random color for the color wheel to be positioned to
					StageColorWheelColor = (ColorWheelColor)random.Next(1, (int)ColorWheelColor.Yellow);
					return true;
				}
				else if (StageState == StageState.PositionColorWheel && StageColorWheelComplete)
				{
					// complete this stage
					StageStates[(int)Stage] = StageState.Complete;
					return true;
				}
			}
		}
		return false;
	}
}
