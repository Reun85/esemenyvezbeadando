#nullable enable

using System.Reflection.Metadata;
using System.Text;

namespace RobotPigs.Persistence
{
    public class Pig
    {
        public const int HP = 3;
        public const int ORDERSIZE = 5;
        private Board _board;

        public Int32[] Orders
        {
            get => _orders;
            private
              set => _orders = value;
        }

        private bool _ready = false;

        public static readonly String[] allowed = { "előre",         "hátra",
                                              "balra",         "jobbra",
                                              "fordulj balra", "fordulj jobbra",
                                              "tűz",           "ütés" };

        private int _hp;
        private Pos _pos;
        private Int32[] _orders = new Int32[5];

        public Pos Pos
        {
            get => _pos;
            private
              set => _pos = value;
        }

        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }

        public bool IsReady { get => _ready; set => _ready = value; }

        public Pig(Pos pos, Board b)
        {
            Pos = pos;
            Hp = HP;
            _board = b;
        }

        public void SetPos(Pos p)
        {
            if (p.X >= 0 && p.Y >= 0 && p.X < _board.N && p.Y < _board.N)
                Pos = p;
        }

        public Action CreateAction(int orderind)
        {
            if (!this.IsReady)
            {
                throw new MissingFieldException("Pig is not ready!");
            }
            // These default should never be necessary, but just to be
            // sure.
            Persistence.Pos newpos;
            Persistence.Action.ActionType type;
            switch (Orders[orderind])
            {
                case 0:
                    newpos = Pos.Move(Persistence.Pos.MovementDirection.Forward, _board.N);
                    type = Persistence.Action.ActionType.Move;
                    break;

                case 1:
                    newpos = Pos.Move(Persistence.Pos.MovementDirection.Back, _board.N);
                    type = Persistence.Action.ActionType.Move;
                    break;

                case 2:
                    newpos = Pos.Move(Persistence.Pos.MovementDirection.Left, _board.N);
                    type = Persistence.Action.ActionType.Move;
                    break;

                case 3:
                    newpos = Pos.Move(Persistence.Pos.MovementDirection.Right, _board.N);
                    type = Persistence.Action.ActionType.Move;
                    break;

                case 4:
                    newpos = Pos.Turn(Persistence.Pos.MovementDirection.Left);
                    type = Persistence.Action.ActionType.Turn;
                    break;

                case 5:
                    newpos = Pos.Turn(Persistence.Pos.MovementDirection.Right);
                    type = Persistence.Action.ActionType.Turn;
                    break;

                case 6:
                    newpos = Pos;
                    type = Persistence.Action.ActionType.Fire;
                    break;

                case 7:
                    newpos = Pos;
                    type = Persistence.Action.ActionType.Hit;
                    break;

                default:
                    throw new ArgumentException(
                        $"\"{Orders[orderind]}\" command not recognised. Did you validate this with Pig.validate?");
            }
            return new Persistence.Action(type, newpos);
        }

        private static Int32[] Validate(String[] inp)
        {
            int[] result = new Int32[inp.Length];
            if (inp.Length != ORDERSIZE)
            {
                throw new ArgumentOutOfRangeException("Not the right amount of lines.");
            }
            for (int i = 0; i < ORDERSIZE; i++)
            {
                int j;
                for(j = 0; j < Pig.allowed.Length && Pig.allowed[j] != inp[i]; j++) { }
                if (j == Pig.allowed.Length)
                {
                    throw new ArgumentException($"\"{inp[i]}\" nem egy értelmes parancs!" + Environment.NewLine + "Használható parancsok: " + AllCommands());
                }
                else
                {
                    result[i] =j;
                }
            }
            return result;
        }

        public static String AllCommands()
        {
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < allowed.Length - 1; i++)
            {
                ret.Append(allowed[i]);
                ret.Append(", ");
            }
            if (allowed.Length > 1)
            {
                ret.Append(allowed[^1]);
            }
            return ret.ToString();
        }

        public void TakeDmg()
        { Hp -= 1; }

        /// <throws>
        /// ArgumentOutOfRangeException => Not enough lines
        /// ArgumentException => what text could not be parsed.
        ///
        /// </throws>
        /// Due to how commands will have a different effect based on the current state of the board
        /// they will actually only be parsed when its time.
        /// Nevertheless they are validated way before.
        public void Parse(String[] inp)
        {
            Orders = Validate(inp);
            _ready = true;
        }
        public void Parse(Int32[] inp)
        {
            if (inp.Length != ORDERSIZE)
            {
                throw new ArgumentOutOfRangeException("Not the right amount of lines.");
            }
            for(int i = 0;i < ORDERSIZE; i++)
            {
                if (inp[i] < 0 || inp[i] >= Pig.allowed.Length)
                {
                    throw new ArgumentException($"Hibás index: {inp[i]}");
                }
            }
            Orders = inp;
            _ready = true;
        }
    }

    public class Action
    {
        public enum ActionType
        { Move, Turn, Hit, Fire }

        private ActionType _type;
        private Pos _newpos;

        public ActionType Type { get => _type; set => _type = value; }
        public Pos NewPos { get => _newpos; set => _newpos = value; }

        public Action(ActionType t, Pos p)
        {
            this._type = t;
            this._newpos = p;
        }
    }
}