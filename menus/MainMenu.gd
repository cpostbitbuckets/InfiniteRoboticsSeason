extends PanelContainer

func _ready():
	# register our buttons to a single event callback
	var buttons = get_tree().get_nodes_in_group("buttons")
	for button in buttons:
		button.connect("pressed", self, "_on_button_pressed", [button.name])

func _on_button_pressed(name):
	match name:
		"NewMatch":
			Signals.emit_signal("new_match")
		"Settings":
			Signals.emit_signal("settings")
