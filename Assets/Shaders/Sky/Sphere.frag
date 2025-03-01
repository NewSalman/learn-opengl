#version 330 core
out vec4 FragColor;


//struct Material {
//	sampler2D diffuse;
//};

//uniform Material material;

in vec3 Color;


void main() 
{
	//FragColor = vec4(texture(material.diffuse, TexCoord).xyz, 1.0);
	FragColor = vec4(Color, 1.0);
}