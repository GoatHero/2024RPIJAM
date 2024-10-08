using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BaseSnakeEnemy : BaseRigidBodyEnemy
{
	[Export]
	public int size = 1;
	[Export]
	public float rotationSpeed = 4000;
	[Export]
	public float minSpeed = 0;
	[Export]
	public float size1Speed = 1000;
	[Export]
	public float size1RotationSpeed = 1000;

	protected bool isHead = true;

	public BaseSnakeEnemy parent;
	public BaseSnakeEnemy child;
	protected Area2D leftWallTrig;
	protected Area2D rightWallTrig;
	protected NavigationAgent2D navAgent;
	

	public override void _Ready() {
		base._Ready();
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		leftWallTrig = GetNode<Area2D>("LeftWallTrig");
		rightWallTrig = GetNode<Area2D>("RightWallTrig");
	}

	public override bool moveToPosition(Vector2 pos) {
		float dt = (float)GetPhysicsProcessDeltaTime();

		Vector2 direction = pos - GlobalPosition;

		bool l = leftWallTrig.HasOverlappingBodies();
		bool r = rightWallTrig.HasOverlappingBodies();
		float moveSpeed = (size == 1) ? size1Speed : Math.Max(minSpeed, speed*size);
		if (direction.Dot(Vector2.Left.Rotated(GlobalRotation)) < 0)
			moveSpeed *= 0.5f;
		if (l && r) {
			ApplyCentralImpulse(-dt*Vector2.Left.Rotated(GlobalRotation)*moveSpeed);
		} else if (l) {
			ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation+0.02f*(float)Math.PI)*moveSpeed);
		} else if (r) {
			ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation-0.02f*(float)Math.PI)*moveSpeed);
		} else {
			ApplyCentralImpulse(dt*Vector2.Left.Rotated(GlobalRotation)*moveSpeed);
		}

		
		
		if (direction.Length() > 0f) {
			ApplyCentralImpulse(dt*direction/direction.Length()*moveSpeed*0.1f);

			float at = direction.Angle()-GlobalRotation+(float)Math.PI;
			
			while (at > Math.PI) {
				at -= (float)Math.PI*2f;
			}
			while (at < -Math.PI) {
				at += (float)Math.PI*2f;
			}
			ApplyTorqueImpulse(dt*at* ((size == 1) ? size1RotationSpeed : rotationSpeed));
		}

		return direction.Length() <= 0.01;
	}

	public virtual void removeSegment() {
		child?.makeHead();
		parent?.updateSize(1);
		QueueFree();
	}

	public void makeSegments(PackedScene linkScene) {
		if (size > 1)
			CallDeferred("makeSegments_", linkScene, size);
	}

	private void makeSegments_(PackedScene linkScene, int size) {
		this.size = size;
		addSegment(linkScene);
		if (size > 2) child.makeSegments_(linkScene, size-1);
	}

	public virtual Vector2 getPathToPos(Vector2 pos) {
		if (Engine.GetPhysicsFrames() < 2) return GlobalPosition;
		navAgent.TargetPosition = pos;
		return navAgent.GetNextPathPosition();
	}

	public virtual void makeHead() {
		if (isHead) return;
		isHead = true;
	}

	public virtual void addSegment(PackedScene newSegmentPackedScene) {
		BaseSnakeEnemy newSegment = newSegmentPackedScene.Instantiate<BaseSnakeEnemy>();
		newSegment.isHead = false;

		AddSibling(newSegment);
		GetParent().MoveChild(newSegment, GetIndex());
		
		Node2D front = newSegment.GetNode<Node2D>("front");
		PinJoint2D back = GetNode<PinJoint2D>("back");
		newSegment.GlobalPosition += back.GlobalPosition - front.GlobalPosition;
	
		newSegment.parent = this;
		child = newSegment;

		back.NodeB = newSegment.GetPath();
	}

	public virtual void updateSize(int newSize) {
		size = newSize;
		parent?.updateSize(newSize+1);
	}
}
