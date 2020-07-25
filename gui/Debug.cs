using Godot;
using System;

public class Debug : MarginContainer
{
	[Export]
	public Robot RedRobot { get; set; }

	[Export]
	public Robot BlueRobot { get; set; }

	public override void _Ready()
	{
		GetNode<Button>("Alliances/Red/RedLowButton").Connect("pressed", this, nameof(OnRedLowButtonPressed));
		GetNode<Button>("Alliances/Red/RedHighButton").Connect("pressed", this, nameof(OnRedHighButtonPressed));
		GetNode<Button>("Alliances/Red/RedInnerButton").Connect("pressed", this, nameof(OnRedInnerButtonPressed));
		GetNode<Button>("Alliances/Red/RedSpinColorWheel").Connect("pressed", this, nameof(OnRedSpinColorWheelPressed));
		GetNode<Button>("Alliances/Red/RedPositionColorWheel").Connect("pressed", this, nameof(OnRedPositionColorWheelPressed));
		GetNode<Button>("Alliances/Blue/BlueLowButton").Connect("pressed", this, nameof(OnBlueLowButtonPressed));
		GetNode<Button>("Alliances/Blue/BlueHighButton").Connect("pressed", this, nameof(OnBlueHighButtonPressed));
		GetNode<Button>("Alliances/Blue/BlueInnerButton").Connect("pressed", this, nameof(OnBlueInnerButtonPressed));
		GetNode<Button>("Alliances/Blue/BlueSpinColorWheel").Connect("pressed", this, nameof(OnBlueSpinColorWheelPressed));
		GetNode<Button>("Alliances/Blue/BluePositionColorWheel").Connect("pressed", this, nameof(OnBluePositionColorWheelPressed));

	}

	/// <summary>
	/// Hide/Show the debug menu
	/// </summary>
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("debug"))
		{
			Visible = !Visible;
		}
	}

	private void OnRedLowButtonPressed() { Signals.PublishLowBallScoredEvent(RedRobot); }
	private void OnRedHighButtonPressed() { Signals.PublishHighBallScoredEvent(RedRobot); }
	private void OnRedInnerButtonPressed() { Signals.PublishInnerBallScoredEvent(RedRobot); }
	private void OnRedSpinColorWheelPressed() { Signals.PublishColorWheelSpunEvent(RedRobot); }
	private void OnRedPositionColorWheelPressed() { Signals.PublishColorWheelPositionedEvent(RedRobot); }

	private void OnBlueLowButtonPressed() { Signals.PublishLowBallScoredEvent(BlueRobot); }
	private void OnBlueHighButtonPressed() { Signals.PublishHighBallScoredEvent(BlueRobot); }
	private void OnBlueInnerButtonPressed() { Signals.PublishInnerBallScoredEvent(BlueRobot); }
	private void OnBlueSpinColorWheelPressed() { Signals.PublishColorWheelSpunEvent(BlueRobot); }
	private void OnBluePositionColorWheelPressed() { Signals.PublishColorWheelPositionedEvent(BlueRobot); }


}
