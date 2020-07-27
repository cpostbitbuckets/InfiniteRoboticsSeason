using Godot;
using System;
using Stateless;

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

    public enum Trigger
    {
        Idle,
        StartShooting,
        Charge,
        Shoot,
        Cooldown,
    }

    public delegate void TryShoot(Shooter shooter);
    public event TryShoot TryShootEvent;

    public ShooterStats Stats { get; set; } = new ShooterStats();

    public State ShootingState { get; private set; } = State.Idle;

    private Timer chargeTimer;

    private Timer cooldownTimer;

    private Particles2D chargingEffect;

    StateMachine<State, Trigger> machine;

    public override void _Ready()
    {
        chargeTimer = GetNode<Timer>("ChargeTimer");
        cooldownTimer = GetNode<Timer>("CooldownTimer");
        chargingEffect = GetNode<Particles2D>("ChargingEffect");

        chargeTimer.WaitTime = Stats.ChargeTime;
        cooldownTimer.WaitTime = Stats.CooldownTime;

        chargeTimer.Connect("timeout", this, nameof(OnChargeTimerTiemout));
        cooldownTimer.Connect("timeout", this, nameof(OnCooldownTimerTiemout));

        // create a new state machine
        machine = new StateMachine<State, Trigger>(() => ShootingState, s => ShootingState = s);

        // the idle state starts charging the shooter when
        // the StartShooting trigger is fired
        machine.Configure(State.Idle)
            .Permit(Trigger.StartShooting, State.Charging)
            .OnEntry(() => OnIdle());

        // The charging state transitions to teh shooting state
        // when complete
        machine.Configure(State.Charging)
            .OnEntry(() => OnCharge())
            .Permit(Trigger.Idle, State.Idle)
            .Permit(Trigger.Shoot, State.Shooting);

        // The shooting state transitions to cooldown when done
        machine.Configure(State.Shooting)
            .OnEntry(() => OnShoot())
            .Permit(Trigger.Idle, State.Idle)
            .Permit(Trigger.Cooldown, State.Cooldown);

        // The cooldown state transitions to shooting again
        machine.Configure(State.Cooldown)
            .OnEntry(() => Cooldown())
            .Permit(Trigger.Idle, State.Idle)
            .Permit(Trigger.Shoot, State.Shooting);

        machine.OnTransitioned(t => GD.Print($"OnTransitioned: {t.Source} -> {t.Destination} via {t.Trigger}({string.Join(", ", t.Parameters)})"));

    }

    public override void _Process(float delta)
    {
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
    }

    public void StartShooting()
    {
        machine.Fire(Trigger.StartShooting);
    }

    public void StopShooting()
    {
        machine.Fire(Trigger.Idle);
    }

    protected virtual void OnIdle()
    {
        // if we stopped shooting, but we aren't idle
        chargeTimer.Stop();
        cooldownTimer.Stop();
        chargingEffect.Visible = false;
    }

    /// <summary>
    /// Begin charging the shooter.
    /// When this is complete, it will change the ShootingState to Shooting
    /// </summary>
    protected virtual void OnCharge()
    {
        // charge up the shooter
        chargingEffect.Visible = true;
        chargingEffect.Scale = new Vector2(.1f, .1f);

        // when the shooter is done charging, fire a shoot state
        chargeTimer.Start();
    }

    protected void OnChargeTimerTiemout()
    {
        // go to the next state
        machine.Fire(Trigger.Shoot);
    }

    protected virtual void OnShoot()
    {
        // try and shoot a ball, then go to cooldown
        PublishTryShootEvent();
        machine.Fire(Trigger.Cooldown);
    }


    /// <summary>
    /// This is called after a shot is fired. It will start a cooldown
    /// timer and attempt to shoot again.
    /// </summary>
    protected virtual void Cooldown()
    {
        cooldownTimer.Start();
    }

    protected void OnCooldownTimerTiemout()
    {
        // go to the next state
        machine.Fire(Trigger.Shoot);
    }

    private void PublishTryShootEvent()
    {
        TryShootEvent?.Invoke(this);
    }


}
