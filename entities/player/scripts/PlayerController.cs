using Godot;
using Wildstead.entities.scripts;

public partial class PlayerController : Entity
{
	[Export] public const float Speed = 50.0f;
	[Export] public Inventory Inventory;

	[Export] public Item MainHand;
	[Export] public Blocks Blocks;

	[Export] public bool IsSwinging;

	private AnimationPlayer _animationPlayer;
	private AnimationPlayer _swingAnimation;
	private Sprite2D _sprite;
    private AudioStreamPlayer _footstep;
    private AudioStreamPlayer _punch;
    private Sprite2D _swingDisplay;
    private RayCast2D _swingRay;
    private Node2D _swing;
    private Camera _camera;
    
	public override void _Ready()
	{
		_animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
		_sprite = GetNodeOrNull<Sprite2D>("Sprite");
		
		_footstep = GetNode<AudioStreamPlayer>("Footstep");
		_punch = GetNode<AudioStreamPlayer>("Punch");
		
		_swing = GetNode<Node2D>("Swing");
		_swingDisplay = GetNode<Sprite2D>("Swing/Display");
		_swingRay = GetNode<RayCast2D>("Swing/WeaponRay");
		_swingAnimation = GetNode<AnimationPlayer>("Swing/SwingAnimation");
		
		_camera = GetNode<Camera>("Camera2D");
		
		SetMainHand(Inventory.Items[0]);
	}

	public void SetMainHand(Item item)
	{
		MainHand = item;
		_swingDisplay.Texture = item?.Icon;
		_swingDisplay.Hide();
	}

	public void Swing()
	{
		_swing.LookAt(GetGlobalMousePosition());
		_camera.ApplyShake();
		_swingAnimation.Play("swing");
		_punch.Play();
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

		if (IsSwinging)
		{
			if (_swingRay.IsColliding())
			{
				GD.Print("Ray colliding");
				var entity = _swingRay.GetCollider();
				if (entity is Entity)
				{
					if (MainHand is Tool)
					{
						var tool = (Tool)MainHand;
						((Entity)entity).TakeDamage(tool.Damage);
					}
					else
					{
						((Entity)entity).TakeDamage(1);
					}
				}
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
