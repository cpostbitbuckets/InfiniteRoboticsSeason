tool
extends Polygon2D

export(Color) var outline_color = Color(0,0,0) setget set_color
export(float) var line_width = 1.0 setget set_width
export(float) var glow_intensity = 1.0 setget set_glow

func _ready():
	color = Color(color.r, color.g, color.b, 0)
	$CollisionPolygon2D.polygon = get_polygon()

func _draw() -> void:
	var poly = get_polygon()
	if poly.size() > 2:
#		draw_colored_polygon(poly, color)
		poly.append(poly[0])
		poly.append(poly[1])
		draw_polyline(poly, outline_color * glow_intensity, line_width, true)
	
func set_color(value: Color) -> void:
	outline_color = value
	update()

func set_width(value: float) -> void:
	line_width = value
	update()

func set_glow(value: float) -> void:
	glow_intensity = value
	update()
