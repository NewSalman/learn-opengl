#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormals;
layout (location = 2) in vec3 aColor;

out vec3 lightColor;

uniform mat4 model;

layout (std140) uniform Matrices {
	mat4 view;
	mat4 projection;
};

void main() 
{
	gl_Position = vec4(aPosition, 1.0) * model * view * projection;
	lightColor = vec3(aColor);
}