

namespace Tankz
{
    interface IDrawable
    {
        DrawLayer DrawLayer { get; }

        void Draw();
    }
}
