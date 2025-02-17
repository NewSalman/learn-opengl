using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.UniformBuffers
{
    public class UniformBufferData<T>(
        int size,
        nint offset,
        T data
    ) where T : struct {
        public int Size = size;
        public nint Offset = offset;
        public T Data = data;
    } 

    public class UniformBuffer : IDisposable
    {
        public int Size { get; set; }
        private int Buffer { get; set; }

        public UniformBuffer(int size, int blockPoint, nint initialValue = 0, BufferUsageHint bufferUsage = BufferUsageHint.StreamDraw)
        {
            Size = size;
            Buffer = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.UniformBuffer, Buffer);
            GL.BufferData(BufferTarget.UniformBuffer, size, initialValue, bufferUsage);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, blockPoint, Buffer, 0, Size);
        }

        public void BindDataMatrix<T>(UniformBufferData<T> buffer) where T : struct
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, Buffer);
            BindDataAndTransposeMatrix(buffer.Offset, buffer.Size, buffer.Data);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        private void BindDataAndTransposeMatrix(nint offset, int size, dynamic matrix)
        {
            if (matrix is Matrix2)
            {
                Matrix2 mat2 = matrix;
                GL.BufferSubData(BufferTarget.UniformBuffer, offset, size, [Matrix2.Transpose(mat2)]);
            }

            if (matrix is Matrix3)
            {
                Matrix3 mat3 = matrix;
                GL.BufferSubData(BufferTarget.UniformBuffer, offset, size, [Matrix3.Transpose(mat3)]);
            }

            if (matrix is Matrix4)
            {
                Matrix4 mat4 = matrix;
                GL.BufferSubData(BufferTarget.UniformBuffer, offset, size, [Matrix4.Transpose(mat4)]);
            }

        }

        public void BindDataVector<T>(UniformBufferData<T> buffer) where T : struct
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, Buffer);
            GL.BufferSubData(BufferTarget.UniformBuffer, buffer.Offset, buffer.Size, [buffer.Data]);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public void BindDataMatrices(List<UniformBufferData<Matrix4>> uniformBuffers)
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, Buffer);
            for(int i = 0; i < uniformBuffers.Count; i++)
            {
                BindDataAndTransposeMatrix(uniformBuffers[i].Offset, uniformBuffers[i].Size, uniformBuffers[i].Data);
            }
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public void BindDataVectors(List<UniformBufferData<Vector3>> uniformBuffers)
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, Buffer);
            for (int i = 0; i < uniformBuffers.Count; i++)
            {
                GL.BufferSubData(
                    BufferTarget.UniformBuffer,
                    uniformBuffers[i].Offset,
                    uniformBuffers[i].Size,
                    [uniformBuffers[i].Data]);
            }
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public void BindData(Action bindBuffer)
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, Buffer);
            bindBuffer();
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Buffer);
        }
    }
}
