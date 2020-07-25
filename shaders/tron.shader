shader_type canvas_item;

uniform vec4 fill_color : hint_color = vec4(0, 0, 0, 1);
uniform vec4 line_color : hint_color = vec4(1, 1, 1, 1);
uniform float line_thickness : hint_range(0, 10) = 2.05;
uniform float line_sharpness : hint_range(0, 100) = 10.0;
uniform float edge_subtract : hint_range(0, 1) = .1;
uniform float glow_strength : hint_range(0, 10) = 1;

void fragment() 
{
	// this spreads out from the center
	vec2 uv = abs(UV - .5) * line_thickness;
	
	// this is our gradient that is thicker in the corners
	uv = pow(uv, vec2(line_sharpness)) - edge_subtract;
	
	// this determines how much "line" we have at this point
	float c = clamp(uv.x + uv.y, 0.0, 1.0) * glow_strength;
	
	// multiply our user specified line color by the "line" amount at this point
	vec4 gradient_line_color = line_color * c;
	
	// our final color is a mix of the fill color and the amount of line, where we 
	// prefer the fill color as the line amount becomes transparent
	vec4 final_color = mix(fill_color, gradient_line_color, (gradient_line_color.a));
	
	// make sure we are only as transparent as the fill_color specified
	final_color.a = fill_color.a;
	
	// we're done!
	COLOR = final_color;
}