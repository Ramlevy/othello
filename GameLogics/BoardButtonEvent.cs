using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogics
{
    public class BoardButtonEvent : EventArgs
    {
        private int m_Line;
        private int m_Row;
        private char m_Mark;
        private bool m_isEmpty;

        /// <summary>
        /// A constructor of an event of a button on the board.
        /// </summary>
        /// <param name="i_Line">The line of the button's location on the board</param>
        /// <param name="i_Row">The row of the button's location on the board</param>
        /// <param name="i_Mark">The button's coin symbol(helps in order to decide whether it belongs to player one or player two(or computer)</param>
        /// <param name="i_isEmpty">Is the button empty or occupied by a coin</param>
        public BoardButtonEvent(int i_Line, int i_Row, char i_Mark, bool i_isEmpty)
        {
            m_Line = i_Line;
            m_Row = i_Row;
            m_Mark = i_Mark;
            m_isEmpty = i_isEmpty;
        }

        public int Line
        {
            get { return m_Line; }
        }

        public int Row
        {
            get { return m_Row; }
        }

        public char MarkingMark
        {
            get { return m_Mark; }
        }

        public bool isEmpty
        {
            get { return m_isEmpty; }
        }
    }
}
