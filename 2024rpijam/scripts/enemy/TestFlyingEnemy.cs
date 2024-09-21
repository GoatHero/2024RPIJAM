using Godot;
using System;
using System.Diagnostics;

public partial class TestFlyingEnemy : BaseFlyingEnemy
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
		base._PhysicsProcess(delta);

		moveToPosition(getPathToPos(player.GlobalPosition));

		if ((player.GlobalPosition - GlobalPosition).Length() < attackRange) {
			attack(player);
		}
	}

	public virtual void attack(Player player) {
		Vector2 dif = player.GlobalPosition - GlobalPosition;
		dif /= dif.Length();
		player.damage(attackDamage, dif*attackKnockback);
	}
}
