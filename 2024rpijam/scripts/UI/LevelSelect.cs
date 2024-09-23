using Godot;
using System;
//using Collections;

public partial class LevelSelect : Control
{
	[Export]
	public int levelCount = 5;
	
	public override void _Ready(){
		GlobalData g = (GlobalData)(GetTree().Root.GetNode("GlobalData"));
		for(int i = 1; i < levelCount + 1; i++){
			string getter = "Level" + i;
			Button button = GetNode<Button>(getter);
			//if(i > g.highestLevel)
				//button.Disabled = true;
			//else
				button.Disabled = false;
		}
	}
}
