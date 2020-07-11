extends Node
# The Global script manages switching scenes. It connects to signals to start new matches or what have you


var current_scene = null
var GameMenu = preload("res://menus/GameMenu.tscn")
onready var in_game_menu = GameMenu.instance()


func _ready():
	var root = get_tree().get_root()
	current_scene = root.get_child(root.get_child_count() - 1)

	Signals.connect("quit", self, "_on_quit")
	Signals.connect("new_match", self, "goto_scene", ["res://matches/Match.tscn"])
	Signals.connect("main_menu", self, "goto_scene", ["res://Main.tscn"])


func _input(event):
	if Input.is_action_just_pressed("ui_cancel") and current_scene.name != "Main":
#		current_scene.add_child(in_game_menu)
#		in_game_menu.popup()
		pass


func _on_quit():
	get_tree().quit()

func goto_scene(path):
	# This function will usually be called from a signal callback,
	# or some other function in the current scene.
	# Deleting the current scene at this point is
	# a bad idea, because it may still be executing code.
	# This will result in a crash or unexpected behavior.

	# The solution is to defer the load to a later time, when
	# we can be sure that no code from the current scene is running:

	call_deferred("_deferred_goto_scene", path)


func _deferred_goto_scene(path):
	# It is now safe to remove the current scene
	current_scene.free()

	# Load the new scene.
	var s = ResourceLoader.load(path)

	# Instance the new scene.
	current_scene = s.instance()

	# Add it to the active scene, as child of root.
	get_tree().get_root().add_child(current_scene)

	# Optionally, to make it compatible with the SceneTree.change_scene() API.
	get_tree().set_current_scene(current_scene)
