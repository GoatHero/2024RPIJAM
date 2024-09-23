using Godot;
using System;

public partial class GlobalData : Node2D
{
	public int highestLevel = 5;
	public int currentLevel = 3;
	
	public void completedLevel(){
		if (highestLevel <= currentLevel)
			highestLevel = currentLevel + 1;
	}
}
