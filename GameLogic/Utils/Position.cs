
using System;

namespace Nashet.UnitsGameLogic
{
    // Representation of 2D vectors and points.    
    
    [Serializable]
    public struct Position : IEquatable<Position>
    {
        public int x { get { return m_X; } set { m_X = value; } }
        public int y { get { return m_Y; } set { m_Y = value; } }

        private int m_X;
        private int m_Y;

        public Position(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        // Set x and y components of an existing Vector.
        public void Set(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        // Access the /x/ or /y/ component using [0] or [1] respectively.
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException(String.Format("Invalid Vector2Int index addressed: {0}!", index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException(String.Format("Invalid Vector2Int index addressed: {0}!", index));
                }
            }
        }

        

        // Returns the squared length of this vector (RO).
        public int sqrMagnitude { get { return x * x + y * y; } }

        // Multiplies two vectors component-wise.
        public static Position Scale(Position a, Position b) { return new Position(a.x * b.x, a.y * b.y); }

        // Multiplies every component of this vector by the same component of /scale/.
        public void Scale(Position scale) { x *= scale.x; y *= scale.y; }

        public void Clamp(Position min, Position max)
        {
            x = Math.Max(min.x, x);
            x = Math.Min(max.x, x);
            y = Math.Max(min.y, y);
            y = Math.Min(max.y, y);
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.x + b.x, a.y + b.y);
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position(a.x - b.x, a.y - b.y);
        }

        public static Position operator *(Position a, Position b)
        {
            return new Position(a.x * b.x, a.y * b.y);
        }

        public static Position operator *(Position a, int b)
        {
            return new Position(a.x * b, a.y * b);
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(Position lhs, Position rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (!(other is Position)) return false;

            return Equals((Position)other);
        }

        public bool Equals(Position other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        /// *listonly*
        public override string ToString()
        {
            return String.Format("({0}, {1})", x, y);
        }

        public static Position zero { get { return s_Zero; } }
        public static Position one { get { return s_One; } }
        public static Position up { get { return s_Up; } }
        public static Position down { get { return s_Down; } }
        public static Position left { get { return s_Left; } }
        public static Position right { get { return s_Right; } }

        private static readonly Position s_Zero = new Position(0, 0);
        private static readonly Position s_One = new Position(1, 1);
        private static readonly Position s_Up = new Position(0, 1);
        private static readonly Position s_Down = new Position(0, -1);
        private static readonly Position s_Left = new Position(-1, 0);
        private static readonly Position s_Right = new Position(1, 0);
    }
}