using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogics
{
    public class Player
    {
        private readonly bool r_IsAi;
        private readonly string r_Name;
        private readonly char r_Mark;
        private int m_Score = 0;
        private int m_Pieces = 0;

        /// <summary>
        /// A constructor of the player
        /// </summary>
        /// <param name="i_IsAi">Is the player human or computer AI</param>
        /// <param name="i_Name">The player's name</param>
        /// <param name="i_Mark">the player's coin symbol</param>
        public Player(bool i_IsAi, string i_Name, char i_Mark)
        {
            r_IsAi = i_IsAi;
            r_Name = i_Name;
            r_Mark = i_Mark;
        }

        public bool IsAi
        {
            get { return r_IsAi; }
        }

        public string Name
        {
            get { return r_Name; }
        }

        public char Mark
        {
            get { return r_Mark; }
        }

        public int Score
        {
            get { return m_Score; }
        }

        public void PlayerWin()
        {
            m_Score++;
        }

        public void ResetScore()
        {
            m_Score = 0;
        }

        public int Pieces
        {
            get { return m_Pieces; }
        }

        public void PieceFound()
        {
            m_Pieces++;
        }

        public void ResetPieces()
        {
            m_Pieces = 0;
        }
    }
}
