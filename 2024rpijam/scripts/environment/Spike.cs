using Godot;
using System;

public partial class Spike : Node2D
{
	public void onEnter(Node2D node){
		float spikeDamage = 35;
		if(IsInstanceValid(node) && node is Player){
			(node as Player).damage(spikeDamage);
		}
	}
}
