using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KesselRun
{
    public partial class GameBoardControl : UserControl
    {

        private const float HEX_WIDTH = 66;
        private const float HEX_HEIGHT = HEX_WIDTH * .866F;
        private const float HEX_ADJUST = HEX_WIDTH * .1875F;

        private static Font coordFont = new Font("Arial", 8);
        private static Font finishFont = new Font("Arial", 7);
        private static Font objectFont = new Font("Courier New", 14);
        private static Brush unsafeBrush = new SolidBrush(Color.FromArgb(50, Color.Red));
        private static Pen probePen = new Pen(Color.Red, 3F);

        private GameBoard board;
        private int moveCount;

        public GameBoardControl()
        {
            this.board = null;
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |
                              ControlStyles.UserPaint |
                              ControlStyles.AllPaintingInWmPaint,
                          true);
            this.UpdateStyles();
        }

        public void Initialize(GameBoard board)
        {
            moveCount = 0;
            gameStatusStripLabel.Text = "Distance Traveled: " + ((float)moveCount) / 2 + " Parsecs";
            this.board = board;
        }

        public void Reset()
        {
            moveCount = 0;
            gameStatusStripLabel.Text = "Distance Traveled: " + ((float)moveCount) / 2 + " Parsecs";
            board.ResetState();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
 	        base.OnPaint(e);
            Graphics g = e.Graphics;

            if (board == null)
            {
                return;
            }

            // Paint all the squares
            for (int i = 0; i < board.ColumnCount; i++)
            {
                for (int j = 0; j < board.RowCount; j++)
                {
                    DrawGameSquare(board.GetSquare(i, j), g);
                }
            }
        }

        protected void DrawGameSquare(GameSquare square,
                                      Graphics g)
        {
            // (x,y) is the upper-left corner of the rectangle that
            // approximates the hexagon
            float x = 10 + square.Column * HEX_WIDTH;
            float y = 20 + square.Row * HEX_HEIGHT;

            // Offset even rows by 1/2 of width
            if (square.Row % 2 == 0)
            {
                x += HEX_WIDTH / 2;
            }
            
            DrawSquareBackground(g, square, x, y);
            DrawOccupant(g, square.Occupant, x, y);
        }

        protected void DrawSquareBackground(Graphics g, GameSquare square, float x, float y)
        {
            // Find the outline of the hexagon
            PointF[] hexagon = {new PointF(x, y + HEX_ADJUST),
                                new PointF(x + HEX_WIDTH / 2, y - HEX_ADJUST),
                                new PointF(x + HEX_WIDTH, y + HEX_ADJUST),
                                new PointF(x + HEX_WIDTH, y + HEX_HEIGHT - HEX_ADJUST),
                                new PointF(x + HEX_WIDTH / 2, y + HEX_HEIGHT + HEX_ADJUST),
                                new PointF(x, y + HEX_HEIGHT - HEX_ADJUST)};

            // Draw the outline
            g.DrawPolygon(Pens.White, hexagon);

            // If the square isn't safe, highlight it
            if (!square.IsSafe)
            {
                g.FillPolygon(unsafeBrush, hexagon);
            }

            // Put in the coordinates (for debugging only)
            // g.DrawString(square.Column + "," + square.Row, coordFont, Brushes.White, x + 23, y - 3);

            // Mark start and finish
            if (square.IsWinningLoc)
            {
                g.DrawString("Finish", finishFont, Brushes.White, x + 20, y - 2);
            }

            // Add tag for special squares
            string squareTag = "";
            Brush tagBrush;
            if (square is SafeSquare)
            {
                squareTag = "A";
                tagBrush = Brushes.Green;
            }
            else if (square is HostileSquare)
            {
                squareTag = "H";
                tagBrush = Brushes.Red;
            }
            else if (square is WarpSquare)
            {
                squareTag = "W";
                tagBrush = ((WarpSquare)square).WarpColor;
            }
            else
            {
                return;
            }

            g.DrawString(squareTag, objectFont, tagBrush, x + 30, y + 20);
        }

        protected void DrawOccupant(Graphics g, GameObject occupant, float x, float y)
        {
            if (occupant == null)
            {
                return;
            }

            // Draw tag
            string occupantTag = "";
            Brush occupantBrush;
            if (occupant is PlayerObject)
            {
                occupantTag = "M";
                occupantBrush = Brushes.White;
            }
            else if (occupant is PatrolObject)
            {
                occupantTag = "SD";
                occupantBrush = Brushes.Red;
            }
            else if (occupant is ProbeObject)
            {
                occupantBrush = Brushes.White;
                // Do not show symbol for probe
            }
            else
            {
                throw new ArgumentException("Unrecognized occupant type");
            }

            g.DrawString(occupantTag, objectFont, occupantBrush, x + 15, y + 20);

            // Draw probe search directions
            if (occupant is ProbeObject)
            {
                ProbeObject probe = (ProbeObject)occupant;

                // Draw center dot
                g.FillEllipse(Brushes.Red,
                              x + (HEX_WIDTH / 2) - 6,
                              y + (HEX_HEIGHT / 2) - 6,
                              12F, 12F);

                // Draw arrows
                foreach (Direction dir in probe.Scans)
                {
                    float lineEndX;
                    float lineEndY;
                    switch (dir)
                    {
                        case Direction.Left:
                            lineEndX = x + 5;
                            lineEndY = y + HEX_HEIGHT / 2;
                            break;
                        case Direction.UpLeft:
                            lineEndX = x + 18;
                            lineEndY = y + 3;
                            break;
                        case Direction.UpRight:
                            lineEndX = x + HEX_WIDTH - 18;
                            lineEndY = y + 3;
                            break;
                        case Direction.Right:
                            lineEndX = x + HEX_WIDTH - 5;
                            lineEndY = y + HEX_HEIGHT/2;
                            break;
                        case Direction.DownRight:
                            lineEndX = x + HEX_WIDTH - 18;
                            lineEndY = y + HEX_HEIGHT - 3;
                            break;
                        case Direction.DownLeft:
                            lineEndX = x + 18;
                            lineEndY = y + HEX_HEIGHT - 3;
                            break;
                        default:
                            throw new ArgumentException("Unknown Direction");
                    }
                    g.DrawLine(probePen, x + HEX_WIDTH/2, y + HEX_HEIGHT/2, lineEndX, lineEndY);
                }

                // Draw direction
                if (probe.RotateClockwise)
                {
                    g.DrawArc(Pens.Red,
                              x + (HEX_WIDTH / 2) - 20,
                              y + (HEX_HEIGHT / 2) - 20,
                              40F, 40F,
                              180, 270);
                    g.DrawLine(Pens.Red,
                               x + (HEX_WIDTH / 2),
                               y + (HEX_HEIGHT / 2) + 20,
                               x + (HEX_WIDTH / 2) + 5,
                               y + (HEX_HEIGHT / 2) + 20 - 5);
                    g.DrawLine(Pens.Red,
                               x + (HEX_WIDTH / 2),
                               y + (HEX_HEIGHT / 2) + 20,
                               x + (HEX_WIDTH / 2) + 5,
                               y + (HEX_HEIGHT / 2) + 20 + 5);
                }
                else
                {
                    g.DrawArc(Pens.Red,
                              x + (HEX_WIDTH / 2) - 20,
                              y + (HEX_HEIGHT / 2) - 20,
                              40F, 40F,
                              90, 270);
                    g.DrawLine(Pens.Red,
                               x + (HEX_WIDTH / 2),
                               y + (HEX_HEIGHT / 2) + 20,
                               x + (HEX_WIDTH / 2) - 5,
                               y + (HEX_HEIGHT / 2) + 20 - 5);
                    g.DrawLine(Pens.Red,
                               x + (HEX_WIDTH / 2),
                               y + (HEX_HEIGHT / 2) + 20,
                               x + (HEX_WIDTH / 2) - 5,
                               y + (HEX_HEIGHT / 2) + 20 + 5);
                }

            }
        }

        private void GameBoardControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            Direction dir;
            switch (e.KeyChar)
            {
                case 'a':
                    dir = Direction.Left;
                    break;
                case 'w':
                    dir = Direction.UpLeft;
                    break;
                case 'e':
                    dir = Direction.UpRight;
                    break;
                case 'd':
                    dir = Direction.Right;
                    break;
                case 'x':
                    dir = Direction.DownRight;
                    break;
                case 'z':
                    dir = Direction.DownLeft;
                    break;
                default:
                    // Do nothing
                    return;
            }

            if (board.MovePlayer(dir))
            {
                moveCount++;
                gameStatusStripLabel.Text = "Distance Traveled: " + ((float)moveCount) / 2 + " Parsecs";
                this.Invalidate();

                if (board.State == GameState.Loss)
                {
                    MessageBox.Show("You moved into an unsafe cell. Play again?", "You Lose!");
                    this.Reset();
                }
                else if (board.State == GameState.Win)
                {
                    if (moveCount < 24)
                    {
                        MessageBox.Show("Congratulations, you broke the puzzle! You found a winning path in less than " +
                                        "24 moves. This isn't supposed to be possible. Please e-mail puzzle central " +
                                        "and let us know.", "Oops, Broken Puzzle");
                    }
                    else if (moveCount == 24)
                    {
                        // byte[] encAnswer = CryptoHelper.CreateEncryptedAnswerBlob("CORELLIA", board.MoveSequence);
                        // MessageBox.Show("Encrypted Answer: " + encAnswer);
                        // string decAnswer = CryptoHelper.GetAnswer(board.MoveSequence);
                        // MessageBox.Show("Decrypted Answer: " + decAnswer);
                        MessageBox.Show("Congratulations, you made the Kessel Run in 12 Parsecs! " +
                                        "Please submit the answer \"CORELLIA\" via the answer submission page.",
                                        "You Win!");
                    }
                    else
                    {
                        MessageBox.Show("You completed the Kessel Run, but it took you " + ((float)moveCount) / 2 +
                                        " Parsecs to do it. See if you can find a shorter route...",
                                        "Your Path Was Too Long");
                    }
                    this.Reset();
                }
            }
        }
    }
}
