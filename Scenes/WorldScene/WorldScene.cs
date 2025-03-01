using MyDailyLife.Scenes.Objects.Chair;
using MyDailyLife.Scenes.Objects.Sphere;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.WorldScene
{
    class WorldScene : Scene
    {
        private Chair? _box;
        private Sphere? _sphere;

        public WorldScene(float aspecRatio) : base(aspecRatio)
        {
            //_box = new(Matrix4.CreateTranslation(-(Vector3.UnitZ * 3.5f)));
            _sphere = new Sphere(Matrix4.Identity);
        }


        public override void Render(double deltaTime)
        {

            base.Render(deltaTime);

            _box?.Render(deltaTime);
            _sphere?.Render(deltaTime);
        }

        protected override void Release()
        {
            _box?.Dispose();
            _sphere?.Dispose();
            base.Release();
        }
    }
}
