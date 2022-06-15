namespace Ex05_DamkaGame
{
    public class Player
    {
        private readonly ePlayerColor r_Color;
        private string m_Name;
        private bool m_IsComputer;
        private int m_NumberOfManCoins;
        private int m_NumberOfKingCoins;
        private int m_Points;
        private int m_WinningNum;
        private bool m_HasValidMoves;

        public Player(string i_PlayerName, ePlayerColor i_PlayerColor, bool i_IsComputer)            
        {
            m_Name = i_PlayerName;
            r_Color = i_PlayerColor;
            m_IsComputer = i_IsComputer;
            Points = 0;
            m_NumberOfKingCoins = 0;
            m_WinningNum = 0;
            m_HasValidMoves = true;
        }

        public ePlayerColor Color
        {
            get 
            { 
                return r_Color;
            }
        }

        public string Name
        {
            get 
            { 
                return m_Name; 
            }

            set 
            {
                m_Name = value;
            }
        }

        public bool IsComputer
        {
            get 
            {
                return m_IsComputer; 
            }

            set
            { 
                m_IsComputer = value;
            }
        }

        public int ManCoins
        {
            get 
            { 
                return m_NumberOfManCoins; 
            }

            set 
            { 
                m_NumberOfManCoins = value;
            }
        }

        public int KingCoins
        {
            get 
            {
                return m_NumberOfKingCoins; 
            }

            set 
            { 
                m_NumberOfKingCoins = value;
            }
        }

        public int Points
        {
            get 
            {
                return m_Points; 
            }

            set 
            {
                m_Points = value; 
            }
        }

        public int WinningNum
        {
            get 
            { 
                return m_WinningNum; 
            }

            set 
            {
                m_WinningNum = value; 
            }
        }

        public bool HasValidMoves
        {
            get
            { 
                return m_HasValidMoves; 
            }

            set 
            { 
                m_HasValidMoves = value;
            }
        }

        public int GetCoins()
        {
            return m_NumberOfKingCoins + m_NumberOfManCoins;
        }
    }
}