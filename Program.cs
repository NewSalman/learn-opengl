// See https://aka.ms/new-console-template for more information
using MyDailyLife;
using OpenTK.Mathematics;

using (Game game = new(new() { Width = 1280, Height = 720, Title = "My Daily Life" }))
{
    game.Run();
}

//Vector4 vector4 = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
//Matrix4 trans = Matrix4.CreateTranslation(1.0f, 1.0f, 0.0f);

//vector4 *= trans;

//Console.WriteLine("result: x = {0}, y = {1}, z = {2},", vector4.X, vector4.Y, vector4.Z);