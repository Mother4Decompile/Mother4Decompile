uniform sampler2D image, palette, gradmap;
uniform float palIndex;
uniform vec4 blend;
uniform float blendMode;
uniform float palSize;
uniform float delta;

void main() {
	vec4 index = texture2D(image, gl_TexCoord[0].xy);
	vec4 baseColor = texture2D(palette, vec2(((index.r * 255.0) + 0.5) / palSize, palIndex + 0.167));
	vec3 baseNoAlpha = baseColor.rgb;
	vec3 finalColor = vec3(0, 0, 0);

	vec4 grad = texture2D(gradmap, gl_TexCoord[0].xy);
	float alpha = 0.0;
	if (delta >= grad.r) {
		alpha = 1.0;
	}
	
	if (blendMode < 0.1) {
		finalColor = blend.rgb;
	} else if (blendMode < 1.1) {
		finalColor = baseNoAlpha * blend.rgb;
	} else if (blendMode < 2.1) {
		finalColor = 1.0 - (1.0 - blend.rgb) * (1.0 - baseNoAlpha);
	}
	
	gl_FragColor = vec4(finalColor, baseColor.a * blend.a * alpha);
}
