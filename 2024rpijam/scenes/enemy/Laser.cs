using Godot;
public partial class Laser : Node2D {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    public void setUp() {

        Vector2 pos = GetNode<RayCast2D>("RayCast2D").GetCollisionPoint();

        GD.Print(pos);
        pos = ToLocal(pos);
        GD.Print(pos);
        Line2D line = GetNode<Line2D>("Line2D");
        GD.Print(line.Points[0]);
        line.SetPointPosition(0, pos);
        line.GetNode<GpuParticles2D>("GPUParticles2D").Position = pos;
        CollisionShape2D laserCollider = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
        ((RectangleShape2D)laserCollider.Shape).Size = new Vector2(Mathf.Abs(line.Points[1].X - pos.X), ((RectangleShape2D)laserCollider.Shape).Size.Y);
        laserCollider.Position = (line.Points[0] + line.Points[1]) / 2;

    }

    public void hit(Node2D node) {
        float laserDamage = 10;
        if(node is Player player) {
            player.damage(laserDamage);
        } else if(node is BaseCharacterBodyEnemy characterBodyEnemy) {
            characterBodyEnemy.damage(laserDamage);
        } else if(node is BaseEnemy baseEnemy) {
            baseEnemy.damage(laserDamage);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta) {
        setUp();
    }
}
