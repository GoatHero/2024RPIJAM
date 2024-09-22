using Godot;
using System;

public partial class SceneChangeButton : Control
{
	//When called by button makes a copy of original untocuhed script and uses that
	//And so you need to pass in scene info through button
	public void ButtonPressed(string stg){
		GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>(stg));
	}
}
