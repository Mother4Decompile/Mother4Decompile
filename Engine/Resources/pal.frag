uniform sampler2D image, palette;
uniform float palIndex;
uniform vec4 blend;
uniform float blendMode;
uniform float palSize;
//uniform float delta;

void main() {
	vec4 index = texture2D(image, gl_TexCoord[0].xy);
	vec4 baseColor = texture2D(palette, vec2(((index.r * 255.0) + 0.5) / palSize, palIndex));
	vec3 baseNoAlpha = baseColor.rgb;
	vec3 finalColor = vec3(0, 0, 0);
					//vec3(
					//	((-cos(delta * baseColor.r) / 2.0) + 0.5),
					//	((-cos(delta * baseColor.r) / 2.0) + 0.5),
					//	((-cos(delta * baseColor.r) / 2.0) + 0.5)
					//	);
	
	if (blendMode < 0.1) {
		finalColor = blend.rgb;
	} else if (blendMode < 1.1) {
		finalColor = baseNoAlpha * blend.rgb;
	} else if (blendMode < 2.1) {
		finalColor = 1.0 - (1.0 - blend.rgb) * (1.0 - baseNoAlpha);
	} else if (blendMode < 3.1) {
		finalColor = clamp(baseNoAlpha + blend.rgb, 0.0, 1.0);
	} else if (blendMode < 4.1) {
		finalColor = clamp(baseNoAlpha - blend.rgb, 0.0, 1.0);
	}
	
	gl_FragColor = vec4(finalColor, baseColor.a);
}