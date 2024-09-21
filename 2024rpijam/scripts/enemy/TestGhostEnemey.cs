using Godot;
using System;
using System.Diagnostics;

public partial class TestGhostEnemey : BaseEnemey
{
	[Export]
	public float speed = 300.0f;

	[Export]
	public float attackDamage = 10;
	[Export]
	public float attackKnockback = 1;
	[Export]
	public float attackRange = 10;

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

		if ((player.Position - Position).Length() < attackRange) {
			attack(player);
		}
	}

	public virtual void attack(Player player) {
		// player.damage(attackDamage, attackKnockback); // add damage to player
	}
}
