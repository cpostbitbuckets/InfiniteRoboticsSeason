extends MarginContainer

var red_robot = Robot.new()
var blue_robot = Robot.new()

func _ready():
	# debug view is disbaled by default
	visible = false
	
	red_robot.alliance = Enums.Alliance.Red
	blue_robot.alliance = Enums.Alliance.Blue

func _input(event):
	# toggle the debug menu
	if Input.is_action_just_pressed("debug"):
		visible = !visible

func _on_RedLowerButton_pressed():
	Signals.emit_signal("scored_low_ball", red_robot)

func _on_RedHighButton_pressed():
	Signals.emit_signal("scored_high_ball", red_robot)

func _on_RedInnerButton_pressed():
	Signals.emit_signal("scored_inner_ball", red_robot)

func _on_RedSpinColorWheel_pressed():
	Signals.emit_signal("color_wheel_spun", red_robot)

func _on_RedPositionColorWheel_pressed():
	Signals.emit_signal("color_wheel_positioned", red_robot)

func _on_BlueLowerButton_pressed():
	Signals.emit_signal("scored_low_ball", blue_robot)

func _on_BlueHighButton_pressed():
	Signals.emit_signal("scored_high_ball", blue_robot)

func _on_BlueInnerButton_pressed():
	Signals.emit_signal("scored_inner_ball", blue_robot)

func _on_BlueSpinColorWheel_pressed():
	Signals.emit_signal("color_wheel_spun", blue_robot)

func _on_BluePositionColorWheel_pressed():
	Signals.emit_signal("color_wheel_positioned", blue_robot)



