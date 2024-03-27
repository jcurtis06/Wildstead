using Godot;
using Wildstead.entities.scripts;

public partial class PlayerController : Entity
{
	[Export] public const float Speed = 50.0f;
	[Export] public Inventory Inventory;

	[Export] public Item MainHand;
	[Export] public Blocks Blocks;

	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite;
    private AudioStreamPlayer _footstep;
    private Sprite2D _swingDisplay;
    
	public override void _Ready()
	{
		_animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
		_sprite = GetNodeOrNull<Sprite2D>("Sprite");
		_footstep = GetNode<AudioStreamPlayer>("Footstep");
		_swingDisplay = GetNode<Sprite2D>("Swing/Display");
		
		SetMainHand(Inventory.Items[0]);
	}

	public void SetMainHand(Item item)
	{
		MainHand = item;
		_swingDisplay.Texture = item?.Icon;
		_swingDisplay.Hide();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("interact"))
		{
			if (MainHand is Block)
			{
				Blocks.SetBlock(GetGlobalMousePosition(), (Block)MainHand);
				Inventory.RemoveItem(MainHand);
			}

			if (MainHand is IConsumable)
			{
				var consumable = (IConsumable)MainHand;
				consumable.Consume(this);
				Inventory.RemoveItem(MainHand);
			}
		}
		
		Vector2 velocity = Velocity;

		velocity = Input.GetVector("left", "right", "up", "down");
		velocity = velocity.Normalized() * Speed;

		Velocity = velocity;
		MoveAndSlide();
		Animate();
	}

	private void Animate()
	{
		if (Velocity.X < 0)
		{
			_sprite.FlipH = true;
		}
		else if (Velocity.X > 0)
		{
			_sprite.FlipH = false;
		}

		if (Velocity.IsZeroApprox())
		{
			_animationPlayer.Play("idle");
		}
		else
		{
			if (!_footstep.Playing)
			{
				_footstep.Play();
				_footstep.PitchScale = (float)GD.RandRange(0.8, 1.2);
			}
			_animationPlayer.Play("walk");
		}
	}
}
