using Godot;
using System;
//using Collections;

public partial class LevelSelect : Control
{
	[Export]
	public int highestLevel = 1;
	[Export]
	public int levelCount = 4;
	public Godot.Collections.Array<string> buttons = new Godot.Collections.Array<string>{
		"level1",
		"level2",
		"level3",
		"level4"
	};
	
	public override void _Ready(){
		GD.Print("Start");
		for(int i = 1; i < buttons.Count + 1; i++){
			string getter = "Level" + i;
			Button button = GetNode<Button>(getter);
			if(i > highestLevel)
				button.Disabled = true;
			else
				button.Disabled = false;
		}
		GD.Print("End");
		/*base._Ready();
		foreach(var child in GetChildren()){
			GD.Print("Child: " + child);
			if(IsInstanceValid(child) && child is BaseButton button){
				GD.Print("found");
			}
		}
		*/
	}
}
