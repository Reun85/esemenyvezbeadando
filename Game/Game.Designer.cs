namespace RobotPigs.WFA
{
    partial class Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _menuStrip = new MenuStrip();
            _menuFile = new ToolStripMenuItem();
            _menuFileNewGame = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            _menuFileLoadGame = new ToolStripMenuItem();
            _menuFileSaveGame = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            _menuFileExit = new ToolStripMenuItem();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            splitContainer1 = new SplitContainer();
            GameArea = new Panel();
            input = new TextBox();
            Automatic = new CheckBox();
            NextButton = new Button();
            activeplr = new Label();
            label2 = new Label();
            SetOrders = new Button();
            Plr2Health = new Label();
            Plr1Health = new Label();
            label3 = new Label();
            label1 = new Label();
            _menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.ImageScalingSize = new Size(20, 20);
            _menuStrip.Items.AddRange(new ToolStripItem[] { _menuFile });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Padding = new Padding(11, 5, 0, 5);
            _menuStrip.Size = new Size(610, 29);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            _menuFile.DropDownItems.AddRange(new ToolStripItem[] { _menuFileNewGame, toolStripMenuItem1, _menuFileLoadGame, _menuFileSaveGame, toolStripMenuItem2, _menuFileExit });
            _menuFile.Name = "_menuFile";
            _menuFile.ShortcutKeys = Keys.F2;
            _menuFile.Size = new Size(45, 19);
            _menuFile.Text = "Játék";
            // 
            // _menuFileNewGame
            // 
            _menuFileNewGame.Name = "_menuFileNewGame";
            _menuFileNewGame.Size = new Size(160, 22);
            _menuFileNewGame.Text = "Új játék";
            _menuFileNewGame.Click += MenuFileNewGame_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(157, 6);
            // 
            // _menuFileLoadGame
            // 
            _menuFileLoadGame.Name = "_menuFileLoadGame";
            _menuFileLoadGame.Size = new Size(160, 22);
            _menuFileLoadGame.Text = "Játék betöltése...";
            _menuFileLoadGame.Click += MenuFileLoadGame_Click;
            // 
            // _menuFileSaveGame
            // 
            _menuFileSaveGame.Name = "_menuFileSaveGame";
            _menuFileSaveGame.Size = new Size(160, 22);
            _menuFileSaveGame.Text = "Játék mentése...";
            _menuFileSaveGame.Click += MenuFileSaveGame_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(157, 6);
            // 
            // _menuFileExit
            // 
            _menuFileExit.Name = "_menuFileExit";
            _menuFileExit.Size = new Size(160, 22);
            _menuFileExit.Text = "Kilépés";
            _menuFileExit.Click += MenuFileExit_Click;
            // 
            // _openFileDialog
            // 
            _openFileDialog.Filter = "Harcos Robotok tábla (*.dat)|*.dat";
            _openFileDialog.Title = "Harcos Robotok tábla betöltése";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.Filter = "Harcos Robotok tábla (*.dat)|*.dat";
            _saveFileDialog.Title = "Harcos Robotok tábla betöltése mentése";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 29);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(GameArea);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(input);
            splitContainer1.Panel2.Controls.Add(Automatic);
            splitContainer1.Panel2.Controls.Add(NextButton);
            splitContainer1.Panel2.Controls.Add(activeplr);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(SetOrders);
            splitContainer1.Panel2.Controls.Add(Plr2Health);
            splitContainer1.Panel2.Controls.Add(Plr1Health);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Size = new Size(610, 590);
            splitContainer1.SplitterDistance = 443;
            splitContainer1.TabIndex = 1;
            splitContainer1.TabStop = false;
            // 
            // GameArea
            // 
            GameArea.Location = new Point(88, 24);
            GameArea.Name = "GameArea";
            GameArea.Size = new Size(440, 380);
            GameArea.TabIndex = 3;
            // 
            // input
            // 
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Location = new Point(12, 27);
            input.Multiline = true;
            input.Name = "input";
            input.Size = new Size(105, 104);
            input.TabIndex = 1;
            // 
            // Automatic
            // 
            Automatic.AutoSize = true;
            Automatic.Location = new Point(283, 78);
            Automatic.Name = "Automatic";
            Automatic.Size = new Size(52, 19);
            Automatic.TabIndex = 4;
            Automatic.Text = "Auto";
            Automatic.UseVisualStyleBackColor = true;
            Automatic.CheckedChanged += Automatic_CheckedChanged;
            // 
            // NextButton
            // 
            NextButton.Location = new Point(273, 18);
            NextButton.Name = "NextButton";
            NextButton.Size = new Size(74, 54);
            NextButton.TabIndex = 3;
            NextButton.Text = "Kezdés";
            NextButton.UseVisualStyleBackColor = true;
            // 
            // activeplr
            // 
            activeplr.AutoSize = true;
            activeplr.Location = new Point(88, 9);
            activeplr.Name = "activeplr";
            activeplr.Size = new Size(24, 15);
            activeplr.TabIndex = 9;
            activeplr.Text = "1es";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(77, 15);
            label2.TabIndex = 8;
            label2.Text = "Aktív játékos:";
            // 
            // SetOrders
            // 
            SetOrders.Location = new Point(140, 50);
            SetOrders.Name = "SetOrders";
            SetOrders.Size = new Size(74, 54);
            SetOrders.TabIndex = 2;
            SetOrders.Text = "Rögzítés";
            SetOrders.UseVisualStyleBackColor = true;
            SetOrders.Click += SetOrders_Click;
            // 
            // Plr2Health
            // 
            Plr2Health.AutoSize = true;
            Plr2Health.Location = new Point(515, 89);
            Plr2Health.Name = "Plr2Health";
            Plr2Health.Size = new Size(13, 15);
            Plr2Health.TabIndex = 4;
            Plr2Health.Text = "3";
            // 
            // Plr1Health
            // 
            Plr1Health.AutoSize = true;
            Plr1Health.Location = new Point(515, 38);
            Plr1Health.Name = "Plr1Health";
            Plr1Health.Size = new Size(13, 15);
            Plr1Health.TabIndex = 3;
            Plr1Health.Text = "3";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(372, 89);
            label3.Name = "label3";
            label3.Size = new Size(126, 15);
            label3.TabIndex = 2;
            label3.Text = "Kettes játékos életereje";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(372, 38);
            label1.Name = "label1";
            label1.Size = new Size(124, 15);
            label1.TabIndex = 0;
            label1.Text = "Egyes játékos életereje";
            // 
            // Game
            // 
            ClientSize = new Size(610, 619);
            Controls.Add(splitContainer1);
            Controls.Add(_menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = _menuStrip;
            Margin = new Padding(5, 8, 5, 8);
            MaximizeBox = false;
            Name = "Game";
            Text = "Harcos robotmalacok csatája";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _menuFile;
        private System.Windows.Forms.ToolStripMenuItem _menuFileNewGame;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _menuFileLoadGame;
        private System.Windows.Forms.ToolStripMenuItem _menuFileSaveGame;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem _menuFileExit;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private SplitContainer splitContainer1;
        private Label label1;
        private Label Plr1Health;
        private Label label3;
        private Label Plr2Health;
        private Label activeplr;
        private Label label2;
        private Button SetOrders;
        private Panel GameArea;
        private Button NextButton;
        private CheckBox Automatic;
        private TextBox input;
    }
}