using Godot;
using System;

public partial class HealthBar : Sprite2D
{
	[Export]
	protected Player player;
	[Export]
	protected Node2D enemy;

	[Export]
	protected bool hideWhenFull = false;

	protected BaseEnemy enemy_;

	public override void _Ready()
	{
		if (enemy != null) {
			if (enemy is BaseEnemy) {
				enemy_ = enemy as BaseEnemy;
			} else {
				enemy = null;
			}
		}
		if (player == null && enemy == null) {
			try {
				player = GetParent<Player>();
			} catch {
			}
			try {
				enemy = GetParent<Node2D>();
				if (enemy is BaseEnemy) {
					enemy_ = enemy as BaseEnemy;
				} else {
					enemy = null;
				}
			} catch {
			}
		}
	}

	public override void _Process(double delta)
	{
		if (hideWhenFull && getHealth() == getMaxHealth())
			Visible = false;
		else {
			Visible = true;
			Frame = Math.Clamp((int)(getHealth() / getMaxHealth() * 12f), 0, Hframes*Vframes-1);
		}
		
	}

	public virtual float getHealth() {
		if (player != null)
			return player.health;
		if (enemy != null)
			return enemy_.getHealth();
		return 0f;
	}

	public virtual float getMaxHealth() {
		if (player != null)
			return player.maxHealth;
		if (enemy != null)
			return enemy_.getMaxHealth();
		return 0f;
	}
}
