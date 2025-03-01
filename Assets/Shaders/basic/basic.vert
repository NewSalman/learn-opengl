#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

uniform mat4 model;
uniform mat3 normalMat3;
//uniform mat4 view;
//uniform mat4 projection;

layout (std140) uniform CameraBlock {
	mat4 view;
	mat4 projection;
} camera;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoord;

void main() 
{
	gl_Position = vec4(aPos, 1.0f) * model * camera.view * camera.projection;
	FragPos = vec3(model * vec4(aPos, 1.0f));
	Normal = normalize(normalMat3 * aNormal);
	TexCoord = aTexCoord;
}