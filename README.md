KesselRun
=========

This is a software puzzle which was written for the Microsoft Intern PuzzleDay 2008.

# Rules

## The Puzzle

To solve this puzzle, just beat the game. There are no hints or tricks in these rules, they're just here to explain how to play. We promise.

## The Goal

> "You've never heard of the Millennium Falcon? She's the ship that made the Kessel Run in less than 12 parsecs." - Captain Han Solo

Captain Solo was a true master, and equaling his feat is nigh impossible, but in this puzzle you will strive to complete the Kessel Run in exactly 12 parsecs. Each cell represents a half-parsec distance (center to center), so you will have to reach the finish in exactly 24 moves.

## Rules

1. You start in the lower-left corner of the map, and must reach the upper-right corner. Your ship is symbolized by a white 'M'.
2. You may move in any direction, using the keys around 's', as follows:
  1. 'a' : left
  2. 'w' : up-left
  3. 'e' : up-right
  4. 'd' : right
  5. 'x' : down-right
  6. 'z' : down-left
3. You may not move off the board, nor "wrap around".
4. The other objects on the map are as follows:
  1. Star Destroyers (symbolized by a red 'SD'): These move around in a hexagonal path. As they move, they scan all adjacent cells for unauthorized activity. If you are found, you lose the game.
  2. Probes (symbolized by red lines and a direction arrow): These are stationary objects. Like Star Destroyers, they scan adjacent cells, but not all at once. The cells currently being scanned are indicated by the radial lines, and the arrow indicates which direction they will rotate each move.
  3. Asteroid Fields (symbolized by a green 'A'): These are impermeable to scanning, and therefore are always safe.
  4. Hostile Planets (symbolized by a red 'H'): These are never safe, even if not being scanned.
  5. Warp Zones (symbolized by a colored 'W'): These come in pairs, and allow you to jump from one end to the other. When you move into a Warp Zone, you immediately (without taking an additional turn) appear on the other end. However, if either end of the Warp is being scanned when you move into it, you lose.
5. As a visual aid, all "unsafe" squares (those being scanned or those containing Hostile Planets) are shaded red.
6. The state of the map "advances" (Star Destroyers move and Probes rotate) when (and only when) you move. You may not "stand still".

That's it. Good luck!
