using Godot;
using System;

public partial class ExitButton : Button
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_cancel")){
			GetTree().Quit();
		}
	}
	public void buttonPressed(){
		GetTree().Quit();
	}
}
