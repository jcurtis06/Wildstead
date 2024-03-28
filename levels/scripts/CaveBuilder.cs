using Godot;
using System;
using Wildstead.data.item.scripts;

public partial class CaveBuilder : TileMap
{
    [Export] public int Width = 64;
    [Export] public int Height = 64;
    
    [Export] public Blocks Blocks;
    [Export] public Block Stone;
    
    private FastNoiseLite _caveNoise = new();
    private BetterTerrain _bt;
    
    public override void _Ready()
    {
        _bt = new BetterTerrain(this);
        
        _caveNoise.Seed = (int)GD.Randi();
        _caveNoise.Frequency = 0.1f;
        
        GenerateCave(Vector2I.Zero);
    }
    
    private void GenerateCave(Vector2I pos)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _bt.SetCell(0, new Vector2I(x, y), 0);
                var noise = _caveNoise.GetNoise2D(x + pos.X, y + pos.Y);
                if (noise > 0.01f)
                {
                    Blocks.SetBlock(new Vector2(x*8, y*8), Stone);
                }
            }
        }
    }
}
