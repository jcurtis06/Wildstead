using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public string Name = "Item";
    [Export] public Texture2D Icon;
    [Export] public int Count = 1;
    [Export] public bool Stackable = true;
}
