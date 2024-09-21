using Godot;
using System;

public partial class HealthBar : Sprite2D
{
	protected Player player;
	protected BaseEnemy enemy;

	public override void _Ready()
	{
		try {
			player = GetParent<Player>();
		} catch {
			player = null;
		}
		try {
			enemy = GetParent<BaseEnemy>();
		} catch {
			enemy = null;
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player != null) {
			Frame = (int)(player.health / player.maxHealth * 12);
		} else if (enemy != null) {
			Frame = (int)(enemy.health / enemy.maxHealth * 12);
		}
	}
}
