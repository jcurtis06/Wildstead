using Godot;
using System;
using Wildstead.data.item.scripts;

[GlobalClass]
public partial class Block : Item
{
    [Export] public ItemMaterial Material;
    [Export] public int TerrainIndex;
}
