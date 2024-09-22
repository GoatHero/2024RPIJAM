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

	protected Sprite2D sprite;
	protected PackedScene headPackedScene;

	public override void _Ready() {
		base._Ready();
		headPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBoss.tscn");
		sprite = GetNode<Sprite2D>("Sprite");
		makeSegments(headPackedScene);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (isHead) {
			sprite.Frame = 0;
			moveToPosition(getPathToPos(player.GlobalPosition));		

			if (canAttack && (player.GlobalPosition - GlobalPosition).Length() < attackRange) {
				attack(player);
				addAttackCooldown();
			}
		} else {
			sprite.Frame = 1;
		}
	}

	public virtual void attack(Player player) {
		Vector2 dif = player.GlobalPosition - GlobalPosition;
		dif /= dif.Length();
		player.damage(attackDamage, dif*attackKnockback);
	}

	public override void kill() {		
		child?.makeHead();
		QueueFree();
	}
}
