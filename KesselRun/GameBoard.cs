using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KesselRun
{
    public enum Direction
    {
        Left,
        UpLeft,
        UpRight,
        Right,
        DownRight,
        DownLeft
    }

    public enum GameState
    {
        InProgress,
        Loss,
        Win
    }

    public class GameBoard
    {

        private GameSquare[,] board;
        private List<GameObject> objects;
        private PlayerObject player;

        public int RowCount
        {
            get { return board.GetLength(1); }
        }

        public int ColumnCount
        {
            get { return board.GetLength(0); }
        }

        private GameState state;
        public GameState State
        {
            get { return state; }
        }

        private List<Direction> moveSequence;
        public List<Direction> MoveSequence
        {
            get { return moveSequence; }
        }

        public GameBoard()
        {
            board = new GameSquare[8, 7];

            // Create non-empty game squares

            // Safe squares...
            board[4, 1] = new SafeSquare(4, 1);
            board[6, 3] = new SafeSquare(6, 3);

            // Hostile squares...
            board[6, 0] = new HostileSquare(6, 0);
            board[2, 4] = new HostileSquare(2, 4);
            board[7, 6] = new HostileSquare(7, 6);

            // Warp squares...
            // Warp gate A
            board[5, 0] = new WarpSquare(5, 0);
            board[1, 3] = new WarpSquare(1, 3);
            ((WarpSquare)board[5, 0]).TargetSquare = board[1, 3];
            ((WarpSquare)board[1, 3]).TargetSquare = board[5, 0];
            ((WarpSquare)board[5, 0]).WarpColor = Brushes.Blue;
            ((WarpSquare)board[1, 3]).WarpColor = Brushes.Blue;

            // Warp gate B
            board[1, 1] = new WarpSquare(1, 1);
            board[7, 5] = new WarpSquare(7, 5);
            ((WarpSquare)board[1, 1]).TargetSquare = board[7, 5];
            ((WarpSquare)board[7, 5]).TargetSquare = board[1, 1];
            ((WarpSquare)board[1, 1]).WarpColor = Brushes.Yellow;
            ((WarpSquare)board[7, 5]).WarpColor = Brushes.Yellow;

            // Warp gate C
            board[2, 0] = new WarpSquare(2, 0);
            board[4, 6] = new WarpSquare(4, 6);
            ((WarpSquare)board[2, 0]).TargetSquare = board[4, 6];
            ((WarpSquare)board[4, 6]).TargetSquare = board[2, 0];
            ((WarpSquare)board[2, 0]).WarpColor = Brushes.Violet;
            ((WarpSquare)board[4, 6]).WarpColor = Brushes.Violet;

            // Everything else is empty
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    if (board[i, j] == null)
                    {
                        board[i, j] = new EmptySquare(i, j);
                    }
                }
            }

            // Mark winning location
            board[7, 0].IsWinningLoc = true;

            // Set state
            ResetState();
        }

        public void ResetState()
        {
            state = GameState.InProgress;
            moveSequence = new List<Direction>();

            // Create objects if they don't exist; if they do, clear out
            // their old state.
            if (objects == null)
            {
                objects = new List<GameObject>();

                // Player
                player = new PlayerObject(this);
                player.StartingLocation = board[0, 6];
                objects.Add(player);

                // Patrols
                PatrolObject pat1 = new PatrolObject(this, new GameSquare[] {board[2, 3],
                                                                             board[1, 4],
                                                                             board[2, 5],
                                                                             board[3, 5],
                                                                             board[3, 4],
                                                                             board[3, 3]});
                pat1.StartingLocation = board[2, 3];
                objects.Add(pat1);

                PatrolObject pat2 = new PatrolObject(this, new GameSquare[] {board[6, 2],
                                                                             board[5, 2],
                                                                             board[5, 3],
                                                                             board[5, 4],
                                                                             board[6, 4],
                                                                             board[7, 3]});
                pat2.StartingLocation = board[6, 2];
                objects.Add(pat2);

                // Probes
                ProbeObject probe1 = new ProbeObject(this, 
                                                     new Direction[] {Direction.Left,
                                                                      Direction.Right},
                                                     false);
                probe1.StartingLocation = board[1, 0];
                objects.Add(probe1);

                ProbeObject probe2 = new ProbeObject(this,
                                                     new Direction[] {Direction.Left,
                                                                      Direction.DownLeft,
                                                                      Direction.Right,
                                                                      Direction.UpRight},
                                                     false);
                probe2.StartingLocation = board[3, 0];
                objects.Add(probe2);

                ProbeObject probe3 = new ProbeObject(this,
                                                     new Direction[] {Direction.Left,
                                                                      Direction.DownLeft,
                                                                      Direction.Right,
                                                                      Direction.UpLeft},
                                                     true);
                probe3.StartingLocation = board[2, 2];
                objects.Add(probe3);

                ProbeObject probe4 = new ProbeObject(this,
                                                     new Direction[] {Direction.Left,
                                                                      Direction.DownLeft,
                                                                      Direction.UpRight,
                                                                      Direction.UpLeft},
                                                     true);
                probe4.StartingLocation = board[4, 2];
                objects.Add(probe4);

                ProbeObject probe5 = new ProbeObject(this,
                                                     new Direction[] {Direction.DownLeft,
                                                                      Direction.Right},
                                                     false);
                probe5.StartingLocation = board[7, 2];
                objects.Add(probe5);

                ProbeObject probe6 = new ProbeObject(this,
                                                     new Direction[] {Direction.Left,
                                                                      Direction.DownRight,
                                                                      Direction.Right,
                                                                      Direction.UpRight},
                                                     true);
                probe6.StartingLocation = board[0, 3];
                objects.Add(probe6);

                ProbeObject probe7 = new ProbeObject(this,
                                                     new Direction[] {Direction.DownRight,
                                                                      Direction.UpLeft},
                                                     true);
                probe7.StartingLocation = board[4, 4];
                objects.Add(probe7);

                ProbeObject probe8 = new ProbeObject(this,
                                                     new Direction[] {Direction.Left,
                                                                      Direction.DownRight,
                                                                      Direction.UpRight},
                                                     false);
                probe8.StartingLocation = board[7, 4];
                objects.Add(probe8);

                ProbeObject probe9 = new ProbeObject(this,
                                                     new Direction[] {Direction.Left,
                                                                      Direction.DownRight,
                                                                      Direction.UpRight},
                                                     false);
                probe9.StartingLocation = board[1, 5];
                objects.Add(probe9);

                ProbeObject probe10 = new ProbeObject(this,
                                                      new Direction[] {Direction.DownLeft,
                                                                       Direction.DownRight,
                                                                       Direction.UpRight,
                                                                       Direction.UpLeft},
                                                      true);
                probe10.StartingLocation = board[2, 6];
                objects.Add(probe10);

                ProbeObject probe11 = new ProbeObject(this,
                                                      new Direction[] {Direction.DownLeft,
                                                                       Direction.Right,
                                                                       Direction.UpLeft},
                                                      true);
                probe11.StartingLocation = board[5, 6];
                objects.Add(probe11);
            }

            // Set initial object state
            foreach (GameObject obj in objects)
            {
                obj.ResetObjectState();
            }

            MarkSafeSquares();
        }

        public GameSquare GetSquare(int x, int y)
        {
            if ((y < 0) || (y >= RowCount) ||
                (x < 0) || (x >= ColumnCount))
            {
                throw new IndexOutOfRangeException();
            }

            return board[x, y];
        }

        public bool MovePlayer(Direction dir)
        {
            bool moveSucceeded = player.Move(dir);
            if (moveSucceeded)
            {
                moveSequence.Add(dir);
                AdvanceGameState();
                if (!player.Location.IsSafe)
                {
                    state = GameState.Loss;
                }
                else if (player.Location.IsWinningLoc)
                {
                    state = GameState.Win;
                }
            }
            return moveSucceeded;
        }

        private void AdvanceGameState()
        {
            foreach (GameObject obj in objects)
            {
                obj.AdvanceObjectState();
            }

            MarkSafeSquares();
        }

        private void MarkSafeSquares()
        {
            // Clear safe square markings
            foreach (GameSquare square in board)
            {
                square.IsSafe = true;
            }

            // Mark unsafe squares
            foreach (GameObject obj in objects)
            {
                if (obj is HostileObject)
                {
                    ((HostileObject)obj).MarkUnsafeSquares();
                }
            }
        }

        #region Utilities for calculating neighbors

        public GameSquare GetNeighbor(GameSquare square, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return LeftNeighbor(square);
                case Direction.UpLeft:
                    return UpLeftNeighbor(square);
                case Direction.UpRight:
                    return UpRightNeighbor(square);
                case Direction.Right:
                    return RightNeighbor(square);
                case Direction.DownRight:
                    return DownRightNeighbor(square);
                case Direction.DownLeft:
                    return DownLeftNeighbor(square);
                default:
                    throw new ArgumentException("Unknown direction");
            }
        }

        private GameSquare LeftNeighbor(GameSquare square)
        {
            Point location = square.Location;
            if ((location.X < 0) || (location.X >= ColumnCount) ||
                (location.Y < 0) || (location.Y >= RowCount))
            {
                throw new IndexOutOfRangeException();
            }

            Point leftNeighbor = new Point(location.X, location.Y);
            leftNeighbor.X -= 1;

            if (leftNeighbor.X < 0)
            {
                return null;
            }
            else
            {
                return board[leftNeighbor.X, leftNeighbor.Y];
            }
        }

        private GameSquare DownLeftNeighbor(GameSquare square)
        {
            Point location = square.Location;
            if ((location.X < 0) || (location.X >= ColumnCount) ||
                (location.Y < 0) || (location.Y >= RowCount))
            {
                throw new IndexOutOfRangeException();
            }

            Point downLeftNeighbor = new Point(location.X, location.Y);
            downLeftNeighbor.Y += 1;

            // For even rows, up-left is the same column index; for odd rows, it's -1
            if (location.Y % 2 == 1)
            {
                downLeftNeighbor.X -= 1;
            }

            if ((downLeftNeighbor.X < 0) || (downLeftNeighbor.Y >= RowCount))
            {
                return null;
            }
            else
            {
                return board[downLeftNeighbor.X, downLeftNeighbor.Y];
            }
        }

        private GameSquare DownRightNeighbor(GameSquare square)
        {
            Point location = square.Location;
            if ((location.X < 0) || (location.X >= ColumnCount) ||
                (location.Y < 0) || (location.Y >= RowCount))
            {
                throw new IndexOutOfRangeException();
            }

            Point downRightNeighbor = new Point(location.X, location.Y);
            downRightNeighbor.Y += 1;

            // For even rows, up-right is +1; for odd rows, it's the same
            if (location.Y % 2 == 0)
            {
                downRightNeighbor.X += 1;
            }

            if ((downRightNeighbor.X >= ColumnCount) || (downRightNeighbor.Y >= RowCount))
            {
                return null;
            }
            else
            {
                return board[downRightNeighbor.X, downRightNeighbor.Y];
            }
        }

        private GameSquare RightNeighbor(GameSquare square)
        {
            Point location = square.Location;
            if ((location.X < 0) || (location.X >= ColumnCount) ||
                (location.Y < 0) || (location.Y >= RowCount))
            {
                throw new IndexOutOfRangeException();
            }

            Point rightNeighbor = new Point(location.X, location.Y);
            rightNeighbor.X += 1;

            if (rightNeighbor.X >= ColumnCount)
            {
                return null;
            }
            else
            {
                return board[rightNeighbor.X, rightNeighbor.Y];
            }
        }

        private GameSquare UpRightNeighbor(GameSquare square)
        {
            Point location = square.Location;
            if ((location.X < 0) || (location.X >= ColumnCount) ||
                (location.Y < 0) || (location.Y >= RowCount))
            {
                throw new IndexOutOfRangeException();
            }

            Point upRightNeighbor = new Point(location.X, location.Y);
            upRightNeighbor.Y -= 1;

            // For even rows, down-right is +1; for odd rows, it's the same
            if (location.Y % 2 == 0)
            {
                upRightNeighbor.X += 1;
            }

            if ((upRightNeighbor.X >= ColumnCount) || (upRightNeighbor.Y < 0))
            {
                return null;
            }
            else
            {
                return board[upRightNeighbor.X, upRightNeighbor.Y];
            }
        }

        private GameSquare UpLeftNeighbor(GameSquare square)
        {
            Point location = square.Location;
            if ((location.X < 0) || (location.X >= ColumnCount) ||
                (location.Y < 0) || (location.Y >= RowCount))
            {
                throw new IndexOutOfRangeException();
            }

            Point upLeftNeighbor = new Point(location.X, location.Y);
            upLeftNeighbor.Y -= 1;

            // For even rows, down-left is the same; for odd rows, it's -1
            if (location.Y % 2 == 1)
            {
                upLeftNeighbor.X -= 1;
            }

            if ((upLeftNeighbor.X < 0) || (upLeftNeighbor.Y < 0))
            {
                return null;
            }
            else
            {
                return board[upLeftNeighbor.X, upLeftNeighbor.Y];
            }
        }

        #endregion
    }
}
