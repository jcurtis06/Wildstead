using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Crafter : Resource
{
    [Export] public string Name;
    [Export] public Array<Recipe> Recipes;
}
