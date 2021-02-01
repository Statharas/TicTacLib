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
        public static Board.State[,] Play(Board.State[,] Last)
        {
            Board.State[,] ReturnState;
            List<Node> z = Node.Neighbourhood[GetLevel(Last)];
            Node q = z.Find(o => o.BoardsEqual(Last));
            q= q.Children.OrderBy(o => o.Value).Last() ;
            Console.WriteLine("Picked Value of " + q.Value);
            ReturnState = q.Board;
            return ReturnState;
        }

        public static int GetLevel(Board.State[,] a){
            int count = 9;
            foreach(Board.State z in a){
                if (z == Board.State.Empty)
                    count--;
            }
            return count;

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
