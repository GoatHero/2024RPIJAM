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
	public float detecionDis = 1200;

	protected Sprite2D sprite;
	protected PackedScene linkPackedScene;
	protected Area2D attackBox;

	public override void _Ready() {
		base._Ready();
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBaby.tscn");
		sprite = GetNode<Sprite2D>("Sprite");
		attackBox = GetNode<Area2D>("attackBox");
		makeSegments(linkPackedScene);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if ((player.GlobalPosition - GlobalPosition).Length() < detecionDis) {
			float dt = (float)delta;

			if (isHead) {
				sprite.Frame = 0;
				
				moveToPosition(getPathToPos(player.GlobalPosition));
				if (canAttack && attackBox.HasOverlappingAreas()) {
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
