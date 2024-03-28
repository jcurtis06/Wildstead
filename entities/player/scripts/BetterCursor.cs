using Godot;
using Wildstead.levels.objects.destructible;

public partial class BetterCursor : RayCast2D
{
    private Sprite2D _sprite;
    
    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("CursorSprite");
    }
    
    public override void _Process(double delta)
    {
        // Look at mouse
        var mousePos = GetGlobalMousePosition();
        LookAt(mousePos);
        
        // Check for collision
        if (IsColliding())
        {
            var collisionPoint = GetCollisionPoint() + GlobalPosition.DirectionTo(mousePos).Normalized();
            
            var snapped = new Vector2(
                Mathf.Floor(collisionPoint.X / 8) * 8,
                Mathf.Floor(collisionPoint.Y / 8) * 8
            );
           
            _sprite.GlobalPosition = snapped;

            if (Input.IsActionJustPressed("attack"))
            {
                HandleBreak(collisionPoint);
            }
        }
        else
        {
            _sprite.GlobalPosition = GlobalPosition;
        }

        _sprite.GlobalRotationDegrees = 0;
    }

    public void HandleBreak(Vector2 collisionPos)
    {
        var player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
        player.Swing();
        var collision = GetCollider();

        if (collision is Destructible destructible)
        {
            destructible.Destroy(GetTree().GetFirstNodeInGroup("Player") as PlayerController);
        }
        
        if (collision is Blocks blocks)
        {
            if (player?.MainHand is Tool tool)
            {
                var broke = blocks.DamageBlock(collisionPos, tool);
                
                if (broke != null)
                {
                    player?.Inventory.AddItem(broke);
                }
            }
        }
    }
}
