using Godot;
using System;
using System.Diagnostics;

public partial class BaseFlyingEnemey : BaseEnemey
{
	[Export]
	public float speed = 300.0f;

	public override void _Ready() {
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 velocity = Velocity;
		Vector2 direction = player.Position - Position;
		direction /= direction.Length();
		if (direction != Vector2.Zero) {
			velocity = direction * speed;
		}
		else {
			velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, speed),  Mathf.MoveToward(Velocity.Y, 0, speed));
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public virtual void getPathToNode() {
		
	}
}
