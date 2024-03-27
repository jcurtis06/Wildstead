using Godot;
using System;
using Wildstead.entities.scripts;

public interface IConsumable
{
    void Consume(Entity user);
}
