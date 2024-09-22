using Godot;

public partial class Player : CharacterBody2D {

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
	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;
		Vector2 velocity = Velocity;
		
		//Handle Attack 
		if (Input.IsActionJustPressed("attack") && canAttack) {
			/* only attack closest */
			// Node2D closest = null;
			// foreach(Node2D node in hitBox.GetOverlappingBodies()) {
			// 	if (IsInstanceValid(node) && node is BaseEnemy) {
			// 		if (
			// 			closest == null ||
			// 			(closest.GlobalPosition - GlobalPosition) > (node.GlobalPosition - GlobalPosition)
			// 		) {
			// 			closest = node;
			// 		}
			// 	}
			// }
			// if (closest != null)
			// 	(closest as BaseEnemy).damage(attackDamage);

			/* attack all in range */
			foreach(Node2D node in hitBox.GetOverlappingBodies()) {
				if (IsInstanceValid(node) && node is BaseEnemy) {
					(node as BaseEnemy).damage(attackDamage);
				}
			}
			addAttackCooldown();
		}
		
		
		bool isPressingHorizontalKey = false;
		// Get the input direction and handle the movement/deceleration.
		float horizontalMovement = Input.GetAxis("move_left", "move_right");
		if (horizontalMovement != 0) {
			isPressingHorizontalKey = true;
			setDirection(horizontalMovement > 0);
			if (!animationPlayer.IsPlaying() || animationPlayer.CurrentAnimation != "Run")
				animationPlayer.Play("Run");
			if (IsOnFloor()) {
				velocity.X += dt * horizontalMovement * speed;
			} else {
				if (horizontalMovement * speed < velocity.X || velocity.X < horizontalMovement * speed) {
					velocity.X += dt * horizontalMovement * airSpeed;
				}
			}
		} else if (animationPlayer.CurrentAnimation == "Run") {
			animationPlayer.Play("Reset");
		}


    private Area2D hitBox;
    private Area2D wallBoxL;
    private Area2D wallBoxR;
    private Timer attackCooldownTimer;
    private Timer dashCoolTimer;
    private Camera2D camera;
    private AnimationNodeStateMachinePlayback animationPlayer;

    public override void _Ready() {
        base._Ready();
        hitBox = GetNode<Area2D>("hitBox");
        wallBoxL = GetNode<Area2D>("wallBoxL");
        wallBoxR = GetNode<Area2D>("wallBoxR");
        attackCooldownTimer = GetNode<Timer>("hitTimer");
        dashCoolTimer = GetNode<Timer>("dashCoolTimer");
        camera = GetNode<Camera2D>("camera");
        animationPlayer = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationPlayer").Get("parameters/playback");

    }

    public override void _PhysicsProcess(double delta) {
        float dt = (float)delta;
        Vector2 velocity = Velocity;
	public void setDirection(bool right) {
		float scale = right ? 1 : -1;
		Scale = new Vector2(1f, scale);
		RotationDegrees = right ? 0 : 180;
		camera.Scale = new Vector2(scale, 1f);
		if (right != (wallBoxL.Position.X < wallBoxR.Position.X))
			(wallBoxL.Position, wallBoxR.Position) = (wallBoxR.Position, wallBoxL.Position);
		rightFacing = right;
	}
	
	public void damage(float amount, Vector2 knockback = new Vector2()) {
		changeHealth(amount);
		Velocity += knockback*50;
	}

        //Handle Attack 
        if(Input.IsActionJustPressed("attack") && canAttack) {
            Node2D closest = null;
            foreach(Node2D node in hitBox.GetOverlappingBodies()) {
                if(IsInstanceValid(node) && node is BaseEnemy) {
                    if(
                        closest == null ||
                        (closest.GlobalPosition - GlobalPosition) > (node.GlobalPosition - GlobalPosition)
                    ) {
                        closest = node;
                    }
                }
            }
            if(closest != null)
                (closest as BaseEnemy).damage(attackDamage);
            addAttackCooldown();
        }


        bool isPressingHorizontalKey = false;
        // Get the input direction and handle the movement/deceleration.
        float horizontalMovement = Input.GetAxis("move_left", "move_right");
        if(horizontalMovement != 0) {
            isPressingHorizontalKey = true;
            setDirection(horizontalMovement > 0);
            animationPlayer.Travel("Run");
            if(IsOnFloor()) {
                velocity.X += dt * horizontalMovement * speed;
            } else {
                if(horizontalMovement * speed < velocity.X || velocity.X < horizontalMovement * speed) {
                    velocity.X += dt * horizontalMovement * airSpeed;
                }
            }
        }

        //Handle Dash
        if(Input.IsActionJustPressed("dash") && canDash) {
            velocity.X += rightFacing ? dashSpeed : -dashSpeed;
            dashCoolTimer.Start(0.01f);
            canDash = false;
        }

        // Add the gravity and friction
        if(IsOnFloor()) {
            velocity.X *= 0.9f;
        } else {
            velocity.X *= 0.99f;
            if(Input.IsActionPressed("fall"))
                velocity += dt * GetGravity() * fallMulti * airPoundMulti;
            velocity += dt * GetGravity() * fallMulti;
        }
        if(wallBoxR.HasOverlappingBodies()) {
            if(velocity.X > 0) {
                velocity.X = 0;
            }
        }
        if(wallBoxL.HasOverlappingBodies()) {
            if(velocity.X < 0) {
                velocity.X = 0;
            }
        }

        // Handle Jump.
        if(Input.IsActionJustPressed("jump")) {
            if(IsOnFloor()) {
                animationPlayer.Travel("Jump");
                velocity.Y = jumpVelocity;
            } else if(wallBoxL.HasOverlappingBodies()) {
                velocity.X = wallJumpVelocity;
                velocity.Y = jumpVelocity;
                if(!isPressingHorizontalKey)
                    setDirection(true);
            } else if(wallBoxR.HasOverlappingBodies()) {
                velocity.X = -wallJumpVelocity;
                velocity.Y = jumpVelocity;
                if(!isPressingHorizontalKey)
                    setDirection(false);
            } else if(velocity.Y < 0) {
                velocity.Y += jumpVelocity * 0.015f;
            }
        }

        if(IsOnFloor())
            canDash = true;

        Velocity = velocity;
        MoveAndSlide();
    }

    public void setDirection(bool right) {
        float scale = right ? 1 : -1;
        Scale = new Vector2(1f, scale);
        RotationDegrees = right ? 0 : 180;
        camera.Scale = new Vector2(scale, 1f);
        if(right != (wallBoxL.Position.X < wallBoxR.Position.X))
            (wallBoxL.Position, wallBoxR.Position) = (wallBoxR.Position, wallBoxL.Position);
        rightFacing = right;
    }

    public void damage(float amount, Vector2 knockback = new Vector2()) {
        GD.Print("hit: ", amount);
        changeHealth(amount);
        Velocity += knockback * 50;
    }

    public virtual void changeHealth(float amount) {
        health -= amount;
        if(health <= 0.0f) {
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
    public void resetDash() {
        Velocity = new Vector2(Velocity.X > 0 ? speed * 0.6f : -speed * 0.6f, 0);
    }
}
