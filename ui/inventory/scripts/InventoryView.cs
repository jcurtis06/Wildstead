using Godot;
using System;

public partial class InventoryView : GridContainer
{
	[Export] public Inventory Inventory;

	public override void _Ready()
	{
		Inventory.MakeItemsUnique();
		Inventory.AddViewer(this);
		UpdateView();
	}

	public void HandleSlotClick(int slotIndex)
	{
		GD.Print("Slot clicked: ", slotIndex);
		Inventory.ProcessSlotSelect(slotIndex, this);
	}
	
	public void UpdateView()
	{
		for (var i = 0; i < Mathf.Min(GetChildCount(), Inventory.Items.Count); i++)
		{
			var slot = GetChildOrNull<SlotView>(i);
			slot?.UpdateView(Inventory.Items[i]);
		}
	}
}
