using System;

namespace PWARemake.Lib.Game
{
    [Flags]
    public enum MoveMode : byte
    {
        GP_MOVE_WALK	= 0,
        GP_MOVE_RUN		= 1,
        GP_MOVE_STAND	= 2,
        GP_MOVE_FALL	= 3,
        GP_MOVE_SLIDE	= 4,
        GP_MOVE_PUSH	= 5,		// only sent to NPC
        GP_MOVE_FLYFALL	= 6,
        GP_MOVE_RETURN	= 7,
        GP_MOVE_JUMP	= 8,
        GP_MOVE_PULL	= 9,		// only sent to NPC
        GP_MOVE_BLINK	= 10,		// only sent to NPC£¨Ë²ÒÆ£©
        GP_MOVE_MASK	= 0x0f,

        GP_MOVE_TURN	= 0x10,		//	Turnaround
        GP_MOVE_DEAD	= 0x20,

        GP_MOVE_AIR		= 0x40,
        GP_MOVE_WATER	= 0x80,
        GP_MOVE_ENVMASK	= 0xc0,
    }
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