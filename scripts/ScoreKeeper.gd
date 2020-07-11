extends Node
class_name ScoreKeeper

var game_statistics = Statistics.new()
var alliance_statistics = {
	Enums.Alliance.Blue: Statistics.new(),
	Enums.Alliance.Red: Statistics.new(),	
}

var robot_statistics = {
	Enums.Alliance.Blue: [],
	Enums.Alliance.Red: [],	
}

var alliance_score = {
	Enums.Alliance.Blue: 0,
	Enums.Alliance.Red: 0,	
}

var blue_score setget , blue_score_get
var red_score setget , red_score_get


func _ready():
	Signals.connect("robot_joined", self, "on_robot_joined")
	Signals.connect("scored_low_ball", self, "on_scored_low")
	Signals.connect("scored_inner_ball", self, "on_scored_inner")
	Signals.connect("scored_high_ball", self, "on_scored_high")
	Signals.connect("color_wheel_spun", self, "on_color_wheel_spun")
	Signals.connect("color_wheel_positioned", self, "on_color_wheel_positioned")

func on_robot_joined(robot: Robot):
	# Add this robot to our robot stats
	var robot_stats = Statistics.new()
	robot_stats.robot = robot
	robot_statistics[robot.alliance].append(robot_stats)

func on_scored_low(robot: Robot):
	print("Alliance %s scored a low ball" % robot.alliance)
	game_statistics.low_balls += 1
	var stats = alliance_statistics[robot.alliance]
	stats.low_balls += 1
	alliance_score[robot.alliance] = _calculate_score(stats)
	Signals.emit_signal("score_updated", self)

func on_scored_high(robot: Robot):
	print("Alliance %s scored a high ball" % robot.alliance)
	game_statistics.high_balls += 1
	var stats = alliance_statistics[robot.alliance]
	stats.high_balls += 1
	alliance_score[robot.alliance] = _calculate_score(stats)
	Signals.emit_signal("score_updated", self)

func on_scored_inner(robot: Robot):
	print("Alliance %s scored an inner ball" % robot.alliance)
	game_statistics.inner_balls += 1
	var stats = alliance_statistics[robot.alliance]
	stats.inner_balls += 1
	alliance_score[robot.alliance] = _calculate_score(stats)
	Signals.emit_signal("score_updated", self)

func on_color_wheel_spun(robot: Robot):
	print("Alliance %s spun the color wheel" % robot.alliance)
	game_statistics.spin_color_wheel = true
	var stats = alliance_statistics[robot.alliance]
	stats.spin_color_wheel = true
	alliance_score[robot.alliance] = _calculate_score(stats)
	Signals.emit_signal("score_updated", self)
	
func on_color_wheel_positioned(robot: Robot):
	print("Alliance %s rotated the color wheel to the required color" % robot.alliance)
	game_statistics.align_color_wheel = true
	var stats = alliance_statistics[robot.alliance]
	stats.align_color_wheel = true
	alliance_score[robot.alliance] = _calculate_score(stats)
	Signals.emit_signal("score_updated", self)

func _calculate_score(s: Statistics) -> int:
		# auto balls
		var score = ((
				s.auto_low_balls * Constants.low_ball_points + 
				s.auto_high_balls * Constants.high_ball_points + 
				s.auto_inner_balls * Constants.inner_ball_points) * Constants.auto_factor + 
				s.auto_leave_lines * Constants.auto_leave_line)
		
		# balls
		score += (s.low_balls * Constants.low_ball_points + s.high_balls * Constants.high_ball_points + s.inner_balls * Constants.inner_ball_points)
		
		print("l: %d, h: %d, i: %d" % [s.low_balls, s.high_balls, s.inner_balls])
		
		if s.spin_color_wheel:
			score += Constants.spin_color_wheel

		if s.align_color_wheel:
			score += Constants.align_color_wheel

		print("score: %d" % score)
		return score


func blue_score_get():
	return alliance_score[Enums.Alliance.Blue]

func red_score_get():
	return alliance_score[Enums.Alliance.Red]
