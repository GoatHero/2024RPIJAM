using Godot;
using System;

public partial class LevelSelect : Control
{
	[Export]
	public int highestLevel = 1;
	
	public override void _Ready(){
		GD.Print("Start");
		base._Ready();
		foreach(Control child in GetChildren()){
			GD.Print("Child: " + child);
			if(IsInstanceValid(child) && child is BaseButton button){
				GD.Print("found");
			}
		}
		GD.Print("End");
	}
}
