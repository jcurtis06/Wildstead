using Godot;
using System;

public partial class MovableCamera : Camera2D
{
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_right"))
		{
			Position = Position with { X = Position.X + 1 };
		}
		
		if (Input.IsActionPressed("ui_left"))
		{
			Position = Position with { X = Position.X - 1 };
		}
		
		if (Input.IsActionPressed("ui_down"))
		{
			Position = Position with { Y = Position.Y + 1 };
		}
		
		if (Input.IsActionPressed("ui_up"))
		{
			Position = Position with { Y = Position.Y - 1 };
		}
	}
}
