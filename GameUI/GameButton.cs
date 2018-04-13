using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GameLogics;
using GameUI;

namespace GameUI
{
    public class GameButton : Button
    {
        private int[] m_Location = new int[2];

        public GameButton(int i_LocationX, int i_LocationY)
            : base()
        {
            m_Location[0] = i_LocationX;
            m_Location[1] = i_LocationY;
        }

        public int Row
        {
            get { return m_Location[0]; }
        }

        public int Line
        {
            get { return m_Location[1]; }
        }
    }
}
