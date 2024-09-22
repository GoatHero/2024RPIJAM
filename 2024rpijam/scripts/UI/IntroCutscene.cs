using Godot;

public partial class IntroCutscene : Node2D {

    public override void _Ready() {
        base._Ready();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("cutscene");
    }
    public void onCutsceneEnd(string junk) {
        GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>("res://scenes/UI/LevelSelect.tscn"));
    }
}
