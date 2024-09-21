using Godot;
using System;
using System.Diagnostics;

public partial class BaseFlyingEnemy : BaseCharacterBodyEnemy
{
	private NavigationAgent2D navAgent;
	
	public override void _Ready() {
		base._Ready();
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
	}

	public virtual Vector2 getPathToPos(Vector2 pos) {
		if (Engine.GetPhysicsFrames() < 2) return GlobalPosition;
		navAgent.TargetPosition = pos;
		return navAgent.GetNextPathPosition();
	}
}
