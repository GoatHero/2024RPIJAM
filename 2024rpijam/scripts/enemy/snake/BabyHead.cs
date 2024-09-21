using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BabyHead : RigidBody2D
{
	[Export]
	public int size = 5;

	private Array<RigidBody2D> segments = new();
	private PackedScene linkPackedScene;


	public override void _Ready() {
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBabyLink.tscn");
		segments.Add(this);
		CallDeferred("makeSegments");
	}

	public void makeSegments() {
		while (size > segments.Count) {
			addToSegment(segments.Last(), linkPackedScene);
		}
	}

	public void addToSegment(RigidBody2D segment, PackedScene newSegmentPackedScene) {
		RigidBody2D newSegment = newSegmentPackedScene.Instantiate<RigidBody2D>();
		segments.Add(newSegment);
		
		segment.AddSibling(newSegment);

		Node2D front = newSegment.GetNode<Node2D>("front");
		PinJoint2D back = segment.GetNode<PinJoint2D>("back");
		newSegment.GlobalPosition += back.GlobalPosition - front.GlobalPosition;
		
		back.NodeB = newSegment.GetPath();
	}
}
