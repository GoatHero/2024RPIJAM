shader_type canvas_item;
uniform float num_frames = 4;

void vertex() {
	int i = int(floor(TIME*15.0)) % int(num_frames);
	vec2 uv = vec2(UV.x/num_frames + float(i) * (1.0/num_frames), UV.y);
	UV = uv;
	}

void fragment() {
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
