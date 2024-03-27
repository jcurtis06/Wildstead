using Godot;

[GlobalClass]
public partial class BiomeObject: Resource
{
    [Export] public float Probability = 1.0f;
    [Export] public PackedScene Object;
}