using System;
using Godot;

public partial class Narrator : BaseCharacterBodyEnemy {
	public override void _PhysicsProcess(double delta) {
		Vector2 dif = GlobalPosition - player.GlobalPosition;
		Vector2 wantedDif = dif/dif.Length()*500;
		if (Math.Abs(wantedDif.X) < Math.Abs(wantedDif.Y)) {
			(wantedDif.X, wantedDif.Y) = (Math.Abs(wantedDif.Y) * Math.Sign(wantedDif.X), Math.Abs(wantedDif.X) * Math.Sign(wantedDif.Y));
		}
		moveToPosition(player.GlobalPosition + wantedDif);
	}

    public void setDialogue(int line) {
        string lines = string.Empty;
        Timer timer = GetNode<Timer>("Timer");
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
        GetNode<Label>("AnimatedSprite2D/Label").Text = lines;
    }

    public void clearText() {
        setDialogue(-1);
    }
}
