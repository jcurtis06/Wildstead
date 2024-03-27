using Godot;
using System;

public partial class SlotView : Panel
{
	private TextureRect _icon;
	private Label _count;

	public override void _Ready()
	{
		_icon = GetNodeOrNull<TextureRect>("Display");
		_count = GetNodeOrNull<Label>("Count");
	}

	public void UpdateView(Item item)
	{
		if (item == null)
		{
			_icon.Hide();
			_count.Hide();
			return;
		}

		_icon.Texture = item.Icon;
		_count.Text = item.Count.ToString();
		_icon.Show();
		_count.Show();
	}

	public void Select()
	{
		StyleBoxFlat styleBox = (StyleBoxFlat)GetThemeStylebox("panel").Duplicate();
		styleBox.BgColor = Colors.Yellow;
		
		AddThemeStyleboxOverride("panel", styleBox);
	}
	
	public void Deselect()
	{
		RemoveThemeStyleboxOverride("panel");
	}
	
	private void OnInput(InputEvent e)
	{
		if (e is InputEventMouseButton && e.IsPressed())
		{
			var mouseEvent = (InputEventMouseButton)e;

			switch (mouseEvent.ButtonIndex)
			{
				case MouseButton.Left:
					GetParentOrNull<InventoryView>()?.HandleSlotClick(GetIndex());
					break;
				// case MouseButton.Right:
				// 	GetParentOrNull<InventoryView>()?.HandleSlotUse(GetIndex());
				// 	break;
			}
		}
	}
}
