using Godot;
using System;

public partial class SceneChangeButton : Button
{
	public void onButtonPressed(string stg, int lvl) {
		GetTree().ChangeSceneToFile(stg);
		if(lvl > -1){
			GlobalData g = (GlobalData)(GetTree().Root.GetNode("GlobalData"));
			g.currentLevel = lvl;
		}
	}
}
