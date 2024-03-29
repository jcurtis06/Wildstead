shader_type canvas_item;

uniform mat4 global_transform;
uniform sampler2D light_data;
uniform int n_lights = 0;
uniform vec4 dark_color : source_color = vec4(0.25, 0.0625, 0.25, 1.0);
uniform vec4 default_light_color : source_color;
uniform float light_level: hint_range(0.0, 1.0, 0.1) = 0.0;
uniform float offset_modifier: hint_range(0.0, 8.0, 0.1) = 1.0;
uniform int n_light_bands: hint_range(1, 13, 1) = 7;
uniform bool will_smooth_shade = false;
uniform float band_decay_rate : hint_range(0.0, 1.0, 0.05) = 0.5;
uniform float light_strength_modifier : hint_range(0.0, 1.0, 0.1) = 1.0;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

varying vec2 world_position;

void vertex() {
	world_position = (global_transform * vec4(VERTEX, 0.0, 1.0)).xy;
}

void fragment() {
	vec2 frag_position = world_position;
	float m_value = 0.0;
	vec4 light_color = default_light_color;
	
	for (int i = 0; i < n_lights; i++) {
		vec4 texel = texelFetch(light_data, ivec2(i, 0), 0);
		float radius = texel.a;
		float strength = texel.b;
		float dist = distance(texel.xy, frag_position);
		dist = min(dist / radius, 1.0);
		dist = max((dist * offset_modifier) - (offset_modifier - 1.0), 0.0);
		
		float value = 0.0;
		if(will_smooth_shade) {
			value = (1.0 - dist) * strength * light_strength_modifier;
		}
		else {
			// decay offset so that max value == 1.0
			float offset = pow(band_decay_rate, float(n_light_bands));
			for(int p = 0; p < n_light_bands; p++) {
				// Get max radius for this light band
				float radius_check = 1.0 - pow(band_decay_rate, float(p + 1)) + offset;
				// if pixel is less than the band's radius, then it's in the pth band
				if(dist < radius_check) {
					// Set it's value to the position of the band before this one
					value = (pow(band_decay_rate, float(p)) - offset)
							* strength * light_strength_modifier;
					// Nearest band was found, so break the loop
					break;
				}
			}
		}
		value = clamp(value, 0.0, 1.0);
		if(value > m_value) {
			m_value = value;
			light_color = texelFetch(light_data, ivec2(i, 1), 0);
		}
	}
	
	// mix darkness with light based on light level
	vec4 ambient_color = mix(dark_color, light_color, m_value);
	// apply global light level
	ambient_color.a *= 1.0 - light_level;
	// get screen color for this pixel
	vec4 screen_color = texture(SCREEN_TEXTURE, SCREEN_UV);
	
	// apply multiply blend mode
	COLOR = screen_color * ambient_color;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
