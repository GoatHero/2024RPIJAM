using System;
using Godot.Collections;
using Godot;

public partial class Narrator : BaseCharacterBodyEnemy {
	[Export]
	public Array<Area2D> areas { get; set; } = new();
	[Export]
	public Array<string> lines { get; set; } = new();
	[Export]
	public Array<float> times { get; set; } = new();

	[Export]
	public bool isStatic = false; 
	public AnimatedSprite2D animatedSprite2D;
	public Timer lineTimer;

	public override void _Ready() {
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Stop();
		lineTimer = GetNode<Timer>("Timer");
		for (int i = 0; i < areas.Count; i++)
		{
			areas[i].BodyEntered += (Node2D node) => {
				if (node is Player)
					say(lines[i], times[i]);
			};
		}
	}

	public override void _PhysicsProcess(double delta) {
		if (!isStatic) {
			Vector2 dif = GlobalPosition - player.GlobalPosition;
			Vector2 wantedDif = dif/dif.Length()*500;
			if (Math.Abs(wantedDif.X) < Math.Abs(wantedDif.Y)) {
				(wantedDif.X, wantedDif.Y) = (Math.Abs(wantedDif.Y) * Math.Sign(wantedDif.X), Math.Abs(wantedDif.X) * Math.Sign(wantedDif.Y));
			}
			moveToPosition(player.GlobalPosition + wantedDif);
		}
		if (!animatedSprite2D.IsPlaying()) {
			animatedSprite2D.Play();
		}
	}

	public void setDialogue(int line) {
		string lines = string.Empty;
		switch(line) {
			case 0:
				break;
			case 1:
				lines = "You barely interacted with it kill every enemy this time";
				break;
			case 2:
				lines = "You violent brute your really did it\n you really killed everything\n you cant be trusted with a weapon you monster";
				break;
			case 3:
				lines = "I think I gave you to much\n this game is meant for a normal person not someone freaky like you";
				break;
			case 4:
				break;
			case 5:
				lines = "NO NO NO\n I WILL NOY LET YOU LEAVE\n";
				break;
			case 6:
				lines = "Ouch";
				timer.Start();
				break;
			case 7:
				lines = "KILL THEM";
				timer.Start();
				break;
			case 8:
				lines = "HAHAHAHA";
				timer.Start();
				break;
			default:
				lines = string.Empty;
				break;
		}
		say(lines);
	}


	public void say(string str, float time = -1) {
		GetNode<Label>("AnimatedSprite2D/Label").Text = str;
		lineTimer.Start(time);
		animatedSprite2D.Play();
	}

    public void clearText() {
        setDialogue(-1);
		animatedSprite2D.Stop();
    }
}
