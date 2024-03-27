using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Biome : Resource
{
    [Export] public float Moisture = -1.0f;
    [Export] public float Temperature = -1.0f;
    [Export] public float Altitude = -1.0f;
    
    [Export] public string TerrainName = "Biome";
    [Export] public int TerrainIndex = 0;
    [Export] public int Layer = 0;

    [Export] public Array<BiomeObject> Objects;
}
