using Godot;
using System;

public partial class Spike : Node2D
{
	public void OnEnter(){
		Area2D hitBox = GetNode<Area2D>("hitbox");
		GD.Print(hitBox);
		foreach(Node2D area in hitBox.GetOverlappingBodies()){
			if(IsInstanceValid(area) && area is Player){
				(area as Player).damage(15);
			}
		}
	}
}
