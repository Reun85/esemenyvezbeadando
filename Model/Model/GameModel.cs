#nullable enable

using RobotPigs.Persistence;

namespace RobotPigs.Model
{
    public class GameModel
    {
        private Persistence.Board _board = null!;

        private readonly Persistence.IRobotPigsDataAccess? _dataAccess;

        public event EventHandler<EventData>? HpChange;

        public event EventHandler<EventData>? Fires;

        public event EventHandler<EventData>? Hits;

        public event EventHandler<EventData>? Loses;

        public event EventHandler<EventData>? Moves;

        public event EventHandler<int>? NewBoard;
        public GameModel(Persistence.IRobotPigsDataAccess? DataAccess,
                         int boardSize)
        {
            _dataAccess = DataAccess;
            NewGame(boardSize);
        }

        public void NewGame(int size)
        {
            _board = new Persistence.Board(size);
            NewBoard?.Invoke(this, size);
        }

        private int _PerformInd = 0;

        public int BoardSize { get => _board.N; }
        public Pig Plr1 { get => _board.Plr1; }
        public Pig Plr2 { get => _board.Plr2; }

        /// <throws>
        /// ArgumentOutOfRangeException => Not enough lines
        /// ArgumentException => what text could not be parsed.
        ///
        /// </throws>
        /// Due to how commands will have a different effect based on the current
        /// state of the board they will actually only be parsed when its time.
        /// Nevertheless they are validated way before.
        public void Plr1Parse(String[] inp) { _board.Plr1.Parse(inp); }
        /// <throws>
        /// ArgumentOutOfRangeException => Not enough lines
        /// ArgumentException => what text could not be parsed.
        ///
        /// </throws>
        /// Due to how commands will have a different effect based on the current
        /// state of the board they will actually only be parsed when its time.
        /// Nevertheless they are validated way before.
        public void Plr1Parse(Int32[] inp) { _board.Plr1.Parse(inp); }
        /// <throws>
        /// ArgumentOutOfRangeException => Not enough lines
        /// ArgumentException => what text could not be parsed.
        ///
        /// </throws>
        /// Due to how commands will have a different effect based on the current
        /// state of the board they will actually only be parsed when its time.
        /// Nevertheless they are validated way before.
        public void Plr2Parse(String[] inp) { _board.Plr2.Parse(inp); }
        /// <throws>
        /// ArgumentOutOfRangeException => Not enough lines
        /// ArgumentException => what text could not be parsed.
        ///
        /// </throws>
        /// Due to how commands will have a different effect based on the current
        /// state of the board they will actually only be parsed when its time.
        /// Nevertheless they are validated way before.
        public void Plr2Parse(Int32[] inp) { _board.Plr2.Parse(inp); }

        public bool PrepareToPerform()
        {

            if (_board.IsReady)
            {
                _PerformInd = 0;
                return true;
            }
            return false;
        }

        public bool PerformNext()
        {
            if (_PerformInd >= Persistence.Pig.ORDERSIZE)
                return false;
            if (!_board.IsReady)
            {
                throw new MissingFieldException(
                    "One of the players does not have orders set.");
            }

            Persistence.Action act1 = _board.Plr1.CreateAction(_PerformInd);
            Persistence.Action act2 = _board.Plr2.CreateAction(_PerformInd);
            if (act1.Type == Persistence.Action.ActionType.Move &&
                act2.Type == Persistence.Action.ActionType.Move &&
                Persistence.Pos.SamePlace(act1.NewPos, act2.NewPos))
            {
                // Attempt to move to same place ignore.
            }
            else if (act1.Type == Persistence.Action.ActionType.Move &&
                       act2.Type == Persistence.Action.ActionType.Move &&
                       Persistence.Pos.SamePlace(act1.NewPos, Plr2.Pos) &&
                       Persistence.Pos.SamePlace(act2.NewPos, Plr1.Pos))
            {
                // They should not move over each other
            }
            else
            {
                if ((act1.Type == Persistence.Action.ActionType.Move &&
                     !Persistence.Pos.SamePlace(act1.NewPos, act2.NewPos)) ||
                    act1.Type == Persistence.Action.ActionType.Turn &&
                        !Pos.equals(_board.Plr1.Pos, act1.NewPos))
                {
                    Moves?.Invoke(this, new EventData(_board.Plr1, 1, act1.NewPos));
                    _board.Plr1.SetPos(act1.NewPos);
                }
                if ((act2.Type == Persistence.Action.ActionType.Move &&
                     !Persistence.Pos.SamePlace(act1.NewPos, act2.NewPos)) ||
                    act2.Type == Persistence.Action.ActionType.Turn &&
                        !Pos.equals(_board.Plr2.Pos, act2.NewPos))
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
            return _PerformInd < Persistence.Pig.ORDERSIZE;
        }

        private void TakeDmg(Persistence.Pig p, int pignum)
        {
            p.TakeDmg();
            HpChange?.Invoke(this, new EventData(p, pignum));
        }

        private void Perform(Persistence.Pig p, int pignum, Persistence.Action act,
                             Persistence.Pig other)
        {
            if (act.Type == Persistence.Action.ActionType.Fire)
            {
                Fires?.Invoke(this, new EventData(p, pignum));
                if (p.Pos.InView(other.Pos))
                    TakeDmg(other, pignum == 1 ? 2 : 1);
            }
            if (act.Type == Persistence.Action.ActionType.Hit)
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
            {
                throw new InvalidOperationException("No data access have been provided.");
            }
            _board = await _dataAccess.LoadAsync(path);
            NewBoard?.Invoke(this, _board.N);
        }

        public async Task SaveGameAsync(String path)
        {
            if (_dataAccess == null)
            {
                throw new InvalidOperationException("No data access have been provided.");
            }
            await _dataAccess.SaveAsync(path, _board);
        }
    }
}
