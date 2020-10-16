using Godot;
using System;

public class GameMenu : PopupPanel
{


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

// This is the non-working gdscript stuff. I'm not sure what
// I was doing here...
//
// func _ready():
// 	# register our buttons to a single event callback
// 	var buttons = get_tree().get_nodes_in_group("buttons")
// 	for button in buttons:
// 		button.connect("pressed", self, "_on_button_pressed", [button.name])

// func _on_button_pressed(name):
// 	match name:
// 		"MainMenu":
// 			Signals.emit_signal("main_menu")
// 		"Quit":
// 			Signals.emit_signal("quit")

}
