namespace PWARemake.Game
{
    public class Point3D {
        public float X { get; internal set; }
        public float Z { get; internal set; }
        public float Y { get; internal set; }

        public Point3D(float x, float z, float y)
        {
            X = x;
            Z = z;
            Y = y;
        }
    }
}