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

//struct Material {
	//sampler2D diffuse;
	//vec3 specular;
	//float shininess;
//};

//uniform Material material;

in vec3 Normal;
in vec3 FragPos;
//in vec2 TexCoords;

void main() 
{	
	vec3 objectColor = vec3(1.0f, 0.5f, 0.31f);
	vec3 lightColor = vec3(1.0f, 1.0f, 1.0f);

	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(light.position - FragPos);

	//vec3 viewDir = normalize(light.viewPosition - FragPos);
	// reflectDir = reflect(-lightDir, norm);

	float ambientStrength = 0.2f;
	vec3 ambient = ambientStrength * lightColor;

	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	//float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	//vec3 specular = 0.5 * spec * lightColor;

	vec3 result = (ambient + diffuse) * objectColor; 
	FragColor = vec4(result, 1.0f);
}