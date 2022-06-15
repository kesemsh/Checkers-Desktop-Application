using System.Collections.Generic;

namespace Ex05_DamkaGame
{
    public static class DamkaRules
    {
        private const int k_KingScore = 4;
        
        public static eRoundStatus UpdateRoundStatus(Player i_CurrentPlayer, Player i_OpponentPlayer, out Player o_WinnerPlayer) 
        {
            eRoundStatus roundStatus = eRoundStatus.NotOver;
            o_WinnerPlayer = null;

            // check if the opponent player has no coins
            if (i_OpponentPlayer.ManCoins == 0 && i_OpponentPlayer.KingCoins == 0)
            {
                o_WinnerPlayer = i_CurrentPlayer;
                o_WinnerPlayer.WinningNum++;
                roundStatus = eRoundStatus.Win;
            }
            else
            {
                if (i_CurrentPlayer.HasValidMoves == false && i_OpponentPlayer.HasValidMoves == false)
                {
                    SetPlayerPoints(i_CurrentPlayer, i_OpponentPlayer);
                    o_WinnerPlayer = getWinnerOfRound(i_CurrentPlayer, i_OpponentPlayer);

                    if (o_WinnerPlayer != null)
                    {
                        o_WinnerPlayer.WinningNum++;
                        roundStatus = eRoundStatus.Win;
                    }
                    else
                    {
                        roundStatus = eRoundStatus.Tie;
                    }
                }
                else if (i_CurrentPlayer.HasValidMoves == false)
                {
                    o_WinnerPlayer = i_OpponentPlayer;
                    o_WinnerPlayer.WinningNum++;
                    roundStatus = eRoundStatus.Win;
                }
                else if (i_OpponentPlayer.HasValidMoves == false)
                {
                    o_WinnerPlayer = i_CurrentPlayer;
                    o_WinnerPlayer.WinningNum++;
                    roundStatus = eRoundStatus.Win;
                }
            }

            return roundStatus;
        }

        private static Player getWinnerOfRound(Player i_Player1, Player i_Player2)
        {
            Player winner;

            if (i_Player1.Points > i_Player2.Points)
            {
                winner = i_Player1;
            }
            else if (i_Player1.Points < i_Player2.Points)
            {
                winner = i_Player2;
            }
            else
            {
                winner = null;
            }

            return winner;
        }

        public static void SetPlayerPoints(Player i_FirstPlayer, Player i_SecondPlayer)
        {
            int firstPlayerPoints, secondPlayerPoints;
            int score;

            firstPlayerPoints = i_FirstPlayer.ManCoins + (i_FirstPlayer.KingCoins * k_KingScore); 
            secondPlayerPoints = i_SecondPlayer.ManCoins + (i_SecondPlayer.KingCoins * k_KingScore);

            if (i_FirstPlayer.GetCoins() > i_SecondPlayer.GetCoins())
            {
                score = firstPlayerPoints - secondPlayerPoints;
                i_FirstPlayer.Points = score;
            }
            else if (i_FirstPlayer.GetCoins() < i_SecondPlayer.GetCoins())
            {
                score = secondPlayerPoints - firstPlayerPoints;
                i_SecondPlayer.Points = score;
            }
        }

        public static bool GetValidMoves(Board i_Board, ePlayerColor i_PlayerColor, List<string> i_ValidMoves, List<string> i_ValidJumpMoves)        
        {
            bool hasValidMoves = true;

            i_ValidJumpMoves.Clear();
            i_ValidMoves.Clear();

            for (int row = 0; row < i_Board.Rows; row++)
            {
                for (int col = 0; col < i_Board.Cols; col++)
                {
                    eDamkaCell playerCell = i_Board[row, col];

                    if (i_Board.IsOwner(i_PlayerColor, playerCell) == true)
                    {
                        if (playerCell == eDamkaCell.BlackMan)
                        {
                            addAboveValidMoves(i_Board, row, col, i_ValidMoves, i_ValidJumpMoves);
                        }
                        else if (playerCell == eDamkaCell.WhiteMan)
                        {
                            addDownValidMoves(i_Board, row, col, i_ValidMoves, i_ValidJumpMoves);
                        }
                        else
                        {
                            addAboveDownValidMoves(i_Board, row, col, i_ValidMoves, i_ValidJumpMoves);
                        }
                    } 
                }
            }

            // in case none the current player has no valid moves at all
            if (i_ValidMoves.Count == 0 && i_ValidJumpMoves.Count == 0)
            {
                hasValidMoves = false;
            }
            else if (i_ValidJumpMoves.Count > 0)
            {
                i_ValidMoves.Clear();
            }

            return hasValidMoves;
        }

        private static void addAboveValidMoves(Board i_Board, int i_Row, int i_Col, List<string> i_ValidMoves, List<string> i_ValidJumpMoves)
        {
            string fromLocation = MoveParser.ConvertIndexesLocationToLocationStr(i_Row, i_Col);
            string move, destLocation;

            addAboveJumpValidMoves(i_Board, i_Row, i_Col, i_ValidJumpMoves, fromLocation);

            if (getCellOfMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.AboveLeft, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(fromLocation, destLocation);
                i_ValidMoves.Add(move);
            }

            if (getCellOfMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.AboveRight, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(fromLocation, destLocation);
                i_ValidMoves.Add(move);
            }
        }

        private static void addAboveJumpValidMoves(Board i_Board, int i_Row, int i_Col, List<string> i_ValidJumpMoves, string i_FromLocation)
        {
            string move, destLocation;
       
            if (getCellOfJumpMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.AboveRight, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(i_FromLocation, destLocation);
                i_ValidJumpMoves.Add(move);
            }
            
            if (getCellOfJumpMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.AboveLeft, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(i_FromLocation, destLocation);
                i_ValidJumpMoves.Add(move);
            }
        }

        private static void addDownValidMoves(Board i_Board, int i_Row, int i_Col, List<string> i_ValidMoves, List<string> i_ValidJumpMoves)
        {
            string fromLocation = MoveParser.ConvertIndexesLocationToLocationStr(i_Row, i_Col);
            string move, destLocation;
            addDownJumpValidMoves(i_Board, i_Row, i_Col, i_ValidJumpMoves, fromLocation);

            if (getCellOfMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.DownLeft, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(fromLocation, destLocation);
                i_ValidMoves.Add(move);
            }

            if (getCellOfMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.DownRight, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(fromLocation, destLocation);
                i_ValidMoves.Add(move);
            }
        }

        private static void addDownJumpValidMoves(Board i_Board, int i_Row, int i_Col, List<string> i_ValidJumpMoves, string i_FromLocation)
        {
            string move, destLocation;
       
            if (getCellOfJumpMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.DownRight, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(i_FromLocation, destLocation);
                i_ValidJumpMoves.Add(move);
            }

            if (getCellOfJumpMoveIfNone(i_Board, i_Row, i_Col, eMoveDirection.DownLeft, out destLocation) == true)
            {
                move = MoveParser.GetFormatMove(i_FromLocation, destLocation);
                i_ValidJumpMoves.Add(move);
            }
        }

        private static void addAboveDownValidMoves(Board i_Board, int i_Row, int i_Col, List<string> i_ValidMoves, List<string> i_ValidJumpMoves)
        {
            addAboveValidMoves(i_Board, i_Row, i_Col, i_ValidMoves, i_ValidJumpMoves);
            addDownValidMoves(i_Board, i_Row, i_Col, i_ValidMoves, i_ValidJumpMoves);
        }

        private static bool getCellOfMoveIfNone(Board i_Board, int i_Row, int i_Col, eMoveDirection i_MoveDirection, out string o_PieceLocation)
        {
            bool isvalidMove = true;
            
            getRowAndColOfCell(i_MoveDirection, ref i_Row, ref i_Col);
            o_PieceLocation = MoveParser.ConvertIndexesLocationToLocationStr(i_Row, i_Col);

            if (i_Board.IsInBounds(i_Row, i_Col) == false)
            {
                isvalidMove = false;
            }            

            if (isvalidMove == true)
            {
                eDamkaCell currentPiece = i_Board[i_Row, i_Col];

                if (i_Board.IsEmpty(currentPiece) == false)
                {
                    isvalidMove = false;
                }                
            }

            return isvalidMove;
        }

        private static void getRowAndColOfCell(eMoveDirection i_MoveDirection, ref int i_Row, ref int i_Col)
        {
            switch (i_MoveDirection)
            {
                case eMoveDirection.AboveLeft:
                    i_Row = i_Row - 1;
                    i_Col = i_Col - 1;
                    break;
                case eMoveDirection.AboveRight:
                    i_Row = i_Row - 1;
                    i_Col = i_Col + 1;
                    break;
                case eMoveDirection.DownLeft:
                    i_Row = i_Row + 1;
                    i_Col = i_Col - 1;
                    break;
                case eMoveDirection.DownRight:
                    i_Row = i_Row + 1;
                    i_Col = i_Col + 1;
                    break;
            }
        }

        private static bool getCellOfJumpMoveIfNone(Board i_Board, int i_Row, int i_Col, eMoveDirection i_MoveDirection, out string o_CellLocation)
        {
            bool isValidMove = false;
            eDamkaCell currentCell = i_Board[i_Row, i_Col];

            o_CellLocation = string.Empty;
            getRowAndColOfCell(i_MoveDirection, ref i_Row, ref i_Col);

            if (i_Board.IsInBounds(i_Row, i_Col) == true)
            {
                eDamkaCell destinationCell = i_Board[i_Row, i_Col];
                bool isCellNone = getCellOfMoveIfNone(i_Board, i_Row, i_Col, i_MoveDirection, out o_CellLocation);

                if (i_Board.AreOpponents(currentCell, destinationCell) == true && isCellNone == true)
                {
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        public static void GetJumpMovesForCoin(Board i_Board, string i_Move, ePlayerColor i_PlayerColor, List<string> i_ValidJumpMoves)
        {
            string fromLocation = MoveParser.GetDestinationLocation(i_Move);
            eDamkaCell playerCell;
            int row, col;

            i_ValidJumpMoves.Clear();
            MoveParser.GetLocationIndexes(fromLocation, out row, out col);
            playerCell = i_Board[row, col];

            if (i_Board.IsOwner(i_PlayerColor, playerCell) == true)
            {
                if (playerCell == eDamkaCell.BlackMan)
                {
                    addAboveJumpValidMoves(i_Board, row, col, i_ValidJumpMoves, fromLocation);
                }
                else if (playerCell == eDamkaCell.WhiteMan)
                {
                    addDownJumpValidMoves(i_Board, row, col, i_ValidJumpMoves, fromLocation);
                }
                else
                {
                    addAboveJumpValidMoves(i_Board, row, col, i_ValidJumpMoves, fromLocation);
                    addDownJumpValidMoves(i_Board, row, col, i_ValidJumpMoves, fromLocation);
                }
            }                                             
        }

        public static void UpdateManCoinInCaseTurnToKingCoin(Player i_CurrentPlayer, int i_PlayerRow, int i_PlayerCol, int i_NumOfRowsOnBoard, ref eDamkaCell io_CurrentPlayerCell)
        {
            int kingRowOfBlackPlayer = 0;
            int kingRowOfWhitePlayer = i_NumOfRowsOnBoard - 1;

            if (i_CurrentPlayer.Color == ePlayerColor.Black_X)
            {
                if (i_PlayerRow == kingRowOfBlackPlayer)
                {
                    io_CurrentPlayerCell = eDamkaCell.BlackKing;
                    i_CurrentPlayer.KingCoins++;
                    i_CurrentPlayer.ManCoins--;
                }
            }
            else
            {
                if (i_PlayerRow == kingRowOfWhitePlayer)
                {
                    io_CurrentPlayerCell = eDamkaCell.WhiteKing;
                    i_CurrentPlayer.KingCoins++;
                    i_CurrentPlayer.ManCoins--;
                }
            }
        }
    }
}