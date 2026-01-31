shader_type canvas_item;
render_mode unshaded;

uniform sampler2D mask_tex;
uniform float threshold = 0.01; // 0..1
uniform float softness  = 0.02; // feather edge

void fragment() {

   	vec4 base = texture(TEXTURE, UV);

    // Mask is rendered full-screen into a viewport; sample using screen UV
    float m = texture(mask_tex, SCREEN_UV).a;

    // Hard cut: float vis = step(threshold, m);
    // Soft cut:
    float vis = smoothstep(threshold, threshold + softness, m);

    COLOR = vec4(base.rgb, base.a * vis);
	//COLOR = texture(mask_tex, SCREEN_UV);
}