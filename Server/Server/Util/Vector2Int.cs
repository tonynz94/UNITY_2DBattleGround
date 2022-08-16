using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Util
{
    class Vector2Int
    {
        public int x;
        public int y;

        public static Vector2Int Up { get { return new Vector2Int(0, 1); } }
        public static Vector2Int Down { get { return new Vector2Int(0, -1); } }
        public static Vector2Int Left { get { return new Vector2Int(-1, 0); } }
        public static Vector2Int Right { get { return new Vector2Int(1, 0); } }

        public Vector2Int(int posX, int posY)
        {
            x = posX;
            y = posY;
        }

        public static Vector2Int operator+(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + a.y);
        }

        public static Vector2Int operator-(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - a.y);
        }
    }
}
