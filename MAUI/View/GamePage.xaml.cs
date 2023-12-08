namespace RobotPigs.View
{

    public partial class GamePage : ContentPage
    {
        double width, height;
        public GamePage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;

                if (width > height)
                {
                    _stackLayout.Orientation = StackOrientation.Horizontal;
                }
                else
                {
                    _stackLayout.Orientation = StackOrientation.Vertical;
                }
            }
        }
    }
}