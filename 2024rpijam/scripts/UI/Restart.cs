using Godot;
using System;

public partial class Restart : Button
{
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("accept")){
			OnPress();
		}
	}
	public void OnPress(){
		GlobalData g = (GlobalData)(GetTree().Root.GetNode("GlobalData"));
		string levelString = "res://scenes/levels/Level" + g.currentLevel + ".tscn";
		GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>(levelString));
	}
}
