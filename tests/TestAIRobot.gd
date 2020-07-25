extends KinematicBody2D

export (float, 0, 2000, 40) var linear_speed_max := 600.0
export (float, 0, 9000, 10.0) var linear_acceleration_max := 40.0
export (float, 0, 100, 0.1) var arrival_tolerance := 10.0
export (float, 0, 500, 10) var deceleration_radius := 100.0
export (float, 0, 5, 0.1) var predict_time := 0.3
export (float, 0, 200, 10.0) var path_offset := 20.0

# Maximum rotation velocity represented in degrees
export var angular_speed_max := 1440
# Maximum change in rotation velocity represented in degrees
export var angular_acceleration_max := 1440*1


var velocity := Vector2.ZERO
var acceleration := GSAITargetAcceleration.new()
var _valid := false
var _drag := 0.1
var linear_drag := 0.1
var angular_drag := 0.1
var angular_velocity := 0.0

onready var agent := GSAIKinematicBody2DAgent.new(self)
onready var path := GSAIPath.new(
	[
		Vector3(global_position.x, global_position.y, 0),
		Vector3(global_position.x, global_position.y, 0)
	],
	true
)
onready var follow := GSAIFollowPath.new(agent, path, 0, 0)

onready var look_path_blend := GSAIBlend.new(agent)

# GSAIPriority will be the main steering behavior we use. It holds sub-behaviors and will pick the
# first one that returns non-zero acceleration, ignoring any afterwards.
onready var priority := GSAIPriority.new(agent)

func _ready() -> void:
	pass

func setup():
	_setup(
		path_offset,
		predict_time,
		linear_acceleration_max,
		linear_speed_max,
		deceleration_radius,
		arrival_tolerance
	)

func _setup(
	path_offset: float,
	predict_time: float,
	accel_max: float,
	speed_max: float,
	decel_radius: float,
	arrival_tolerance: float
) -> void:
	owner.drawer.connect("path_established", self, "_on_Drawer_path_established")
	follow.path_offset = path_offset
	follow.prediction_time = predict_time
	follow.deceleration_radius = decel_radius
	follow.arrival_tolerance = arrival_tolerance

	agent.linear_acceleration_max = accel_max
	agent.linear_speed_max = speed_max
	agent.linear_drag_percentage = _drag
	agent.angular_speed_max = deg2rad(angular_speed_max)
	agent.angular_acceleration_max = deg2rad(angular_acceleration_max)
	agent.angular_drag_percentage = .1

	# LookWhereYouGo turns the agent to keep looking towards its direction of travel.
	var look := GSAILookWhereYouGo.new(agent)
	# How close for the agent to be 'aligned', if not exact
	look.alignment_tolerance = deg2rad(5)
	# When to start slowing down.
	look.deceleration_radius = deg2rad(60)
	
	look_path_blend.add(follow, 1)
	look_path_blend.add(look, 1)

#	priority.add(look_path_blend)
	priority.add(look)
	priority.add(follow)

func _physics_process(delta: float) -> void:
	update_agent()
	if _valid:
		priority.calculate_steering(acceleration)
#		agent._apply_steering(_accel, delta)
		# We add the discovered acceleration to our linear velocity. The toolkit does not limit
		# velocity, just acceleration, so we clamp the result ourselves here.
		velocity = (velocity + Vector2(acceleration.linear.x, acceleration.linear.y) * delta).clamped(
			agent.linear_speed_max
		)
	
		# This applies drag on the agent's motion, helping it to slow down naturally.
		velocity = velocity.linear_interpolate(Vector2.ZERO, linear_drag)
	
		# And since we're using a KinematicBody2D, we use Godot's excellent move_and_slide to actually
		# apply the final movement, and record any change in velocity the physics engine discovered.
		velocity = move_and_slide(velocity)
	
		# We then do something similar to apply our agent's rotational speed.
		angular_velocity = clamp(
			angular_velocity + acceleration.angular * delta, -agent.angular_speed_max, agent.angular_speed_max
		)
		
		# This applies drag on the agent's rotation, helping it slow down naturally.
		angular_velocity = lerp(angular_velocity, 0, angular_drag)
		rotation += angular_velocity * delta

# In order to support both 2D and 3D, the toolkit uses Vector3, so the conversion is required
# when using 2D nodes. The Z component can be left to 0 safely.
func update_agent() -> void:
	agent.position.x = global_position.x
	agent.position.y = global_position.y
	agent.orientation = rotation
	agent.linear_velocity.x = velocity.x
	agent.linear_velocity.y = velocity.y
	agent.angular_velocity = angular_velocity
	
func _on_Drawer_path_established(points: Array) -> void:
	var positions := PoolVector3Array()
	for p in points:
		positions.append(Vector3(p.x, p.y, 0))
	path.create_path(positions)
	_valid = true
