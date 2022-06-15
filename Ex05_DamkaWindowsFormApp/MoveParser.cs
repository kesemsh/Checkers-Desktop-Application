namespace Ex05_DamkaGame
{
    public static class MoveParser
    {
        private const char k_BeginCol = 'A';
        private const char k_BeginRow = 'a';

        public static string GetFromLocation(string i_Move)
        {
            return i_Move.Substring(0, 2);
        }

        public static string GetDestinationLocation(string i_Move)
        {
            return i_Move.Substring(3, 2);
        }

        public static void GetLocationIndexes(string i_Move, out int o_Row, out int o_Col)
        {
            int indexCol = 0;
            int indexRow = 1;

            o_Col = i_Move[indexCol] - k_BeginCol;
            o_Row = i_Move[indexRow] - k_BeginRow;        
        }

        public static string ConvertIndexesLocationToLocationStr(int i_Row, int i_Col)
        {
            char row = (char)(k_BeginRow + i_Row);
            char col = (char)(k_BeginCol + i_Col);
            
            return col.ToString() + row.ToString();
        }

        public static string GetFormatMove(string i_FromLocation, string i_DestLocation)
        {
            return string.Format("{0}>{1}", i_FromLocation, i_DestLocation);
        }

        public static string ConvertIndexesMoveToStr(int i_FromRow, int i_FromCol, int i_DestRow, int i_DestCol)
        {
            string fromLocation = ConvertIndexesLocationToLocationStr(i_FromRow, i_FromCol);
            string destLocation = ConvertIndexesLocationToLocationStr(i_DestRow, i_DestCol);

            return GetFormatMove(fromLocation, destLocation);
        }
    }
}