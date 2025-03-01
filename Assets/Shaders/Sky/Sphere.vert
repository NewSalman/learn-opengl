#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aColor;

uniform mat4 model;

layout (std140) uniform CameraBlock {
	mat4 view;
	mat4 projection;
} camera;

out vec3 Color;

void main() 
{
	gl_Position = vec4(aPos, 1.0f) * model * camera.view * camera.projection;
	Color = aColor;
}