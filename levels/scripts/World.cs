using Godot;
using System;
using Wildstead.entities.scripts;

public partial class World : Node2D
{
    public void SpawnEntity(Entity entity)
    {
        entity.World = this;
        AddChild(entity);
    }
}
