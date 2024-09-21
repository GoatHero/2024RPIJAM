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


	public override void _Ready()
	{
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBabyLink.tscn");
		segments.Add(this);
		int i = 0;
		while (size < segments.Count && i < 10) {
			addToSegment(segments.Last(), linkPackedScene);
			i ++;
		}
	}

	public void addToSegment(RigidBody2D segment, PackedScene newSegmentPackedScene) {
		
		Joint2D back = segment.GetNode<Joint2D>("back");
		RigidBody2D newSegment = newSegmentPackedScene.Instantiate() as RigidBody2D;
		
		GetTree().Root.GetNode("root").AddChild(newSegment);
		Node2D front = newSegment.GetNode<Node2D>("front");
		
		newSegment.GlobalPosition += back.GlobalPosition - front.GlobalPosition;
		GD.Print("adding", newSegment.GlobalPosition);
		segments.Add(newSegment);
		GD.Print(segments);
		 back.NodeB = newSegment.GetPath();
	}
}
