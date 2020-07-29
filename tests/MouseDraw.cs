using Godot;
using System;

public class MouseDraw : Node2D
{
	Vector2 mouseStart = Vector2.Zero;
	Vector2 mouseEnd = Vector2.Zero;

	bool mouseHeld = false;

	public override void _Ready()
	{

	}

	public override void _Draw()
	{
		if (mouseHeld)
		{
			DrawRect(new Rect2(mouseStart, mouseEnd - mouseStart), new Color("ffffff"), true, 1, true);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left)
		{
			if (mouseButtonEvent.Pressed)
			{
				mouseHeld = true;
				mouseStart = mouseButtonEvent.Position;
				mouseEnd = mouseButtonEvent.Position;
			}
			else
			{
				mouseHeld = false;
				mouseEnd = Vector2.Zero;
				mouseStart = Vector2.Zero;
			}
		}
		if (@event is InputEventMouseMotion mouseMotion && mouseHeld)
		{
			mouseEnd = mouseMotion.Position;
		}
		Update();
	}

}
