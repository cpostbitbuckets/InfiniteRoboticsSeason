using Godot;
using System;
using System.Collections.Generic;
using Stateless;

/// <summary>
/// Represents the overall state of an alliance in a match. This is
/// for storing all the robots, AI or otherwise, and determining what the
/// alliance team behind the wall is doing, whether giving balls, or moving balls 
/// from the scoring area to the loading area
/// </summary>
public class MatchAlliance : Node
{
	public enum States
	{
		// The alliance is idle, not doing anything
		Idle,

		// The alliance is giving balls to robots through the ball drop
		GivingBalls,

		// The alliance is moving balls from the scoring area to the ball drop area
		LoadingBalls,
	}

	public enum Triggers
	{
		AllianceNeedsBalls,
		RobotInLoadingZone,
		BallScored,
		Done,
	}

	#region Exports 

	[Export]
	public Alliance Alliance { get; set; } = Alliance.Blue;


	[Export]
	public int BallDropBalls
	{
		get => ballDropBalls;
		private set
		{
			ballDropBalls = value;
			PublishBallDropBallsUpdatedEvent();
		}
	}

	[Export]
	public int MaxBallDropBalls { get; set; } = 14;

	#endregion

	#region Events

	public delegate void BallDropBallsUpdated(MatchAlliance matchAlliance);
	public event BallDropBallsUpdated BallDropBallsUpdatedEvent;

	#endregion

	// the number of balls in the balldrop
	private int ballDropBalls = 5;

	public List<Robot> Robots { get; private set; } = new List<Robot>();
	private States State { get; set; } = States.Idle;
	private StateMachine<States, Triggers> machine;

	// the time it takes to load a ball from the scoring area to the ball drop area
	private Timer loadingBallTimer;

	// the time it takes for a ball to drop to a user from the ball drop
	private Timer givingBallTimer;

	public override void _Ready()
	{
		loadingBallTimer = GetNode<Timer>("LoadingBallTimer");
		givingBallTimer = GetNode<Timer>("GivingBallTimer");
		givingBallTimer.Connect("timeout", this, nameof(OnBallDropped));
		loadingBallTimer.Connect("timeout", this, nameof(OnBallLoaded));

		machine = new StateMachine<States, Triggers>(() => State, (s) => State = s);
		machine.Configure(States.Idle)
			.Permit(Triggers.AllianceNeedsBalls, States.GivingBalls)
			.Permit(Triggers.RobotInLoadingZone, States.GivingBalls)
			.Permit(Triggers.BallScored, States.LoadingBalls)
			.OnEntry(() => OnIdle());

		machine.Configure(States.GivingBalls)
			.Permit(Triggers.Done, States.Idle)
			.Permit(Triggers.BallScored, States.LoadingBalls)
			.OnEntry(() => OnGivingBalls());

		machine.Configure(States.LoadingBalls)
			.Permit(Triggers.Done, States.Idle)
			// .Permit(Triggers.BallScored, States.LoadingBalls)
			.OnEntry(() => OnLoadingBalls());

	}

	private void OnIdle()
	{
	}

	private void OnLoadingBalls()
	{
		if (BallDropBalls < MaxBallDropBalls)
		{
			loadingBallTimer.Start();
		}
	}

	private void OnGivingBalls()
	{
		// if we have balls in the ball drop area and we aren't
		// currently dropping a ball, put one in the drop chute and
		// start our timer.
		if (BallDropBalls > 0 && givingBallTimer.IsStopped())
		{
			BallDropBalls--;
			givingBallTimer.Start();
		}
	}

	private void OnBallDropped()
	{
		// if we have balls to give, drop one out
		Signals.PublishBallDroppedEvent(Alliance);
	}

	private void OnBallLoaded()
	{
		BallDropBalls++;
	}

	private void PublishBallDropBallsUpdatedEvent()
	{
		BallDropBallsUpdatedEvent?.Invoke(this);
	}

}
