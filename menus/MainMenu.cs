using Godot;
using System;

public class MainMenu : Control
{
	public override void _Ready()
	{
		Godot.Collections.Array buttons = GetTree().GetNodesInGroup("buttons");
		GD.Print($"Binding events for {buttons.Count} buttons in group");
		foreach (Button button in buttons)
		{
			GD.Print($"Binding pressed event for {button.Name}");
			Godot.Collections.Array binds = new Godot.Collections.Array(new[] { button.Name });
			button.Connect("pressed", this, nameof(OnButtonPressed), binds);
		}
	}

	void OnButtonPressed(string buttonName)
	{
		GD.Print($"Button {buttonName} pressed");
		switch (buttonName)
		{
			case "NewMatch":
				Signals.PublishNewMatchEvent();
				break;
			case "Quit":
				Signals.PublishQuitEvent();
				break;

		}
	}
}
