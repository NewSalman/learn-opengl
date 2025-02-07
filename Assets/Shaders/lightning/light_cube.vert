#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;
//uniform mat4 view;
//uniform mat4 projection;

layout (std140) uniform Matrices {
	mat4 view;
	mat4 projection;
} block_matrices;


void main() 
{
	// gl_Position = block_matrices.projection * block_matrices.view * model * vec4(aPos, 1.0f);
	gl_Position = vec4(aPos, 1.0f) * model * block_matrices.view * block_matrices.projection;
}