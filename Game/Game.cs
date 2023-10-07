using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RobotPigs.Pers;
using RobotPigs.Model;
using System.Diagnostics.Eventing.Reader;

namespace RobotPigs.WFA
{
    public partial class Game : Form
    {
        private IRobotPigsDataAccess _dataAccess;
        private GameModel _model;
        private Button[,] _grid;
        public Game()
        {
            InitializeComponent();

            _dataAccess = new RobotPigsDataAccess();

            _model = new GameModel(_dataAccess);

            _model.Loses += Game_GameOver;
            _model.HpChange += HPChange;

            _round = new System.Windows.Forms.Timer();
            _round.Interval = 1000;
            _round.Tick += new EventHandler(Timer_Tick);

            //GenerateTable();
            //SetupMenus();

            // Default is 6x6, can be easily changed.
            _model.NewGame(6);
            //SetupTable();

        }


        private void Game_GameOver(Object? sender, EventData e)
        {

            //foreach (Button button in _grid)
            //    button.Enabled = false;
            _round.Stop();

            //_menuFileSaveGame.Enabled = false;

            if(e.Id == 3 && MessageBox.Show("Döntetlen!" + Environment.NewLine + "Szeretnétek új játékot indítani?",
                            "Harcos robotmalacok csatája",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No || (e.Id!= 3&& MessageBox.Show("Gratulálok " + (e.Id == 1 ? "egyes" : "kettes") + " játékos győztél!" + Environment.NewLine + "Szeretnétek új játékot indítani?",
                            "Harcos robotmalacok csatája",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
            {
                Close();
            }
        }


        private void NewGame()
        {
            //_model.NewGame();
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

        private void MenuFileNewGame_Click(Object sender, EventArgs e)
        {
            _menuFileSaveGame.Enabled = true;
            NewGame();
            //SetupTable();
            //SetupMenus();
        }

        private async void MenuFileLoadGame_Click(Object sender, EventArgs e)
        {

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játék betöltése
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (BoardDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    NewGame();
                    _menuFileSaveGame.Enabled = true;
                }

                //SetupTable();
            }
        }

        private async void MenuFileSaveGame_Click(Object sender, EventArgs e)
        {

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
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

            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Biztosan ki szeretnétek lépni?", "Harcos robotmalacok csatája", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ha igennel válaszol
                Close();
            }
        }

        private System.Windows.Forms.Timer? _round;
        private void StartRound()
        {
            button2.Enabled = false;
            input.Enabled = false;
            _model.PreparetoPerform();



            
            _round.Start();
        }
        private int activep = 1;

        private void Timer_Tick(Object? sender, EventArgs e)
        {
            if (!_model.PerformNext())
            {
               
                _round!.Stop();
                NewRound();
            }
        }

        private void NewRound()
        {
            button2.Enabled = true;
            button2.Text = "Rögzítés";
            input.Enabled = true;
            activep = 1;
            activeplr.Text = activep.ToString() + "es";
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (activep == 3)
                StartRound();
            else
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
                        if (_model.Board != null)// cannot but happen, but compiler likes it
                            _model.Board.Plr1.parse(inp);
                    }
                    else if (activep == 2)
                    {
                        if (_model.Board != null)// cannot but happen, but compiler likes it
                            _model.Board.Plr2.parse(inp);
                        activeplr.Text = "";
                        button2.Text = "Kezdés";
                        activep = 3;
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
            
        }
    }
}
