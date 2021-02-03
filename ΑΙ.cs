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
            List<Node> FoundNode = Node.Neighbourhood[GetLevel(Last)];
            List<Node> Plays = new List<Node>();
            int rot = 0;
            int mir = 0;
            foreach (Node Play in FoundNode){
                if (Play.BoardsEqual(Last, out rot,out mir)){
                    Plays = new List<Node>(Play.Children);
                    break;
                }
            }
            Node Selected = new Node();
            FoundNode = new List<Node>(Plays);


            DiscardLosing();
            GetWin();
            if (Selected.Win != Board.State.P2)
                Selected = FindTrap();

            if (!Plays.Contains(Selected))
            {
                if (Plays.Count != 0)
                {
                    Selected = Plays.OrderBy(o => o.Value).Last();
                    Console.WriteLine("Picking Last, " + Selected.Value);
                }
                else
                    Selected = FoundNode.Last();
            }

            //Restore to our original orientation and mirror
            Selected.Rotate(rot);
            Selected.Unmirror(mir);

            ReturnState = Selected.Board;
            return ReturnState;
            //--------------------------------
            //AI actions

            //Finds if one of the possible plays leads to a perfect win. Play is the AI play, SubPlay is the Player Play. If a play has all player plays lead to AI win, we can return that.
            Node FindTrap()
            {
                //we should expand this to find more traps hidden in our tree
                foreach (Node Play in Plays)
                {

                    foreach (Node SubPlay in Play.Children)
                    {
                        bool Trap = true;
                        foreach (Node OurPlay in SubPlay.Children)
                            if (SubPlay.Win != Board.State.P2)
                            {
                                Trap = false;
                                break;
                            }

                        if (Trap)
                            return Play;
                    }
                }
                return new Node();
            }

            void DiscardLosing()
            {
                List<Node> Remove = new List<Node>();
                foreach (Node a in Plays)
                {
                    foreach(Node q in a.Children){ 
                    if (q.Win == Board.State.P1)
                        Remove.Add(a);
                    }
                }
                Plays = Plays.Except(Remove).ToList();
            }
            void GetWin()
            {
                Node q = Plays.Find(o => o.Win == Board.State.P2);
                if (q != null)
                    Selected = q;
            }

        }
        public static int GetLevel(Board.State[,] a)
        {
            int count = 9;
            foreach (Board.State z in a)
            {
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
