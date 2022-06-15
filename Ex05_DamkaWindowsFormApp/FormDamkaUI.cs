using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05_DamkaGame
{
    public class FormDamkaUI : Form
    {
        private LogicManager m_LogicManager;
        private FormGameSettings m_FormGameSettings;
        private ButtonDamka[,] m_ButtonsBoard;
        private int m_BoardSize;
        private Label m_LabelPlayersDetails;
        private Label m_LableCurrentPlayerTurn;
        private ButtonDamka m_ButtonDamkaHumanPlayerFromMove;
        private ButtonDamka m_ButtonDamkaHumanPlayerDestinationMove;
        private Board m_Board;

        public FormDamkaUI()
        {
            m_LogicManager = new LogicManager();
            m_FormGameSettings = new FormGameSettings();
            this.Text = "Damka";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            m_FormGameSettings.ShowDialog();

            if (this.m_FormGameSettings.DialogResult == DialogResult.OK)
            {
                getParametersFromGameSettings();                
                initiateControls();
                m_Board.CellChanged += new BoardListener(m_LogicManager_CellChanged);
            }
            else
            {
                this.Close();
            }
        }

        private void getParametersFromGameSettings()
        {
            int numOfHumanPlayers = m_FormGameSettings.NumOfHumanPlayers;

            m_BoardSize = m_FormGameSettings.BoardSize;
            initiateGame(m_FormGameSettings.FirstPlayerName, m_FormGameSettings.SecondPlayerName, numOfHumanPlayers);
        }

        private void initiateGame(string i_FirstPlayer, string i_SecondPlayer, int i_numOfHumanPlayer)
        {
            m_LogicManager.InitiateGame(i_FirstPlayer, i_SecondPlayer, i_numOfHumanPlayer, m_BoardSize);
            m_Board = m_LogicManager.Board;
            m_ButtonsBoard = new ButtonDamka[m_BoardSize, m_BoardSize];
        }

        private void initiateControls()
        {
            createDamkaButtonBoard();
            setGameWindowSize();

            m_LabelPlayersDetails = new Label();
            m_LabelPlayersDetails.Location = new Point(30, 20);
            m_LabelPlayersDetails.Font = new Font("Microsoft Sans Serif", 10.5F, FontStyle.Bold);
            m_LabelPlayersDetails.Width = m_Board.Cols * ButtonDamka.k_Width;
            m_LabelPlayersDetails.BackColor = Color.Turquoise;
            m_LabelPlayersDetails.TextAlign = ContentAlignment.TopCenter;            
            this.Controls.Add(m_LabelPlayersDetails);

            m_LableCurrentPlayerTurn = new Label();
            m_LableCurrentPlayerTurn.Location = new Point(30, this.Height - 70);
            m_LableCurrentPlayerTurn.Font = new Font("Microsoft Sans Serif", 10.5F, FontStyle.Bold);
            m_LableCurrentPlayerTurn.AutoSize = true;
            this.Controls.Add(m_LableCurrentPlayerTurn);

            displayPlayersDetailsInLabel();
            displayCurrentPlayerTurnInLabel();
        }

        private void createDamkaButtonBoard()
        {
            for (int indexRow = 0; indexRow < m_Board.Rows; indexRow++)
            {
                for (int indexCol = 0; indexCol < m_Board.Cols; indexCol++)
                {
                    bool isEnabled = true;

                    if (m_Board[indexRow, indexCol].Equals(eDamkaCell.Illegal) == true)
                    {
                        isEnabled = false;
                    }

                    m_ButtonsBoard[indexRow, indexCol] = new ButtonDamka(m_Board.CellToStr(m_Board[indexRow, indexCol]), isEnabled);

                    ButtonDamka currentButtonDamka = m_ButtonsBoard[indexRow, indexCol];
                    int x = (currentButtonDamka.Size.Width * indexCol) + 30;
                    int y = (currentButtonDamka.Size.Height * indexRow) + 50;

                    currentButtonDamka.Location = new Point(x, y);
                    currentButtonDamka.LogicLocationOnBoard = new Point(indexRow, indexCol);
                    currentButtonDamka.Click += new EventHandler(buttonDamka_Click);
                    this.Controls.Add(currentButtonDamka);
                }
            }
        }

        private void setGameWindowSize()
        {
            int windowWidth = (ButtonDamka.k_Width * m_Board.Cols) + 80;
            int windowHeight = (ButtonDamka.k_Height * m_Board.Rows) + 130;

            this.Size = new Size(windowWidth, windowHeight);
        }

        private void displayPlayersDetailsInLabel()
        {
            string blackPlayerName = m_LogicManager.BlackPlayer.Name;
            string whitePlayerName = m_LogicManager.WhitePlayer.Name;
            int blackPlayerWinNum = m_LogicManager.BlackPlayer.WinningNum;
            int whitePlayerWinNum = m_LogicManager.WhitePlayer.WinningNum;

            m_LabelPlayersDetails.Text = string.Format("{0}:{1}                {2}:{3}", blackPlayerName, blackPlayerWinNum, whitePlayerName, whitePlayerWinNum);
        }

        private void displayCurrentPlayerTurnInLabel()
        {
            string playerName = m_LogicManager.CurrentPlayer.Name;
            string playerColor = m_LogicManager.CurrentPlayer.Color.ToString();

            m_LableCurrentPlayerTurn.Text = string.Format("{0}'s Turn ({1}) ", playerName, playerColor);
        }

        private void m_LogicManager_CellChanged(Point i_PieceLocation, string i_NewPieceStr)
        {
            m_ButtonsBoard[i_PieceLocation.X, i_PieceLocation.Y].SetButtonDamkaText(i_NewPieceStr);
        }

        private void buttonDamka_Click(object sender, EventArgs e)
        {
            ButtonDamka buttonDamkaClicked = sender as ButtonDamka;

            if (buttonDamkaClicked.BackColor == Color.White)
            {
                buttonDamkaClicked.BackColor = Color.LightSkyBlue;

                if (m_ButtonDamkaHumanPlayerFromMove == null)
                {
                    m_ButtonDamkaHumanPlayerFromMove = buttonDamkaClicked;
                }
                else if (m_ButtonDamkaHumanPlayerDestinationMove == null && buttonDamkaClicked != m_ButtonDamkaHumanPlayerFromMove)
                {
                    m_ButtonDamkaHumanPlayerDestinationMove = buttonDamkaClicked;
                    makeHumanPlayerMove();
                }
            }
            else
            {
                buttonDamkaClicked.BackColor = Color.White;
                m_ButtonDamkaHumanPlayerFromMove = null;
            }
        }

        private void makeHumanPlayerMove()
        {
            string Move = setHumanPlayerMoveToStr();
            eMoveStauts moveStatus = m_LogicManager.MakeHumanPlayerMove(Move);

            checkHumanPlayerMoveStatus(moveStatus);
        }

        private eRoundStatus checkIfRoundOver()
        {
            eRoundStatus roundStatus = m_LogicManager.CheckRoundStatus();

            if (roundStatus == eRoundStatus.Tie || roundStatus == eRoundStatus.Win)
            {
                displayRoundOver(m_LogicManager.WinnerPlayer, m_LogicManager.GetOpponentPlayer(m_LogicManager.WinnerPlayer), roundStatus);
            }

            return roundStatus;
        }

        private void initRoundWithTheSameSetting()
        {
            m_LogicManager.InitRoundWithTheSameSetting();
            clearButtonDamkaGameBoard();
            displayPlayersDetailsInLabel();
            displayCurrentPlayerTurnInLabel();
        }

        private void clearButtonDamkaGameBoard()
        {
            for (int indexRow = 0; indexRow < m_Board.Rows; indexRow++)
            {
                for (int indexCol = 0; indexCol < m_Board.Cols; indexCol++)
                {
                    m_ButtonsBoard[indexRow, indexCol].Text = m_Board.CellToStr(m_Board[indexRow, indexCol]);
                }
            }
        }

        private void displayRoundOver(Player i_Winner, Player i_Loser, eRoundStatus i_RoundStatus)
        {
            if (i_RoundStatus == eRoundStatus.Win)
            {
                if (MessageBox.Show(string.Format("{0} WON! {1}Another Round?", i_Winner.Name, Environment.NewLine), "Damka", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    initRoundWithTheSameSetting();
                }
                else
                {
                    this.Close();
                }
            }
            else if (i_RoundStatus == eRoundStatus.Tie)
            {
                if (MessageBox.Show(string.Format("Tie! {0}Another Round?", Environment.NewLine), "Damka", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    initRoundWithTheSameSetting();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private string setHumanPlayerMoveToStr()
        {
            string playerMove;
            Point fromLocation = m_ButtonDamkaHumanPlayerFromMove.LogicLocationOnBoard;
            Point destLocation = m_ButtonDamkaHumanPlayerDestinationMove.LogicLocationOnBoard;

            playerMove = MoveParser.ConvertIndexesMoveToStr(fromLocation.X, fromLocation.Y, destLocation.X, destLocation.Y);

            return playerMove;
        }

        private void checkHumanPlayerMoveStatus(eMoveStauts i_MoveStatus)
        {
            resetHumanPlayerMoveOnBoard();

            if (i_MoveStatus == eMoveStauts.Illegal)
            {
                MessageBox.Show("Illegal move, Please try agian.", "Damka");
            }
            else if (i_MoveStatus == eMoveStauts.Complete)
            {
                if (checkIfRoundOver() == eRoundStatus.NotOver)
                {
                    switchPlayerTurn();
                }
            }
        }

        private void resetHumanPlayerMoveOnBoard()
        {
            m_ButtonDamkaHumanPlayerFromMove.BackColor = Color.White;
            m_ButtonDamkaHumanPlayerDestinationMove.BackColor = Color.White;
            m_ButtonDamkaHumanPlayerFromMove = null;
            m_ButtonDamkaHumanPlayerDestinationMove = null;
        }

        private void switchPlayerTurn()
        {
            m_LogicManager.SwitchPlayerTurn();
            displayCurrentPlayerTurnInLabel();
            makeComuterMoveIfPlaying();
        }

        private void makeComuterMoveIfPlaying()
        {
            if (m_LogicManager.CurrentPlayer.IsComputer == true)
            {
                m_LogicManager.MakeComputerPlayerMove();

                if (checkIfRoundOver() == eRoundStatus.NotOver)
                {
                    m_LogicManager.SwitchPlayerTurn();
                    displayCurrentPlayerTurnInLabel();
                }
            }
        }
    }
}