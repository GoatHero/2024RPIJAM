using Godot;
using System;
using System.Diagnostics;

public partial interface BaseEnemy
{
	public bool moveToPosition(Vector2 pos);
	public void damage(float amount, Vector2 knockback = new Vector2());
	public void changeHealth(float amount);
	public void kill();
	public void addAttackCooldown(float amount = -1f);
	public float getHealth();
	public float getMaxHealth();
	public void resetAttackCooldown();
	public void _Ready();
}
