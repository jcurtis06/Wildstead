using Godot;
using System;
using Godot.Collections;

public partial class Blocks : TileMap
{
    private BetterTerrain _bt;
    private int count = 0;

    private Dictionary<Vector2I, Block> _blocks = new();
    
    public override void _Ready()
    {
        _bt = new BetterTerrain(this);
    }

    public void SetBlock(Vector2 pos, Block block)
    {
        count += 1;
        GD.Print("setting block ", count);
        if (_bt == null)
        {
            _bt = new BetterTerrain(this);
        }
        
        var mapPos = LocalToMap(pos);
        _blocks[mapPos] = block.Duplicate() as Block;
        _bt.SetCell(0, mapPos, block.TerrainIndex);
        _bt.UpdateTerrainCells(0, GetSurroundingCells(mapPos));
    }

    public Vector2 GetClosestBlock(Vector2 pos)
    {
        var mapPos = LocalToMap(pos);
        
        var closest = Vector2I.Zero;
        
        foreach (Vector2I cell in GetUsedCells(0))
        {
            if (closest == Vector2.Zero || (cell - mapPos).Length() < (closest - mapPos).Length())
            {
                closest = cell;
            }
        }
        
        return MapToLocal(closest);
    }

    public Block? DamageBlock(Vector2 pos, Tool tool)
    {
        var mapPos = LocalToMap(pos);
        
        if (_bt.GetCell(0, mapPos) == -1)
        {
            return null;
        }
        
        if (!_blocks.ContainsKey(mapPos))
        {
            return null;
        }
        
        var block = _blocks[mapPos];
        var broke = block.Damage(tool);

        if (broke)
        {
            _bt.SetCell(0, mapPos, -1);
            _bt.UpdateTerrainCells(0, GetSurroundingCells(mapPos));
            _blocks.Remove(mapPos);
            return block;
        }

        return null;
    }
}
