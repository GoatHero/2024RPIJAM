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
	private PackedScene headPackedScene;

	public override void _Ready() {
		base._Ready();
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBossLink.tscn");
		headPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBoss.tscn");
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
		GD.Print("segments.Count: ",segments.Count);
		foreach (RigidBody2D segment in segments) {
			if (segment is BaseSnakeLink) {
				(segments[1] as BaseSnakeLink).head = null;
			}
		}
		if (segments.Count > 1) {
			if (segments[1] is BaseSnakeLink)
				(segments[1] as BaseSnakeLink).makeHead(headPackedScene);
		}
		QueueFree();
	}
}
