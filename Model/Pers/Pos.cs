#nullable enable

namespace RobotPigs.Pers
{
    public struct Pos
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Dir { get; private set; }

        public Pos(int x, int y, Direction d = Direction.East)
        {
            this.X = x;
            this.Y = y;
            this.Dir = d;
        }

        public static bool SamePlace(Pos lhs, Pos rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public enum Direction
        { North = 0, East = 1, South = 2, West = 3 }

        public enum MovementDirection
        { Forward = 0, Right = 1, Back = 2, Left = 3 }

        private int Max(int a, int b)
        { return a > b ? a : b; }

        private int Min(int a, int b)
        { return a < b ? a : b; }

        public Pos Move(MovementDirection movement, int n)
        {
            Direction d = AddRelativeDirections(movement, this.Dir);
            switch (d)
            {
                case Direction.North:
                    return new Pos(X, Max(Y - 1, 0), this.Dir);

                case Direction.East:
                    return new Pos(Min(X + 1, n - 1), Y, this.Dir);

                case Direction.South:
                    return new Pos(X, Min(Y + 1, n - 1), this.Dir);
                default: // C# compiler is drunk. Thinks not all path return.....
                    return new Pos(Max(X - 1, 0), Y, this.Dir);
            }
        }

        public Pos Turn(MovementDirection d)
        {
            return new Pos(this.X, this.Y, AddRelativeDirections(d, this.Dir));
        }

        public static Direction AddRelativeDirections(MovementDirection mov, Direction dir)
        {
            // Mágia, össze adjuk a két relatív irányt és abszolút irányt kapunk.
            return (Direction)(((int)mov + (int)dir) % 4);
        }

        public bool InView(Pos rhs)
        {
            switch (this.Dir)
            {
                case Pos.Direction.North:
                    return rhs.X == this.X && rhs.Y <= this.Y;

                case Pos.Direction.East:
                    return rhs.Y == this.Y && rhs.X >= this.X;

                case Pos.Direction.South:
                    return rhs.X == this.X && rhs.Y >= this.Y;

                default: // C# compiler is drunk. Thinks not all path return.....
                    return rhs.Y == this.Y && rhs.X <= this.X;

            }
        }

        public bool InRadius(Pos rhs, int r = 1)
        {
            return this.X + r >= rhs.X && this.X - r <= rhs.X && this.Y + r >= rhs.Y &&
                   this.Y - r <= rhs.Y;
        }

        public static bool operator !=(Pos lhs, Pos rhs)
        {
            return !Pos.SamePlace(lhs, rhs) || lhs.Dir != rhs.Dir;
        }

        public static bool operator ==(Pos lhs, Pos rhs)
        {
            return Pos.SamePlace(lhs, rhs) && lhs.Dir == rhs.Dir;
        }

        public override bool Equals(object? obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Pos rhs = (Pos)obj;
            return this == rhs;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        { return base.GetHashCode(); }
    }
}