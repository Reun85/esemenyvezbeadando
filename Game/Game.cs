using RobotPigs.Model;
using RobotPigs.Pers;

namespace RobotPigs.WFA
{
    public partial class Game : Form
    {
        private IRobotPigsDataAccess _dataAccess;
        private GameModel _model;
        private Label[,] _grid = new Label[0, 0];
        private System.Windows.Forms.Timer? _round;
        public int N { get; set; } = 6;
        public bool active { get; set; } = false;
        public static readonly String[] orientation = { "⇑", "⇒", "⇓", "⇐" };

        public static readonly Color[] colors = { Color.OrangeRed, Color.GreenYellow, Color.SkyBlue };
        public static readonly Color backcolor = Color.Gray;

        #region setup

        private void MenuFileNewGame_Click(Object sender, EventArgs e)
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

        private void NewGame()
        {
            NumberInp inp = new NumberInp(this);
            if (inp.ShowDialog() == DialogResult.OK)
            {
                splitContainer1.Panel2.Enabled = true;
                _menuFileSaveGame.Enabled = true;
                bool same = _model.Board != null && _model.Board.n == N;
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


        public bool isValidSize(int n)
        {
            if (n < 3)
            {
                throw new ArgumentException("Kettőnél nagyobb számot adjatok meg!");
            }
            int smaller = min(GameArea.Size.Width, GameArea.Size.Height);
            int size = max(smaller / n, 40);
            if (size < 20 || size * n > smaller)
            {
                throw new ArgumentException("Túl nagy számot választottatok!");
            }
            return true;
        }
        private bool GenerateTable()
        {
            // Board shouldn't be null here.
            if (_model.Board == null)
            {
                return false; // maybe recursive
            }
            int n = N;
            _grid = new Label[n, n];
            int smaller = min(GameArea.Size.Width, GameArea.Size.Height);
            int size = max(smaller / n, 40);
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
                    _grid[i, j] = new Label();
                    _grid[i, j].Location = new Point(startingposx + size * i, startingposy + size * j);
                    _grid[i, j].Size = new Size(size, size);
                    _grid[i, j].Font = new Font(FontFamily.GenericMonospace, size * 3 / 4, FontStyle.Bold);
                    _grid[i, j].BorderStyle = BorderStyle.FixedSingle;
                    _grid[i, j].Parent = GameArea;
                }
            return true;
        }

        private bool SetupTable()
        {
            if (_model.Board == null)
            {
                return false; // maybe recursive
            }
            int n = _model.Board.n;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    _grid[i, j].BackColor = backcolor;
                    _grid[i, j].Text = "";
                }
            Pos p1 = _model.Board.Plr1.Pos;
            Label plr1 = _grid[p1.x, p1.y];
            plr1.Text = orientation[(int)p1.dir];
            plr1.ForeColor = colors[0];
            Plr1Health.Text = _model.Board.Plr1.Hp.ToString();

            Pos p2 = _model.Board.Plr2.Pos;
            Label plr2 = _grid[p2.x, p2.y];
            plr2.Text = orientation[(int)p2.dir];
            plr2.ForeColor = colors[1];
            Plr2Health.Text = _model.Board.Plr2.Hp.ToString();

            return true;
        }

        #endregion setup

        #region modelevents

        private void Hits(object? sender, EventData e)
        {
            if (e.P != null && e.Pos != null && _model.Board != null)
            {
                int n = _model.Board.n;
                Pos p = (Pos)e.Pos;
                for (int i = p.x - 1; i <= p.x + 1; i++)
                {
                    for (int j = p.y - 1; j <= p.y + 1; j++)
                    {
                        if ((i != p.x || j != p.y) && i >= 0 && i < n && j >= 0 && j < n)
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
            if (e.P != null && e.Pos != null && _model.Board != null)
            {
                int n = _model.Board.n;
                Pos p = (Pos)e.Pos;
                int dir = (int)p.dir;

                if (dir == 1 || dir == 3)
                {
                    int change = dir == 1 ? 1 : -1;
                    int j = p.y;
                    for (int i = p.x; i >= 0 && i < n; i += change)
                    {
                        if (i != p.x || j != p.y)
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
                    int i = p.x;
                    for (int j = p.y; j >= 0 && j < n; j += change)
                    {
                        if (i != p.x || j != p.y)
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
                        l = _grid[p.x, p.y];
                        l.Text = "";//forecolor doesn't need to be changed
                    }
                    l = _grid[newpos.x, newpos.y];
                    l.Text = orientation[(int)newpos.dir];
                    l.ForeColor = colors[e.Id - 1];
                }
            }
        }

        private void Game_GameOver(Object? sender, EventData e)
        {
            active = false;
            _menuFileSaveGame.Enabled = false;
            if (_round != null)
                _round.Stop();
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



        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                    splitContainer1.Panel2.Enabled = true;
                    N = _model.Board!.n;
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


        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
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
        private void StartRound()
        {
            _menuFileSaveGame.Enabled = false;
            button2.Enabled = false;
            input.Enabled = false;
            _model.PreparetoPerform();
            NextButton.Text = "Következő";
            NextButton.Click += Round;
            NextButton.Click -= NextButton_Click;
            if (Automatic.Checked)
            {
                _round = new System.Windows.Forms.Timer();
                _round.Interval = 1000;
                _round.Tick += Round;
                _round.Start();
                NextButton.Enabled = false;
            }
        }

        private int activep = 1;

        private void Round(Object? sender, EventArgs e)
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
            else if (Automatic.Checked && _round == null && active)
            {
                _round = new System.Windows.Forms.Timer();
                _round.Interval = 1000;
                _round.Tick += Round;
                _round.Start();
                NextButton.Enabled = false;
            }
        }

        private void ClearScreen()
        {
            if (_model.Board != null)
            {
                int n = _model.Board.n;
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

        private void NewRound()
        {
            active = true;
            ClearScreen();
            if (
                _round != null)
                _round.Stop();
            _round = null;
            NextButton.Click -= NewRoundEvent;
            NextButton.Click -= Round;
            NextButton.Click += NextButton_Click;
            NextButton.Enabled = false;
            NextButton.Text = "Kezdés";
            button2.Enabled = true;
            button2.Text = "Rögzítés";
            input.Enabled = true;

            _menuFileSaveGame.Enabled = true;
            activep = 1;
            activeplr.Text = activep.ToString() + "es";
        }

        #endregion

        #region buttons

        private void button2_Click(object sender, EventArgs e)
        {
            String[] inp = input.Lines;
            inp = inp.TakeWhile(x => x.Length > 0).ToArray();
            try
            {
                Pig.validate(inp);
                input.Text = "";
                if (activep == 1)
                {
                    activep = 2;
                    activeplr.Text = activep.ToString() + "es";
                    if (_model.Board != null)// cannot happen, but compiler likes it
                        _model.Board.Plr1.parse(inp);
                }
                else if (activep == 2)
                {
                    if (_model.Board != null)// cannot happen, but compiler likes it
                        _model.Board.Plr2.parse(inp);
                    activeplr.Text = "";
                    activep = 3;
                    button2.Enabled = false;
                    NextButton.Enabled = true;
                }
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

        private void Automatic_CheckedChanged(object sender, EventArgs e)
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

        private int max(int v1, int v2)
        {
            return v1 > v2 ? v1 : v2;
        }

        private int min(int width, int height)
        {
            return width < height ? width : height;
        }

        #endregion stuff

        private void Plr2Health_Click(object sender, EventArgs e)
        {

        }
    }
}