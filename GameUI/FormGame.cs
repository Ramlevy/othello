using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GameLogics;
using GameUI;

namespace GameUI
{
    public class FormGame : Form
    {
        private const int k_ButtonLength = 35;
        private const int k_SpaceFromEdge = 10;
        private bool m_IsDialogOpen = false;
        private Logics m_gameLogics;
        private GameButton[,] m_ButtonBoard;
        private FormSettings m_Settings = new FormSettings();

        public FormGame()
        {
            if (m_Settings.ShowDialog() == DialogResult.OK)
            {
                m_ButtonBoard = new GameButton[m_Settings.BoardSize, m_Settings.BoardSize];
                this.Text = "Othello";
                Application.EnableVisualStyles();
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.MaximizeBox = false;
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                this.Size = new Size((3 * k_SpaceFromEdge) + (m_Settings.BoardSize * k_ButtonLength), (5 * k_SpaceFromEdge) + (m_Settings.BoardSize * k_ButtonLength));
                m_gameLogics = new Logics(m_Settings.BoardSize, m_Settings.isVsAI);
                m_gameLogics.GameEnded += onGameEnded;
                m_gameLogics.BoardButtonChanged += onBoardButtonChanged;
                newButtonBoard();
                startGame();
            }
        }

        private void startGame()
        {
            drawStartingPieces();
            enableLegalMoves();
            if (m_IsDialogOpen == false)
            {
                m_IsDialogOpen = true;
                this.ShowDialog();
            }
        }

        private void newButtonBoard()
        {
            m_ButtonBoard = new GameButton[m_Settings.BoardSize, m_Settings.BoardSize];
            GameButton nextButton;
            int locationX = k_SpaceFromEdge;
            int locationY = k_SpaceFromEdge;

            for (int i = 0; i < m_Settings.BoardSize; i++)
            {
                locationX = k_SpaceFromEdge;
                for (int j = 0; j < m_Settings.BoardSize; j++)
                {
                    nextButton = new GameButton(i, j);
                    nextButton.Width = k_ButtonLength; 
                    nextButton.Height = k_ButtonLength; 
                    nextButton.Location = new Point(locationX, locationY);
                    locationX += k_ButtonLength;
                    nextButton.Enabled = false;
                    m_ButtonBoard[i, j] = nextButton;
                    nextButton.Click += new EventHandler(buttonChosen);
                    this.Controls.Add(m_ButtonBoard[i, j]);
                }

                locationY += k_ButtonLength;
            }

            drawStartingPieces();
        }

        private void drawStartingPieces()
        {
            int halfSize = (m_Settings.BoardSize / 2) - 1;

            m_ButtonBoard[halfSize, halfSize].Text = "O";
            m_ButtonBoard[halfSize, halfSize].BackColor = Color.White;
            m_ButtonBoard[halfSize, halfSize].ForeColor = Color.Black;
            m_ButtonBoard[halfSize, halfSize].Enabled = true;

            m_ButtonBoard[halfSize + 1, halfSize].Text = "O";
            m_ButtonBoard[halfSize + 1, halfSize].BackColor = Color.Black;
            m_ButtonBoard[halfSize + 1, halfSize].ForeColor = Color.White;
            m_ButtonBoard[halfSize + 1, halfSize].Enabled = true;

            m_ButtonBoard[halfSize, halfSize + 1].Text = "O";
            m_ButtonBoard[halfSize, halfSize + 1].BackColor = Color.Black;
            m_ButtonBoard[halfSize, halfSize + 1].ForeColor = Color.White;
            m_ButtonBoard[halfSize, halfSize + 1].Enabled = true;

            m_ButtonBoard[halfSize + 1, halfSize + 1].Text = "O";
            m_ButtonBoard[halfSize + 1, halfSize + 1].BackColor = Color.White;
            m_ButtonBoard[halfSize + 1, halfSize + 1].ForeColor = Color.Black;
            m_ButtonBoard[halfSize + 1, halfSize + 1].Enabled = true;
        }

        private void enableLegalMoves()
        {
            List<int> list = new List<int>();

            if (m_gameLogics.m_PlayerTurn == 1)
            {
                 list = m_gameLogics.LegalMovesToList(m_gameLogics.m_Player1, m_gameLogics.m_Player2);
            }
            else if (m_Settings.isVsAI == false && m_gameLogics.m_PlayerTurn == -1)
            {
                 list = m_gameLogics.LegalMovesToList(m_gameLogics.m_Player2, m_gameLogics.m_Player1);
            }
            else if (m_Settings.isVsAI == true && m_gameLogics.m_PlayerTurn == -1)
            {
                m_gameLogics.ComputerMove(m_gameLogics.m_Player2, m_gameLogics.m_Player1);
                disableButtons();
                if (m_gameLogics.IsAnyLegalMoves(m_gameLogics.m_Player1, m_gameLogics.m_Player2) == true)
                {
                    m_gameLogics.m_PlayerTurn *= -1;
                    enableLegalMoves();
                }
                else if (m_gameLogics.IsAnyLegalMoves(m_gameLogics.m_Player2, m_gameLogics.m_Player1) == true)
                {
                    m_gameLogics.m_PlayerTurn = -1;
                    enableLegalMoves();
                }
                else
                {
                    m_gameLogics.endGame();
                }
            }

            for (int i = 0; i < list.Count / 6; i++)
            {
                m_ButtonBoard[list[(i * 6)] - 1, list[(i * 6) + 1] - 1].Enabled = true;
                m_ButtonBoard[list[i * 6] - 1, list[(i * 6) + 1] - 1].BackColor = Color.Gray;
            }      
        }

        private void disableButtons()
        {
            for (int i = 0; i < m_Settings.BoardSize; i++)
            {
                for (int j = 0; j < m_Settings.BoardSize; j++)
                {
                    if (m_ButtonBoard[i, j].Text != "O")
                    {
                        m_ButtonBoard[i, j].Enabled = false;
                        m_ButtonBoard[i, j].BackColor = default(Color);
                    }
                    else
                    {
                        m_ButtonBoard[i, j].Enabled = true;
                    }
                }
             }
        }

        private void anyValidMoves()
        {
            bool anyLegalMovesForPlayerOne = true;
            bool anyLegalMovesForPlayerTwo = true;

            m_gameLogics.m_PlayerTurn *= -1;
            if (m_gameLogics.m_PlayerTurn == 1)
            {
                anyLegalMovesForPlayerOne = m_gameLogics.IsAnyLegalMoves(m_gameLogics.m_Player1, m_gameLogics.m_Player2);
                if (anyLegalMovesForPlayerOne == false)
                {
                    anyLegalMovesForPlayerTwo = m_gameLogics.IsAnyLegalMoves(m_gameLogics.m_Player2, m_gameLogics.m_Player1);
                    if (anyLegalMovesForPlayerTwo == false)
                    {
                        m_gameLogics.m_PlayerTurn = 0;
                        m_gameLogics.endGame();
                    }
                    else
                    {
                        m_gameLogics.m_PlayerTurn *= -1;
                    }
                }
            }
            else
            {
                anyLegalMovesForPlayerTwo = m_gameLogics.IsAnyLegalMoves(m_gameLogics.m_Player2, m_gameLogics.m_Player1);
                if (anyLegalMovesForPlayerTwo == false)
                {
                    anyLegalMovesForPlayerOne = m_gameLogics.IsAnyLegalMoves(m_gameLogics.m_Player1, m_gameLogics.m_Player2);
                    if (anyLegalMovesForPlayerOne == false)
                    {
                        m_gameLogics.m_PlayerTurn = 0;
                        m_gameLogics.endGame();
                    }
                    else
                    {
                        m_gameLogics.m_PlayerTurn *= -1;
                    }
                }
            }
        }

        private void resetButtonBoard()
        {
            for (int i = 0; i < m_Settings.BoardSize; i++)
            {
                for (int j = 0; j < m_Settings.BoardSize; j++)
                {
                     m_ButtonBoard[i, j].BackColor = default(Color);
                     m_ButtonBoard[i, j].Enabled = false;
                     m_ButtonBoard[i, j].Text = string.Empty;            
                }
            }
        }

        private void buttonChosen(object sender, EventArgs e)
        {
            int[] chosenLocation = { (sender as GameButton).Row, (sender as GameButton).Line };

            if (m_gameLogics.isCellEmpty(chosenLocation[0], chosenLocation[1]))
            {
                if (m_gameLogics.m_PlayerTurn == 1)
                {
                    m_gameLogics.DrawInCell(m_gameLogics.m_Player1, chosenLocation[0], chosenLocation[1]);
                    m_gameLogics.NewMoveLogic(m_gameLogics.m_Player1, m_gameLogics.m_Player2, chosenLocation[0], chosenLocation[1]);
                }
                else if (m_Settings.isVsAI == false)
                {
                    m_gameLogics.DrawInCell(m_gameLogics.m_Player2, chosenLocation[0], chosenLocation[1]);
                    m_gameLogics.NewMoveLogic(m_gameLogics.m_Player2, m_gameLogics.m_Player1, chosenLocation[0], chosenLocation[1]);
                }
            }
            else
            {
                MessageBox.Show("This cell is already taken !", "Othello", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_gameLogics.m_PlayerTurn *= -1;
            }

            anyValidMoves();
            disableButtons();
            enableLegalMoves();
        }

        private void onBoardButtonChanged(object sender, EventArgs e)
        {
            BoardButtonEvent refEvent = e as BoardButtonEvent;

            if (refEvent.isEmpty == false)
            {
                m_ButtonBoard[refEvent.Line, refEvent.Row].Enabled = false;
                m_ButtonBoard[refEvent.Line, refEvent.Row].Text = "O";
                if (m_gameLogics.m_PlayerTurn == 1)
                {
                    m_ButtonBoard[refEvent.Line, refEvent.Row].BackColor = Color.Black;
                    m_ButtonBoard[refEvent.Line, refEvent.Row].ForeColor = Color.White;
                }
                else
                {
                    m_ButtonBoard[refEvent.Line, refEvent.Row].BackColor = Color.White;
                    m_ButtonBoard[refEvent.Line, refEvent.Row].ForeColor = Color.Black;
                }
            }
        }

        private void onGameEnded(object sender, EventArgs e)
        {
            GameEndedEvent refEvent = e as GameEndedEvent;
            string message;

            if (m_gameLogics.m_Player1.Pieces != m_gameLogics.m_Player2.Pieces)
            {
                message = string.Format(
@"{0} Won !! ({1}/{2}) ({3}/{4})
Would you like to play another round ?",
                          refEvent.WinnerName, refEvent.PlayerOnePieces, refEvent.PlayerTwoPieces, m_gameLogics.m_Player1.Score, m_gameLogics.m_Player2.Score);
            }
            else
            {
                message = string.Format(
@"Its a Tie ! !! ({0}/{1}) ({2}/{3})
Would you like to play another round ?",
                          refEvent.PlayerOnePieces, refEvent.PlayerTwoPieces, m_gameLogics.m_Player1.Score, m_gameLogics.m_Player2.Score);
            }
                                           
            DialogResult dialogResult = MessageBox.Show(message, "Othello", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                resetButtonBoard();
                m_gameLogics.NewGame();
                startGame();
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
        }
    }
}