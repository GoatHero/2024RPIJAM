using Godot;
using System;
using System.Diagnostics;

public partial class BaseEnemey : CharacterBody2D
{
	[Export]
	public float speed = 40.0f;

	[Export]
	public float health = 10;

	protected Player player;

	public override void _Ready() {
		base._Ready();
		player = GetTree().Root.GetNode<Player>("root/Player");
	}

	public virtual bool moveToPosition(Vector2 pos) {
		Vector2 direction = pos - Position;
		
		if (direction != Vector2.Zero) {
			Velocity += direction/direction.Length() * speed;
		}
		Velocity *= 0.9f;

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
