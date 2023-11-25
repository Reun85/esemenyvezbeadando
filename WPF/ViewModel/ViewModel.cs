using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using RobotPigs.Model;
using RobotPigs.Persistence;

namespace RobotPigs.WPF.View
{
    public class ViewModel : ViewModelBase
    {
        #region Fields

        private GameModel _model; // modell
        private int _n;
        private bool _canSave = true;
        private bool _canStart = false;
        private bool _moreInp = false;
        private bool _inRound = false;
        private int[] _inps = new int[5];
        private int _activePlayer = 0;

        #endregion Fields

        #region Delegates

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand SetOrdersCommand { get; private set; }
        public DelegateCommand NextCommand { get; private set; }

        #endregion Delegates

        #region Properties

        public String ActivePlayer
        {
            get { return new String[] { "piros", "zöld", "" }[_activePlayer]; }
        }

        public Int32[] PlayerInp
        {
            get { return _inps; }
            set
            {
                _inps = value;
                OnPropertyChanged(nameof(PlayerInp));
            }
        }

        public ObservableCollection<Field> Fields { get; set; } = null!;
        public ObservableCollection<String> PossibleInps { get; set; } = null!;

        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                _canSave = value;
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public bool InRound
        {
            get { return _inRound; }
            set
            {
                _inRound = value;
                OnPropertyChanged(nameof(InRound));
            }
        }

        public bool CanStart
        {
            get { return _canStart; }
            set
            {
                _canStart = value;
                OnPropertyChanged(nameof(CanStart));
            }
        }

        public bool MoreInp
        {
            get { return _moreInp; }
            set
            {
                _moreInp = value;
                OnPropertyChanged(nameof(MoreInp));
            }
        }

        public Int32 BoardSize
        {
            get { return _n; }
        }

        public Int32 Player1Health
        {
            get { return _model.Plr1.Hp; }
        }

        public Int32 Player2Health
        {
            get { return _model.Plr2.Hp; }
        }

        #endregion Properties

        #region Events

        public event EventHandler<int>? NewGame;

        public event EventHandler? LoadGame;

        public event EventHandler? SaveGame;

        public event EventHandler? ExitGame;

        #endregion Events

        #region Constructors

        public ViewModel(GameModel model)
        {
            // játék csatlakoztatása
            _model = model;

            _model.NewBoard += NewGameEvent;
            _model.Fires += new EventHandler<EventData>(Model_Fires);
            _model.Hits += new EventHandler<EventData>(Model_Hits);
            _model.Moves += new EventHandler<EventData>(Model_Moves);
            _model.HpChange += new EventHandler<EventData>(Model_HpChange);
            _model.Loses += new EventHandler<EventData>(Model_GameOver);
            _model.NewGame(4);

            // parancsok kezelése
            NewGameCommand =
                new DelegateCommand(param => { OnNewGame(Convert.ToInt32(param)); });
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            SetOrdersCommand = new DelegateCommand(param => SetOrders());
            NextCommand = new DelegateCommand(param => Next());

            PossibleInps = new ObservableCollection<String>(Pig.allowed);
        }

        public void NewGameEvent(Object? sender, EventArgs e)
        {
            InRound = false;
            CanStart = false;
            MoreInp = true;
            CanSave = true;
            _activePlayer = 0;
            OnPropertyChanged(nameof(ActivePlayer));
            int N = _model.BoardSize;
            if (_n != N)
            {
                _n = N;
                OnPropertyChanged(nameof(BoardSize));
                int size = 440 / _n > 40 ? 440 / _n : 40;
                Field.FontSize = size * 3 / 4;
                Fields = new ObservableCollection<Field>();

                for (int i = 0; i < _n; i++)
                {
                    for (int j = 0; j < _n; j++)
                    {
                        var gridItem = new Field
                        {
                            Data = 4,
                            X = i,
                            Y = j,
                        };
                        Fields.Add(gridItem);
                    }
                }
                Fields[GetElement(_model.Plr1.Pos)].Data = (int)_model.Plr1.Pos.Dir;
                Fields[GetElement(_model.Plr1.Pos)].ForeColor = 1;
                Fields[GetElement(_model.Plr2.Pos)].ForeColor = 2;
                Fields[GetElement(_model.Plr2.Pos)].Data = (int)_model.Plr2.Pos.Dir;
            }
            else
            {
                foreach (Field f in Fields)
                {
                    f.Data = 4;
                    f.Background = 0;
                }
                Fields[GetElement(_model.Plr1.Pos)].Data = (int)_model.Plr1.Pos.Dir;
                Fields[GetElement(_model.Plr1.Pos)].ForeColor = 1;
                Fields[GetElement(_model.Plr2.Pos)].ForeColor = 2;
                Fields[GetElement(_model.Plr2.Pos)].Data = (int)_model.Plr2.Pos.Dir;
            }
            OnPropertyChanged(nameof(Fields));

            OnPropertyChanged(nameof(Player1Health));
            OnPropertyChanged(nameof(Player2Health));
        }

        #endregion Constructors

        #region Private methods

        private void ClearScreen()
        {
            foreach (Field f in Fields)
            {
                f.Background = 0;
            }
        }

        private int GetElement(Pos p) { return p.X + p.Y * _n; }

        private int GetElement(int i, int j) { return i + j * _n; }

        #endregion Private methods

        #region Game event handlers

        private void Model_Hits(Object? sender, EventData e)
        {
            if (e.P != null && e.NewPos != null)
            {
                Pos p = (Pos)e.NewPos;
                for (int i = p.X - 1; i <= p.X + 1; i++)
                {
                    for (int j = p.Y - 1; j <= p.Y + 1; j++)
                    {
                        if ((i != p.X || j != p.Y) && i >= 0 && i < _n && j >= 0 && j < _n)
                        {
                            if (Fields[GetElement(i, j)].Background != 0)
                                Fields[GetElement(i, j)].Background = 3;
                            else
                                Fields[GetElement(i, j)].Background = e.Id;
                        }
                    }
                }
            }
        }

        private void Model_Fires(Object? sender, EventData e)
        {
            if (e.P != null && e.NewPos != null)
            {
                Pos p = (Pos)e.NewPos;
                int dir = (int)p.Dir;

                if (dir == 1 || dir == 3)
                {
                    int change = dir == 1 ? 1 : -1;
                    int j = p.Y;
                    for (int i = p.X; i >= 0 && i < _n; i += change)
                    {
                        if (i != p.X || j != p.Y)
                        {
                            if (Fields[GetElement(i, j)].Background != 0)
                                Fields[GetElement(i, j)].Background = 3;
                            else
                                Fields[GetElement(i, j)].Background = e.Id;
                        }
                    }
                }
                else
                {
                    int change = dir == 0 ? -1 : 1;
                    int i = p.X;
                    for (int j = p.Y; j >= 0 && j < _n; j += change)
                    {
                        if (i != p.X || j != p.Y)
                        {
                            if (Fields[GetElement(i, j)].Background == 0)
                                Fields[GetElement(i, j)].Background = 3;
                            else
                                Fields[GetElement(i, j)].Background = e.Id;
                        }
                    }
                }
            }
        }

        private void Model_Moves(Object? sender, EventData e)
        {
            if (e.P != null)
            {
                Pig plr = (Pig)e.P;
                if (e.NewPos != null)
                {
                    Pos p = plr.Pos;
                    Pos newPos = (Pos)e.NewPos;
                    if (!Persistence.Pos.equals(p, newPos))
                    {
                        if (Fields[GetElement(p)].ForeColor ==
                            e.Id) // It is possible that the other player has moved to their
                                  // starting position
                            Fields[GetElement(p)].Data =
                                4; // forecolor doesn't need to be changed
                    }
                    Fields[GetElement(newPos)].Data = (int)newPos.Dir;
                    Fields[GetElement(newPos)].ForeColor = e.Id;
                }
            }
        }

        private void Model_HpChange(Object? sender, EventData e)
        {
            if (e.P == null)
                return;
            if (e.Id == 1)
            {
                OnPropertyChanged(nameof(Player1Health));
            }
            else
            {
                OnPropertyChanged(nameof(Player2Health));
            }
        }

        private void Model_GameOver(Object? sender, EventData e)
        {
            // This disables essentially everything in the view
            InRound = false;
            CanStart = false;
            MoreInp = false;
            CanSave = false;
        }

        #endregion Game event handlers

        #region Event methods

        private void SetOrders()
        {
            if (_activePlayer == 0)
            {
                _model.Plr1Parse(_inps);
                _activePlayer = 1;
                OnPropertyChanged(nameof(ActivePlayer));
            }
            else
            {
                _model.Plr2Parse(_inps);
                _activePlayer = 2;
                OnPropertyChanged(nameof(ActivePlayer));
                CanStart = true;
                MoreInp = false;
            }
            PlayerInp = new Int32[5] { 0, 0, 0, 0, 0 };
        }

        private void OnNewGame(int size) { NewGame?.Invoke(this, size); }

        private void StartRound()
        {
            InRound = true;
            CanSave = false;

            _model.PrepareToPerform();
        }

        private void EndRound()
        {
            _activePlayer = 0;
            OnPropertyChanged(nameof(ActivePlayer));
            InRound = false;
            CanStart = false;
            MoreInp = true;
            CanSave = true;
        }

        private void Next()
        {
            if (InRound == false)
            {
                StartRound();
            }
            ClearScreen();
            if (!_model.PerformNext())

            {
                EndRound();
            }
        }

        private void OnLoadGame() { LoadGame?.Invoke(this, EventArgs.Empty); }

        private void OnSaveGame() { SaveGame?.Invoke(this, EventArgs.Empty); }

        private void OnExitGame() { ExitGame?.Invoke(this, EventArgs.Empty); }

        #endregion Event methods
    }
}
