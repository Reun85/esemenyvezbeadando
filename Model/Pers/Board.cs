namespace RobotPigs.Pers
{
    public class Board
    {
        
        
        
        /// <summary>
        /// n x n board.
        /// </summary>
        /// Numbered from top left.
        public int N { get; private set; }

        public Pig Plr1 { get; private set; }
        public Pig Plr2 { get; private set; }


        public Board(int s)
        {
            this.N = s;
            int center = this.N / 2;
            this.Plr1 = new Pig(new Pos(center - 1 - (N % 2 == 0 ? 1 : 0), center, Pos.Direction.East));
            this.Plr2 = new Pig(new Pos(center + 1, center, Pos.Direction.West));
        }
        public Board(int s,Pig plr1, Pig plr2)
        {
            this.N = s;
            this.Plr1 = plr1;
            this.Plr2 = plr2;
        }


        public bool IsReady => Plr1.IsReady && Plr2.IsReady;
    }
}