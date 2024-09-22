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
	protected Area2D attackBox;

	public override void _Ready() {
		base._Ready();
		headPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBoss.tscn");
		sprite = GetNode<Sprite2D>("Sprite");
		attackBox = GetNode<Area2D>("attackBox");
		makeSegments(headPackedScene);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		float dt = (float) delta;

		if (isHead) {
			sprite.Frame = 0;

			moveToPosition(getPathToPos(player.GlobalPosition));		

			if (canAttack && attackBox.HasOverlappingAreas()) {
				attack(player);
				addAttackCooldown();
			}
		} else {
			sprite.Frame = 1;

			bool l = leftWallTrig.HasOverlappingBodies();
			bool r = rightWallTrig.HasOverlappingBodies();
			if (l && r) {

			} else if (l) {
				ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation+0.1f*(float)Math.PI)*speed*size/10f);
			} else if (r) {
				ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation-0.1f*(float)Math.PI)*speed*size/10f);
			} else {
				ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation)*speed*size/10f);
			}
		}
	}

	public virtual void attack(Player player) {
		Vector2 dif = player.GlobalPosition - GlobalPosition;
		dif /= dif.Length();
		player.damage(attackDamage, dif*attackKnockback);
	}

	public override void kill() {		
		removeSegment();
	}
}
