using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05_DamkaGame
{
    public class FormGameSettings : Form
    {
        private Label m_LabelBoardSize;
        private Label m_LabelPlayers;
        private Label m_LabelFirstPlayer;
        private TextBox m_TextBoxFirstPlayerName;
        private CheckBox m_CheckBoxSecondPlayer;
        private TextBox m_TextBoxSecondPlayerName;
        private RadioButton m_RadioButtonSmall;
        private RadioButton m_RadioButtonMedium;
        private RadioButton m_RadioButtonLarge;
        private Button m_ButtonDone;
        private int m_BoardSize;
        private int m_numOfHumanPlayers;

        public FormGameSettings()
        {
            this.Size = new Size(320, 250);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Microsoft Sans Serif", 10.5F);
            this.Text = "Game Settings";
            m_BoardSize = Board.k_SmallSizeOfBoardGame;
            m_numOfHumanPlayers = 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitControls();
        }

        private void InitControls()
        {
            m_LabelBoardSize = new Label();
            m_LabelBoardSize.Text = "Board Size:";
            m_LabelBoardSize.AutoSize = true;
            m_LabelBoardSize.Location = new Point(10, 20);
            this.Controls.Add(m_LabelBoardSize);

            m_RadioButtonSmall = new RadioButton();
            m_RadioButtonSmall.Text = "6 X 6";
            m_RadioButtonSmall.AutoSize = true;
            m_RadioButtonSmall.Checked = true;
            m_RadioButtonSmall.Location = new Point(m_LabelBoardSize.Left + 20, m_LabelBoardSize.Bottom);
            m_RadioButtonSmall.CheckedChanged += new EventHandler(m_RadioButtonSmall_CheckedChanged);
            m_RadioButtonSmall.TabIndex = 0;
            this.Controls.Add(m_RadioButtonSmall);

            m_RadioButtonMedium = new RadioButton();
            m_RadioButtonMedium.Text = "8 X 8";
            m_RadioButtonMedium.AutoSize = true;
            m_RadioButtonMedium.Location = new Point(m_RadioButtonSmall.Right + 12, m_LabelBoardSize.Bottom);
            m_RadioButtonMedium.CheckedChanged += new EventHandler(m_RadioButtonMedium_CheckedChanged);
            m_RadioButtonMedium.TabIndex = 1;
            this.Controls.Add(m_RadioButtonMedium);

            m_RadioButtonLarge = new RadioButton();
            m_RadioButtonLarge.Text = "10 X 10";
            m_RadioButtonLarge.AutoSize = true;
            m_RadioButtonLarge.Location = new Point(m_RadioButtonMedium.Right + 12, m_LabelBoardSize.Bottom);
            m_RadioButtonLarge.CheckedChanged += new EventHandler(m_RadioButtonLarge_CheckedChanged);
            m_RadioButtonLarge.TabIndex = 2;
            this.Controls.Add(m_RadioButtonLarge);

            m_LabelPlayers = new Label();
            m_LabelPlayers.Text = "Players:";
            m_LabelPlayers.AutoSize = true;
            m_LabelPlayers.Location = new Point(m_LabelBoardSize.Left, m_RadioButtonSmall.Bottom);
            this.Controls.Add(m_LabelPlayers);

            m_LabelFirstPlayer = new Label();
            m_LabelFirstPlayer.Text = "Player 1:";
            m_LabelFirstPlayer.AutoSize = true;
            m_LabelFirstPlayer.Location = new Point(m_LabelBoardSize.Left + 20, m_LabelPlayers.Bottom + 15);
            this.Controls.Add(m_LabelFirstPlayer);

            m_TextBoxFirstPlayerName = new TextBox();
            m_TextBoxFirstPlayerName.Location = new Point(m_LabelFirstPlayer.Right + 20, m_LabelFirstPlayer.Top);
            m_TextBoxFirstPlayerName.Width += 80;
            m_TextBoxFirstPlayerName.TabIndex = 3;
            this.Controls.Add(m_TextBoxFirstPlayerName);

            m_CheckBoxSecondPlayer = new CheckBox();
            m_CheckBoxSecondPlayer.Text = "Player2";
            m_CheckBoxSecondPlayer.AutoSize = true;
            m_CheckBoxSecondPlayer.Location = new Point(m_LabelFirstPlayer.Left, m_LabelFirstPlayer.Bottom + 15);
            m_CheckBoxSecondPlayer.CheckedChanged += new EventHandler(m_CheckBoxPlayer2_CheckedChanged);
            m_CheckBoxSecondPlayer.TabIndex = 4;
            this.Controls.Add(m_CheckBoxSecondPlayer);

            m_TextBoxSecondPlayerName = new TextBox();
            m_TextBoxSecondPlayerName.Text = "[Computer]";
            m_TextBoxSecondPlayerName.Enabled = false;
            m_TextBoxSecondPlayerName.MaxLength = 20;
            m_TextBoxSecondPlayerName.Location = new Point(m_TextBoxFirstPlayerName.Left, m_CheckBoxSecondPlayer.Top);
            m_TextBoxSecondPlayerName.Width = m_TextBoxFirstPlayerName.Width;
            m_TextBoxSecondPlayerName.TabIndex = 5;
            this.Controls.Add(m_TextBoxSecondPlayerName);

            m_ButtonDone = new Button();
            m_ButtonDone.Text = "Done";
            m_ButtonDone.Location = new Point(m_TextBoxSecondPlayerName.Left + m_TextBoxSecondPlayerName.Width - m_ButtonDone.Width, m_TextBoxSecondPlayerName.Bottom + 15);
            m_ButtonDone.Click += new EventHandler(m_ButtonDone_Click);
            m_ButtonDone.TabIndex = 6;
            this.Controls.Add(m_ButtonDone);
        }

        private void m_RadioButtonLarge_CheckedChanged(object sender, EventArgs e)
        {
            m_BoardSize = Board.k_LargeSizeOfBoardGame;
        }

        private void m_RadioButtonMedium_CheckedChanged(object sender, EventArgs e)
        {
            m_BoardSize = Board.k_MediumSizeOfBoardGame;
        }

        private void m_RadioButtonSmall_CheckedChanged(object sender, EventArgs e)
        {
            m_BoardSize = Board.k_SmallSizeOfBoardGame;
        }

        private void m_ButtonDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void m_CheckBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if (m_CheckBoxSecondPlayer.Checked == true)
            {
                m_TextBoxSecondPlayerName.Enabled = true;
                m_TextBoxSecondPlayerName.Text = string.Empty;
                m_numOfHumanPlayers = 2;
            }
            else
            {
                m_TextBoxSecondPlayerName.Enabled = false;
                m_TextBoxSecondPlayerName.Text = "[Computer]";
                m_numOfHumanPlayers = 1;
            }
        }

        public int BoardSize 
        { 
            get 
            {
                return m_BoardSize;
            }  
        }

        public string FirstPlayerName
        {
            get
            {
                return m_TextBoxFirstPlayerName.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return m_TextBoxSecondPlayerName.Text;
            }
        }

        public int NumOfHumanPlayers
        {
            get
            {
                return m_numOfHumanPlayers;
            }

            set
            {
                m_numOfHumanPlayers = value;
            }
        }
    }
}