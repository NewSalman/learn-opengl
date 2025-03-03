using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife
{
    public class TextureLoader
    {
        private List<(string Name, int Location, Texture Texture)> ActiveTextures { get; set; } = new();


        public int Load(string name, string path)
        {
            Texture texture = new(path);

            ActiveTextures.Add((name, ActiveTextures.Count, texture));

            return ActiveTextures.Count;
        }

        public int GetTextureLocation(string name)
        {
            int location = ActiveTextures.Where(tex => tex.Name == name).First().Location;


            return location;
        }

        public void ActivateAll()
        {
            for (int i = 0; i < ActiveTextures.Count; i++)
            {
                ActiveTextures[i].Texture.Use(TextureUnitLocations.At[ActiveTextures[i].Location]);
            }
        }
    }
}
