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
		float atPlayer = (GlobalPosition - player.GlobalPosition).Angle()-GlobalRotation;
		if (GlobalScale.X * GlobalScale.Y > 0)
			atPlayer += Rotation;
		else
			atPlayer -= Rotation;

		while (atPlayer > Math.PI) {
			atPlayer -= (float)Math.PI*2f;
		}
		while (atPlayer < -Math.PI) {
			atPlayer += (float)Math.PI*2f;
		}

		if (Math.Abs(atPlayer) <= limit) {
			if (GlobalScale.X * GlobalScale.Y > 0)
				Rotation += dt * (atPlayer - Rotation) * 8f;
			else
				Rotation += dt * (atPlayer + Rotation) * -8f;
		} else {
			Rotation -= dt * Rotation * 3f;
		}
	}
}
