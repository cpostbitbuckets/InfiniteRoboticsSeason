extends Robot

var rot_dir: int = 0
var velocity_change = 0

#func _init(id: int).(id):
#	pass

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

func _input(_event):
	if Input.is_action_just_pressed("reset"):
		global_position = _initial_position
		global_rotation = _initial_rotation

	if Input.is_action_pressed('rotate_right'):
		rot_dir = 1
	elif Input.is_action_pressed('rotate_left'):
		rot_dir = -1
	else:
		rot_dir = 0
	
	if Input.is_action_pressed('forward'):
		velocity_change = max_speed
	elif Input.is_action_pressed('backward'):
		velocity_change = -max_speed
	else:
		velocity_change = 0
	if Input.is_action_just_pressed("intake"):
		intaking = true
	elif Input.is_action_just_released("intake"):
		intaking = false
	if Input.is_action_just_pressed("shoot"):
		shoot()

func control(delta: float) -> void:
	rotation += rotation_speed * rot_dir * delta
	velocity = Vector2(velocity_change, 0).rotated(rotation)
