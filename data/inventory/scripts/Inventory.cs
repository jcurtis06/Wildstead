using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Wildstead.data.inventory.scripts;

[GlobalClass]
public partial class Inventory : Resource
{
    [Export] public Array<Item> Items;
    
    public List<InventoryView> Viewers = new();
    
    private static MovingItem _movingItem;
    
    public void MakeItemsUnique()
    {
        var newItems = new Array<Item>();
        newItems.Resize(Items.Count);

        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i] == null)
                continue;
            
            newItems[i] = Items[i].Duplicate() as Item;
        }
        Items = newItems;
    }

    public void AddItem(Item itemToAdd, Inventory secondPriority = null)
    {
        foreach (var item in Items)
        {
            if (item == null)
                continue;
            
            if (item.Name == itemToAdd.Name)
            {
                item.Count += itemToAdd.Count;
                UpdateViewers();
                return;
            }
        }


        for (var i = 0; i < Items.Count; i++)
        {
            GD.Print("Placing in first slot");
            if (Items[i] == null)
            {
                GD.Print("Placing in slot: ", i);
                Items[i] = itemToAdd;
                UpdateViewers();
                return;
            }
        }
    }

    public void RemoveItem(int slot, int count = 1)
    {
        if (slot > Items.Count)
            return;

        if (Items[slot] == null)
            return;

        Items[slot].Count -= count;

        if (Items[slot].Count <= 0)
        {
            Items[slot] = null;
        }
        
        UpdateViewers();
    }

    public void RemoveItem(Item item, int count = 1)
    {
        // Find the slot of the item, then call RemoveItem(slot)
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i] == null)
                continue;
            
            if (Items[i].Name == item.Name)
            {
                GD.Print("removing");
                RemoveItem(i, count);
                return;
            }
        }
    }

    public bool HasItem(Item item)
    {
        foreach (var inventoryItem in Items)
        {
            if (inventoryItem == null)
                continue;
            
            if (inventoryItem.Name == item.Name)
            {
                if (inventoryItem.Count >= item.Count)
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    public void ProcessSlotSelect(int slotIndex, InventoryView view)
    {
        if (_movingItem == null)
        {
            ProcessPickUp(slotIndex);
        }
        else
        {
            ProcessSetDown(slotIndex);
        }
        
        UpdateViewers();
    }
    
    private void ProcessPickUp(int slotIndex)
    {
        if (Items[slotIndex] == null)
            return;

        _movingItem = new MovingItem(this, slotIndex, Items[slotIndex]);
        // HeldItem = Items[itemIndex];
        Items[slotIndex] = null;
    }
    
    private void ProcessSetDown(int newSlot)
    {
        if (_movingItem == null)
            return;
        
        if (Items[newSlot] == null)
        {
            Items[newSlot] = _movingItem.Item;
            _movingItem = null;
        }
        else
        {
            Item slotItem = Items[newSlot];

            if (slotItem.Name == _movingItem.Item.Name)
            {
                slotItem.Count += _movingItem.Item.Count;
                _movingItem = null;
            }
            else
            {
                Items[newSlot] = _movingItem.Item;
                _movingItem = new MovingItem(this, newSlot, slotItem);
            }
        }
    }

    private void UpdateViewers()
    {
        foreach (var viewer in Viewers)
        {
            viewer.UpdateView();
        }
    }
}
