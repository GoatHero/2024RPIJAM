using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float fallMulti = 1.5f;
	public const float airMoveMulti = 3f;
	public const float speed = 300.0f;
	public const float jumpVelocity = -400.0f;
	

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor()) {
			if (Input.IsActionPressed("fall"))
				velocity += GetGravity() * (float)delta * fallMulti * 3f;
			velocity += GetGravity() * (float)delta * fallMulti;
			
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			velocity.Y = jumpVelocity;
		} else if(Input.IsActionPressed("jump") && velocity.Y < 0){
			velocity.Y += jumpVelocity * 0.015f;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "fall");
		if(IsOnFloor()) {
			if (direction != Vector2.Zero) {
				velocity.X = direction.X * speed;
			} else {
				velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			}
		} else {
			if(Mathf.Abs(velocity.X) < direction.X * speed) {
				velocity.X += direction.X * speed * (float)delta * airMoveMulti;
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	
	public void damage(float amount, Vector2 knockback = new Vector2()) {
		
	}
}
