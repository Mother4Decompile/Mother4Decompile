uniform sampler2D texture;
uniform vec4 color;
uniform float threshold;
uniform float t;

void main() {
	vec4 texCol = texture2D(texture, gl_TexCoord[0].xy);
	float alpha = ceil(texCol.a - threshold) * color.a;
	gl_FragColor = vec4(color.rgb, alpha);
}