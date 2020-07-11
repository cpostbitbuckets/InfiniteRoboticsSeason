extends Node2D

const Ball = preload("res://matches/Ball.tscn")

func _on_ready() -> void:
	Signals.emit_signal("robot_joined", $Player)
	
func _on_Robot_shoot(robot, position, direction, force):
	ball_shot(robot, position, direction, force)

func _on_Player_shoot(robot, position, direction, force):
	ball_shot(robot, position, direction, force)

func ball_shot(robot, position, direction, force):
	var ball = Ball.instance()
	
	# this is a shot that could go in a goal
	ball.shot_by_player = true
	ball.shooter = robot
	
	# add it to our scene at the robot position and send it away with some force
	add_child(ball)
	ball.position = position
	ball.add_to_group("ShotBall")
	ball.apply_impulse(ball.global_position, direction * force)
