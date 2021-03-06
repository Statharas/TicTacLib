﻿using System;
using System.Collections.Generic;

namespace TicTac
{
    public static partial class ΑΙ
    {
        public class Node
        {
            private readonly List<Node> _parents = new();
            private readonly List<Node> _children = new();
            public List<Node> Children { get { return _children; } }
            private static readonly List<Node>[] _neighbor = {
                new(), new(), new(), new(), new(),
                new(), new(), new(), new(), new()
            };
            public static List<Node>[] Neighbourhood { get { return _neighbor; } }
            private int _level;

            private Board.State _win = TicTac.Board.State.Empty;
            private Board.State[,] _board { get; set; }
            public Board.State[,] Board { get { return _board; } }
            private int _id;
            private int _value;
            public int Value { get { return _value; } }

            public Board.State Win { get => _win; }

            private static int _cId = 1;
            private static Node _root;

            public Node(Board.State[,] state)
            {
                _board = state;

                _id = _cId;
                _cId++;
                //Console.WriteLine(ID);
            }

            public Node()
            {
                this._board = new Board.State[3, 3]
                {
                    {TicTac.Board.State.Empty, TicTac.Board.State.Empty, TicTac.Board.State.Empty},
                    {TicTac.Board.State.Empty, TicTac.Board.State.Empty, TicTac.Board.State.Empty},
                    {TicTac.Board.State.Empty, TicTac.Board.State.Empty, TicTac.Board.State.Empty}
                };
                
            }

            void GenerateChildren()
            {
                if (_win == TicTac.Board.State.Empty)
                {
                    int index = 0;
                    foreach (Board.State a in _board)
                    {
                        if (a == TicTac.Board.State.Empty)
                        {
                            Board.State[,] n = new Board.State[3, 3];
                            n = _board.Clone() as Board.State[,];
                            n[index / 3, index % 3] = GetPlayer(_level + 1);
                            Node q = new Node(n)
                            {
                                _level = _level + 1
                            };
                            bool exists = false;
                            foreach (Node f in _neighbor[q._level])
                            {
                                if (f.BoardsEqual(q.Board))
                                {
                                    if (!f._parents.Contains(this))
                                        f._parents.Add(this);
                                    if (!_children.Contains(f))
                                        _children.Add(f);
                                    exists = true;
                                    break;
                                }
                            }

                            if (!exists)
                            {
                                _children.Add(q);
                                q._parents.Add(this);
                                _neighbor[q._level].Add(q);
                                q._win = CheckForWins(q._board);
                                if (q._win == TicTac.Board.State.P1)
                                    q.UpValue(1);
                                if (q._win == TicTac.Board.State.P2)
                                    q.UpValue(-1);
                                if (q._win != TicTac.Board.State.Full)
                                    q.GenerateChildren();
                            }
                            else
                            {
                                q = null;
                            }
                        }

                        index++;
                        //Console.WriteLine(index);
                    }
                }
            }

            private void UpValue(int a)
            {
                _value += a;
                foreach (Node q in _parents)
                {
                    q.UpValue(a);
                }
            }

            public void Rotate(int a = 1)
            {
                if (a <= 0)
                    return;

                Board.State[,] rotated = new Board.State[3, 3];
                /*Rotations go counter-clockwise
                 * 1 2 3
                 * 4 5 6
                 * 7 8 9
                 * to
                 * 3 6 9
                 * 2 5 8
                 * 1 4 7
                 * */
                rotated[0, 0] = _board[0, 2];
                rotated[0, 1] = _board[1, 2];
                rotated[0, 2] = _board[2, 2];
                rotated[1, 0] = _board[0, 1];
                rotated[1, 1] = _board[1, 1];
                rotated[1, 2] = _board[2, 1];
                rotated[2, 0] = _board[0, 0];
                rotated[2, 1] = _board[1, 0];
                rotated[2, 2] = _board[2, 0];
                _board = rotated;

                Rotate(--a);
            }

            public void Mirror(int a)
            {
                /*Rotations go counter-clockwise
                 *Mirrored view. 0 to 3 flip it. 
                 * */
                if (a < 0)
                    return;
                if (a == 0 || a == 2)
                {
                    MirrorX();
                }

                if (a == 1 || a == 3)
                {
                    MirrorY();
                }

                if (a > 3)
                    throw new InvalidOperationException("Cannot Mirror on 4");
            }
            private void MirrorX()
            {
                Board.State[,] mirrored = new Board.State[3, 3];
                mirrored[0, 0] = _board[2, 0];
                mirrored[0, 1] = _board[2, 1];
                mirrored[0, 2] = _board[2, 2];
                mirrored[2, 0] = _board[0, 0];
                mirrored[2, 1] = _board[0, 1];
                mirrored[2, 2] = _board[0, 2];

                mirrored[1, 0] = _board[1, 0];
                mirrored[1, 1] = _board[1, 1];
                mirrored[1, 2] = _board[1, 2];
                _board = mirrored;
            }
            private void MirrorY()
            {
                Board.State[,] mirrored = new Board.State[3, 3];
                mirrored[0, 0] = _board[0, 2];
                mirrored[0, 2] = _board[0, 0];
                mirrored[1, 0] = _board[1, 2];
                mirrored[1, 2] = _board[1, 0];
                mirrored[2, 0] = _board[2, 2];
                mirrored[2, 2] = _board[2, 0];

                mirrored[0, 1] = _board[0, 1];
                mirrored[1, 1] = _board[1, 1];
                mirrored[2, 1] = _board[2, 1];
                _board = mirrored;
            }
            public void Unmirror(int a)
            {
                a--;
                if (a == 0)
                    MirrorX();
                if (a == 2)
                    MirrorY();
                if (a == 1)
                {
                    MirrorX();
                    MirrorY();
                }
            }
            public bool BoardsEqual(Board.State[,] b)
            {
                return BoardsEqual(b, out _, out _);
            }
            /// <summary>
            /// Checks if this board is equal to another, including possible mirrors and rotations
            /// </summary>
            /// <param name="b">The target board to check</param>
            /// <param name="Rots">The rotations needed to get the target board.</param>
            /// <param name="Mirr">The mirrors needed to get the target board.</param>
            /// <returns>True if the boards are equal.</returns>
            public bool BoardsEqual(Board.State[,] b, out int Rots, out int Mirr)
            {
                Rots = 0;
                Mirr = 0;
                if (ReferenceEquals(this.Board, b))
                    return true;
                if (this is null || b is null)
                    return false;


                Board.State[,] d = b.Clone() as Board.State[,];
                Node c = new Node(d);

                bool CompareNodes()
                {
                    for (int q = 0; q < 3; q++)
                    {
                        for (int w = 0; w < 3; w++)
                        {
                            if (this._board[q, w] == c._board[q, w]) continue;
                            return false;
                        }
                    }

                    return true;
                }

                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {

                        if (CompareNodes())
                        {

                            if (x != 0)
                                Rots = 4 - x;//If the rotation is at 1, then we have to do 3 more rotations to get back to original
                            else
                                Rots = 0;
                            Mirr = y; //rotations cancel eachother out. If y=0, X mirror, y=1, y mirror, y=2 x mirror to get Y, y=3 y mirror to get original

                            return true;
                        }
                        c.Mirror(y);
                    }
                    c.Rotate();
                }
                Rots = Mirr = 0;
                return false;
            }


            public static void GenerateTree()
            {
                _root = new();
                _neighbor[_root._level].Add(_root);
                _root.GenerateChildren();
                //The following is a benchmark line for use with breakpoints
                ;
            }
        }
        private static void FlipPlayer(Board.State player)
        {
            if (player == Board.State.P1)
                player = Board.State.P2;
            else if (player == Board.State.P2)
                player = Board.State.P1;
        }

        public static Board.State GetPlayer(int a) => a % 2 != 0 ? Board.State.P1 : Board.State.P2;
    }
}