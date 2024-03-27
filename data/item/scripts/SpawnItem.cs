using Godot;
using System;
using Wildstead.entities.scripts;

[GlobalClass]
public partial class SpawnItem : Item, IConsumable
{
    [Export] public PackedScene EntityScene;

    public void Consume(Entity user)
    {
        var entity = EntityScene.Instantiate() as Entity;
        
        if (entity is Boss)
        {
            var boss = (Boss)entity;
            boss.Summoner = user;
        }
        
        user.World.SpawnEntity(entity);
    }
}
