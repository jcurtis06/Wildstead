using Godot;
using System;
using Wildstead.levels.objects.destructible;

public partial class Ladder : Area2D, Destructible
{
	bool _playerOnLadder = false;

	public override void _Process(double delta)
	{
		if (_playerOnLadder)
		{
			if (Input.IsActionJustPressed("use"))
			{
				var player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;

				if (player.World.Name == "Caverns")
					GoToSurface();
				else
					GoToCaverns();
			}
		}
	}

	private void HandleEnter(Node2D node)
	{
		GD.Print("Enter ladder");
		if (node is PlayerController player)
		{
			_playerOnLadder = true;
		}
	}

	private void HandleExit(Node2D node)
	{
		GD.Print("Exit ladder");
		if (node is PlayerController player)
		{
			_playerOnLadder = false;
		}
	}
	
	public void GoToCaverns()
	{
		GetTree().ChangeSceneToFile("res://levels/caverns.tscn");
		GetTree().ReloadCurrentScene();
	}
	
	public void GoToSurface()
	{
		GetTree().ChangeSceneToFile("res://levels/world.tscn");
		GetTree().ReloadCurrentScene();
	}
	
	public void Destroy(PlayerController player)
	{
		QueueFree();
	}
}
