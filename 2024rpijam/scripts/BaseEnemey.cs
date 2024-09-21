using Godot;
using System;

public partial class BaseEnemey : CharacterBody2D
{
	[Export]
	public float speed = 300.0f;
	[Export]
	public float health = 10;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		Vector2 direction =  new Vector2(0, 1);
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();
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
