using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BabyHead : BaseSnakeEnemy
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
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBabyLink.tscn");
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

		GD.Print("sake ded",  segments);
		for (int i = segments.Count-1; i >= 0; i--) {
			GD.Print(i, segments[i]);
			segments[i].QueueFree();
		}
	}

	public override void setupNewSegment(RigidBody2D newSegment) {
		base.setupNewSegment(newSegment);
		if (newSegment is BabyLink) {
			(newSegment as BabyLink).head = this;
		}
	}
}
