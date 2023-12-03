using RobotPigs.Persistence;
using RobotPigs.ViewModel;
using RobotPigs.Model;

namespace RobotPigs.View
{
    public partial class AppShell : Shell
    {
        #region Fields

        private IRobotPigsDataAccess _dataAccess;
        private readonly GameModel _gameModel;
        private readonly RobotPigsViewModel _viewModel;


        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

        #endregion

        #region Application methods

        public AppShell(IStore store,
            IRobotPigsDataAccess dataAccess,
            GameModel gameModel,
            RobotPigsViewModel viewModel)
        {
            InitializeComponent();

            // játék összeállítása
            _store = store;
            _dataAccess = dataAccess;
            _gameModel = gameModel;
            _viewModel = viewModel;

            _gameModel.GameOver += Model_GameOver;

            _viewModel.NewGame += ViewModel_NewGame;
            _viewModel.LoadGame += ViewModel_AsyncLoadGame;
            _viewModel.SaveGame += ViewModel_AsyncSaveGame;

            // a játékmentések kezelésének összeállítása
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        #endregion


        #region Model event handlers

        private async void Model_GameOver(object? sender, EventData e)
        {
            if (e.Id == 3)
            {
                await DisplayAlert("Harcos robotmalacok csatája","Döntetlen!","OK");
            }
            else
            {
                await DisplayAlert("Harcos robotmalacok csatája","Gratulálok " + (e.Id == 1 ? "zöld" : "piros") +
                                    " játékos győztél!","OK");


            }
        }
        #endregion

        #region ViewModel event handlers


        private void ViewModel_NewGame(object? sender, int size)
        {
            _gameModel.NewGame(size);
        }


        private async void ViewModel_AsyncLoadGame(object? sender,
                                                   System.EventArgs e)
        {

            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }
        private async void ViewModel_AsyncSaveGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        //private async void ViewModel_Exit(object? sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new SettingsPage
        //    {
        //        BindingContext = _viewModel
        //    });// átnavigálunk a beállítások lapra
        //}


        /// <summary>
        ///     Betöltés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await Navigation.PopAsync(); // visszanavigálunk a játék táblára
                await _gameModel.LoadGameAsync(e.Name);

                await DisplayAlert("Harcos robotmalacok csatája játék", "Sikeres betöltés.", "OK");

            }
            catch
            {
                await DisplayAlert("Harcos robotmalacok csatájaoku játék", "Sikertelen betöltés.", "OK");
            }
        }

        /// <summary>
        ///     Mentés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            

            try
            {
                // elmentjük a játékot
                await Navigation.PopAsync(); // visszanavigálunk
                await _gameModel.SaveGameAsync(e.Name);
                await DisplayAlert("Harcos robotmalacok csatája játék", "Sikeres mentés.", "OK");
            }
            catch
            {
                await DisplayAlert("Harcos robotmalacok csatája játék", "Sikertelen mentés.", "OK");
            }
        }

        #endregion
    }
}