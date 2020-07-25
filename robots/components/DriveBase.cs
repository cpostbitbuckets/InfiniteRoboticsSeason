using Godot;
using System;

public class DriveBase : RobotComponent
{
	public override void _Ready()
	{
	}

	override public void UpdateAllianceColor()
	{
		base.UpdateAllianceColor();
		// set the outline color of our drivebase glowgon to our alliance color
		GetNode("Glowgon")?.Set("outline_color", color);
	}

}
