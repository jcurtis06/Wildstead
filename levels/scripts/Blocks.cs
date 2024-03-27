using Godot;
using System;

public partial class Blocks : TileMap
{
    private BetterTerrain _bt;

    public override void _Ready()
    {
        _bt = new BetterTerrain(this);
    }

    public void SetBlock(Vector2 pos, Block block)
    {
        var mapPos = LocalToMap(pos);
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

    public bool DamageBlock(Vector2 pos)
    {
        var mapPos = LocalToMap(pos);
        
        if (_bt.GetCell(0, mapPos) == -1)
        {
            return false;
        }
        
        _bt.SetCell(0, mapPos, -1);
        _bt.UpdateTerrainCells(0, GetSurroundingCells(mapPos));

        return true;
    }
}
