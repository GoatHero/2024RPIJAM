using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SnakeBossLink : BaseSnakeLink
{
	private PackedScene headPackedScene;

	public override void _Ready() {
		base._Ready();
		headPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBoss.tscn");
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
	}

	public override void kill() {
		head?.removeSegment(this);
		
		BaseSnakeLink baseSnakeLink = getBackNode();
		baseSnakeLink?.makeHead(headPackedScene);
		
		QueueFree();
	}
}
