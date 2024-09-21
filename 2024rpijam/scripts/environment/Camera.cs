using Godot;
using System;

public partial class Camera : Sprite2D
{
	[Export]
	public float maxRotationDegrees = 70f;
	private Player player;

	public override void _Ready()
	{
		base._Ready();
		player = GetTree().Root.GetNode<Player>("root/Player");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		float dt = (float)delta;

		float limit = maxRotationDegrees/180.0f*(float)Math.PI;
		float atPlayer = (GlobalPosition - player.GlobalPosition).Angle();
		if (Math.Abs(atPlayer) <= limit) {
			Rotation += dt * (atPlayer - Rotation) * 8f;
		} else {
			Rotation -= dt * Rotation * 3f;
		}
	}
}
