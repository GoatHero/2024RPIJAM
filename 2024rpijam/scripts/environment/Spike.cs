using Godot;
using System;

public partial class Spike : Node2D
{
	public void onEnter(Node2D node){
		GD.Print("Ran");
		float spikeDamage = 15;
		if(IsInstanceValid(node) && node is Player){
			(node as Player).damage(spikeDamage);
		}
	}
}
