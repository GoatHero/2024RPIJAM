using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BaseSnakeEnemy : BaseRigidBodyEnemy
{
	[Export]
	public int size = 5;
	[Export]
	public int rotationSpeed = 500;

	[Export]
	protected Area2D leftWallTrig;
	[Export]
	protected Area2D rightWallTrig;

	protected NavigationAgent2D navAgent;
	protected Array<RigidBody2D> segments = new();


	public override void _Ready() {
		base._Ready();
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		segments.Add(this);
	}

	public override bool moveToPosition(Vector2 pos) {
		float dt = (float)GetPhysicsProcessDeltaTime();

		bool l = leftWallTrig.HasOverlappingBodies();
		bool r = rightWallTrig.HasOverlappingBodies();
		if (l && r) {
			ApplyCentralImpulse(-dt*Vector2.Left.Rotated(GlobalRotation)*speed*segments.Count);
		} else if (l) {
			ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation+0.25f*(float)Math.PI)*speed*segments.Count);
		} else if (r) {
			ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation-0.25f*(float)Math.PI)*speed*segments.Count);
		} else {
			ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation)*speed*segments.Count);
		}

		Vector2 direction = pos - GlobalPosition;
		
		if (direction.Length() > 0f) {
			ApplyCentralImpulse(dt*direction/direction.Length()*speed*segments.Count*0.1f);

			float at = direction.Angle()-GlobalRotation+(float)Math.PI;
			
			while (at > Math.PI) {
				at -= (float)Math.PI*2f;
			}
			while (at < -Math.PI) {
				at += (float)Math.PI*2f;
			}
			ApplyTorqueImpulse(dt*at*rotationSpeed*segments.Count);
		}

		return direction.Length() <= 0.01;
	}

	public void makeSegments(PackedScene linkScene) {
		CallDeferred("makeSegments_", linkScene);
	}

	private void makeSegments_(PackedScene linkScene) {
		while (size > segments.Count) {
			addToSegment(segments.Last(), linkScene);
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

		setupNewSegment(newSegment);
	}

	public virtual Vector2 getPathToPos(Vector2 pos) {
		if (Engine.GetPhysicsFrames() < 2) return GlobalPosition;
		navAgent.TargetPosition = pos;
		return navAgent.GetNextPathPosition();
	}

	public virtual void setupNewSegment(RigidBody2D newSegment) {}
}
