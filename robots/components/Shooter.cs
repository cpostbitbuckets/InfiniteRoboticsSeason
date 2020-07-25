using Godot;
using System;

/// <summary>
/// The Shooter Component fires balls
/// The robot will tell the shooter to start shooting, at which point it will charge up 
/// and tell the robot that it is firing. It's up to the robot to actually fire the balls
/// remove them from the hopper, etc.
/// </summary>
public class Shooter : RobotComponent
{
	public enum State
	{
		Idle,
		Charging,
		Shooting,
		Cooldown
	}

	public delegate void TryShoot(Shooter shooter);
	public static event TryShoot TryShootEvent;

	public ShooterStats Stats { get; set; } = new ShooterStats();

	private bool shooting = false;
	public bool Shooting { get; set; }

	public State ShootingState { get; private set; } = State.Idle;

	private Timer chargeTimer;

	private Timer cooldownTimer;

	private Particles2D chargingEffect;

	public override void _Ready()
	{
		chargeTimer = GetNode<Timer>("ChargeTimer");
		cooldownTimer = GetNode<Timer>("CooldownTimer");
		chargingEffect = GetNode<Particles2D>("ChargingEffect");

		chargeTimer.WaitTime = Stats.ChargeTime;
		cooldownTimer.WaitTime = Stats.CooldownTime;
	}

	public override void _Process(float delta)
	{
		if (Shooting)
		{
			// if the robot wants the shooter to start shooting and
			// it is currently idle, start charging.
			if (ShootingState == State.Idle)
			{
				GD.Print("Charging the Shooter");
				ShootingState = State.Charging;
				Charge();
			}

			// Adjust the particle effects based on how close we are to shooting
			if (ShootingState == State.Charging)
			{
				float timeLeftPercent = (chargeTimer.WaitTime - chargeTimer.TimeLeft) / chargeTimer.WaitTime;
				chargingEffect.Scale = new Vector2(timeLeftPercent, timeLeftPercent);
			}
			if (ShootingState == State.Cooldown)
			{
				float timeLeftPercent = (cooldownTimer.WaitTime - cooldownTimer.TimeLeft) / cooldownTimer.WaitTime;
				chargingEffect.Scale = new Vector2(timeLeftPercent, timeLeftPercent);
			}

			// if the shooter is in the Shooting state and we are 
			// still commanded to shoot, publish a TryShoot event to let
			// any listeners know the shooter tried to fire
			// The listening robot will handle checking if the shooter has balls or not
			if (ShootingState == State.Shooting)
			{
				GD.Print("Trying to shoot ball");
				PublishTryShootEvent();

				// after shooting, cooldown and try again
				ShootingState = State.Cooldown;
				Cooldown();
			}
		}
		else if (ShootingState != State.Idle)
		{
			// if we stopped shooting, but we aren't idle
			chargeTimer.Stop();
			cooldownTimer.Stop();
			chargingEffect.Visible = false;

			// turn us to idle
			ShootingState = State.Idle;
		}
		else
		{
			chargingEffect.Visible = false;
		}
	}

	/// <summary>
	/// Begin charging the shooter.
	/// When this is complete, it will change the ShootingState to Shooting
	/// </summary>
	async protected virtual void Charge()
	{
		// charge up the shooter
		chargingEffect.Visible = true;
		chargingEffect.Scale = new Vector2(.1f, .1f);

		chargeTimer.Start();
		await ToSignal(chargeTimer, "timeout");
		ShootingState = State.Shooting;
	}

	/// <summary>
	/// This is called after a shot is fired. It will start a cooldown
	/// timer and attempt to shoot again.
	/// </summary>
	async protected virtual void Cooldown()
	{
		cooldownTimer.Start();
		await ToSignal(cooldownTimer, "timeout");
		GD.Print("Done cooling down");
		ShootingState = State.Shooting;
	}

	private void PublishTryShootEvent()
	{
		TryShootEvent?.Invoke(this);
	}


}
