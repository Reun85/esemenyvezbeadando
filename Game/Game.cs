using RobotPigs.Model;
using RobotPigs.Persistence;

namespace RobotPigs.WFA
{
    public partial class Game : Form
    {
        private readonly IRobotPigsDataAccess _dataAccess;
        private readonly GameModel _model;
        private Label[,] _grid = new Label[0, 0];
        private System.Windows.Forms.Timer? _round;
        public int N { get; set; } = 6;
        public bool Active { get; set; } = false;
        public static readonly String[] orientation = { "⇑", "⇒", "⇓", "⇐" };

        public static readonly Color[] colors = { Color.OrangeRed, Color.GreenYellow, Color.SkyBlue };
        public static readonly Color backcolor = Color.Gray;

        #region setup

        public void MenuFileNewGame_Click(Object sender, EventArgs e)
        {

            NewGame();
        }
        public Game()
        {
            InitializeComponent();

            _dataAccess = new RobotPigsDataAccess();

            _model = new GameModel(_dataAccess);

            _model.Loses += Game_GameOver;
            _model.HpChange += HPChange;
            _model.Moves += Moves;
            _model.Fires += Fires;
            _model.Hits += Hits;

            splitContainer1.Panel2.Enabled = false;
            _menuFileSaveGame.Enabled = false;
            //NewGame();
        }

        public void NewGame()
        {
            NumberInp inp = new NumberInp(this);
            if (inp.ShowDialog() == DialogResult.OK)
            {
                splitContainer1.Panel2.Enabled = true;
                _menuFileSaveGame.Enabled = true;
                bool same = _model.BoardSize != null && _model.BoardSize == N;
                _model.NewGame(N);
                if (!same)
                {
                    foreach (var item in _grid)
                    {
                        item.Dispose();
                    }
                    if (!GenerateTable() || !SetupTable())
                    {
                        Close();
                    }
                }
                else
                {
                    SetupTable();
                    ClearScreen();
                }
                NewRound();
            }
            else
            {
            }

        }


        public bool IsValidSize(int n)
        {
            if (n < 3)
            {
                throw new ArgumentException("Kettőnél nagyobb számot adjatok meg!");
            }
            int smaller = Min(GameArea.Size.Width, GameArea.Size.Height);
            int size = Max(smaller / n, 40);
            if (size < 20 || size * n > smaller)
            {
                throw new ArgumentException("Túl nagy számot választottatok!");
            }
            return true;
        }
        private bool GenerateTable()
        {
            // Board shouldn't be null here.
            if (_model.BoardSize == null)
            {
                return false; // maybe recursive
            }
            int n = N;
            _grid = new Label[n, n];
            int smaller = Min(GameArea.Size.Width, GameArea.Size.Height);
            int size = Max(smaller / n, 40);
            if (size < 20 || size * n > smaller) // again, cannot happen, but to be sure.
            {
                if (MessageBox.Show("Túl nagy táblát választottatok." + Environment.NewLine + "Szeretnétek egy új játékot indítani?", "Harcos robotmalacok csatája", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return false; // maybe recursive
                }
            }

            int startingposx = (GameArea.Size.Width - size * n) / 2;
            int startingposy = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    _grid[i, j] = new()
                    {
                        Location = new Point(startingposx + size * i, startingposy + size * j),
                        Size = new Size(size, size),
                        Font = new Font(FontFamily.GenericMonospace, size * 3 / 4, FontStyle.Bold),
                        BorderStyle = BorderStyle.FixedSingle,
                        Parent = GameArea
                    };
                }
            return true;
        }

        private bool SetupTable()
        {
            if (_model.BoardSize == null)
                return false;
            int n = (int)_model.BoardSize;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    _grid[i, j].BackColor = backcolor;
                    _grid[i, j].Text = "";
                }
            Pos p1 = _model.Plr1!.Pos;
            Label plr1 = _grid[p1.X, p1.Y];
            plr1.Text = orientation[(int)p1.Dir];
            plr1.ForeColor = colors[0];
            Plr1Health.Text = _model.Plr1!.Hp.ToString();

            Pos p2 = _model.Plr2!.Pos;
            Label plr2 = _grid[p2.X, p2.Y];
            plr2.Text = orientation[(int)p2.Dir];
            plr2.ForeColor = colors[1];
            Plr2Health.Text = _model.Plr2!.Hp.ToString();

            return true;
        }

        #endregion setup

        #region modelevents

        private void Hits(object? sender, EventData e)
        {
            if (e.P != null && e.Pos != null)
            {
                int n = (int)_model.BoardSize!;
                Pos p = (Pos)e.Pos;
                for (int i = p.X - 1; i <= p.X + 1; i++)
                {
                    for (int j = p.Y - 1; j <= p.Y + 1; j++)
                    {
                        if ((i != p.X || j != p.Y) && i >= 0 && i < n && j >= 0 && j < n)
                        {
                            if (_grid[i, j].BackColor == colors[0])
                                _grid[i, j].BackColor = colors[2];
                            else
                                _grid[i, j].BackColor = colors[e.Id - 1];
                        }
                    }
                }
            }
        }

        private void Fires(object? sender, EventData e)
        {
            if (e.P != null && e.Pos != null)
            {
                if (_model.BoardSize == null) return;
                int n = (int)_model.BoardSize;
                Pos p = (Pos)e.Pos;
                int dir = (int)p.Dir;

                if (dir == 1 || dir == 3)
                {
                    int change = dir == 1 ? 1 : -1;
                    int j = p.Y;
                    for (int i = p.X; i >= 0 && i < n; i += change)
                    {
                        if (i != p.X || j != p.Y)
                        {
                            if (_grid[i, j].BackColor == colors[0])
                                _grid[i, j].BackColor = colors[2];
                            else
                                _grid[i, j].BackColor = colors[e.Id - 1];
                        }
                    }
                }
                else
                {
                    int change = dir == 0 ? -1 : 1;
                    int i = p.X;
                    for (int j = p.Y; j >= 0 && j < n; j += change)
                    {
                        if (i != p.X || j != p.Y)
                        {
                            if (_grid[i, j].BackColor == colors[0])
                                _grid[i, j].BackColor = colors[2];
                            else
                                _grid[i, j].BackColor = colors[e.Id - 1];
                        }
                    }
                }
            }
        }

        private void Moves(Object? sender, EventData e)
        {
            if (e.P != null)
            {
                Pig plr = e.P;
                if (e.Pos != null)
                {
                    Pos p = plr.Pos;
                    Pos newpos = (Pos)e.Pos;
                    Label l;
                    if (p != newpos)
                    {
                        l = _grid[p.X, p.Y];
                        l.Text = "";//forecolor doesn't need to be changed
                    }
                    l = _grid[newpos.X, newpos.Y];
                    l.Text = orientation[(int)newpos.Dir];
                    l.ForeColor = colors[e.Id - 1];
                }
            }
        }

        private void Game_GameOver(Object? sender, EventData e)
        {
            Active = false;
            _menuFileSaveGame.Enabled = false;
            _round?.Stop();
            _round = null;
            splitContainer1.Panel2.Enabled = false;
            if (e.Id == 3 && MessageBox.Show("Döntetlen!" + Environment.NewLine + "Szeretnétek új játékot indítani?",
                            "Harcos robotmalacok csatája",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No || (e.Id != 3 && MessageBox.Show("Gratulálok " + (e.Id == 1 ? "kettes" : "egyes") + " játékos győztél!" + Environment.NewLine + "Szeretnétek új játékot indítani?",
                            "Harcos robotmalacok csatája",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
            {
            }
            else
            {
                NewGame();
            }
        }

        private void HPChange(Object? sender, EventData e)
        {
            if (e.P == null)
                return;
            if (e.Id == 1)
                Plr1Health.Text = e.P.Hp.ToString();
            else
                Plr2Health.Text = e.P.Hp.ToString();
        }

        #endregion modelevents

        #region files



        public async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                    splitContainer1.Panel2.Enabled = true;
                    N = (int)_model.BoardSize!;
                    foreach (var item in _grid)
                    {
                        item.Dispose();
                    }
                    if (!GenerateTable() || !SetupTable())
                    {
                        Close();
                    }
                    NewRound();

                }
                catch (BoardDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _menuFileSaveGame.Enabled = true;
                }
            }
        }


        public async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.SaveAsync(_saveFileDialog.FileName);
                }
                catch (BoardDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MenuFileExit_Click(Object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan ki szeretnétek lépni?", "Harcos robotmalacok csatája", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        #endregion files

        #region Round
        public void StartRound()
        {
            _menuFileSaveGame.Enabled = false;
            
            _model.PrepareToPerform();
            NextButton.Text = "Következő";
            NextButton.Click += Round;
            NextButton.Click -= NextButton_Click;
            if (Automatic.Checked)
            {
                _round = new();
                _round.Interval = 1000;
                _round.Tick += Round;
                _round.Start();
                NextButton.Enabled = false;
            }
            else
            {
                Round(this, EventArgs.Empty);
            }
        }

        private int activep = 1;

        public void Round(Object? sender, EventArgs e)
        {
            ClearScreen();
            if (!_model.PerformNext())
            {
                if (_round != null)
                {
                    _round.Tick += NewRoundEvent;
                    _round.Tick -= Round;
                }
                NextButton.Click += NewRoundEvent;
                NextButton.Click -= Round;
            }
            else if (Automatic.Checked && _round == null && Active)
            {
                _round = new();
                _round.Interval = 1000;
                _round.Tick += Round;
                _round.Start();
                NextButton.Enabled = false;
            }
        }

        private void ClearScreen()
        {
            if (_model.BoardSize != null)
            {
                int n = (int)_model.BoardSize;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        _grid[i, j].BackColor = backcolor;
                    }
            }
        }

        private void NewRoundEvent(Object? sender, EventArgs e)
        {
            NewRound();
        }

        public void NewRound()
        {
            Active = true;
            ClearScreen();
            _round?.Stop();
            _round = null;
            NextButton.Click -= NewRoundEvent;
            NextButton.Click -= Round;
            NextButton.Click += NextButton_Click;
            NextButton.Enabled = false;
            NextButton.Text = "Kezdés";
            SetOrders.Enabled = true;
            SetOrders.Text = "Rögzítés";
            input.Enabled = true;

            _menuFileSaveGame.Enabled = true;
            activep = 1;
            activeplr.Text = activep.ToString() + "es";
        }

        #endregion

        #region buttons

        public void SetOrders_Click(object sender, EventArgs e)
        {
            String[] inp = input.Lines;
            inp = inp.SkipWhile(x => x.Length == 0).TakeWhile(x => x.Length > 0).ToArray();
            try
            {

                if (activep == 1)
                {
                    //if (_model.N != null)// cannot happen, but compiler likes it
                    _model.Plr1Parse(inp); // may throw
                    activep = 2;
                    activeplr.Text = activep.ToString() + "es";

                }
                else if (activep == 2)
                {
                    //if (_model.N != null)// cannot happen, but compiler likes it
                    _model.Plr2Parse(inp); // may throw
                    activeplr.Text = "";
                    activep = 3;
                    SetOrders.Enabled = false;
                    input.Enabled = false;
                    NextButton.Enabled = true;
                }
                input.Text = "";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Pontosan " + Pig.ORDERSIZE + " darab sorban írd le a parancsaid!" + Environment.NewLine, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Automatic_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(CheckBox))
            {
                if (!Automatic.Checked)
                {
                    if (_round != null)
                    {
                        _round.Stop();
                        NextButton.Enabled = true;
                        _round = null;
                    }
                }
            }
        }

        private void NextButton_Click(object? sender, EventArgs e)
        {
            StartRound();
        }

        #endregion buttons

        #region stuff

        private int Max(int v1, int v2)
        {
            return v1 > v2 ? v1 : v2;
        }

        private int Min(int width, int height)
        {
            return width < height ? width : height;
        }

        #endregion stuff
    }
}