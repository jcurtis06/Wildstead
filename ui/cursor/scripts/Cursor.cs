using Godot;
using System.Collections.Generic;
using Wildstead.levels.objects.destructible;

public partial class Cursor : Area2D
{
	[ExportCategory("Special FX")]
	[Export] public AudioStreamPlayer AttackSound;
	[Export] public Camera Camera;
	[Export] public Node2D SwingNode;
	
	private Node2D _closestNode;
	private List<Node2D> _nodesWithinRange = new();

	private Vector2 _posOverride = Vector2.Inf;
	private Blocks _blocks;

	public override void _Process(double delta)
	{
		if (_posOverride != Vector2.Inf)
		{
			GetNode<Sprite2D>("CursorSprite").GlobalPosition = _posOverride;
			GetNode<Sprite2D>("CursorSprite").Show();

			if (Input.IsActionJustPressed("attack"))
			{
				var destroyed = _blocks?.DamageBlock(_posOverride);

				if (destroyed == true)
				{
					_posOverride = Vector2.Inf;
				}
				
				ApplyEffects();
			}
			
			return;
		}
		
		Node2D closestNode = null;
		float closestDistance = float.MaxValue;

		foreach (Node2D node in _nodesWithinRange)
		{
			float distance = GlobalPosition.DistanceTo(node.GlobalPosition);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestNode = node;
			}
		}

		_closestNode = closestNode;
		GetNode<Sprite2D>("CursorSprite").GlobalPosition = _closestNode?.GlobalPosition ?? GlobalPosition;
		
		if (_closestNode != null)
		{
			GetNode<Sprite2D>("CursorSprite").Show();
		}
		else
		{
			GetNode<Sprite2D>("CursorSprite").Hide();
		}
		
		if (Input.IsActionJustPressed("attack") && _closestNode is Destructible destructible)
		{
			ApplyEffects();
			
			destructible.Destroy(
					GetTree().GetFirstNodeInGroup("Player") as PlayerController
				);
		}
	}

	private void ApplyEffects()
	{
		Camera?.ApplyShake();

		if (AttackSound != null)
		{
			AttackSound.PitchScale = (float)GD.RandRange(0.8, 1.2);
			AttackSound.Play();
		}
		
		if (SwingNode != null)
		{
			SwingNode.LookAt(_closestNode.GlobalPosition);
			SwingNode.GetNode<AnimationPlayer>("SwingAnimation").Play("swing");
		}
	}

	private void NodeEntered(Node2D node)
	{
		if (node is Blocks)
		{
			_blocks = node as Blocks;
			
			_posOverride = _blocks.GetClosestBlock(GetNode<Sprite2D>("CursorSprite").GlobalPosition);
			return;
		}
		
		if (!_nodesWithinRange.Contains(node) && node is Destructible)
		{
			_nodesWithinRange.Add(node);
		}
	}

	private void NodeExited(Node2D node)
	{
		if (node is Blocks)
		{
			_blocks = null;
			_posOverride = Vector2.Inf;
		}
		
		if (_nodesWithinRange.Contains(node))
		{
			_nodesWithinRange.Remove(node);
		}
	}
	
	private void Area2DEntered(Area2D area)
	{
		NodeEntered(area);
	}
	
	private void Area2DExited(Area2D area)
	{
		NodeExited(area);
	}
}
