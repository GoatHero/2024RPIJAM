using Godot;
using System;

public partial class Area2d2 : Area2D
{
	// for end boss fight
	public override void _Process(double delta)
	{
		if (!HasOverlappingBodies()) {
			// GlobalData g = (GlobalData)(GetTree().Root.GetNode("GlobalData"));
			// g.completedLevel();
			GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>("res://scenes/UI/LevelSelect.tscn"));
		}
	}
}
