using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KesselRun
{
    public abstract class GameSquare
    {

        protected bool isSafe;
        public virtual bool IsSafe
        {
            get { return isSafe; }
            set { isSafe = value; }
        }

        private bool isWinningLoc;
        public bool IsWinningLoc
        {
            get { return isWinningLoc; }
            set { isWinningLoc = value; }
        }

        private Point location;
        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        public int Row
        {
            get { return location.Y; }
            set { location.Y = value; }
        }

        public int Column
        {
            get { return location.X; }
            set { location.X = value; }
        }

        private GameObject occupant;
        public GameObject Occupant
        {
            get { return occupant; }
            set { occupant = value; }
        }

        public GameSquare(int x, int y)
        {
            location = new Point(x, y);
            isSafe = true;
            isWinningLoc = false;
        }
    }

    class EmptySquare : GameSquare
    {
        public EmptySquare(int x, int y)
            : base(x, y)
        {
        }
    }

    class SafeSquare : GameSquare
    {
        public override bool IsSafe
        {
            get { return true; }
            set { }
        }

        public SafeSquare(int x, int y)
            : base(x, y)
        {
        }
    }

    class HostileSquare : GameSquare
    {
        public override bool IsSafe
        {
            get { return false; }
            set { }
        }

        public HostileSquare(int x, int y)
            : base(x, y)
        {
        }
    }

    class WarpSquare : GameSquare
    {
        private GameSquare targetSquare;
        public GameSquare TargetSquare
        {
            get { return targetSquare; }
            set { targetSquare = value; }
        }

        public override bool IsSafe
        {
            get { return isSafe; }
            set
            { 
                isSafe = value;
                if ((value == false) && (targetSquare is WarpSquare))
                {
                    ((WarpSquare)targetSquare).isSafe = false;
                }
            }
        }

        private Brush warpColor;
        public Brush WarpColor
        {
            get { return warpColor; }
            set { warpColor = value; }
        }

        public WarpSquare(int x, int y)
            : base(x, y)
        {
        }
    }
}
