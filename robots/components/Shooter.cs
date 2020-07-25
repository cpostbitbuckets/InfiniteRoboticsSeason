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

	public ShooterStats Stats { get; set; } = new ShooterStats();

	private bool shooting = false;
	public bool Shooting
	{
		get { return shooting; }
		set { shooting = value; ShootStateChanged(); }
	}

	public ShootingState State { get; set; }

	private Timer chargeTimer;

	private Timer cooldownTimer;

	public override void _Ready()
	{
		chargeTimer = GetNode<Timer>("ChargeTimer");
		cooldownTimer = GetNode<Timer>("CooldownTimer");
	}

	/// <summary>
	/// Begin charging the shooter
	/// /// </summary>
	async public void Charge()
	{
		// charge up the shooter
		chargeTimer.Start();
		await ToSignal(chargeTimer, "timeout");

		if (shooting)
		{
			while (shooting)
			{
				GD.Print("Trying to shoot ball");
				Robot.TryShoot();
				cooldownTimer.Start();
				await ToSignal(cooldownTimer, "timeout");
			}
		}

	}

	void ShootStateChanged()
	{
		GD.Print($"Shoot state change {shooting}");

		if (shooting)
		{
			Charge();
		}
		else
		{
			chargeTimer.Stop();
			cooldownTimer.Stop();
		}
	}

}
