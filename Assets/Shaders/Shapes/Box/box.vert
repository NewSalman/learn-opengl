#version 330 core
layout (location = 0) in vec3 aPos;
//layout (location = 1) in vec3 aNormal;
//layout (location = 2) in vec2 aTexCoord;

//uniform mat4 model;

//layout (std140) uniform CameraBlock {
	//mat4 view;
	//mat4 projection;
//} block_matrices;

//out vec3 Normal;
//out vec3 FragPos;
//out vec2 TexCoord;

void main() 
{
	gl_Position = vec4(aPos, 1.0f);
	//Normal = aNormal;
	//FragPos = vec3(model * vec4(aPos, 1.0f));
	//TexCoord = aTexCoord;
}