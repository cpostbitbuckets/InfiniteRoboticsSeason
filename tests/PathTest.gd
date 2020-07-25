extends Node2D

onready var drawer := $PathDrawer
onready var path := $Navigation2D

func _ready():
	$PathDrawer.start_node = $A1IRobotRed1
	$A1IRobotRed1.setup()
	
