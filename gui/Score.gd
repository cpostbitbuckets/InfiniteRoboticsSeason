extends HBoxContainer

func _ready() -> void:
	# connect up to the score event
	Signals.connect("score_updated", self, "on_score_updated")

func on_score_updated(scorekeeper):
	$TeamScores/TeamScores/BlueScore/Label.text = "%d" % scorekeeper.blue_score
	$TeamScores/TeamScores/RedScore/Label.text = "%d" % scorekeeper.red_score
