using Godot;
using System;
using System.Diagnostics;

public partial class BaseFlyingEnemey : BaseEnemey
{
	

	public override void _Ready() {
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		moveToPosition(player.Position);
	}

	public virtual void getPathToNode() {
		
	}
}
