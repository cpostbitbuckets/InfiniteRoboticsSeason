using Godot;
using System;

/// <summary>
/// The C# Signals class wires up events using standard
/// .net events, but also emits godot signals for gdscript objects
/// </summary>
public class Signals : Node
{

	[Signal]
	public delegate void RobotJoined(Robot robot);
	public static event RobotJoined RobotJoinedEvent;

	[Signal]
	public delegate void BallDropRequested(Robot robot, Position2D position, Vector2 direction);
	public static event BallDropRequested BallDropRequestedEvent;

	[Signal]
	public delegate void MatchCompleted();
	public static event MatchCompleted MatchCompletedEvent;

	[Signal]
	public delegate void MatchTimeUpdated(float timeElapsed);
	public static event MatchTimeUpdated MatchTimeUpdatedEvent;

	[Signal]
	public delegate void LowBallScored(Robot robot);
	public static event LowBallScored LowBallScoredEvent;

	[Signal]
	public delegate void HighBallScored(Robot robot);
	public static event HighBallScored HighBallScoredEvent;

	[Signal]
	public delegate void InnerBallScored(Robot robot);
	public static event InnerBallScored InnerBallScoredEvent;

	[Signal]
	public delegate void ColorWheelSpun(Robot robot);
	public static event ColorWheelSpun ColorWheelSpunEvent;

	[Signal]
	public delegate void ColorWheelPositioned(Robot robot);
	public static event ColorWheelPositioned ColorWheelPositionedEvent;

	[Signal]
	public delegate void ScoreUpdated(ScoreKeeper scoreKeeper);
	public static event ScoreUpdated ScoreUpdatedEvent;

	[Signal]
	public delegate void StageChanged(AllianceStage allianceStage);
	public static event StageChanged StageChangedEvent;

	[Signal]
	public delegate void Quit();
	public static event Quit QuitEvent;

	[Signal]
	public delegate void NewMatch();
	public static event NewMatch NewMatchEvent;

	[Signal]
	public delegate void GoToMainMenu();
	public static event GoToMainMenu GoToMainMenuEvent;

	private static Signals instance;
	public static Signals Instance
	{
		get
		{
			return instance;
		}
	}

	Signals()
	{
		instance = this;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	#region Event Publishers

	public static void PublishRobotJoinedEvent(Robot robot)
	{
		RobotJoinedEvent?.Invoke(robot);
		Instance.EmitSignal(nameof(RobotJoined), robot);
	}

	public static void PublishLowBallScoredEvent(Robot robot)
	{
		LowBallScoredEvent?.Invoke(robot);
		Instance.EmitSignal(nameof(LowBallScored), robot);
	}

	public static void PublishHighBallScoredEvent(Robot robot)
	{
		HighBallScoredEvent?.Invoke(robot);
		Instance.EmitSignal(nameof(HighBallScored), robot);
	}

	public static void PublishInnerBallScoredEvent(Robot robot)
	{
		InnerBallScoredEvent?.Invoke(robot);
		Instance.EmitSignal(nameof(InnerBallScored), robot);
	}

	public static void PublishColorWheelSpunEvent(Robot robot)
	{
		ColorWheelSpunEvent?.Invoke(robot);
		Instance.EmitSignal(nameof(ColorWheelSpun), robot);
	}


	public static void PublishColorWheelPositionedEvent(Robot robot)
	{
		ColorWheelPositionedEvent?.Invoke(robot);
		Instance.EmitSignal(nameof(ColorWheelPositioned), robot);
	}

	public static void PublishScoreUpdatedEvent(ScoreKeeper scoreKeeper)
	{
		ScoreUpdatedEvent?.Invoke(scoreKeeper);
		Instance.EmitSignal(nameof(ScoreUpdated), scoreKeeper);
	}

	public static void PublishStageChangedEvent(AllianceStage allianceStage)
	{
		StageChangedEvent?.Invoke(allianceStage);
		Instance.EmitSignal(nameof(StageChanged), allianceStage);
	}

	public static void PublishBallDropRequestedEvent(Robot robot, Position2D position, Vector2 direction)
	{
		BallDropRequestedEvent?.Invoke(robot, position, direction);
		Instance.EmitSignal(nameof(BallDropRequested), robot, position, direction);
	}

	public static void PublishMatchCompletedEvent()
	{
		MatchCompletedEvent?.Invoke();
		Instance.EmitSignal(nameof(MatchCompleted));
	}

	public static void PublishMatchTimeUpdatedEvent(float timeElapsed)
	{
		MatchTimeUpdatedEvent?.Invoke(timeElapsed);
		Instance.EmitSignal(nameof(MatchTimeUpdated), timeElapsed);
	}

	public static void PublishQuitEvent()
	{
		QuitEvent?.Invoke();
		Instance.EmitSignal(nameof(Quit));
	}

	public static void PublishNewMatchEvent()
	{
		NewMatchEvent?.Invoke();
		Instance.EmitSignal(nameof(NewMatch));
	}

	public static void PublishGoToMainMenuEvent()
	{
		GoToMainMenuEvent?.Invoke();
		Instance.EmitSignal(nameof(GoToMainMenu));
	}

	#endregion
}
