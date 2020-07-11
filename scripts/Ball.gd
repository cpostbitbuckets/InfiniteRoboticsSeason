extends RigidBody2D

var shot_by_player = false
var shooter: Robot = null

func _on_Ball_body_entered(_body):
	# As soon as we come into contact with some collision object, we are no longer
	# "shot by the player"
	# defer this 
	call_deferred("remove_shot_ball_group")

func remove_shot_ball_group():
	shot_by_player = false
	shooter = null
