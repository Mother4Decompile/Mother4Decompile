uniform sampler2D layerTex, bottomTex, palette;
uniform float amp, freq, scale, comp, time;
uniform float width, height;
uniform float mode, blend, opacity;
uniform float xTrans, yTrans;
uniform float palIndex, palShift, palSize;

vec4 final_mix(vec4 NewColor, vec4 BaseColor, vec4 BlendColor) {
	float A2 = BlendColor.a * opacity;
	vec3 mixRGB = A2 * NewColor.rgb;
	mixRGB += ((1.0-A2) * BaseColor.rgb);
	return vec4(mixRGB, BaseColor.a + BlendColor.a);
}

void main() {
	vec2 aux = gl_TexCoord[0].xy;
	vec2 offset = vec2(xTrans, yTrans);
	float y = aux.y * height;
	
	float distx = amp * sin(freq * y + scale * time);
	//aux.x = 0;

	if (mode > -0.1 && mode < 0.1)
	{
		offset.x += distx;
	}
	else if (mode > 0.9 && mode < 1.1)
	{
		offset.x += (mod(floor(y), 2.0) == 1.0) ? distx : -distx;
	}
	else if (mode > 1.9 && mode < 2.1)
	{
		offset.y += aux.y * comp + distx;
	}
	else if (mode > 2.9 && mode < 3.1)
	{
		offset.x += distx;
		offset.y += aux.y * comp + distx;
	}
	else if (mode > 3.9 && mode < 4.1)
	{
		offset.x += (mod(floor(y), 2.0) == 1.0) ? distx : -distx;
		offset.y += aux.y * comp + distx;
	}

	aux += offset;
	vec4 botColor = texture2D(bottomTex, gl_TexCoord[0].st);
	vec4 index = texture2D(layerTex, aux);
	float calcIndex = ((index.r * 255.0) + 0.5) / palSize;
	vec4 topColor = texture2D(palette, vec2(mod(calcIndex + palShift, 1.0), palIndex + 0.01));
	
	if (topColor.a > 0.01)
	{
		if (blend < 0.1) // none
		{
			gl_FragColor = final_mix(topColor, botColor, topColor);
		}
		else if (blend < 1.1) // add
		{
			vec4 addCol = clamp(botColor + topColor, 0.0, 1.0);
			gl_FragColor = final_mix(addCol, botColor, topColor);
		}
		else if (blend < 2.1) // subtract
		{
			vec4 subCol = clamp(botColor - topColor, 0.0, 1.0);
			gl_FragColor = final_mix(subCol, botColor, topColor);
		}
		else if (blend < 3.1) // multiply
		{
			gl_FragColor = final_mix(botColor * topColor, botColor, topColor);
		}
		else if (blend < 4.1) // screen
		{
			gl_FragColor = final_mix(1.0 - ( 1.0 - botColor) * (1.0 - topColor), botColor, topColor);
		}
		else if (blend < 5.1) // overlay
		{
			vec4 lumCoeff = vec4(0.25,0.65,0.1,0.0);
			float L = clamp(10.0*(dot(lumCoeff,botColor)- 0.45), 0.0, 1.0);
			vec4 result1 = 2.0 * botColor * topColor;
			vec4 result2 = 1.0 - 2.0*(1.0-topColor)*(1.0-botColor);
			gl_FragColor = final_mix(mix(result1,result2,L),botColor,topColor);
		}
	}
	else
	{
		gl_FragColor = botColor;
	}
}