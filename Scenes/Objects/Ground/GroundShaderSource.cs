using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Shaders;

namespace MyDailyLife.Scenes.Objects.Ground
{
    public class GroundShaderSource
    {
        public static string VertexSource = """
            #version 330 core

            layout (location = 0) in vec3 aPos;
            layout (location = 1) in vec3 aNormal;
            layout (location = 2) in vec2 aTexCoord;

            layout (std140) uniform CameraBlock {
            	mat4 view;
            	mat4 projection;
            } camera;

            uniform mat4 model;

            out vec3 Normal;
            out vec3 FragPos;
            out vec2 TexCoord;

            void main() {
                gl_Position = vec4(aPos, 1.0) * model * camera.view * camera.projection;
                FragPos = vec3(model * vec4(aPos, 1.0));
                Normal = aNormal;
                TexCoord = aTexCoord;
            }
            """.TrimStart('\uFEFF');


        public static string FragmentSource = """
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
                sampler2D normal;
                //sampler2D arm;
            };

            uniform Material material;

            in vec3 Normal;
            in vec3 FragPos;
            in vec2 TexCoord;

            void main() {
                vec3 lightDir = normalize(light.position - FragPos);
                vec3 norm = texture(material.normal, TexCoord).rgb;

                norm = normalize(norm * 2.0 - 1.0);

                vec3 viewDir = normalize(light.viewPos - FragPos);

                vec3 reflectDir = reflect(-lightDir, norm);

                vec3 ambient = light.ambient * texture(material.diffuse, TexCoord).rgb;

                float diff = max(dot(norm, lightDir), 0.0);
                vec3 diffuse = light.diffuse * (diff * texture(material.diffuse, TexCoord).rgb);

                float spec = pow(max(dot(viewDir, reflectDir), 0.0), 0.445);
                vec3 specular = light.specular * (spec * vec3(0.66));

                vec3 color = ambient + diffuse + specular;
                FragColor = vec4(color, 1.0);
            }
            
            """.TrimStart('\uFEFF');
    }
}