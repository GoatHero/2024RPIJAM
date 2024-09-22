using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BaseSnakeLink : BaseRigidBodyEnemy
{
	public BaseSnakeEnemy head;

	public virtual void makeHead(PackedScene newHeadPackedScene) {
		BaseSnakeEnemy newHead = newHeadPackedScene.Instantiate<BaseSnakeEnemy>();
		AddSibling(newHead);

		PinJoint2D back = newHead.GetNode<PinJoint2D>("back");
		Node2D front = GetNode<Node2D>("front");	
		newHead.GlobalPosition += front.GlobalPosition - back.GlobalPosition;

		back.NodeB = GetPath();

		newHead.health = health;
		newHead.maxHealth = maxHealth;
		newHead.scanForSegments();

		head = null;
	}

	public virtual BaseSnakeLink getBackNode() {
		PinJoint2D back = GetNode<PinJoint2D>("back");
		if (back.NodeB == null) return null;
	
		Node node = GetNodeOrNull(back.NodeB);
		if (node is not BaseSnakeLink) return null;
		
		return node as BaseSnakeLink;
	}
}
