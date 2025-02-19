#version 330 core
out vec4 FragColor;

layout (std140) uniform LightBlock {
	vec3 position;
	float pad1;
	
	vec3 viewPosition;
	float pad2;

	vec3 ambient;
	float pad3;

	vec3 diffuse;
	float pad4;

	vec3 specular;
	float pad5;
} light;

struct Material {
	sampler2D diffuse;
	vec3 specular;
	float shininess;
};

uniform Material material;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

void main() 
{	
	vec3 norm = Normal;
	vec3 lightDir = normalize(light.position - FragPos);

	vec3 viewDir = normalize(light.viewPosition - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);

	//ambient
	vec3 ambient = light.ambient * texture(material.diffuse, TexCoords).rgb;

	// diffuse
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;

	// specular
	float spec = pow(max(dot(viewDir, reflectDir), 0.0f), material.shininess);
	vec3 specular = light.specular * (spec * material.specular);

	vec3 result = (ambient + diffuse + specular); 
	FragColor = vec4(result, 1.0f);
}