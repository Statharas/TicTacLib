# TicTacLib

TicTacLib is meant to create a fast library that can be used to create the environment of a TicTacToe game (3x3) with an AI effortlessly.

The library builds a tree of possible game states, discarding mirrored or rotated versions. 
Each finished game sends back along its path which player won, P1 ++, P2 --. The end result is a tree of values that let us decide the best move to play.
