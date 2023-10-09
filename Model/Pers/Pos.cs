#nullable enable

namespace RobotPigs.Pers
{
    public struct Pos
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public Dir dir { get; private set; }

        public Pos(int x, int y, Dir d = Dir.East)
        {
            this.x = x;
            this.y = y;
            this.dir = d;
        }

        public static bool sameplace(Pos lhs, Pos rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public enum Dir
        { North = 0, East = 1, South = 2, West = 3 }

        public enum MovDir
        { Forward = 0, Right = 1, Back = 2, Left = 3 }

        private int max(int a, int b)
        { return a > b ? a : b; }

        private int min(int a, int b)
        { return a < b ? a : b; }

        public Pos move(MovDir movement, int n)
        {
            Dir d = AddRelativeDirections(movement, this.dir);
            switch (d)
            {
                case Dir.North:
                    return new Pos(x, max(y - 1, 0), this.dir);

                case Dir.East:
                    return new Pos(min(x + 1, n - 1), y, this.dir);

                case Dir.South:
                    return new Pos(x, min(y + 1, n - 1), this.dir);

                default: // Dir.West, csak különben azt hiszi a C# hogy nincs visszatérési
                         // érték minden úton :c
                    return new Pos(max(x - 1, 0), y, this.dir);
            }
        }

        public Pos turn(MovDir d)
        {
            return new Pos(this.x, this.y, AddRelativeDirections(d, this.dir));
        }

        public static Dir AddRelativeDirections(MovDir mov, Dir dir)
        {
            // Mágia, össze adjuk a két relatív irányt és abszolút irányt kapunk.
            return (Dir)(((int)mov + (int)dir) % 4);
        }

        public bool inview(Pos rhs)
        {
            switch (this.dir)
            {
                case Pos.Dir.North:
                    return rhs.x == this.x && rhs.y <= this.y;

                case Pos.Dir.East:
                    return rhs.y == this.y && rhs.x >= this.x;

                case Pos.Dir.South:
                    return rhs.x == this.x && rhs.y >= this.y;

                case Pos.Dir.West:
                    return rhs.y == this.y && rhs.x <= this.x;

                default:
                    return false;
            }
        }

        public bool inradius(Pos rhs, int r = 1)
        {
            return this.x + r >= rhs.x && this.x - r <= rhs.x && this.y + r >= rhs.y &&
                   this.y - r <= rhs.y;
        }

        public static bool operator !=(Pos lhs, Pos rhs)
        {
            return !Pos.sameplace(lhs, rhs) || lhs.dir != rhs.dir;
        }

        public static bool operator ==(Pos lhs, Pos rhs)
        {
            return Pos.sameplace(lhs, rhs) && lhs.dir == rhs.dir;
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