using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Recipe : Resource
{
    [Export] public Item Result;
    [Export] public Array<Item> Ingredients;
}
