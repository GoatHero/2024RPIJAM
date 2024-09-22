using Godot;
using System;

public partial class HoldDirection : Node2D
{
	[Export]
	public float direction = 0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalRotation = direction/180*(float)Math.PI;
	}
}
