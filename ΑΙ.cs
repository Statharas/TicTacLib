using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TicTac.Board;

namespace TicTac
{
    public static partial class ΑΙ
    {
        public static ushort[] Play()
        {

            return new ushort[2] { 1, 1 };
        }
        public static State CheckForWins(Board.State[,] Pattern)
        {
            State z = State.P1;
            for (int x = 0; x < 2; x++)
            {
                if (x == 1)
                    z = State.P2;
                if (//Horizontal check
                       (Pattern[0, 0] == z && Pattern[0, 1] == z && Pattern[0, 2] == z)
                    || (Pattern[1, 0] == z && Pattern[1, 1] == z && Pattern[1, 2] == z)
                    || (Pattern[2, 0] == z && Pattern[2, 1] == z && Pattern[2, 2] == z)
                    //Vertical Check
                    || (Pattern[0, 0] == z && Pattern[1, 0] == z && Pattern[2, 0] == z)
                    || (Pattern[0, 1] == z && Pattern[1, 1] == z && Pattern[2, 1] == z)
                    || (Pattern[0, 2] == z && Pattern[1, 2] == z && Pattern[2, 2] == z)
                    //Cross Check
                    || (Pattern[0, 0] == z && Pattern[1, 1] == z && Pattern[2, 2] == z)
                    || (Pattern[2, 0] == z && Pattern[1, 1] == z && Pattern[0, 2] == z)
                    )
                    return z;
            }
            //if there's any blank place, return empty (as in, there's at least 1 empty slot
            foreach (State q in Pattern)
            {
                if (q == State.Empty)
                    return State.Empty;
            }
            //If we reach this point, it means that the board doesn't have any slots left. We should tell this to the front end.
            return State.Full;
        }
    }
}
