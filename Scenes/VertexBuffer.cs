using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes
{
    public class VertexBuffer
    {
        public List<float> Data { get; set; }
        public List<uint> Indices { get; set; } 
        public List<BufferType> BufferOrder { get; set; }

        public VertexBuffer(List<float> data, List<uint> indices, List<BufferType> bufferOrder)
        {
            Data = data;
            Indices = indices ?? [];
            BufferOrder = bufferOrder;
        }
    }
}
