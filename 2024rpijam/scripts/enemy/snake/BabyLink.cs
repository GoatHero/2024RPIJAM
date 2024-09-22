using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BabyLink : BaseRigidBodyEnemy
{
	public BaseSnakeEnemy head;

	public override void _Ready() {
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		float dt = (float)delta;

		Vector2 vel = LinearVelocity.Rotated(-GlobalRotation);
		vel.X = 0;
		if (vel.Length() > 0) {
			vel.Y *= -10f;
			vel = vel.Rotated(GlobalRotation);
			ApplyImpulse(dt*vel);
		}
	}

	public override void damage(float amount, Vector2 knockback = new Vector2()) {
		ApplyCentralImpulse(knockback*50);
		head.damage(amount);
	}
}
