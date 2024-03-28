using Godot;
using System;
using Wildstead.entities.scripts;

[GlobalClass]
public partial class ObjectItem : Item, IConsumable
{
    [Export] public PackedScene Scene;

    public void Consume(Entity user)
    {
        var instance = Scene.Instantiate();

        if (instance is Node2D node)
        {
            node.GlobalPosition = user.GlobalPosition;
            user.World.AddChild(node);
            return;
        }
        
        user.World.AddChild(instance);
    }
}
