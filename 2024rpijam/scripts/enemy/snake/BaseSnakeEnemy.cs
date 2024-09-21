using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BaseSnakeEnemy : BaseRigidBodyEnemy
{
	[Export]
	public int size = 5;
	[Export]
	public int rotationSpeed = 50;

	private NavigationAgent2D navAgent;
	private Array<RigidBody2D> segments = new();
	private PackedScene linkPackedScene;


	public override void _Ready() {
		base._Ready();
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		linkPackedScene = GD.Load<PackedScene>("res://scenes/enemy/snake/SnakeBabyLink.tscn");
		segments.Add(this);
		CallDeferred("makeSegments");
	}

	public override bool moveToPosition(Vector2 pos) {
		float dt = (float)GetPhysicsProcessDeltaTime();

		ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation)*speed*segments.Count);

		Vector2 direction = pos - GlobalPosition;
		
		if (direction.Length() > 0f) {
			float at = direction.Angle()-GlobalRotation+(float)Math.PI;
			
			while (at > Math.PI) {
				at -= (float)Math.PI*2f;
			}
			while (at < -Math.PI) {
				at += (float)Math.PI*2f;
			}
			ApplyTorqueImpulse(dt*at*rotationSpeed);
		}

		return direction.Length() <= 0.01;
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

	public virtual Vector2 getPathToPos(Vector2 pos) {
		if (Engine.GetPhysicsFrames() < 2) return GlobalPosition;
		navAgent.TargetPosition = pos;
		return navAgent.GetNextPathPosition();
	}
}
