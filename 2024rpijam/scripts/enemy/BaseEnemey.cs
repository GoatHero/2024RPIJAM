using Godot;
using System;
using System.Diagnostics;

public partial class BaseEnemy : CharacterBody2D
{
	[Export]
	public float speed = 100.0f;

	[Export]
	public float health = 10;

	protected Player player;

	public override void _Ready() {
		base._Ready();
		player = GetTree().Root.GetNode<Player>("root/Player");
	}

	public virtual bool moveToPosition(Vector2 pos) {
		float dt = (float)GetPhysicsProcessDeltaTime();

		Vector2 direction = pos - GlobalPosition;
		
		if (direction != Vector2.Zero) {
			Velocity += dt * direction/direction.Length() * speed;
		}
		Velocity -= dt * Velocity * 0.9f;

		MoveAndSlide();

		return direction.Length() <= 0.01;
	}

	public virtual void damage(float amount, Vector2 knockback = new Vector2()) {
		changeHealth(amount);
		Velocity += knockback;
	}

	public virtual void changeHealth(float amount) {
		health -= amount;
		if (health <= 0.0f) {
			kill();
		}
	}

	public virtual void kill() {
		QueueFree();
	}
}
