#nullable enable

using RobotPigs.Pers;

namespace RobotPigs.Model
{
    public class GameModel
    {
        private Pers.Board? _board;
        private Pers.IRobotPigsDataAccess? _dataAccess;

        public event EventHandler<EventData>? HpChange;

        public event EventHandler<EventData>? Fires;

        public event EventHandler<EventData>? Hits;

        public event EventHandler<EventData>? Loses;

        public event EventHandler<EventData>? Moves;

        public Board? Board { get => _board; set => _board = value; }
        public IRobotPigsDataAccess? DataAccess { get => _dataAccess; set => _dataAccess = value; }

        public GameModel(Pers.IRobotPigsDataAccess? dataaccess)
        {
            _dataAccess = dataaccess;
        }

        public void NewGame(int size)
        { _board = new Pers.Board(size); }

        private int _performind = 0;

        public bool PreparetoPerform()
        {
            if (_board == null)
            {
                throw new InvalidOperationException("Create a new game with NewGame!");
            }

            if (_board.isReady())
            {
                _performind = 0;
                return true;
            }
            return false;
        }

        public bool PerformNext()
        {
            if (_board == null)
            {
                throw new InvalidOperationException("Create a new game with NewGame!");
            }
            if (_performind >= Pers.Pig.ORDERSIZE)
                return false;
            if (!_board.isReady())
            {
                throw new MissingFieldException(
                    "One of the players does not have orders set.");
            }

            Pers.Action act1 = createaction(_board.Plr1, _performind);
            Pers.Action act2 = createaction(_board.Plr2, _performind);
            if (act1.Type == Pers.Action.ActionType.Move &&
                act2.Type == Pers.Action.ActionType.Move &&
                Pers.Pos.sameplace(act1.NewPos, act2.NewPos))
            {
                // Attempt to move to same place ignore.
            }
            else
            {
                if ((act1.Type == Pers.Action.ActionType.Move && !Pers.Pos.sameplace(act1.NewPos, act2.NewPos) )|| act1.Type == Pers.Action.ActionType.Turn  && _board.Plr1.Pos != act1.NewPos)
                {
                    Moves?.Invoke(this, new EventData(_board.Plr1, 1, act1.NewPos));
                    _board.Plr1.setPos(act1.NewPos);
                }
                if ((act2.Type == Pers.Action.ActionType.Move && !Pers.Pos.sameplace(act1.NewPos, act2.NewPos) )|| act2.Type == Pers.Action.ActionType.Turn && _board.Plr2.Pos != act2.NewPos)
                {
                    Moves?.Invoke(this, new EventData(_board.Plr2, 2, act2.NewPos));
                    _board.Plr2.setPos(act2.NewPos);
                }
            }

            perform(_board.Plr1, 1, act1, _board.Plr2);
            perform(_board.Plr2, 2, act2, _board.Plr1);
            if (_board.Plr1.Hp == 0 && _board.Plr2.Hp == 0)
            {
                Loses?.Invoke(this, new EventData(null, 3));
            }
            else if (_board.Plr1.Hp == 0)
            {
                Loses?.Invoke(this, new EventData(_board.Plr1, 1));
            }
            else if (_board.Plr2.Hp == 0)
            {
                Loses?.Invoke(this, new EventData(_board.Plr2, 2));
            }
            _performind++;
            return _performind < Pers.Pig.ORDERSIZE;
        }

        private Pers.Action createaction(Pers.Pig p, int orderind)
        {
            if (_board == null)
            {
                throw new InvalidOperationException("Create a new game with NewGame!");
            }
            if (!p.Ready)
            {
                throw new MissingFieldException((nameof(p)), " is not ready!");
            }
            // These default should never be necessary, but just to be
            // sure.
            Pers.Pos newpos;
            Pers.Action.ActionType type;
            switch (p.Orders[orderind])
            {
                case "előre":
                    newpos = p.Pos.move(Pers.Pos.MovDir.Forward, _board.n);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "hátra":
                    newpos = p.Pos.move(Pers.Pos.MovDir.Back, _board.n);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "balra":
                    newpos = p.Pos.move(Pers.Pos.MovDir.Left, _board.n);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "jobbra":
                    newpos = p.Pos.move(Pers.Pos.MovDir.Right, _board.n);
                    type = Pers.Action.ActionType.Move;
                    break;

                case "fordulj balra":
                    newpos = p.Pos.turn(Pers.Pos.MovDir.Left);
                    type = Pers.Action.ActionType.Turn;
                    break;

                case "fordulj jobbra":
                    newpos = p.Pos.turn(Pers.Pos.MovDir.Right);
                    type = Pers.Action.ActionType.Turn;
                    break;

                case "tűz":
                    newpos = p.Pos;
                    type = Pers.Action.ActionType.Fire;
                    break;

                case "ütés":
                    newpos = p.Pos;
                    type = Pers.Action.ActionType.Hit;
                    break;

                default:
                    throw new ArgumentException(
                        $"\"{p.Orders[orderind]}\" command not recognised. Did you validate this with Pig.validate?");
            }
            return new Pers.Action(type, newpos);
        }

        private void takedmg(Pers.Pig p, int pignum)
        {
            p.takedmg();
            HpChange?.Invoke(this, new EventData(p, pignum));
        }

        private void perform(Pers.Pig p, int pignum, Pers.Action act,
                             Pers.Pig other)
        {
            if (act.Type == Pers.Action.ActionType.Fire)
            {
                Fires?.Invoke(this, new EventData(p, pignum));
                if (p.Pos.inview(other.Pos))
                    takedmg(other, pignum == 1 ? 2 : 1);
            }
            if (act.Type == Pers.Action.ActionType.Hit)
            {
                Hits?.Invoke(this, new EventData(p, pignum));
                if (p.Pos.inradius(other.Pos))
                {
                    takedmg(other, pignum == 1 ? 2 : 1);
                }
            }
        }

        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            _board = await _dataAccess.LoadAsync(path);
        }

        public async Task SaveAsync(String path)
        {
            if (_board == null)
            {
                throw new InvalidOperationException("Create a new game with NewGame!");
            }
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            await _dataAccess.SaveAsync(path, _board);
        }

        // This is done this way to allow our representation to call our code
        // periodically
    }
}