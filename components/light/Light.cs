using Godot;
using System;

public partial class Light : Node2D
{
	[Export] public float Strength { get; set; } = 1.0f;
	[Export] public float Radius { get; set; } = 100.0f;
}
