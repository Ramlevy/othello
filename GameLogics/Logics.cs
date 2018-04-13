using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogics
{
    public class Logics
    {
        public delegate void BoardButtonEventHandler(object sender, EventArgs e);
        public event BoardButtonEventHandler BoardButtonChanged;
        public delegate void GameEndedEventHandler(object sender, EventArgs e);
        public event GameEndedEventHandler GameEnded;

        private const bool k_IsAi = true;
        private GameBoard[,] m_GameBoard;
        private GameBoard[,] m_TempGameBoard;
        public Player m_Player1;
        public Player m_Player2;
        public int m_PlayerTurn = 1;
        private readonly int r_BoardSize;
        private readonly bool r_IsVsAI;
        public Player m_LastWonPlayer;

        /// <summary>
        /// A constructor of the game's logics component.
        /// </summary>
        /// <param name="i_BoardSize">The size of the board</param>
        /// <param name="i_IsVsAI">Is the second player human or computer AI</param>
        public Logics(int i_BoardSize, bool i_IsVsAI)
        {
            m_GameBoard = new GameBoard[i_BoardSize, i_BoardSize];
            r_BoardSize = i_BoardSize;
            r_IsVsAI = i_IsVsAI;
            m_Player1 = new Player(!k_IsAi, "Black", 'X');
            if (i_IsVsAI == true)
            {
                m_Player2 = new Player(k_IsAi, "White", 'O');
            }
            else
            {
                m_Player2 = new Player(!k_IsAi, "White", 'O');
            }

            NewGame();
        }

        public void NewGame()
        {
            int halfSize = (r_BoardSize / 2) - 1;

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    m_GameBoard[i, j].X = false;
                    m_GameBoard[i, j].O = false;
                    m_GameBoard[i, j].isEmpty = true;
                }
            }
            
            m_GameBoard[halfSize, halfSize].O = true;
            m_GameBoard[halfSize + 1, halfSize + 1].O = true;
            m_GameBoard[halfSize + 1, halfSize].X = true;
            m_GameBoard[halfSize, halfSize + 1].X = true;
            m_GameBoard[halfSize, halfSize].isEmpty = false;
            m_GameBoard[halfSize + 1, halfSize + 1].isEmpty = false;
            m_GameBoard[halfSize + 1, halfSize].isEmpty = false;
            m_GameBoard[halfSize, halfSize + 1].isEmpty = false;
            m_PlayerTurn = 1;
        }

        public int NewTurnLegal()
        {
            bool anyLegalMovesForPlayerOne = true;
            bool anyLegalMovesForPlayerTwo = true;
            if (m_PlayerTurn == 1)
            {
                anyLegalMovesForPlayerOne = IsAnyLegalMoves(m_Player1, m_Player2);
                if (anyLegalMovesForPlayerOne == false)
                {
                    anyLegalMovesForPlayerTwo = IsAnyLegalMoves(m_Player2, m_Player1);
                    if (anyLegalMovesForPlayerTwo == false)
                    {
                        m_PlayerTurn = 0;
                    }
                    else
                    {
                        m_PlayerTurn *= -1;
                    }
                }
            }
            else
            {
                anyLegalMovesForPlayerTwo = IsAnyLegalMoves(m_Player2, m_Player1);
                if (anyLegalMovesForPlayerTwo == false)
                {
                    anyLegalMovesForPlayerOne = IsAnyLegalMoves(m_Player1, m_Player2);
                    if (anyLegalMovesForPlayerOne == false)
                    {
                        m_PlayerTurn = 0;
                    }
                    else
                    {
                        m_PlayerTurn *= -1;
                    }
                }
            }

            return m_PlayerTurn;
        }

        public bool IsAnyLegalMoves(Player player, Player otherPlayer)
        {
            bool anyLegalMoves = false;
            for (int i = 0; i < r_BoardSize; i++)
            {
                if (anyLegalMoves == true)
                {
                    break;
                }

                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (m_GameBoard[i, j].isEmpty == true)
                    {
                        if (IsLegalMove(player, otherPlayer, i + 1, j + 1) == true)
                        {
                            anyLegalMoves = true;
                            break;
                        }
                    }
                }
            }

            return anyLegalMoves;
        }

        public bool IsLegalMove(Player player, Player otherPlayer, int i_Line, int i_Row)
        {
            bool isFoundSelf = false, isFoundOther = false, isValidMove = false;
            char currentPlayerMark = player.Mark;
            char otherPlayerMark = otherPlayer.Mark;
            int indexLine = i_Line - 1;
            int indexRow = i_Row - 1;

            for (int directionLine = -1; directionLine <= 1; directionLine++)
            {
                if (isValidMove == true)
                {
                    break;
                }

                for (int directionRow = -1; directionRow <= 1; directionRow++)
                {
                    if (directionLine == 0 && directionRow == 0)
                    {
                        directionRow++;
                    }

                    indexLine += directionLine;
                    indexRow += directionRow;
                    while (IsValidCellRange(indexLine + 1, indexRow + 1))
                    {
                        if (m_GameBoard[indexLine, indexRow].isEmpty)
                        {
                            break;
                        }
                        else if (cellToMarkChar(indexLine, indexRow) == otherPlayerMark)
                        {
                            isFoundOther = true;
                        }
                        else if (cellToMarkChar(indexLine, indexRow) == currentPlayerMark)
                        {
                            isFoundSelf = true;
                            break;
                        }

                        indexLine += directionLine;
                        indexRow += directionRow;
                    }

                    if (isFoundSelf == true && isFoundOther == true)
                    {
                        isValidMove = true;
                        break;
                    }

                    isFoundSelf = false;
                    isFoundOther = false;
                    indexLine = i_Line - 1;
                    indexRow = i_Row - 1;
                }
            }

            return isValidMove;
         }
        
        public List<int> LegalMovesToList(Player player, Player otherPlayer)
        {
            char currentPlayerMark = player.Mark;
            char otherPlayerMark = otherPlayer.Mark;
            int indexLine, indexRow;
            List<int> list = new List<int>();

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if ((player.Mark == 'O' && m_GameBoard[i, j].O == true) || (player.Mark == 'X' && m_GameBoard[i, j].X == true))
                    {
                        for (int directionLine = -1; directionLine <= 1; directionLine++)
                        {
                            for (int directionRow = -1; directionRow <= 1; directionRow++)
                            {
                                indexLine = i;
                                indexRow = j;
                                if (directionLine == 0 && directionRow == 0)
                                {
                                    directionRow++;
                                }

                                indexLine += directionLine;
                                indexRow += directionRow;
                                while (IsValidCellRange(indexLine + 1, indexRow + 1) == true)
                                {
                                    if (m_GameBoard[indexLine, indexRow].isEmpty || cellToMarkChar(indexLine, indexRow) == currentPlayerMark)
                                    {
                                        break;
                                    }

                                    if (cellToMarkChar(indexLine, indexRow) == otherPlayerMark)
                                    {
                                        indexLine += directionLine;
                                        indexRow += directionRow;
                                        if (IsValidCellRange(indexLine + 1, indexRow + 1) && m_GameBoard[indexLine, indexRow].isEmpty)
                                        {
                                            if (list.Contains(indexLine + 1) && list.Contains(indexRow + 1)) 
                                            {
                                                if (list.IndexOf(indexLine + 1) + 1 == list.IndexOf(indexRow + 1))
                                                {
                                                    break;
                                                }
                                            }

                                            list.Add(indexLine + 1);
                                            list.Add(indexRow + 1);
                                            list.Add(i);
                                            list.Add(j);
                                            list.Add(directionLine);
                                            list.Add(directionRow);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        public void NewMoveLogic(Player player, Player otherPlayer, int i_Line, int i_Row)
        {
            bool isFoundSelf = false, isFoundOther = false;
            char currentPlayerMark = player.Mark;
            char otherPlayerMark = otherPlayer.Mark;
            int indexLine = i_Line;
            int indexRow = i_Row;

            for (int directionLine = -1; directionLine <= 1; directionLine++)
            {
                for (int directionRow = -1; directionRow <= 1; directionRow++)
                {
                    if (directionLine == 0 && directionRow == 0)
                    {
                        directionRow++;
                    }

                    indexLine += directionLine;
                    indexRow += directionRow;
                    while (IsValidCellRange(indexLine + 1, indexRow + 1))
                    {
                        if (m_GameBoard[indexLine, indexRow].isEmpty)
                        {
                            break;
                        }
                        else if (cellToMarkChar(indexLine, indexRow) == otherPlayerMark)
                        {
                            isFoundOther = true;
                        }
                        else if (cellToMarkChar(indexLine, indexRow) == currentPlayerMark)
                        {
                            isFoundSelf = true;
                            break;
                        }

                        indexLine += directionLine;
                        indexRow += directionRow;
                    }

                    if (isFoundSelf == true && isFoundOther == true)
                    {
                        flipPieces(player, otherPlayer, i_Line, i_Row, directionLine, directionRow);
                    }

                    isFoundSelf = false;
                    isFoundOther = false;
                    indexLine = i_Line;
                    indexRow = i_Row;
                }
            }
        }

        private void flipPieces(Player player, Player otherPlayer, int i_IndexLine, int i_IndexRow, int i_DirLine, int i_DirRow)
        {
            char currentPlayerMark = player.Mark;
            char otherPlayerMark = otherPlayer.Mark;

            i_IndexLine += i_DirLine;
            i_IndexRow += i_DirRow;

            while (IsValidCellRange(i_IndexLine + 1, i_IndexRow + 1))
            {
                if (cellToMarkChar(i_IndexLine, i_IndexRow) == otherPlayerMark)
                {
                    DrawInCell(player, i_IndexLine, i_IndexRow );
                }
                else if (cellToMarkChar(i_IndexLine, i_IndexRow) == currentPlayerMark)
                {
                    break;
                }

                i_IndexLine += i_DirLine;
                i_IndexRow += i_DirRow;
            }
        }

        public void ComputerMove(Player computer, Player otherPlayer)
        {
            List<int> list = new List<int>();
            int index, listIndex;
            list = LegalMovesToList(computer, otherPlayer);
            index = getBestMove(computer, otherPlayer, list);
            listIndex = index * 6;
            DrawInCell(computer, list[listIndex] - 1, list[listIndex + 1] - 1);
            flipPieces(computer, otherPlayer, list[listIndex + 2], list[listIndex + 3], list[listIndex + 4], list[listIndex + 5]);
        }

        private int getBestMove(Player computer, Player player, List<int> i_List)
        {
            int[] scoreMove = new int[i_List.Count / 6];
            int listIndex, line, row, currentComputerPieces, afterMoveComputerPieces, newComputerPiecesGainedFromMove;

            currentComputerPieces = CountComputerPieces(m_GameBoard);
            initializeArrayWithZero(scoreMove);
            m_TempGameBoard = new GameBoard[r_BoardSize, r_BoardSize];
            for (int index = 0; index < scoreMove.Length; index++)
            {
                m_TempGameBoard = TransferGameBoardData(m_GameBoard, m_TempGameBoard);
                listIndex = index * 6;
                line = i_List[listIndex];
                row = i_List[listIndex + 1];
                if (isCornerCell(m_TempGameBoard, r_BoardSize, line, row) == true)
                {
                    scoreMove[index] += 4;
                }
                else if (isEdgeCell(m_TempGameBoard, r_BoardSize, line, row) == true)
                {
                    scoreMove[index] += 2;
                }

                DrawInCellInTempBoard(computer, line - 1, row - 1);
                flipPiecesInTempBoard(computer, player, i_List[listIndex + 2], i_List[listIndex + 3], i_List[listIndex + 4], i_List[listIndex + 5]);
                afterMoveComputerPieces = CountComputerPieces(m_TempGameBoard);
                newComputerPiecesGainedFromMove = afterMoveComputerPieces - currentComputerPieces;
                scoreMove[index] += newComputerPiecesGainedFromMove;
            }

            return maxFromArray(scoreMove);
        }

        public void endGame()
        {
            string winnerName = GameScore();
            GameEndedEvent gameEnded = new GameEndedEvent(winnerName, m_Player1.Pieces, m_Player2.Pieces);

            OnGameEndedEvent(gameEnded);
        }

        public string GameScore()
        {
            string nameOfWinner = string.Empty;

            m_Player1.ResetPieces();
            m_Player2.ResetPieces();

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (m_GameBoard[i, j].X == true)
                    {
                        m_Player1.PieceFound();
                    }
                    else if (m_GameBoard[i, j].O == true)
                    {
                        m_Player2.PieceFound();
                    }
                }
            }

            if (m_Player1.Pieces > m_Player2.Pieces)
            {
                nameOfWinner = m_Player1.Name;
                m_Player1.PlayerWin();
            }
            else if (m_Player1.Pieces < m_Player2.Pieces)
            {
                nameOfWinner = m_Player2.Name;
                m_Player2.PlayerWin();
            }

            return nameOfWinner;
        }

        public void DrawInCell(Player player, int i_Line, int i_Row)
        {
            if (player.Mark == 'X')
            {
                m_GameBoard[i_Line, i_Row].X = true;
                m_GameBoard[i_Line, i_Row].O = false;
            }
            else if(player.Mark == 'O')
            {
                m_GameBoard[i_Line, i_Row].O = true;
                m_GameBoard[i_Line, i_Row].X = false;
            }

            m_GameBoard[i_Line, i_Row].isEmpty = false;
            BoardButtonEvent buttonEvent = new BoardButtonEvent(i_Line, i_Row, player.Mark, m_GameBoard[i_Line, i_Row].isEmpty);
            OnBoardButtonChanged(buttonEvent);  
        }

        private char cellToMarkChar(int i_Line, int i_Row)
        {
            char mark;

            if (m_GameBoard[i_Line, i_Row].X == true)
            {
                mark = 'X';
            }
            else
            {
                mark = 'O';
            }

            return mark;
        }

        public bool isCellEmpty(int i_Line, int i_Row)
        {
            bool isEmpty;

            if (m_GameBoard[i_Line, i_Row].isEmpty == true)
            {
                isEmpty = true;
            }
            else
            {
                isEmpty = false;
            }

            return isEmpty;
        }

        private static void initializeArrayWithZero(int[] i_Array)
        {
            for (int i = 0; i < i_Array.Length; i++)
            {
                i_Array[i] = 0;
            }
        }

        private static int maxFromArray(int[] i_Array)
        {
            int maxValue = -99;
            int indexOfMax = 0;
            for (int i = 0; i < i_Array.Length; i++)
            {
                if (i_Array[i] > maxValue)
                {
                    maxValue = i_Array[i];
                    indexOfMax = i;
                }
            }

            return indexOfMax;
        }

        public GameBoard[,] TransferGameBoardData(GameBoard[,] i_FromGameboard, GameBoard[,] i_ToGameBoard)
        {
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (i_FromGameboard[i, j].X == true)
                    {
                        i_ToGameBoard[i, j].X = true;
                    }

                    if (i_FromGameboard[i, j].O == true)
                    {
                        i_ToGameBoard[i, j].O = true;
                    }

                    if (i_FromGameboard[i, j].isEmpty == true)
                    {
                        i_ToGameBoard[i, j].isEmpty = true;
                    }
                }
            }

            return i_ToGameBoard;
        }

        public void ResetBoard(GameBoard[,] i_GameBoard)
        {
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    i_GameBoard[i, j].X = false;
                    i_GameBoard[i, j].O = false;
                    i_GameBoard[i, j].isEmpty = true;
                }
            }
        }

        private void flipPiecesInTempBoard(Player player, Player otherPlayer, int i_IndexLine, int i_IndexRow, int i_DirLine, int i_DirRow)
        {
            char currentPlayerMark = player.Mark;
            char otherPlayerMark = otherPlayer.Mark;

            i_IndexLine += i_DirLine;
            i_IndexRow += i_DirRow;

            while (IsValidCellRange(i_IndexLine + 1, i_IndexRow + 1))
            {
                if (cellToMarkChar(i_IndexLine, i_IndexRow) == otherPlayerMark)
                {
                    DrawInCellInTempBoard(player, i_IndexLine, i_IndexRow);
                }
                else if (cellToMarkChar(i_IndexLine, i_IndexRow) == currentPlayerMark)
                {
                    break;
                }

                i_IndexLine += i_DirLine;
                i_IndexRow += i_DirRow;
            }
        }

        public void DrawInCellInTempBoard(Player player, int i_Line, int i_Row)
        {
            if (player.Mark == 'X')
            {
                m_TempGameBoard[i_Line, i_Row].X = true;
                m_TempGameBoard[i_Line, i_Row].O = false;
            }
            else if (player.Mark == 'O')
            {
                m_TempGameBoard[i_Line, i_Row].O = true;
                m_TempGameBoard[i_Line, i_Row].X = false;
            }

            m_TempGameBoard[i_Line, i_Row].isEmpty = false;
        }

        private static bool isCornerCell(GameBoard[,] i_Gameboard, int i_BoardSize, int i_Line, int i_Row)
        {
            bool isCorner = false;

            if (i_Line == 1 || i_Line == i_BoardSize)
            {
                if (i_Row == 1 || i_Row == i_BoardSize)
                {
                    isCorner = true;
                }
            }

            return isCorner;
        }

        private static bool isEdgeCell(GameBoard[,] i_Gameboard, int i_BoardSize, int i_Line, int i_Row)
        {
            bool isEdge = false;

            if (i_Line == 1 || i_Line == i_BoardSize || i_Row == 1 || i_Row == i_BoardSize)
            {
                isEdge = true;
            }

            return isEdge;
        }

        public int CountComputerPieces(GameBoard[,] i_GameBoard)
        {
            int countPieces = 0;

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (i_GameBoard[i, j].O == true)
                    {
                        countPieces++;
                    }
                }
            }

            return countPieces;
        }

        public int CountPlayerPieces(GameBoard[,] i_GameBoard)
        {
            int countPieces = 0;

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (i_GameBoard[i, j].X == true)
                    {
                        countPieces++;
                    }
                }
            }

            return countPieces;
        }

        public bool IsValidCellRange(int i_Line, int i_Row)
        {
            bool isValid = true;

            if (i_Line < 1 || i_Line > r_BoardSize || i_Row < 1 || i_Row > r_BoardSize)
            {
                isValid = false;
            }

            return isValid;
        }

        protected virtual void OnBoardButtonChanged(EventArgs e)
        {
            if (BoardButtonChanged != null)
            {
                BoardButtonChanged.Invoke(this, e);
            }
        }

        protected virtual void OnGameEndedEvent(EventArgs e)
        {
            if (GameEnded != null)
            {
                GameEnded.Invoke(this, e);
            }
        }

        public struct GameBoard
        {
            private bool m_isX;
            private bool m_isO;
            private bool m_isEmpty;

            public bool X
            {
                get { return m_isX; }
                set { m_isX = value; }
            }

            public bool O
            {
                get { return m_isO; }
                set { m_isO = value; }
            }

            public bool isEmpty
            {
                get { return m_isEmpty; }
                set { m_isEmpty = value; }
            }
      }
}
}