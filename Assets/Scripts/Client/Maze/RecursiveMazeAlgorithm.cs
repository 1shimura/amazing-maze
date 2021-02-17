
using UnityEngine;

namespace Client.Maze
{
    public class RecursiveMazeAlgorithm : BasicMazeGenerator
    {
        public RecursiveMazeAlgorithm(int rows, int columns) : base(rows, columns)
        {
        }

        public override void GenerateMaze()
        {
            VisitCell(0, 0, MoveDirection.Start);
        }

        private void VisitCell(int row, int column, MoveDirection madeMove)
        {
            var availableMoves = new MoveDirection[4];
            var availableMovesCount = 0;

            do
            {
                availableMovesCount = 0;

                //check move right

                if (column + 1 < ColumnCount && !GetMazeCell(row, column + 1).IsVisited)
                {
                    availableMoves[availableMovesCount] = MoveDirection.Right;
                    availableMovesCount++;
                }
                else if (!GetMazeCell(row, column).IsVisited && madeMove != MoveDirection.Left)
                {
                    GetMazeCell(row, column).WallRight = true;
                }

                //check move forward

                if (row + 1 < RowCount && !GetMazeCell(row + 1, column).IsVisited)
                {
                    availableMoves[availableMovesCount] = MoveDirection.Front;
                    availableMovesCount++;
                }
                else if (!GetMazeCell(row, column).IsVisited && madeMove != MoveDirection.Back)
                {
                    GetMazeCell(row, column).WallFront = true;
                }

                //check move left

                if (column > 0 && column - 1 >= 0 && !GetMazeCell(row, column - 1).IsVisited)
                {
                    availableMoves[availableMovesCount] = MoveDirection.Left;
                    availableMovesCount++;
                }
                else if (!GetMazeCell(row, column).IsVisited && madeMove != MoveDirection.Right)
                {
                    GetMazeCell(row, column).WallLeft = true;
                }
                //check move backward

                if (row > 0 && row - 1 >= 0 && !GetMazeCell(row - 1, column).IsVisited)
                {
                    availableMoves[availableMovesCount] = MoveDirection.Back;
                    availableMovesCount++;
                }
                else if (!GetMazeCell(row, column).IsVisited && madeMove != MoveDirection.Front)
                {
                    GetMazeCell(row, column).WallBack = true;
                }

                if (availableMovesCount == 0 && !GetMazeCell(row, column).IsVisited)
                {
                    GetMazeCell(row, column).IsGoal = true;
                }

                GetMazeCell(row, column).IsVisited = true;

                if (availableMovesCount > 0)
                {
                    switch (availableMoves[Random.Range(0, availableMovesCount)])
                    {
                        case MoveDirection.Start:
                            break;
                        case MoveDirection.Right:
                            VisitCell(row, column + 1, MoveDirection.Right);
                            break;
                        case MoveDirection.Front:
                            VisitCell(row + 1, column, MoveDirection.Front);
                            break;
                        case MoveDirection.Left:
                            VisitCell(row, column - 1, MoveDirection.Left);
                            break;
                        case MoveDirection.Back:
                            VisitCell(row - 1, column, MoveDirection.Back);
                            break;
                    }
                }
                
            } while (availableMovesCount > 0);
        }
    }
}