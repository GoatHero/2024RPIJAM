using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SnakeBossHead : BaseSnakeEnemy
{
	[Export]
	public float attackDamage = 10;
	[Export]
	public float attackKnockback = 10;
	[Export]
	public float attackRange = 30;

	private PackedScene linkPackedScene;

	public override void _Ready() {
		base._Ready();
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBossLink.tscn");
		makeSegments(linkPackedScene);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		moveToPosition(getPathToPos(player.GlobalPosition));		

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

	public override void kill() {
		for (int i = segments.Count-1; i >= 0; i--) {
			segments[i].QueueFree();
		}
	}
}
