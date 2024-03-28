using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class WorldBuilder : TileMap
{
	[Export] public int Width = 64;
	[Export] public int Height = 64;
	
	[Export] public Array<Biome> Biomes;
	
	private FastNoiseLite _moisture = new FastNoiseLite();
	private FastNoiseLite _temperature = new FastNoiseLite();
	private FastNoiseLite _altitude = new FastNoiseLite();

	private BetterTerrain _bt;
	
	public override void _Ready()
	{
		_bt = new BetterTerrain(this);
		
		_moisture.Seed = (int)GD.Randi();
		_temperature.Seed = (int)GD.Randi();
		_altitude.Seed = (int)GD.Randi();

		_altitude.Frequency = 0.01f;
		
		GenerateChunk(Vector2I.Zero);
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("regen_world"))
		{
			_moisture.Seed = (int)GD.Randi();
			_temperature.Seed = (int)GD.Randi();
			_altitude.Seed = (int)GD.Randi();
			
			foreach (Node child in GetChildren())
			{
				child.QueueFree();
			}
			
			GenerateChunk(Vector2I.Zero);
		}
	}

	private void GenerateChunk(Vector2I pos)
	{
		var biomesList = new List<Biome>();

		foreach (var biome in Biomes)
		{
			biomesList.Add(biome);
		}
		
		Stopwatch stopwatch = Stopwatch.StartNew();
		
		biomesList.Sort((a, b) => (a.Moisture + a.Temperature + a.Altitude).CompareTo(b.Moisture + b.Temperature + b.Altitude));

		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				var moisture = _moisture.GetNoise2D(i + pos.X, j + pos.Y);
				var temperature = _temperature.GetNoise2D(i + pos.X, j + pos.Y);
				var altitude = _altitude.GetNoise2D(i + pos.X, j + pos.Y);

				Biome closestBiome = null;
				float smallestDifference = float.MaxValue;

				foreach (var biome in biomesList)
				{
					float difference = Math.Abs(biome.Moisture - moisture)
					                   + Math.Abs(biome.Temperature - temperature)
					                   + Math.Abs(biome.Altitude - altitude);

					if (difference < smallestDifference)
					{
						smallestDifference = difference;
						closestBiome = biome;
					}
					else
					{
						break;
					}
				}

				if (closestBiome == null)
				{
					GD.Print("No biome found for: ", moisture, ", ", temperature, ", ", altitude);
					continue;
				}

				_bt.SetCell(0, new Vector2I(i, j), closestBiome.TerrainIndex);
				
				if (closestBiome.Objects == null)
				{
					continue;
				}
				
				foreach (var obj in closestBiome.Objects)
				{
					if (GD.Randf() < obj.Probability)
					{
						var instance = obj.Object.Instantiate();
						if (instance is Node2D node)
						{
							node.Position = new Vector2(i + pos.X, j + pos.Y) * 8;
							AddChild(node);
						}
					}
				}
			}
		}

		_bt.UpdateTerrainArea(0, new Rect2I(pos, new Vector2I(Width, Height)));
		stopwatch.Stop();
		GD.Print("Generating world took ", stopwatch.ElapsedMilliseconds, "ms");
	}
}
