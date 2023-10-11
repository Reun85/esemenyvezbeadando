#nullable enable

using System.Text;

namespace RobotPigs.Pers
{
    public class Pig
    {
        public const int HP = 3;
        public const int ORDERSIZE = 5;

        public String[] Orders
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
        private string[] _orders = new String[0];

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

        public Pig(Pos pos)
        {
            Pos = pos;
            Hp = HP;
        }

        public void SetPos(Pos p)
        { Pos = p; }

        public Action CreateAction(int orderind,int boardSize)
        {
            if (!this.IsReady)
            {
                throw new MissingFieldException("Pig is not ready!");
            }
            // These default should never be necessary, but just to be
            // sure.
            Pers.Pos newpos;
            Pers.Action.ActionType type;
            switch (Orders[orderind])
            {
                case "előre":
                    newpos = Pos.Move(Pers.Pos.MovementDirection.Forward, boardSize);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "hátra":
                    newpos = Pos.Move(Pers.Pos.MovementDirection.Back, boardSize);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "balra":
                    newpos = Pos.Move(Pers.Pos.MovementDirection.Left, boardSize);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "jobbra":
                    newpos = Pos.Move(Pers.Pos.MovementDirection.Right, boardSize);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "fordulj balra":
                    newpos = Pos.Turn(Pers.Pos.MovementDirection.Left);
                    type = Pers.Action.ActionType.Turn;
                    break;

                case "fordulj jobbra":
                    newpos = Pos.Turn(Pers.Pos.MovementDirection.Right);
                    type = Pers.Action.ActionType.Turn;
                    break;

                case "tűz":
                    newpos = Pos;
                    type = Pers.Action.ActionType.Fire;
                    break;

                case "ütés":
                    newpos = Pos;
                    type = Pers.Action.ActionType.Hit;
                    break;

                default:
                    throw new ArgumentException(
                        $"\"{Orders[orderind]}\" command not recognised. Did you validate this with Pig.validate?");
            }
            return new Pers.Action(type, newpos);
        }

        public static void Validate(String[] inp)
        {
            if (inp.Length != ORDERSIZE)
            {
                throw new ArgumentOutOfRangeException("Not the right amount of lines.");
            }
            for (int i = 0; i < ORDERSIZE; i++)
            {
                if (!Pig.allowed.Contains(inp[i]))
                {
                    throw new ArgumentException($"\"{inp[i]}\" nem egy értelmes parancs!" + Environment.NewLine + "Használható parancsok: " + AllCommands());
                }
            }
        }

        public static String AllCommands()
        {
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < allowed.Length - 1; i++)
            {
                ret.Append(allowed[i]);
                ret.Append(",");
            }
            if (allowed.Length > 1)
            {
                ret.Append(allowed[allowed.Length - 1]);
            }
            return ret.ToString();
        }

        public void TakeDmg()
        { Hp -= 1; }

        /// <throws>
        /// ArgumentOutOfRangeException => Not enough lines
        /// ArgumentException => what text could not be parsed.
        /// </throws>
        public void Parse(String[] inp)
        {
            Validate(inp);
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