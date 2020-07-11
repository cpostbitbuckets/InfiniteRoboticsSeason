extends Area2D


enum IntakeState { Up, Lowering, Down, Raising }

var state = IntakeState.Up


func _ready() -> void:
	$CollisionShape2D.disabled = true

func _input(event: InputEvent) -> void:
	if Input.is_action_just_pressed("toggle_intake"):
		if state == IntakeState.Down:
			$Intake.animation = "raise"
			$Intake.play("raise")
			state = IntakeState.Raising
			$CollisionShape2D.disabled = true
		elif state == IntakeState.Up:
			$Intake.animation = "raise"
			$Intake.play("raise", true)
			state = IntakeState.Lowering
			$CollisionShape2D.disabled = true

func _process(delta: float) -> void:
	if state == IntakeState.Lowering and $Intake.frame == 0:
		state = IntakeState.Down
		$Intake.animation = "down"
		$Intake.play()
		# turn on the intake collision detection once we are down
		$CollisionShape2D.disabled = false
	elif state == IntakeState.Raising and $Intake.frame == 2:
		state = IntakeState.Up
		$Intake.animation = "up"
		$Intake.play()
		$CollisionShape2D.disabled = true

