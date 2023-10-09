namespace RobotPigs.Pers
{
    public class Board
    {
        
        
        
        /// <summary>
        /// n x n board.
        /// </summary>
        /// Numbered from top left.
        public int n { get; private set; }

        public Pig Plr1 { get; set; }
        public Pig Plr2 { get; set; }


        public Board(int s)
        {
            this.n = s;
            int center = this.n / 2;
            this.Plr1 = new Pig(new Pos(center - 1 - (n % 2 == 0 ? 1 : 0), center, Pos.Dir.East));
            this.Plr2 = new Pig(new Pos(center + 1, center, Pos.Dir.West));
        }


        public bool isReady()
        { return Plr1.Ready && Plr2.Ready; }
    }
}