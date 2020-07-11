extends Node
# Singleton interface for emitting and subscribing to signals

signal new_match
signal quit

# triggered when a robot joins the match
signal robot_joined(robot)

signal scored_low_ball(robot)
signal scored_high_ball(robot)
signal scored_inner_ball(robot)
signal color_wheel_spun(robot)
signal color_wheel_positioned(robot)

signal stage_changed(match_state, alliance)

signal score_updated

func _ready() -> void:
	pass

