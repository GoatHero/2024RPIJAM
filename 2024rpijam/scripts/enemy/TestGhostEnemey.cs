using Godot;
using System;
using System.Diagnostics;

public partial class TestGhostEnemey : BaseEnemey
{
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
		moveToPosition(player.Position);

		if ((player.Position - Position).Length() < attackRange) {
			attack(player);
		}
	}

	public virtual void attack(Player player) {
		Vector2 dif = player.Position - Position;
		dif /= dif.Length();
		player.damage(attackDamage, dif*attackKnockback);
	}
}
