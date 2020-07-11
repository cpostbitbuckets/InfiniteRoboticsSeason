extends Robot

const red_drive_base_texture = preload("res://assets/Robot/ai_robot_red.png")

func _ready():
	
	# update our texture if we are red team
	if alliance == Enums.Alliance.Red:
		$DriveBase.texture = red_drive_base_texture

