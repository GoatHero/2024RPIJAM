using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float fallMulti = 1.5f;
	[Export]
	public float speed = 2700.0f;
	[Export]
	public float airSpeed = 800.0f;
	[Export]
	public float jumpVelocity = -500.0f;
	[Export]
	public float wallJumpVelocity = 400.0f;
	[Export]
	public  float airPoundMulti = 3f;
	

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;

		Vector2 velocity = Velocity;

		// Handle Jump.
		if (Input.IsActionPressed("jump")) {
			if (IsOnFloor()) {
				velocity.Y = jumpVelocity;
			} else if(IsOnWall()) {
				Vector2 wallNormal = GetWallNormal();
				if(wallNormal == Vector2.Right || wallNormal == Vector2.Left){
					velocity.X = wallNormal.X * wallJumpVelocity;
					velocity.Y = jumpVelocity; 
				}
			} else if(velocity.Y < 0){
				velocity.Y += jumpVelocity * 0.015f;
			}
		}
		
		// Get the input direction and handle the movement/deceleration.
		float horizontalMovement = Input.GetAxis("move_left", "move_right");
		if (horizontalMovement != 0) {
			if(IsOnFloor()) {
					velocity.X += dt * horizontalMovement * speed;
			} else {
				if(horizontalMovement * speed < velocity.X || velocity.X < horizontalMovement * speed) {
					velocity.X += dt * horizontalMovement * airSpeed;
				}
			}
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

		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void damage(float amount, Vector2 knockback = new Vector2()) {
		
	}
}
