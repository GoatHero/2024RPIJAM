using Godot;
using System;

public partial class HealthBar : Sprite2D
{
	[Export]
	protected Player player;
	[Export]
	protected BaseEnemy enemy;

	public override void _Ready()
	{
		if (player == null && enemy == null) {
			try {
				player = GetParent<Player>();
			} catch {
			}
			try {
				enemy = GetParent<BaseEnemy>();
			} catch {
			}
		}
	}

	public override void _Process(double delta)
	{
		if (player != null) {
			Frame = (int)(player.health / player.maxHealth * 12);
		} else if (enemy != null) {
			Frame = (int)(enemy.health / enemy.maxHealth * 12);
		}
	}
}
