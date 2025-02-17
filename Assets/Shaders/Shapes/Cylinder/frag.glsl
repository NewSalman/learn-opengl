#version 330 core
out vec4 outputColor;

layout (std140) uniform LightBlock {
	vec3 position;
	vec3 viewPos;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
} light;

//struct Material {
//	vec3 ambient;
//	vec3 diffuse;
//	vec3 specular;
//	float shininess;
//};

//in vec3 Normal;
//in vec3 FragPos;

//uniform Material material;

void main() 
{
	//vec3 ambient = light.ambient * material.ambient;
	
	//vec3 norm = normalize(Normal);
	//vec3 lightDir = normalize(light.position - FragPos);

	//float diff = max(dot(norm, lightDir), 0.0);
	//vec3 diffuse = light.diffuse * (diff * material.diffuse);

	//vec3 result = (diffuse + ambient) * material.ambient; 
	outputColor = vec4(light.position, 1.0f);
}