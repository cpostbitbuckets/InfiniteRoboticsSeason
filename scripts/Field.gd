extends StaticBody2D

const Ball = preload("res://scripts/Ball.gd")

signal score(alliance, goal)

func _on_BlueGoal_body_entered(body: Ball):
	# only score balls shot by the blue players
	if body and body.get_groups().find("ShotBall") != -1:
		var ball = body as Ball
		if ball and ball.shot_by_player and ball.shooter.alliance == Enums.Alliance.Blue:
			ball.queue_free()
			
			# a high ball was scored, signal the scorekeeper and anyone else listening
			Signals.emit_signal("scored_high_ball", ball.shooter)
