using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SnakeLink : BaseRigidBodyEnemy
{
	public override void _Ready() {
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		float dt = (float)delta;

		Vector2 vel = LinearVelocity.Rotated(-GlobalRotation);
		vel.X = 0;
		if (vel.Length() > 0) {
			vel.Y *= -0.05f;
			vel.Rotated(GlobalRotation);
			// ApplyImpulse(dt*vel);
		}
		
	}
}
