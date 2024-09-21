using Godot;
using System;

public partial class HealthNumber : RichTextLabel
{
	[Export]
	protected Player player;
	[Export]
	protected BaseEnemy enemy;

	[Export]
	private string beforeText;

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
			Text = beforeText+((int)(player.health / player.maxHealth*100f)).ToString() + "%";
		} else if (enemy != null) {
			Text = beforeText+((int)(enemy.health / enemy.maxHealth*100f)).ToString() + "%";
		}
	}
}
