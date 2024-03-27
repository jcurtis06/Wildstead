using Godot;
using Wildstead.data.item.scripts;

[GlobalClass]
public partial class Tool : Item
{
    [Export] public ItemMaterial Strength;
    [Export] public float Speed;
    [Export] public float Power;
}
