using Godot;
public partial class Laser : Node2D {
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        GetTree().PhysicsFrame += setUp;
    }

    public void setUp() {
        var spaceState = GetWorld2D().DirectSpaceState;
        // use global coordinates, not local to node
        var query = PhysicsRayQueryParameters2D.Create(Vector2.Zero, new Vector2(50, 100));

        query.Exclude.Add(GetNode<StaticBody2D>("StaticBody2D").GetRid());
        Godot.Collections.Dictionary result = spaceState.IntersectRay(query);

        if(result.Count != 0) {
            Vector2 pos = (Vector2)result["position"];
            pos = ToLocal(pos);
            Line2D line = GetNode<Line2D>("Line2D");
            line.Points[1] = pos;
            line.GetChild<Node2D>(0).Position = pos;
            CollisionShape2D laserCollider = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
            ((RectangleShape2D)laserCollider.Shape).Size = new Vector2(Mathf.Abs(line.Points[0].X - pos.X), ((RectangleShape2D)laserCollider.Shape).Size.Y);
            laserCollider.Position = line.Points[0] + line.Points[0] - pos;
        } else {
            GD.PrintErr("No Object for laser to Collide with");
        }
        GetTree().PhysicsFrame -= setUp;
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
    public override void _Process(double delta) {
    }
}
