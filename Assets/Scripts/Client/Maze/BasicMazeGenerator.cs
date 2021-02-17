
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Client.Maze
{
    public abstract class BasicMazeGenerator
    {
        //Used to obtain the Row and ColumnPrefab from the private variables 
        public int RowCount => _mazeRows;
        public int ColumnCount => _mazeColumns;

        private int _mazeRows;
        private int _mazeColumns;
        private MazeCell[,] _maze;

        //A constructor that makes the rows and columns non-zero
        //and instantiates a new MazeCell at that specific rank and range

        public BasicMazeGenerator(int rows, int columns)
        {
            _mazeRows = Mathf.Abs(rows);
            _mazeColumns = Mathf.Abs(columns);

            if (_mazeRows == 0)
            {
                _mazeRows = 1;
            }

            if (_mazeColumns == 0)
            {
                _mazeColumns = 1;
            }

            _maze = new MazeCell[rows, columns];

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    _maze[row, column] = new MazeCell();
                }
            }
        }

        //called by the algorithm class to start the algorithm
        public abstract void GenerateMaze();

        public MazeCell GetMazeCell(int row, int column)
        {
            if (row >= 0 && column >= 0 && row < _mazeRows && column < _mazeColumns)
            {
                return _maze[row, column];
            }
            
            throw new System.ArgumentOutOfRangeException();
        }

        public Dictionary<Point, MazeCell> GetRandomMazeCells(int number, bool skipStart = true)
        {
            var resultDict = new Dictionary<Point, MazeCell>();
            var searchIndex = 0;

            while (resultDict.Count < number && searchIndex < _maze.Length)
            {
                searchIndex++;

                var randRowIndex = Random.Range(0, _maze.GetLength(0));
                var randColumnIndex = Random.Range(0, _maze.GetLength(1));
                
                var cell = GetMazeCell(randRowIndex, randColumnIndex);
                var cellPosition = new Point(randRowIndex, randColumnIndex);
                
                if (skipStart && cellPosition == new Point(0,0)) continue;

                if (!resultDict.ContainsKey(cellPosition))
                {
                    resultDict.Add(cellPosition, cell);
                }
            }

            return resultDict;
        }
    }
}