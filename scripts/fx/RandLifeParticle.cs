using Godot;
using System;

public partial class RandLifeParticle : GpuParticles2D
{
	[Export] public float MinLifetime = 1;
	[Export] public float MaxLifetime = 5;
	
	public override void _Ready()
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		Lifetime = rng.RandfRange(MinLifetime, MaxLifetime);
		GD.Print(Lifetime);
	}
}
