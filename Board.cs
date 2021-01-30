using System;

namespace TicTac
{
    public static partial class Board
    {
        [Flags]
        public enum State
        {
            Empty = 0,
            P1 = 1,
            P2 = 2,
            Full=3
        }
        private static State[,] Pattern = new State[3,3];
        /// <summary>
        /// Resets the board state. Use this when you want to restart the game.
        /// </summary>
        public static void Clear(){
            Pattern = new State[3,3] {
                { State.Empty, State.Empty, State.Empty },
                { State.Empty, State.Empty, State.Empty }, 
                { State.Empty, State.Empty, State.Empty }};
        }
        /// <summary>
        /// Assigns a player move to the board. This is player agnostic, but we record it on the playlist.
        /// </summary>
        /// <param name="Check">The change of state pushed by a player or AI</param>
        /// <returns>Returns true if it succeeds, false if it's an illegal move</returns>
        public static bool Assign(State Check, ushort Row, ushort Column){
            if(Check==State.Empty||Check==State.Full){
                return false;
            }
            if (Pattern[Row, Column] != Board.State.Empty)
                return false;
            Pattern[Row, Column] = Check;
            return true;
        }
        /// <summary>
        /// Gets the boardstate, which is player agnostic. Use this to mask which options are unavailable.
        /// </summary>
        /// <returns>Returns the board state</returns>
        public static State[,] GetBoard(){
            return Pattern;
        }
        public static void SetBoard(State[,] q){
            Pattern = q;
        }
        
    }
}
