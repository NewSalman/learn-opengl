#version 330 core
out vec4 outputColor;

in vec3 lightColor;

void main() 
{
	outputColor = vec4(lightColor, 1.0f);
}