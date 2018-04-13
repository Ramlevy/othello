using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogics
{
    public class GameEndedEvent : EventArgs
    {
        private string m_NameOfWinner = string.Empty;
        private int m_PlayerOnePieces;
        private int m_PlayerTwoPieces;

        /// <summary>
        /// A constructor of an event of a the game's end
        /// </summary>
        /// <param name="i_NameOfWinner">The name of the player(or computer) that won the game</param>
        /// <param name="i_PlayerOnePieces">The number of coins that player one managed to put on the board</param>
        /// <param name="i_PlayerTwoPieces">The number of coins that player two(or computer) managed to put on the board</param>
        public GameEndedEvent(string i_NameOfWinner, int i_PlayerOnePieces, int i_PlayerTwoPieces)
        {
            m_NameOfWinner = i_NameOfWinner;
            m_PlayerOnePieces = i_PlayerOnePieces;
            m_PlayerTwoPieces = i_PlayerTwoPieces;
        }

        public string WinnerName
        {
            get { return m_NameOfWinner; }
        }

        public int PlayerOnePieces
        {
            get { return m_PlayerOnePieces; }
        }

        public int PlayerTwoPieces
        {
            get { return m_PlayerTwoPieces; }
        }
    }
}
