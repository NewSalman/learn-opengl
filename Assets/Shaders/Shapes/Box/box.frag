#version 330 core


//layout (std140) uniform LightBlock {
	//vec3 position;
	//float pad1;
	
	//vec3 viewPosition;
	//float pad2;

	//vec3 ambient;
	//float pad3;

	//vec3 diffuse;
	//float pad4;

	//vec3 specular;
	//float pad5;
//} light;

//struct Material {
	//sampler2D diffuse;
	//vec3 specular;
	//float shininess;
//};

//uniform Material material;


in vec2 TexCoord;
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

void main()
{


	//vec3 norm = normalize(Normal);
	//vec3 lightDir = normalize(vec3(1.0f) - FragPos);

	
	//vec3 viewDir = normalize(vec3(1.0f) - FragPos);
	//vec3 reflectDir = reflect(-lightDir, norm); 

    //vec3 ambient = light.ambient * texture(material.diffuse, TexCoord).rgb;

	//float diff = max(dot(norm, lightDir), 0.0);
	//vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoord).rgb;

	//float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	//vec3 specular = light.specular * (spec * material.specular);  

    //vec3 result = (ambient + diffuse);
    FragColor = vec4(vec3(1.0f), 1.0);
}