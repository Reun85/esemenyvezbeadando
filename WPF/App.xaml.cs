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
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private GameModel _model = null!;
        private ViewModel _viewModel = null!;
        private MainWindow _view = null!;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _model = new GameModel(new RobotPigsDataAccess(), 4);
            _model.Loses += new EventHandler<EventData>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new ViewModel(_model);
            _viewModel.NewGame += ViewModel_NewGame;
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object? sender, CancelEventArgs e)
        {

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Sudoku", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

            }
        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object? sender, int size)
        {
            _model.NewGame(size);
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Harcos robotmalacok játék betöltése";
                openFileDialog.Filter = "Harcos robotmalacok játék|*.dat";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await _model.LoadGameAsync(openFileDialog.FileName);
                }
            }
            catch (BoardDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Harcos robotmalacok", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Harcos robotmalacok játék betöltése";
                saveFileDialog.Filter = "Harcos robotmalacok játék|*.dat";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (BoardDataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Harcos robotmalacok", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        #endregion

        #region Model event handlers

        private void Model_GameOver(object? sender, EventData e)
        {
            if (e.Id == 3)
            {
                MessageBox.Show("Döntetlen!",
                            "Harcos robotmalacok csatája",
                            MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Gratulálok " + (e.Id == 1 ? "zöld" : "piros") + " játékos győztél!",
                        "Harcos robotmalacok csatája",
                        MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);

                #endregion
            }
        }
    }
}