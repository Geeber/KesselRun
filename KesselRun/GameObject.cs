using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KesselRun
{
    public abstract class GameObject
    {
        private GameSquare location;
        public GameSquare Location
        {
            get { return location; }
            set
            {
                if (location != null)
                {
                    location.Occupant = null;
                }
                location = value;
                location.Occupant = this;
            }
        }

        private GameSquare startingLocation;
        public GameSquare StartingLocation
        {
            get { return startingLocation; }
            set { startingLocation = value; }
        }

        protected GameBoard board;

        public GameObject(GameBoard board)
        {
            this.board = board;
            location = null;
        }

        public abstract void AdvanceObjectState();

        public virtual void ResetObjectState()
        {
            Location = StartingLocation;
        }
    }

    class PlayerObject : GameObject
    {
        public PlayerObject(GameBoard board)
            : base(board)
        {
        }

        public override void AdvanceObjectState()
        {
            // Do nothing
        }

        public bool Move(Direction dir)
        {
            GameSquare target = board.GetNeighbor(Location, dir);
            if (target == null)
            {
                return false;
            }

            if (target is WarpSquare)
            {
                Location = ((WarpSquare)target).TargetSquare;
            }
            else
            {
                Location = target;
            }
            return true;
        }
    }

    abstract class HostileObject : GameObject
    {
        public HostileObject(GameBoard board)
            : base(board)
        {
        }

        public abstract void MarkUnsafeSquares();
    }

    class PatrolObject : HostileObject
    {

        private GameSquare[] path;

        public PatrolObject(GameBoard board, GameSquare[] path)
            : base(board)
        {
            this.path = path;
        }

        public override void AdvanceObjectState()
        {
            int currentIndex = 0;
            for (; currentIndex < path.Length; currentIndex++)
            {
                if (path[currentIndex] == Location)
                {
                    break;
                }
            }

            int nextIndex = currentIndex + 1;
            if (nextIndex >= path.Length)
            {
                nextIndex = 0;
            }

            Location = path[nextIndex];
        }

        public override void MarkUnsafeSquares()
        {
            Location.IsSafe = false;

            GameSquare leftNeighbor = board.GetNeighbor(Location, Direction.Left);
            if (leftNeighbor != null) leftNeighbor.IsSafe = false;

            GameSquare upLeftNeighbor = board.GetNeighbor(Location, Direction.UpLeft);
            if (upLeftNeighbor != null) upLeftNeighbor.IsSafe = false;

            GameSquare upRightNeighbor = board.GetNeighbor(Location, Direction.UpRight);
            if (upRightNeighbor != null) upRightNeighbor.IsSafe = false;

            GameSquare rightNeighbor = board.GetNeighbor(Location, Direction.Right);
            if (rightNeighbor != null) rightNeighbor.IsSafe = false;

            GameSquare downRightNeighbor = board.GetNeighbor(Location, Direction.DownRight);
            if (downRightNeighbor != null) downRightNeighbor.IsSafe = false;

            GameSquare downLeftNeighbor = board.GetNeighbor(Location, Direction.DownLeft);
            if (downLeftNeighbor != null) downLeftNeighbor.IsSafe = false;
        }
    }

    class ProbeObject : HostileObject
    {
        private Direction[] startScans;

        private List<Direction> scans; 
        public List<Direction> Scans
        {
            get { return scans; }
        }
            
        private bool rotateClockwise;
        public bool RotateClockwise
        {
            get { return rotateClockwise; }
        }

        public ProbeObject(GameBoard board, Direction[] startScans, bool rotateClockwise)
            : base(board)
        {
            this.startScans = startScans;
            this.rotateClockwise = rotateClockwise;
        }

        public override void ResetObjectState()
        {
            base.ResetObjectState();

            scans = new List<Direction>(startScans);
        }

        public override void AdvanceObjectState()
        {
            for (int i = 0; i < scans.Count; i++)
            {
                Direction newDir = NextDirection(scans[0], rotateClockwise);
                scans.Add(newDir);
                scans.RemoveAt(0);
            }
        }

        public override void MarkUnsafeSquares()
        {
            Location.IsSafe = false;

            foreach (Direction dir in scans)
            {
                GameSquare square = board.GetNeighbor(Location, dir);
                if (square != null)
                {
                    square.IsSafe = false;
                }
            }
        }

        private static Direction NextDirection(Direction currentDir, bool rotateClockwise)
        {
            if (rotateClockwise)
            {
                switch (currentDir)
                {
                    case Direction.Left:
                        return Direction.UpLeft;
                    case Direction.UpLeft:
                        return Direction.UpRight;
                    case Direction.UpRight:
                        return Direction.Right;
                    case Direction.Right:
                        return Direction.DownRight;
                    case Direction.DownRight:
                        return Direction.DownLeft;
                    case Direction.DownLeft:
                        return Direction.Left;
                    default:
                        throw new ArgumentException("Unknown Direction");
                }
            }
            else
            {
                switch (currentDir)
                {
                    case Direction.Left:
                        return Direction.DownLeft;
                    case Direction.DownLeft:
                        return Direction.DownRight;
                    case Direction.DownRight:
                        return Direction.Right;
                    case Direction.Right:
                        return Direction.UpRight;
                    case Direction.UpRight:
                        return Direction.UpLeft;
                    case Direction.UpLeft:
                        return Direction.Left;
                    default:
                        throw new ArgumentException("Unknown Direction");
                }
            }
        }
    }
}
