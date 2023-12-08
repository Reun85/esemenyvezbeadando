namespace RobotPigs.View
{

    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
        }
        double width, height;
        protected override void OnSizeAllocated(
Double width, Double height)
        // megkapjuk az aktuális
        // szélességet/magasságot
        {
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                base.OnSizeAllocated(width, height);
                // orientáció meghatározása
                if (width > height)
                {
                    _pageLayout.Orientation = StackOrientation.Horizontal;
                }
                else {
                    _pageLayout.Orientation = StackOrientation.Vertical;
            }
               
            } 
        }
    }
}