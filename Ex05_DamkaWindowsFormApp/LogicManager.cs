using System;
using System.Collections.Generic;

namespace Ex05_DamkaGame
{
    public class LogicManager
    {
        private readonly List<string> r_ValidMoves;
        private readonly List<string> r_ValidJumpMoves;
        private Board m_Board;
        private Player m_BlackPlayer;
        private Player m_WhitePlayer;
        private Player m_CurrentPlayer;
        private Player m_WinnerPlayer;

        public LogicManager()
        {
            r_ValidMoves = new List<string>();
            r_ValidJumpMoves = new List<string>();
        }

        public Player WinnerPlayer
        {
            get
            {
                return m_WinnerPlayer;
            }
        }

        public Player BlackPlayer
        {
            get
            {
                return m_BlackPlayer;
            }
        }

        public Player WhitePlayer
        {
            get
            {
                return m_WhitePlayer;
            }
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
        }

        public void InitiateGame(string i_FirstPlayer, string i_SecondPlayer, int i_numOfHumanPlayers, int i_BoardSize)
        {
            createGamePlayers(i_FirstPlayer, i_SecondPlayer, i_numOfHumanPlayers);
            m_Board = new Board(i_BoardSize);
            setPlayersCoins();
            setFirstPlayerInRound();
        }

        private void createGamePlayers(string i_FirstPlayerName, string i_SecondPlayerName, int i_numOfHumanPlayers)
        {
            bool isComputerPlaying = false;

            m_BlackPlayer = new Player(i_FirstPlayerName, ePlayerColor.Black_X, false);

            if (i_numOfHumanPlayers == 1)
            {
                isComputerPlaying = true;
            }

            m_WhitePlayer = new Player(i_SecondPlayerName, ePlayerColor.White_O, isComputerPlaying);
        }

        private void setFirstPlayerInRound()
        {
            m_CurrentPlayer = m_BlackPlayer;
        }

        public void InitRoundWithTheSameSetting()
        {
            resetPlayersSettings();
            m_Board.InitiateBoard();
            m_CurrentPlayer = m_BlackPlayer;
        }

        private void resetPlayersSettings()
        {
            setPlayersCoins();
            m_BlackPlayer.Points = 0;
            m_BlackPlayer.HasValidMoves = true;
            m_WhitePlayer.Points = 0;
            m_WhitePlayer.HasValidMoves = true;
        }

        public void MakeComputerPlayerMove()
        {
            string Move;
            DamkaRules.GetValidMoves(m_Board, m_CurrentPlayer.Color, r_ValidMoves, r_ValidJumpMoves);

            Move = getMoveFromComputerPlayer();
            setPlayerMoveOnBoard(Move);
            System.Threading.Thread.Sleep(200);
            checkIfComputerPlayerMoveCompleted(Move);
        }

        public eMoveStauts MakeHumanPlayerMove(string i_Move)
        {
            eMoveStauts moveStatus = eMoveStauts.Incomplete;
            DamkaRules.GetValidMoves(m_Board, m_CurrentPlayer.Color, r_ValidMoves, r_ValidJumpMoves);

            if (isPlayerMoveValid(i_Move) == true)
            {
                setPlayerMoveOnBoard(i_Move);
                checkIfHumanPlayerMoveCompleted(i_Move, ref moveStatus);
            }
            else
            {
                moveStatus = eMoveStauts.Illegal;
            }
         
            return moveStatus;
        }

        private bool isPlayerMoveValid(string i_Move)
        {
            return (r_ValidMoves.Contains(i_Move) == true) || (r_ValidJumpMoves.Contains(i_Move) == true);
        }

        private void checkIfComputerPlayerMoveCompleted(string i_Move)
        {
            if (checkIfPlayerTurnOver() == eMoveStauts.Incomplete)
            {
                DamkaRules.GetJumpMovesForCoin(m_Board, i_Move, m_CurrentPlayer.Color, r_ValidJumpMoves);
            }

            while (checkIfPlayerTurnOver() == eMoveStauts.Incomplete)
            {
                i_Move = getMoveFromComputerPlayer();
                setPlayerMoveOnBoard(i_Move);
                System.Threading.Thread.Sleep(200);
                DamkaRules.GetJumpMovesForCoin(m_Board, i_Move, m_CurrentPlayer.Color, r_ValidJumpMoves);
             }
        }

        private void checkIfHumanPlayerMoveCompleted(string i_Move, ref eMoveStauts i_MoveStatus)
        {
            // check if the last move was a jump one
            if (checkIfPlayerTurnOver() == eMoveStauts.Incomplete)
            {
                DamkaRules.GetJumpMovesForCoin(m_Board, i_Move, m_CurrentPlayer.Color, r_ValidJumpMoves);
            }

            i_MoveStatus = checkIfPlayerTurnOver();
        }

        private string getMoveFromComputerPlayer()
        {
            string Move;
            Random rand = new Random();

            // in case the computer player has jump valid moves
            if (r_ValidJumpMoves.Count > 0)
            {
                int index = rand.Next(0, r_ValidJumpMoves.Count);

                Move = r_ValidJumpMoves[index];
            }
            else
            {
                int index = rand.Next(0, r_ValidMoves.Count);

                Move = r_ValidMoves[index];
            }

            return Move;
        }

        private void setPlayersCoins()
        {
            m_BlackPlayer.ManCoins = ((m_Board.Rows / 2) - 1) * m_Board.Cols / 2;
            m_BlackPlayer.KingCoins = 0;
            m_WhitePlayer.ManCoins = ((m_Board.Rows / 2) - 1) * m_Board.Cols / 2;
            m_WhitePlayer.KingCoins = 0;
        }

        private void setPlayerMoveOnBoard(string i_Move)
        {
            int fromLocationRow, fromLocationCol, destLocationRow, destLocationCol;
            string fromPlayerLocation = MoveParser.GetFromLocation(i_Move);
            string destPlayerLocation = MoveParser.GetDestinationLocation(i_Move);
            eDamkaCell currentPlayerCell;

            // set current player cell to none 
            MoveParser.GetLocationIndexes(fromPlayerLocation, out fromLocationRow, out fromLocationCol);
            currentPlayerCell = m_Board[fromLocationRow, fromLocationCol];
            m_Board.SetCellInBoard(fromLocationRow, fromLocationCol, eDamkaCell.None);

            // set destination move in board to current player
            MoveParser.GetLocationIndexes(destPlayerLocation, out destLocationRow, out destLocationCol);
            DamkaRules.UpdateManCoinInCaseTurnToKingCoin(m_CurrentPlayer, destLocationRow, destLocationCol, m_Board.Rows, ref currentPlayerCell);
            m_Board.SetCellInBoard(destLocationRow, destLocationCol, currentPlayerCell);

            // in case the destination move is a jumping move 
            if (m_Board.IsInRange(fromLocationRow, fromLocationCol, destLocationRow, destLocationCol) == false)
            {
                int midRow = (fromLocationRow + destLocationRow) / 2;
                int midCol = (fromLocationCol + destLocationCol) / 2;

                if (m_Board.IsMan(m_Board[midRow, midCol]) == true)
                {
                    GetOpponentPlayer(m_CurrentPlayer).ManCoins--;
                }
                else
                {
                    GetOpponentPlayer(m_CurrentPlayer).KingCoins--;
                }

                m_Board.SetCellInBoard(midRow, midCol, eDamkaCell.None);
            }
        }

        public eRoundStatus CheckRoundStatus()
        {
            Player opponentPlayer = GetOpponentPlayer(m_CurrentPlayer);

            // if its the second player turn
            if (m_CurrentPlayer.Color == ePlayerColor.White_O)
            {
                m_CurrentPlayer.HasValidMoves = DamkaRules.GetValidMoves(m_Board, m_CurrentPlayer.Color, r_ValidMoves, r_ValidJumpMoves);
            }

            opponentPlayer.HasValidMoves = DamkaRules.GetValidMoves(m_Board, opponentPlayer.Color, r_ValidMoves, r_ValidJumpMoves);

            return DamkaRules.UpdateRoundStatus(m_CurrentPlayer, opponentPlayer, out m_WinnerPlayer);
        }

        private eMoveStauts checkIfPlayerTurnOver()
        {
            eMoveStauts isTurnOver = eMoveStauts.Incomplete;

            if (r_ValidJumpMoves.Count == 0)
            {
                isTurnOver = eMoveStauts.Complete;
            }

            return isTurnOver;
        }

        public void SwitchPlayerTurn()
        {
            if (m_CurrentPlayer.Color == ePlayerColor.Black_X)
            {
                m_CurrentPlayer = m_WhitePlayer;
            }
            else
            {
                m_CurrentPlayer = m_BlackPlayer;
            }
        }

        public Player GetOpponentPlayer(Player i_Player)
        {
            Player opponentPlayer = m_WhitePlayer;

            if (i_Player == m_WhitePlayer)
            {
                opponentPlayer = m_BlackPlayer;
            }

            return opponentPlayer;
        }
    }
}