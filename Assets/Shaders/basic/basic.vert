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

out vec3 Normal;
out vec3 FragPos;

void main() 
{
	gl_Position = vec4(aPos, 1.0f) * model * block_matrices.view * block_matrices.projection;
	FragPos = vec3(model * vec4(aPos, 1.0f));
	Normal = aNormal;
}