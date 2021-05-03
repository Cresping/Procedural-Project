using UnityEngine;

namespace Pathfind
{
    public class Point
    {
        public int x;
        public int y;

        public Point()
        {
            x = 0;
            y = 0;
        }
        public Point(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
        }

        public Point(Point b)
        {
            x = b.x;
            y = b.y;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public override bool Equals(System.Object obj)
        {

            Point p = (Point)obj;

            if (ReferenceEquals(null, p))
            {
                return false;
            }

            return (x == p.x) && (y == p.y);
        }

        public bool Equals(Point p)
        {
            if (ReferenceEquals(null, p))
            {
                return false;
            }
            return (x == p.x) && (y == p.y);
        }

        public static bool operator ==(Point a, Point b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (ReferenceEquals(null, a))
            {
                return false;
            }
            if (ReferenceEquals(null, b))
            {
                return false;
            }
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public Point Set(int iX, int iY)
        {
            this.x = iX;
            this.y = iY;
            return this;
        }
        public Vector3 Position => new Vector2(this.x, this.y);
    }
}