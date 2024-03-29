using Godot;
using System;
using Godot.Collections;
using Wildstead.data.item.scripts;

public partial class Lighting : ColorRect
{
	private Image _image;
	private ImageTexture _texture = new();
	
	public override void _Ready()
	{
		Show();
		_image = Image.Create(128, 2, false, Image.Format.Rgbah);
	}

	private void UpdateTexture()
	{
		var lights = GetTree().GetNodesInGroup("light");
		GD.Print(lights.Count);

		for (var i = 0; i < lights.Count; i++)
		{
			if (lights[i] is Light light)
			{
				var lightPos = light.GlobalPosition.Floor();
				_image.SetPixel(
					i,
					0, 
					new Color(
						lightPos.X, 
						lightPos.Y,
						light.Strength,
						light.Radius
						)
					);
			}
		}
		
		_texture = ImageTexture.CreateFromImage(_image);
		Material.Set("shader_parameter/n_lights", lights.Count);
		Material.Set("shader_parameter/light_data", _texture);
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateTexture();

		var canvasTransform = Globals.Camera.GetCanvasTransform();
		var topLeft = -canvasTransform.Origin / canvasTransform.Scale;

		var t = new Transform2D(0, topLeft);
		
		Material.Set("shader_parameter/global_transform", t);
	}
}
