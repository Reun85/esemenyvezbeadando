using RobotPigs.Persistence;
using RobotPigs.ViewModel;
using RobotPigs.Model;

namespace RobotPigs.View
{

    public partial class App : Application
    {
        /// <summary>
        /// Erre az útvonalra mentjük a félbehagyott játékokat
        /// </summary>
        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly IRobotPigsDataAccess _dataAccess;
        private readonly GameModel _gameModel;
        private readonly IStore _store;
        private readonly RobotPigsViewModel _viewModel;

        public App()
        {
            InitializeComponent();

            _store = new RobotPigsStore();
            _dataAccess = new RobotPigsDataAccess(FileSystem.AppDataDirectory);

            _gameModel = new GameModel(_dataAccess);
            _viewModel = new RobotPigsViewModel(_gameModel);

            _appShell = new AppShell(_store, _dataAccess, _gameModel, _viewModel)
            {
                BindingContext = _viewModel
            };
            MainPage = _appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            // az alkalmazás indításakor
            window.Created += (s, e) =>
            {
            };

            // amikor az alkalmazás fókuszba kerül
            window.Activated += (s, e) =>
            {
                if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                    return;

                Task.Run(async () =>
                {
                    // betöltjük a felfüggesztett játékot, amennyiben van
                    try
                    {
                        await _gameModel.LoadGameAsync(SuspendedGameSavePath);

                    }
                    catch
                    {
                    }
                });
            };

            // amikor az alkalmazás fókuszt veszt
            window.Deactivated += (s, e) =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // elmentjük a jelenleg folyó játékot
                        await _gameModel.SaveGameAsync(SuspendedGameSavePath);
                    }
                    catch
                    {
                    }
                });
            };

            return window;
        }
    }
}