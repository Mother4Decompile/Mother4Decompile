uniform vec2 size;
uniform float progress;

void main() {
	vec2 pos = floor(gl_TexCoord[0].xy * size);
	vec2 center = vec2(160.0, 90.0);
	
	float width = size.x * progress;
	float height = size.y * progress;

	vec2 off = pos - center;
	bool d = (off.x * off.x * height * height + off.y * off.y * width * width > height * height * width * width);

	if (d || progress < 0.001) {
		gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
	} else {
		gl_FragColor = vec4(0.0, 0.0, 0.0, 0.0);
	}
}