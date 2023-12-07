namespace RobotPigs.View
{

    public partial class GamePage : ContentPage
    {
        private bool isLandscape = false;
        public bool IsLandscape { get { return isLandscape; } set { isLandscape = value; OnPropertyChanged(nameof(IsLandscape)); } }
        public GamePage()
        {
            InitializeComponent();
        }
        protected override void OnSizeAllocated(
Double width, Double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
                IsLandscape = true;
            else
                        IsLandscape = false;
        }
    }
}