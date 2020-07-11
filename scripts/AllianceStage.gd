extends Object
class_name AllianceStage

# Helper class for storing alliance stage state
var stage = Enums.Stage.One
var stage_states = [Enums.StageState.Balls, Enums.StageState.Locked, Enums.StageState.Locked]
var stage_balls_remaining = Constants.stage_one_balls
var stage_color_wheel_complete = false
var stage_color_wheel_color = Enums.ColorWheelColor.Blue # todo - randomize this

var alliance

func _init(alliance) -> void:
	self.alliance = alliance

func get_stage_state() -> int:
	return stage_states[stage]

func score_ball() -> void:
	if stage_states[stage] == Enums.StageState.Balls:
		# only decrease the stage balls remaining if we're in the balls stage state
		# but not if we are in one of the color wheel states
		stage_balls_remaining -= 1
		
		# update the stage if necessary
		check_stage_complete()

		# notify any listeners that we changed stages
		Signals.emit_signal("stage_changed", self, self.alliance)

func spin_wheel() -> void:
	if stage_states[stage] == Enums.StageState.RotateColorWheel or stage_states[stage] == Enums.StageState.RotateToColor:
		stage_color_wheel_complete = true
		# update the stage
		check_stage_complete()
		# notify any listeners that we changed stages
		Signals.emit_signal("stage_changed", self, self.alliance)

func check_stage_complete() -> bool:
	if stage_balls_remaining == 0:
		# check for stage one transition
		if stage == Enums.Stage.One:
			# complete this stage
			stage_states[stage] = Enums.StageState.Complete

			# go to the next stage
			stage = Enums.Stage.Two
			stage_states[stage] = Enums.StageState.Balls
			stage_balls_remaining = Constants.stage_two_balls
			return true
		elif stage == Enums.Stage.Two:
			if stage_states[stage] == Enums.StageState.Balls:
				stage_states[stage] = Enums.StageState.RotateColorWheel
				return true
			elif stage_states[stage] == Enums.StageState.RotateColorWheel and stage_color_wheel_complete:
				# complete this stage
				stage_states[stage] = Enums.StageState.Complete
								
				# go to the next stage
				stage = Enums.Stage.Three
				stage_states[stage] = Enums.StageState.Balls
				stage_balls_remaining = Constants.stage_three_balls
				stage_color_wheel_complete = false
				return true

		elif stage == Enums.Stage.Three:
			if stage_states[stage] == Enums.StageState.Balls:
				stage_states[stage] = Enums.StageState.RotateToColor
				# todo, set the color
				return true
			elif stage_states[stage] == Enums.StageState.RotateToColor and stage_color_wheel_complete:
				stage_states[stage] = Enums.StageState.Complete
				return true
	return false
