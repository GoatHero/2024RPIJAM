using Godot;

public partial class TestFlyingEnemy : BaseFlyingEnemy {
    [Export]
    public float attackDamage = 10;
    [Export]
    public float attackKnockback = 10;
    [Export]
    public float attackRange = 30;
	[Export]
	public float detecionDis = 800;

    public override void _Ready() {
        base._Ready();
        GetNode<AnimatedSprite2D>("Icon/Icon").Play();
    }

    public override void _PhysicsProcess(double delta) {
		if ((player.GlobalPosition - GlobalPosition).Length() < detecionDis) {
			base._PhysicsProcess(delta);
			Vector2 nextPos = getPathToPos(player.GlobalPosition);
			GetNode<Node2D>("Icon").LookAt(nextPos);
			moveToPosition(nextPos);

			if (canAttack && (player.GlobalPosition - GlobalPosition).Length() < attackRange) {
				attack(player);
				addAttackCooldown();
			}
		}
    }

    public virtual void attack(Player player) {
        Vector2 dif = player.GlobalPosition - GlobalPosition;
        dif /= dif.Length();
        player.damage(attackDamage, dif * attackKnockback);
    }
}
