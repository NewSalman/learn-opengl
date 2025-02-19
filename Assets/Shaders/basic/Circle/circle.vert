#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;

out vec3 Normal;

layout (std140) uniform Matrices {
	mat4 view;
	mat4 projection;
} block_matrices;


void main() 
{
	gl_Position = vec4(aPosition, 1.0f) * model * block_matrices.view * block_matrices.projection;
	Normal = aNormal;
}