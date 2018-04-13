using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GameLogics;

namespace GameUI
{
    public class FormSettings : Form
    {
        private const int k_FormWidth = 330;
        private const int k_FormHeight = 170;
        private const int k_ButtonSizeHeight = 35;
        private const int k_ButtonSpaceFromEdge = 15;
        private const int k_PlayButtonWidth = 140;
        private const int k_PlayButtonHeight = 40;
        private const int k_ButtonYSpace = 25;
        private const string k_MaxBoardSize = "12x12";
        private string m_CurrentSize = "6x6";
        private int m_BoardSize = 6;
        private bool m_IsVsAI;
        private Button m_ButtonSize;
        private Button m_ButtonComputer;
        private Button m_ButtonPlayer;

        public FormSettings()
        {
            this.Text = "Othello - Game Settings";
            Application.EnableVisualStyles();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Size = new Size(k_FormWidth, k_FormHeight);
            this.StartPosition = FormStartPosition.CenterScreen;

            m_ButtonSize = new Button();
            m_ButtonSize.Height = k_ButtonSizeHeight;
            m_ButtonSize.Width = k_FormWidth - (2 * k_ButtonSpaceFromEdge);
            m_ButtonSize.Location = new Point(k_ButtonSpaceFromEdge, k_ButtonSpaceFromEdge);
            m_ButtonSize.Text = "Board Size: " + m_CurrentSize + " (click to increase)";
            this.Controls.Add(m_ButtonSize);
            m_ButtonSize.Click += new EventHandler(size_Increase);

            m_ButtonComputer = new Button();
            m_ButtonComputer.Height = k_PlayButtonHeight;
            m_ButtonComputer.Width = k_PlayButtonWidth;
            m_ButtonComputer.Location = new Point(k_ButtonSpaceFromEdge, m_ButtonSize.Bottom + k_ButtonYSpace);
            m_ButtonComputer.Text = "Play against the computer";
            this.Controls.Add(m_ButtonComputer);
            m_ButtonComputer.Click += new EventHandler(computer_Game);

            m_ButtonPlayer = new Button();
            m_ButtonPlayer.Height = k_PlayButtonHeight;
            m_ButtonPlayer.Width = k_PlayButtonWidth;
            m_ButtonPlayer.Location = new Point(m_ButtonComputer.Right + k_ButtonSpaceFromEdge, m_ButtonSize.Bottom + k_ButtonYSpace);
            m_ButtonPlayer.Text = "Play against your friend";
            this.Controls.Add(m_ButtonPlayer);
            m_ButtonPlayer.Click += new EventHandler(player_Game);
        }

        private void size_Increase(object sender, EventArgs e)
        {
            string[] availableSizes = { "6x6", "8x8", "10x10", "12x12" };
            int length = 4, i = 1;

            foreach (string possibleSize in availableSizes)
            {
                if (possibleSize == m_CurrentSize)
                {
                    if (i != length)
                    {
                        m_CurrentSize = availableSizes[i];
                    }
                    else
                    {
                        m_CurrentSize = availableSizes[0];
                    }

                    (sender as Button).Text = "Board Size: " + m_CurrentSize;
                    (sender as Button).Text += (m_CurrentSize == k_MaxBoardSize) ? " (click to decrease)" : " (click to increase)";
                    switch (possibleSize)
                    {
                        case "6x6": m_BoardSize = 8;
                            break;
                        case "8x8": m_BoardSize = 10;
                            break;
                        case "10x10": m_BoardSize = 12;
                            break;
                        case "12x12": m_BoardSize = 6;
                            break;
                    }

                    break;
                }

                    i++;
            }
        }

         private void computer_Game(object sender, EventArgs e)
         {
             m_IsVsAI = true;
             this.DialogResult = DialogResult.OK;
         }

         private void player_Game(object sender, EventArgs e)
         {
             m_IsVsAI = false;
             this.DialogResult = DialogResult.OK;
         }

         public int BoardSize
         {
             get
             {
                 return m_BoardSize;
             }
         }

         public bool isVsAI
         {
             get
             {
                 return m_IsVsAI;
             }
         }
    }
}