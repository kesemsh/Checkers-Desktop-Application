using System;
using System.Drawing;

namespace Ex05_DamkaGame
{
    public delegate void BoardListener(Point i_CellLocation, string i_NewCellStr);

    public class Board
    {
        private readonly eDamkaCell[,] r_Cells;
        private int m_Rows;
        private int m_Cols;
        public const int k_SmallSizeOfBoardGame = 6;
        public const int k_MediumSizeOfBoardGame = 8;
        public const int k_LargeSizeOfBoardGame = 10;

        public event BoardListener CellChanged;

        public Board(int i_Size)
        {
            m_Rows = i_Size;
            m_Cols = i_Size;
            r_Cells = new eDamkaCell[m_Rows, m_Cols];
            InitiateBoard();
        }
        
        public int Rows 
        { 
            get 
            { 
                return m_Rows; 
            } 
        }

        public int Cols 
        { 
            get 
            { 
                return m_Cols; 
            } 
        }

        public eDamkaCell this[int i_Idx, int j_idx]
        {
            get 
            { 
                return r_Cells[i_Idx, j_idx]; 
            }

            set 
            { 
                r_Cells[i_Idx, j_idx] = value;
                doWhenCellChange(new Point(i_Idx, j_idx), CellToStr(value));
            }
        }

        private void doWhenCellChange(Point i_CellLocation, string i_NewCellStr)
        {
            if (CellChanged != null)
            {
                CellChanged.Invoke(i_CellLocation, i_NewCellStr);
            }
        }

        public void InitiateBoard()
        {       
            for (int indexRow = 0; indexRow < Rows; indexRow++)
            {
                for (int indexCol = 0; indexCol < Cols; indexCol++)
                {
                    if ((indexRow % 2 == 0) && (indexCol % 2 == 0))
                    {
                        r_Cells[indexRow, indexCol] = eDamkaCell.Illegal;
                    }
                    else if ((indexRow % 2 != 0) && (indexCol % 2 != 0))
                    {
                        r_Cells[indexRow, indexCol] = eDamkaCell.Illegal;
                    }
                    else
                    {
                        if (indexRow < ((Rows / 2) - 1))
                        {
                            r_Cells[indexRow, indexCol] = eDamkaCell.WhiteMan;
                        }
                        else if (indexRow > (Rows / 2))
                        {
                            r_Cells[indexRow, indexCol] = eDamkaCell.BlackMan;
                        }
                        else
                        {
                            r_Cells[indexRow, indexCol] = eDamkaCell.None;
                        }
                    }
                }
            }
        }

        public bool IsInBounds(int i_Row, int i_Col)
        {
            return (i_Row >= 0) && (i_Row < Rows) && (i_Col >= 0) && (i_Col < Cols);
        } 

        public bool IsMan(eDamkaCell i_Cell)
        {
            return (i_Cell == eDamkaCell.BlackMan) || (i_Cell == eDamkaCell.WhiteMan);
        }

        public bool IsBlack(eDamkaCell i_Cell)
        {
            return (i_Cell == eDamkaCell.BlackMan) || (i_Cell == eDamkaCell.BlackKing);
        }

        public bool IsWhite(eDamkaCell i_Cell)
        {
            return (i_Cell == eDamkaCell.WhiteMan) || (i_Cell == eDamkaCell.WhiteKing);
        }

        public bool IsEmpty(eDamkaCell i_Cell)
        {
            return i_Cell == eDamkaCell.None;
        }

        public bool IsOwner(ePlayerColor i_PlayerColor, eDamkaCell i_Cell)
        {
            bool isOwner = false;

            if (i_Cell != eDamkaCell.Illegal && i_Cell != eDamkaCell.None)
            {
                ePlayerColor CellOwnerColor = getPlayerColorInCell(i_Cell);

                if (i_PlayerColor == CellOwnerColor)
                {
                    isOwner = true;
                }            
            }

            return isOwner;
        }

        public bool AreOpponents(eDamkaCell i_FirstCell, eDamkaCell i_SecondCell)
        {
            bool areOpponents = false;

            if (i_FirstCell != eDamkaCell.Illegal && i_FirstCell != eDamkaCell.None && i_SecondCell != eDamkaCell.Illegal && i_SecondCell != eDamkaCell.None)
            {
                if (getPlayerColorInCell(i_FirstCell) != getPlayerColorInCell(i_SecondCell))
                {
                    areOpponents = true;
                }
            }

            return areOpponents;
        }

        private ePlayerColor getPlayerColorInCell(eDamkaCell i_Cell)
        {
            ePlayerColor playerColor = ePlayerColor.Black_X;
            
            if (IsWhite(i_Cell) == true)
            {
                playerColor = ePlayerColor.White_O;
            }

            return playerColor;
        }

        public void SetCellInBoard(int i_Row, int i_Col, eDamkaCell i_NewCell)
        {
            this[i_Row, i_Col] = i_NewCell;
        }

        public bool IsInRange(int i_FirstCellRow, int i_FirstCellCol, int i_SecondCellRow, int i_SecondCellCol)
        {
            return (Math.Abs(i_FirstCellRow - i_SecondCellRow) == 1) && (Math.Abs(i_FirstCellCol - i_SecondCellCol) == 1);
        }

        public string CellToStr(eDamkaCell i_Cell)
        {
            string cellRepresentiveStr = string.Empty;

            switch (i_Cell)
            {
                case eDamkaCell.None:
                    cellRepresentiveStr = string.Empty;
                    break;
                case eDamkaCell.BlackMan:
                    cellRepresentiveStr = "X";
                    break;
                case eDamkaCell.WhiteMan:
                    cellRepresentiveStr = "O";
                    break;
                case eDamkaCell.BlackKing:
                    cellRepresentiveStr = "K";
                    break;
                case eDamkaCell.WhiteKing:
                    cellRepresentiveStr = "Q";
                    break;
            }

            return cellRepresentiveStr;
        }
    }
}