using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] public float RandomStrength = 30.0f;
	[Export] public float ShakeFade = 5.0f;

	private RandomNumberGenerator _rng = new();
	private float _shakeStrength = 0.0f;

	public override void _Ready()
	{
		Globals.Camera = this;
	}

	public void ApplyShake()
	{
		_shakeStrength = RandomStrength;
	}

	public override void _Process(double delta)
	{
		if (_shakeStrength > 0)
		{
			_shakeStrength = Mathf.Lerp(_shakeStrength, 0, ShakeFade * (float)delta);
			Offset = RandomOffset();
		}
		
		GD.Print(GlobalPosition);
	}

	private Vector2 RandomOffset()
	{
		return new Vector2(
			_rng.RandfRange(-_shakeStrength, _shakeStrength),
			_rng.RandfRange(-_shakeStrength, _shakeStrength)
		);
	}
}
