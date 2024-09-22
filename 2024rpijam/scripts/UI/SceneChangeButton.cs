using Godot;
using System;

public partial class SceneChangeButton : Button
{
	//When called by button makes a copy of original untocuhed script and uses that
	//And so you need to pass in scene info through button
	public void buttonPressed(string stg, int level) {
		GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>(stg));
		((GlobalData)(GetTree().Root.GetNode("GlobalData"))).currentLevel = level;
	}
}
