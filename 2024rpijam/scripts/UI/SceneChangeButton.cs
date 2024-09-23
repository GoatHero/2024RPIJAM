using Godot;
using System;

public partial class SceneChangeButton : Button
{
	[Export]
	public string scenePath = "";
	[Export]
	public int level = -1;

	public override void _EnterTree() => Pressed += OnButtonPressed;
    public override void _ExitTree() => Pressed -= OnButtonPressed;
    private void OnButtonPressed() {
		GetTree().ChangeSceneToFile(scenePath);
		if(level > -1)
			GetTree().Root.GetNode<GlobalData>("GlobalData").currentLevel = level;
	}
}
