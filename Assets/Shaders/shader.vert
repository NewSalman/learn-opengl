#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 2) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 model;

layout (std140) uniform Matrices {
	mat4 projection;
	mat4 view;
};


void main() 
{
	gl_Position = projection * view * model * vec4(aPosition, 1.0);
	texCoord = vec2(aTexCoord);
}