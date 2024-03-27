using Godot;

namespace Wildstead.entities.scripts;

public partial class Entity : CharacterBody2D
{
    [Export] public World World;
    [Export] public float Health;
    [Export] public float MaxHealth;

    public void TakeDamage(float dmg)
    {
        Health -= dmg;

        if (Health <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        QueueFree();
    }
}
