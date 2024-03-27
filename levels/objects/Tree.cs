using Godot;
using Wildstead.levels.objects.destructible;

public partial class Tree : StaticBody2D, Destructible
{
    [Export] public Item Drop;
    
    private Sprite2D _sprite;
    
    public override void _Ready()
    {
        _sprite = GetNodeOrNull<Sprite2D>("Fade/Sprite");
    }
    
    public void MakeTransparent(Node2D body)
    {
        if (body.IsInGroup("Entity"))
        {
            _sprite.Modulate = new Color(1, 1, 1, 0.5f);
        }
    }
    
    public void MakeOpaque(Node2D body)
    {
        _sprite.Modulate = new Color(1, 1, 1, 1);
    }
    
    public void Destroy(PlayerController player)
    {
        if (player.MainHand == null)
        {
            // TODO: damage the player for using their hands
        }
        
        if (Drop != null)
        {
            player.Inventory.AddItem(Drop.Duplicate() as Item);
        }
        QueueFree();
    }
}
