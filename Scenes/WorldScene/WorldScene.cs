using MyDailyLife.Constants;
using MyDailyLife.Scenes.Objects.Chair;
using MyDailyLife.Scenes.Objects.Ground;
using MyDailyLife.Scenes.Objects.Sphere;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.WorldScene
{
    class WorldScene : Scene
    {
        private Chair? _box;
        private Sphere? _sphere;
        private Ground? _ground;

        public WorldScene(float aspecRatio) : base(aspecRatio)
        {

            _sphere = new Sphere(Matrix4.Identity);
            _box = new(Matrix4.CreateTranslation(-(Vector3.UnitZ * 3.5f)));
            _ground = new(Matrix4.Identity);
        }


        public override void Render(double deltaTime)
        {

            base.Render(deltaTime);

            _sphere?.Render(deltaTime);

            _ground?.Render(deltaTime);

            _box?.Render(deltaTime);
        }

        protected override void Release()
        {
            _box?.Dispose();
            _sphere?.Dispose();
            _ground?.Dispose();
            base.Release();
        }
    }
}
