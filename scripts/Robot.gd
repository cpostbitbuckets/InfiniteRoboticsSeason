extends KinematicBody2D
class_name Robot

signal shoot(robot, position, direction, force)

export (int) var max_speed = 200
export (float) var rotation_speed = 5
export (int) var balls_in_hopper = 3
export (int) var max_balls = 5
export (Enums.Alliance) var alliance = Enums.Alliance.Blue
export (int) var id = 0
export (String) var team_name = ''

var velocity = Vector2()
var _initial_position: Vector2
var _initial_rotation: float

var intaking = false

#func _init(id: int) -> void:
#	# construct a new robot with an id
#	self.id = id

func _ready():
	_initial_position = global_transform.get_origin()
	_initial_rotation = global_transform.get_rotation()
	show_hopper_balls()

# by default, we don't respond to controls with AI robots
func control(_delta: float) -> void:
	pass

func _physics_process(delta) -> void:
	control(delta)
	move_and_slide(velocity)

func shoot():
	# remove a ball from the hopper
	if balls_in_hopper > 0:
		balls_in_hopper -= 1
		show_hopper_balls()
		emit_signal(
			"shoot", 
			self, 
			global_position + Vector2(1, 0).rotated(global_rotation)*32, 
			Vector2(1, 0).rotated(global_rotation), 
			500
		)

func intake():
	# remove a ball from the hopper
	balls_in_hopper += 1
	show_hopper_balls()

func show_hopper_balls():
	$Hopper/HopperBall1.visible = false
	$Hopper/HopperBall2.visible = false
	$Hopper/HopperBall3.visible = false
	$Hopper/HopperBall4.visible = false
	if balls_in_hopper > 0:
		$Hopper/HopperBall1.visible = true
	if balls_in_hopper > 1:
		$Hopper/HopperBall2.visible = true
	if balls_in_hopper > 2:
		$Hopper/HopperBall3.visible = true
	if balls_in_hopper > 3:
		$Hopper/HopperBall4.visible = true

func _on_IntakeArea_body_entered(body: Node):
	print("Intake collided with %s intaking: %s, balls_in_hopper: %d, max_balls: %d" %[body, intaking, balls_in_hopper, max_balls])
	if intaking and balls_in_hopper < max_balls:
		if body.get_groups().find("Ball") != -1:
			print("Picked up ball!")
			balls_in_hopper += 1
			body.queue_free()
			show_hopper_balls()
