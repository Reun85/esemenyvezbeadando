namespace RobotPigs.Persistence
{
    public class Board
    {


        private int _n;
        private Pig _plr1;
        private Pig _plr2;
        /// <summary>
        /// n x n board.
        /// </summary>
        /// Numbered from top left.
        public int N { get => _n; private set=> _n=value; }

        public Pig Plr1 { get => _plr1; private set => _plr1 = value; }
        public Pig Plr2 { get => _plr2; private set => _plr2 = value; }


        public Board(int s)
        {
            this._n = s;
            int center = this.N / 2;
            this._plr1 = new Pig(new Pos(center - 1 - (N % 2 == 0 ? 1 : 0), center, Pos.Direction.East),this);
            this._plr2 = new Pig(new Pos(center + 1, center, Pos.Direction.West), this);
        }
        public Board(int s,(Pos,int) plr1, (Pos, int) plr2)
        {
            this._n = s;
            this._plr1 = new Pig(plr1.Item1, this);
            this._plr1.Hp = plr1.Item2;
            this._plr2 = new Pig(plr2.Item1, this);
            this._plr2.Hp = plr2.Item2;
        }

        public Board(Board b)
        {
            this._n = b._n;
            this._plr1 = new Pig(b.Plr1.Pos, this);
            this._plr1.Hp = b.Plr1.Hp;
            this._plr2 = new Pig(b.Plr2.Pos, this);
            this._plr2.Hp = b.Plr2.Hp;
        }


        public bool IsReady => Plr1.IsReady && Plr2.IsReady;
    }
}