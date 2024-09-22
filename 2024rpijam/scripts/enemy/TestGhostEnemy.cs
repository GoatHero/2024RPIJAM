using Godot;
using System;
using System.Diagnostics;

public partial class TestGhostEnemy : BaseCharacterBodyEnemy
{
	[Export]
	public float attackDamage = 10;
	[Export]
	public float attackKnockback = 10;
	[Export]
	public float attackRange = 50;

	public override void _Ready() {
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		moveToPosition(player.GlobalPosition);

		if (canAttack && (player.GlobalPosition - GlobalPosition).Length() < attackRange) {
			attack(player);
			addAttackCooldown();
		}
	}

	public virtual void attack(Player player) {
		Vector2 dif = player.GlobalPosition - GlobalPosition;
		dif /= dif.Length();
		player.damage(attackDamage, dif*attackKnockback);
	}
}
