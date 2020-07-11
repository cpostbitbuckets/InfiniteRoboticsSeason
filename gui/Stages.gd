extends VBoxContainer

const red_style: StyleBox = preload("res://themes/red_box_round.stylebox")

export (Enums.Alliance) var alliance = Enums.Alliance.Blue

# Called when the node enters the scene tree for the first time.
func _ready():	
	$HBoxContainer/Stage1/Lock.visible = true
	$HBoxContainer/Stage2/Lock.visible = true
	$HBoxContainer/Stage3/Lock.visible = true

	$HBoxContainer/Stage1/Label.visible = false
	$HBoxContainer/Stage1/Complete.visible = false
	$HBoxContainer/Stage1/RotateColorWheel.visible = false
	$HBoxContainer/Stage2/Label.visible = false
	$HBoxContainer/Stage1/Complete.visible = false
	$HBoxContainer/Stage1/RotateColorWheel.visible = false
	$HBoxContainer/Stage3/Label.visible = false
	$HBoxContainer/Stage3/Complete.visible = false
	$HBoxContainer/Stage3/RotateColorWheel.visible = false
	
	Signals.connect("stage_changed", self, "_on_stage_changed")
	# defaults to blue
	match [alliance]:
		[Enums.Alliance.Red]:
			$Level.set("custom_styles/panel", red_style)			
			$HBoxContainer/Stage1.set("custom_styles/panel", red_style)			
			$HBoxContainer/Stage2.set("custom_styles/panel", red_style)			
			$HBoxContainer/Stage3.set("custom_styles/panel", red_style)			

func _on_stage_changed(stage: AllianceStage, alliance) -> void:
	if alliance == self.alliance:
		match [stage.stage]:
			[Enums.Stage.One]:
				$HBoxContainer/Stage1/Label.visible = true
				$HBoxContainer/Stage1/Label.text = "%d" % stage.stage_balls_remaining
				$HBoxContainer/Stage1/Lock.visible = false
			[Enums.Stage.Two]:
				$HBoxContainer/Stage1/Label.visible = false
				$HBoxContainer/Stage1/Complete.visible = true
				$HBoxContainer/Stage2/Lock.visible = false
				if stage.get_stage_state() == Enums.StageState.Balls:
					$HBoxContainer/Stage2/Label.visible = true
					$HBoxContainer/Stage2/Label.text = "%d" % stage.stage_balls_remaining
				elif stage.get_stage_state() == Enums.StageState.RotateColorWheel:
					$HBoxContainer/Stage2/Label.visible = false
					$HBoxContainer/Stage2/RotateColorWheel.visible = true
			[Enums.Stage.Three]:
				$HBoxContainer/Stage2/RotateColorWheel.visible = false
				$HBoxContainer/Stage2/Complete.visible = true
				$HBoxContainer/Stage3/Lock.visible = false
				if stage.get_stage_state() == Enums.StageState.Balls:
					$HBoxContainer/Stage3/Label.visible = true
					$HBoxContainer/Stage3/Label.text = "%d" % stage.stage_balls_remaining
				elif stage.get_stage_state() == Enums.StageState.RotateToColor:
					$HBoxContainer/Stage3/Label.visible = false
					$HBoxContainer/Stage3/RotateColorWheel.visible = true
				elif stage.get_stage_state() == Enums.StageState.Complete:
					$HBoxContainer/Stage3/RotateColorWheel.visible = false
					$HBoxContainer/Stage3/Complete.visible = true
