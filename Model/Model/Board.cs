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


        public GameModel(Pers.IRobotPigsDataAccess? DataAccess)
        {
            _dataAccess = DataAccess;
        }

        public void NewGame(int size)
        { _board = new Pers.Board(size); }

        private int _PerformInd = 0;


        public int? N { get => _board?.N; } // We cant always propagate errors
        public Pig? Plr1 { get => _board?.Plr1; } // We cant always propagate errors
        public Pig? Plr2 { get => _board?.Plr2; } // We cant always propagate errors


        public void Plr1Parse(String[] inp)
        {
            if(_board == null || _board.Plr1 == null)
            {
                throw new NullReferenceException();
            }
            _board.Plr1.Parse(inp);
        }
        public void Plr2Parse(String[] inp)
        {
            if (_board == null || _board.Plr1 == null)
            {
                throw new NullReferenceException();
            }
            _board.Plr2.Parse(inp);
        }

        public bool PrepareToPerform()
        {
            if (_board == null)
            {
                throw new InvalidOperationException("Create a new game with NewGame!");
            }

            if (_board.IsReady)
            {
                _PerformInd = 0;
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
            if (_PerformInd >= Pers.Pig.ORDERSIZE)
                return false;
            if (!_board.IsReady)
            {
                throw new MissingFieldException(
                    "One of the players does not have orders set.");
            }

            Pers.Action act1 = _board.Plr1.CreateAction(_PerformInd,_board.N);
            Pers.Action act2 = _board.Plr2.CreateAction(_PerformInd, _board.N);
            if (act1.Type == Pers.Action.ActionType.Move &&
                act2.Type == Pers.Action.ActionType.Move &&
                Pers.Pos.SamePlace(act1.NewPos, act2.NewPos))
            {
                // Attempt to move to same place ignore.
            }
            else
            {
                if ((act1.Type == Pers.Action.ActionType.Move && !Pers.Pos.SamePlace(act1.NewPos, act2.NewPos) )|| act1.Type == Pers.Action.ActionType.Turn  && _board.Plr1.Pos != act1.NewPos)
                {
                    Moves?.Invoke(this, new EventData(_board.Plr1, 1, act1.NewPos));
                    _board.Plr1.SetPos(act1.NewPos);
                }
                if ((act2.Type == Pers.Action.ActionType.Move && !Pers.Pos.SamePlace(act1.NewPos, act2.NewPos) )|| act2.Type == Pers.Action.ActionType.Turn && _board.Plr2.Pos != act2.NewPos)
                {
                    Moves?.Invoke(this, new EventData(_board.Plr2, 2, act2.NewPos));
                    _board.Plr2.SetPos(act2.NewPos);
                }
            }

            Perform(_board.Plr1, 1, act1, _board.Plr2);
            Perform(_board.Plr2, 2, act2, _board.Plr1);
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
            _PerformInd++;
            return _PerformInd < Pers.Pig.ORDERSIZE;
        }
        

        private void TakeDmg(Pers.Pig p, int pignum)
        {
            p.TakeDmg();
            HpChange?.Invoke(this, new EventData(p, pignum));
        }

        private void Perform(Pers.Pig p, int pignum, Pers.Action act,
                             Pers.Pig other)
        {
            if (act.Type == Pers.Action.ActionType.Fire)
            {
                Fires?.Invoke(this, new EventData(p, pignum));
                if (p.Pos.InView(other.Pos))
                    TakeDmg(other, pignum == 1 ? 2 : 1);
            }
            if (act.Type == Pers.Action.ActionType.Hit)
            {
                Hits?.Invoke(this, new EventData(p, pignum));
                if (p.Pos.InRadius(other.Pos))
                {
                    TakeDmg(other, pignum == 1 ? 2 : 1);
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