using Godot;
using System;

public partial class HealthBar : Sprite2D
{
	[Export]
	protected Player player;
	[Export]
	protected Node2D enemy;
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
		if (player != null) {
			Frame = (int)(player.health / player.maxHealth * 12);
		} else if (enemy != null) {
			Frame = (int)(enemy_.getHealth() / enemy_.getMaxHealth() * 12);
		}
	}
}
