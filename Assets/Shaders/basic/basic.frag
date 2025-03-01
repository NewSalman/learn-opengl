#version 330 core
out vec4 FragColor;

layout (std140) uniform LightBlock {
	vec3 position;
	float pad0;

	vec3 viewPos;
	float pad1;

	vec3 ambient;
	float pad2;

	vec3 diffuse;
	float pad3;

	vec3 specular;
	float pad4;
} light;

struct Material {
    sampler2D diffuse;
    vec3 specular;
   float shininess;
}; 

uniform Material material;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoord;

void main() 
{
	vec3 ambient = light.ambient * texture(material.diffuse, TexCoord).xyz;

	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(light.position - FragPos);
	
	// diffuse
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoord).xyz;

	// specular
	vec3 viewDir = normalize(light.viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * (spec * material.specular);

	vec3 result = ambient + diffuse + specular;

	FragColor = vec4(result, 1.0f);
}