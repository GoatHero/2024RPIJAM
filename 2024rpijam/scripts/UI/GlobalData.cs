using Godot;
using System;

public partial class GlobalData : Node2D
{
	public int highestLevel = 1;
	public int currentLevel = 1;
	
	public void completedLevel(){
		if (highestLevel <= currentLevel)
			highestLevel = currentLevel + 1;
	}
}
