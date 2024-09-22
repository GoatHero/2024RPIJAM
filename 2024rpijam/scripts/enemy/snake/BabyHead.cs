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

	protected Sprite2D sprite;
	protected PackedScene linkPackedScene;

	public override void _Ready() {
		base._Ready();
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBaby.tscn");
		sprite = GetNode<Sprite2D>("Sprite");
		makeSegments(linkPackedScene);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		float dt = (float)delta;

		if (isHead) {
			sprite.Frame = 0;
			
			moveToPosition(getPathToPos(player.GlobalPosition));		

			if (canAttack && (player.GlobalPosition - GlobalPosition).Length() < attackRange) {
				attack(player);
				addAttackCooldown();
			}
		} else {
			sprite.Frame = 1;

			Vector2 vel = LinearVelocity.Rotated(-GlobalRotation);
			vel.X = 0;
			if (vel.Length() > 0) {
				vel.Y *= -10f;
				vel = vel.Rotated(GlobalRotation);
				ApplyImpulse(dt*vel);
			}
		}
	}

	public override void damage(float amount, Vector2 knockback = new Vector2()) {
		if (isHead) {
			base.damage(amount, knockback);
		} else {
			ApplyCentralImpulse(knockback*50);
			parent.damage(amount);
		}
	}

	public virtual void attack(Player player) {
		Vector2 dif = player.GlobalPosition - GlobalPosition;
		dif /= dif.Length();
		player.damage(attackDamage, dif*attackKnockback);
	}

	public override void kill() {
		child?.kill();
		QueueFree();
	}
}
