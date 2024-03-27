using Godot;
using System;

public partial class HotBar : InventoryView
{
	[Export] public PlayerController player;

	private int _currentSlot = 0;
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (Input.IsActionJustPressed("slot_1"))
		{
			player.SetMainHand(Inventory.Items[0]);

			GetChild<SlotView>(_currentSlot).Deselect();
			_currentSlot = 0;
			GetChild<SlotView>(0).Select();
		}
		
		if (Input.IsActionJustPressed("slot_2"))
		{
			player.SetMainHand(Inventory.Items[1]);
			
			GetChild<SlotView>(_currentSlot).Deselect();
			_currentSlot = 1;
			GetChild<SlotView>(1).Select();
		}
		
		if (Input.IsActionJustPressed("slot_3"))
		{
			player.SetMainHand(Inventory.Items[2]);
			
			GetChild<SlotView>(_currentSlot).Deselect();
			_currentSlot = 2;
			GetChild<SlotView>(2).Select();
		}
	}
}
