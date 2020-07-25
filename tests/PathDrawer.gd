extends Node2D

signal path_established(points)

const BASE_LINE_WIDTH = 3.0
const DRAW_COLOR = Color('#fff')

var point_start: Vector2
var point_end: Vector2
var start_node: Node2D setget _set_start_node
var _point_path

func _unhandled_input(event: InputEvent) -> void:
#	if event is InputEventMouseButton and event.button_index == BUTTON_LEFT and event.is_pressed():
#		point_start = Vector2(event.position.x, event.position.y)
#		print("Set start point: %s" % point_start)
#		_update_path()
	if event is InputEventMouseButton and event.button_index == BUTTON_RIGHT and event.is_pressed():
		point_end = Vector2(event.position.x, event.position.y)
		print("Set end point: %s" % point_end)
		_update_path()
	

func _update_path():
	if start_node and point_end:
		point_start = start_node.global_position
		_point_path = owner.path.get_simple_path(point_start, point_end)
		update()
		print("Computed point path: %s" % _point_path)		
		emit_signal("path_established", _point_path)

func _draw():
	if not _point_path:
		return
	var point_start = _point_path[0]
	var point_end = _point_path[len(_point_path) - 1]

	var last_point = Vector2(point_start.x, point_start.y)

	for index in range(1, len(_point_path)):
		var current_point = Vector2(_point_path[index].x, _point_path[index].y)
		draw_line(last_point, current_point, DRAW_COLOR, BASE_LINE_WIDTH, true)
		draw_circle(current_point, BASE_LINE_WIDTH * 2.0, DRAW_COLOR)
		last_point = current_point
		
func _set_start_node(node: Node2D) -> void:
	start_node = node
