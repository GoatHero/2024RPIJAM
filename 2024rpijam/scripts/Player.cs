using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float fallMulti = 1.5f;
	[Export]
	public float dashSpeed = 4800.0f;
	[Export]
	public float speed = 2800.0f;
	[Export]
	public float airSpeed = 1000.0f;
	[Export]
	public float jumpVelocity = -600.0f;
	[Export]
	public float wallJumpVelocity = 750.0f;
	[Export]
	public float airPoundMulti = 3f;

	[Export]
	public float health = 100;
	[Export]
	public float maxHealth = 100;
	[Export]
	public float attackDamage = 5f;
	[Export]
	public float attackCooldown = 0.6f;
	
	public bool canAttack = true;
	private bool rightFacing = true;
	private bool canDash = false;
	
	
	private Area2D hitbox;
	private Area2D wallbox;
	private Timer attackCooldownTimer;
	private Timer dashCoolTimer;
	private Camera2D camera;
	
	public override void _Ready(){
		base._Ready();
		hitbox = GetNode<Area2D>("hitbox");
		wallbox = GetNode<Area2D>("wallBox");
		attackCooldownTimer = GetNode<Timer>("hitTimer");
		dashCoolTimer = GetNode<Timer>("dashCoolTimer");
		camera = GetNode<Camera2D>("camera");
	}
	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;
		Vector2 velocity = Velocity;
		
		//Handle Attack 
		if (Input.IsActionJustPressed("attack") && canAttack){
			foreach(Node2D nodeH in hitbox.GetOverlappingBodies()){
				if(nodeH is BaseEnemy){
					BaseEnemy be = nodeH as BaseEnemy;
					be.damage(attackDamage);
					addAttackCooldown();
				}
			}
		}
		
		
		// Get the input direction and handle the movement/deceleration.
		float horizontalMovement = Input.GetAxis("move_left", "move_right");
		if (horizontalMovement != 0) {
			setDirection(horizontalMovement > 0);
			if(IsOnFloor()) {
				velocity.X += dt * horizontalMovement * speed;
			} else {
				if(horizontalMovement * speed < velocity.X || velocity.X < horizontalMovement * speed) {
					velocity.X += dt * horizontalMovement * airSpeed;
				}
			}
		}
		//Handle Dash
		if(Input.IsActionJustPressed("dash") && canDash){
			velocity.X += horizontalMovement * dashSpeed;
			velocity.Y = 0f;
			dashCoolTimer.Start(0.01f);
			canDash = false;
		}

		// Add the gravity and friction
		if (IsOnFloor()) {
			velocity.X *= 0.9f;
		} else {
			velocity.X *= 0.99f;
			if (Input.IsActionPressed("fall"))
				velocity += dt * GetGravity() * fallMulti * airPoundMulti;
			velocity += dt * GetGravity() * fallMulti;
		}
		if(wallbox.GetOverlappingBodies().Count > 0){
			if((velocity.X > 0 && rightFacing) || (velocity.X < 0 && !rightFacing)){
				velocity.X = 0;
			}
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump")) {
			if (IsOnFloor()) {
				velocity.Y = jumpVelocity;
			} else if(wallbox.GetOverlappingBodies().Count > 0) {
				float scaleX = rightFacing ? -1f : 1f;
				velocity.X += scaleX * wallJumpVelocity;
				velocity.Y = jumpVelocity;
				setDirection(!rightFacing);
			} else if(velocity.Y < 0){
				velocity.Y += jumpVelocity * 0.015f;
			}
		}

		if(IsOnFloor())
			canDash = true;
		Velocity = velocity;
		MoveAndSlide();
	}

	public void setDirection(bool right){
		float scale = right ? 1 : -1;
		Scale = new Vector2(1f, scale);
		RotationDegrees = right ? 0 : 180;
		camera.Scale = new Vector2(scale, 1f);
		rightFacing = right;
	}
	
	public void damage(float amount, Vector2 knockback = new Vector2()) {
		GD.Print("hit: ", amount);
		changeHealth(amount);
		Velocity += knockback*50;
	}

	public virtual void changeHealth(float amount) {
		health -= amount;
		if (health <= 0.0f) {
			kill();
		}
	}

	public virtual void kill() {
		GD.Print("Dead");
	}

	public void addAttackCooldown(float amount = -1f) {
		attackCooldownTimer.Start((double)amount);
		canAttack = false;
	}
	
	public void resetAttackCooldown() {
		canAttack = true;
	}
	public void resetDash(){
		Velocity = new Vector2(Velocity.X > 0 ? speed * 0.6f: -speed * 0.6f, 0);
	}
}
