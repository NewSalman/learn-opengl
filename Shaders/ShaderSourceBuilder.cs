using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife.Shaders
{
    public class ShaderSourceBuilder
    {
        private string Source { get; set; }

        public void AddHeaderSource(string path = "Header/version.glsl")
        {
            string versionHeader = File.ReadAllText(path);
            Source = versionHeader;
        }


        // TODO: Implement layout location 
        public void AddLayoutLocation(Dictionary<string, (dynamic type, int location)> datatype)
        {

        }

        public void AddUniformBufferBlock(string name, int binding, Dictionary<string, dynamic> dataType)
        {

        }

        public void AddOutVar(Dictionary<string, dynamic> data)
        {

        }

        public void AddInVar(Dictionary<string, dynamic> data)
        {

        }

        public void AddCodes(string source)
        {
            Source += source;
        }

        public string Build()
        {
            return Source;
        }
    }
}
