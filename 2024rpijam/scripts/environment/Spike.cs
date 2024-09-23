using Godot;
using System;

public partial class Spike : Node2D
{
	[Export]
	float spikeDamage = 15;
	public void onEnter(Node2D node){
		if(IsInstanceValid(node) && node is Player){
			(node as Player).damage(spikeDamage);
		}
	}
}
