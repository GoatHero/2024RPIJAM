using Godot;
using System;

public partial class Door : Node2D
{
	public void onEnter(Node2D node2D) {
		if (node2D is Player) {
			// next level
		}
	}
}
