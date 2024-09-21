using Godot;
using System;
using System.Diagnostics;

public partial class BaseRigidBodyEnemy : RigidBody2D, BaseEnemy
{
	[Export]
	public float speed = 1000.0f;
	[Export]
	public float health = 10;
	[Export]
	public float maxHealth = 10;

	protected bool canAttack = true;
	protected Player player;
	protected Timer timer;

	public override void _Ready() {
		base._Ready();
		player = GetTree().Root.GetNode<Player>("root/Player");
		timer = GetNode<Timer>("AttackCooldown");
	}

	public virtual bool moveToPosition(Vector2 pos) {
		float dt = (float)GetPhysicsProcessDeltaTime();

		Vector2 direction = pos - GlobalPosition;
		
		if (direction != Vector2.Zero) {
			ApplyCentralImpulse(dt * direction/direction.Length() * speed);
		}

		return direction.Length() <= 0.01;
	}

	public virtual void damage(float amount, Vector2 knockback = new Vector2()) {
		changeHealth(amount);
		ApplyCentralImpulse(knockback*50);
	}

	public virtual void changeHealth(float amount) {
		health -= amount;
		if (health <= 0.0f) {
			kill();
		}
	}

	public virtual void kill() {
		QueueFree();
	}

	public virtual void addAttackCooldown(float amount = -1f) {
		timer.Start((double)amount);
		canAttack = false;
	}
	
	public virtual void resetAttackCooldown() {canAttack = true;}
	public virtual float getHealth() {return health;}
	public virtual float getMaxHealth() {return maxHealth;}
}
