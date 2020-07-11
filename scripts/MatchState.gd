extends Node
class_name MatchState

var red_stage = AllianceStage.new(Enums.Alliance.Red)
var blue_stage = AllianceStage.new(Enums.Alliance.Blue)

func _ready():
	Signals.connect("robot_joined", self, "_on_robot_joined")
	Signals.connect("scored_low_ball", self, "_on_ball_scored")
	Signals.connect("scored_high_ball", self, "_on_ball_scored")
	Signals.connect("scored_inner_ball", self, "_on_ball_scored")
	Signals.connect("color_wheel_spun", self, "_on_color_wheel_spun")
	Signals.connect("color_wheel_positioned", self, "_on_color_wheel_positioned")

	# setup the stages
	Signals.emit_signal("stage_changed", red_stage, red_stage.alliance)
	Signals.emit_signal("stage_changed", blue_stage, blue_stage.alliance)

func _on_robot_joined(robot: Robot):
	pass

func _on_ball_scored(robot: Robot):
	match [robot.alliance]:
		[Enums.Alliance.Red]:
			red_stage.score_ball()
		[Enums.Alliance.Blue]:
			blue_stage.score_ball()

func _on_color_wheel_spun(robot: Robot):
	match [robot.alliance]:
		[Enums.Alliance.Red]:
			red_stage.spin_wheel()
		[Enums.Alliance.Blue]:
			blue_stage.spin_wheel()

func _on_color_wheel_positioned(robot: Robot):
	# currently this does the same thing
	_on_color_wheel_spun(robot)
