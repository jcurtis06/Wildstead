using Godot;
using System;
using Wildstead.data.item.scripts;

[GlobalClass]
public partial class Block : Item
{
    [Export] public ItemMaterial Material;
    [Export] public int TerrainIndex;
    [Export] public float Hardness = 10;

    public bool Damage(Tool tool)
    {
        if (tool.Strength >= Material)
        {
            Hardness -= tool.Power;
        }
        else
        {
            Hardness -= tool.Power / 2;
        }
        
        if (Hardness <= 0)
        {
            return true;
        }
        
        return false;
    }
}
