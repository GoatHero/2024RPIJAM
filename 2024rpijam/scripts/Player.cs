using Godot;
using System.Collections.Generic;

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
    private bool canBeHit = true;

    // private Area2D hitBox;
    private Timer iFrameTimer;
    private Area2D attackBox;
    private Area2D wallBoxL;
    private Area2D wallBoxR;
    private Timer attackCooldownTimer;
    private Timer dashCoolTimer;
    private Camera2D camera;
    private AnimationNodeStateMachinePlayback animationPlayer;

    public override void _Ready() {
        base._Ready();
        // hitBox = GetNode<Area2D>("hitBox");
        iFrameTimer = GetNode<Timer>("iFrameTimer");
        attackBox = GetNode<Area2D>("attackBox");
        wallBoxL = GetNode<Area2D>("wallBoxL");
        wallBoxR = GetNode<Area2D>("wallBoxR");
        attackCooldownTimer = GetNode<Timer>("hitTimer");
        dashCoolTimer = GetNode<Timer>("dashCoolTimer");
        camera = GetNode<Camera2D>("camera");
        animationPlayer = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
    }

    Dictionary<string, int> animationPriority = new()
    {
        { "RESET", 0 },
        { "Attack", 100 },
        { "Wall_Kick", 80 },
        { "Run", 30 },
        { "Jump", 50 },
        { "Dash", 70 },
    };

    public override void _PhysicsProcess(double delta) {
        float dt = (float)delta;
        Vector2 velocity = Velocity;
        string wantedAnimation = "RESET";

        //Handle Attack 
        if(Input.IsActionJustPressed("attack") && canAttack) {
            /* ---- animation ---- */
            if(animationPriority["Attack"] > animationPriority[wantedAnimation])
                wantedAnimation = "Attack";
            /* ------------------- */

        }


        bool isPressingHorizontalKey = false;
        // Get the input direction and handle the movement/deceleration.
        float horizontalMovement = Input.GetAxis("move_left", "move_right");
        if(horizontalMovement != 0) {
            isPressingHorizontalKey = true;
            if(animationPlayer.GetCurrentNode() != "Attack")
                setDirection(horizontalMovement > 0);
            /* ---- animation ---- */
            if(animationPriority["Run"] > animationPriority[wantedAnimation])
                wantedAnimation = "Run";
            /* ------------------- */
            if(IsOnFloor()) {
                velocity.X += dt * horizontalMovement * speed;
            } else {
                if(horizontalMovement * speed < velocity.X || velocity.X < horizontalMovement * speed) {
                    velocity.X += dt * horizontalMovement * airSpeed;
                }
            }
        }

        //Handle Dash
        if(((GlobalData)(GetTree().Root.GetNode("GlobalData"))).currentLevel != 4) {
            if(Input.IsActionJustPressed("dash") && canDash) {
                /* ---- animation ---- */
                if(animationPriority["Dash"] > animationPriority[wantedAnimation])
                    wantedAnimation = "Dash";
                /* ------------------- */
                velocity.X += rightFacing ? dashSpeed : -dashSpeed;
                dashCoolTimer.Start(0.01f);
                canDash = false;
                addIFrames(0.5f);
            }
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
                /* ---- animation ---- */
                if(animationPriority["Jump"] > animationPriority[wantedAnimation])
                    wantedAnimation = "Jump";
                /* ------------------- */
                velocity.Y = jumpVelocity;
            } else if(wallBoxL.HasOverlappingBodies()) {
                /* ---- animation ---- */
                if(animationPriority["Wall_Kick"] > animationPriority[wantedAnimation])
                    wantedAnimation = "Wall_Kick";
                /* ------------------- */
                velocity.X = wallJumpVelocity;
                velocity.Y = jumpVelocity;
                if(!isPressingHorizontalKey)
                    setDirection(true);
            } else if(wallBoxR.HasOverlappingBodies()) {
                /* ---- animation ---- */
                if(animationPriority["Wall_Kick"] > animationPriority[wantedAnimation])
                    wantedAnimation = "Wall_Kick";
                /* ------------------- */
                velocity.X = -wallJumpVelocity;
                velocity.Y = jumpVelocity;
                if(!isPressingHorizontalKey)
                    setDirection(false);
            } else if(velocity.Y < 0) {
                /* ---- animation ---- */
                if(animationPriority["Jump"] > animationPriority[wantedAnimation])
                    wantedAnimation = "Jump";
                /* ------------------- */
                velocity.Y += jumpVelocity * 0.015f;
            }
        }

        if(IsOnFloor())
            canDash = true;

        animationPlayer.Travel(wantedAnimation);

        Velocity = velocity;
        MoveAndSlide();
    }

	public void setDirection(bool right) {
		float scale = right ? 1 : -1;
		Scale = new Vector2(1f, scale);
		RotationDegrees = right ? 0 : 180;
		camera.Scale = new Vector2(scale, 1f);
		if (right != (wallBoxL.Position.X < wallBoxR.Position.X))
			(wallBoxL.Position, wallBoxR.Position) = (wallBoxR.Position, wallBoxL.Position);
		rightFacing = right;
	}
	
	public void attack() {
		foreach(Node2D node in attackBox.GetOverlappingBodies()) {
			if (IsInstanceValid(node) && node is BaseEnemy) {
				Vector2 dif = node.GlobalPosition - GlobalPosition;
				(node as BaseEnemy).damage(attackDamage, dif/dif.Length()*6);
			}
		}
		addAttackCooldown();
	}

    public void damage(float amount, Vector2 knockback = new Vector2()) {
        if(!canBeHit)
            return;
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
        GD.Print("death");
        GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>("res://scenes/UI/DeathScreen.tscn"));
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

    public void addIFrames(float timeSec = -1) {
        iFrameTimer.Start(timeSec);
        canBeHit = false;
    }

    public void loseIFrames() {
        canBeHit = true;
    }
}
