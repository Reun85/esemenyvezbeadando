using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using RobotPigs.Model;
using RobotPigs.Persistence;
using RobotPigs.WPF;
using RobotPigs.WPF.View;
using Microsoft.Win32;

namespace RobotPigs.WPF
{
    public partial class App : Application
    {
        #region Fields

        private GameModel _model = null!;
        private ViewModel _viewModel = null!;
        private MainWindow _mainView = null!;
        private IRobotPigsDataAccess _dataAccess = null!;

        #endregion

        #region Constructors

        public App() { Startup += new StartupEventHandler(App_Startup); }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _dataAccess = new RobotPigsDataAccess();

            // modell létrehozása
            _model = new GameModel(_dataAccess);
            _model.GameOver += new EventHandler<EventData>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new ViewModel(_model);
            _viewModel.NewGame += ViewModel_NewGame;
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_AsyncLoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_AsyncSaveGame);

            // nézet létrehozása
            _mainView = new MainWindow();
            _mainView.DataContext = _viewModel;
            _mainView.Closing += new System.ComponentModel.CancelEventHandler(
                View_Closing);
            _mainView.Show();
        }

        #endregion

        #region View event handlers

        private void View_Closing(object? sender, CancelEventArgs e)
        {

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Harcos robotmalacok",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; 
            }
        }

        #endregion

        #region ViewModel event handlers


        private void ViewModel_NewGame(object? sender, int size)
        {
            _model.NewGame(size);
        }


        private async void ViewModel_AsyncLoadGame(object? sender,
                                                   System.EventArgs e)
        {

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Harcos robotmalacok játék betöltése";
                openFileDialog.Filter = "Harcos robotmalacok játék|*.dat";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _model.LoadGameAsync(openFileDialog.FileName);
                }
            }
            catch (BoardDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Harcos robotmalacok",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ViewModel_AsyncSaveGame(object? sender, EventArgs e)
        {

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); 
                saveFileDialog.Title = "Harcos robotmalacok játék betöltése";
                saveFileDialog.Filter = "Harcos robotmalacok játék|*.dat";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (BoardDataException)
                    {
                        MessageBox.Show(
                            "Játék mentése sikertelen!" + Environment.NewLine +
                                "Hibás az elérési út, vagy a könyvtár nem írható.",
                            "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Harcos robotmalacok",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _mainView.Close();
        }

        #endregion

        #region Model event handlers

        private void Model_GameOver(object? sender, EventData e)
        {
            if (e.Id == 3)
            {
                MessageBox.Show("Döntetlen!", "Harcos robotmalacok csatája",
                                MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Gratulálok " + (e.Id == 1 ? "zöld" : "piros") +
                                    " játékos győztél!",
                                "Harcos robotmalacok csatája", MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);

               
            }
        }
        #endregion
    }
}
