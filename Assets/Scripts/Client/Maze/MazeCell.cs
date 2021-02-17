namespace Client.Maze
{
    public class MazeCell
    {
        public bool IsVisited = false;
        public bool WallRight = false;
        public bool WallFront = false;
        public bool WallLeft = false;
        public bool WallBack = false;
        public bool IsGoal = false;
    }

    public enum MoveDirection
    {
        Start = 0,
        Right = 1,
        Front = 2,
        Left = 3,
        Back = 4,
    };
}