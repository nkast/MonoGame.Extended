using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Extended.ViewportAdapters
{
    public abstract class ViewportAdapter : IDisposable
    {
        protected ViewportAdapter(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public virtual void Dispose()
        {
        }

        public GraphicsDevice GraphicsDevice { get; }
        public Viewport Viewport => GraphicsDevice.Viewport;

        public abstract int VirtualWidth { get; }
        public abstract int VirtualHeight { get; }
        public abstract int ViewportWidth { get; }
        public abstract int ViewportHeight { get; }

        public Rectangle BoundingRectangle => new Rectangle(0, 0, VirtualWidth, VirtualHeight);
        public Point Center => BoundingRectangle.Center;
        public abstract Matrix GetScaleMatrix();

        public Point PointToScreen(Point point)
        {
            return PointToScreen(point.X, point.Y);
        }

        public virtual Point PointToScreen(int x, int y)
        {
            var scaleMatrix = GetScaleMatrix();
            var invertedMatrix = Matrix.Invert(scaleMatrix);
            var pt = Vector2.Transform(new Vector2(x, y), invertedMatrix);
            return new Point((int)pt.X, (int)pt.Y);
        }

        public virtual void Reset()
        {
        }
    }
}