using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialize game board
            GameBoard board = new GameBoard();
            
            //variables for determing who is First
            Dictionary<String,Boolean> isfirstDict = new Dictionary<string,bool>();
            isfirstDict.Add("P",true);
            isfirstDict.Add("p",true);
            isfirstDict.Add("C",false);
            isfirstDict.Add("c",false);
            string whoIsFirst,playerPiece;
            bool isfirst=false;

            //variables used to get the move from the player
            String mymove;
            int myMoveInt;

            //loop till correct input is given
            bool valid = false;
            do
            {
                Console.Write("Who will go first?\nEnter 'C' for Computer and 'P' for Player: ");
                whoIsFirst = Console.ReadLine();
                if(whoIsFirst.Length==1 && (whoIsFirst.ToUpper().Equals("P")||whoIsFirst.ToUpper().Equals("C")))
                {
                    valid = true;
                    isfirst = isfirstDict[whoIsFirst];
                }
                else
                {
                    Console.WriteLine("Please Enter either 'C' or 'P'.  ");  
                }
            } while (!valid);

            Computer computer = new Computer();

            if(isfirst)
            {
                playerPiece = "X";
                computer.compPiece="O";
            }
            else
            {
                playerPiece = "O";
                computer.compPiece="X";
                board.setMove(computer.compMove(board.gridArray), computer.compPiece);
            }

            //loop until game is over
            while (board.gameWon==-1)
            {
                myMoveInt=0;
                board.drawBoard();
                do{
                    Console.Write("Enter the number of \nyour next move: ");
                    mymove = Console.ReadLine();
                    if(mymove.Length==1)
                    {
                        try
                        {
                            myMoveInt = Convert.ToInt32(mymove);
                        }
                        catch (Exception e)
                        {
                            
                            throw;
                        }
                    }
                }while(myMoveInt==0);
                board.setMove(myMoveInt-1, playerPiece);
                board.isWon();
                
                if (board.gameWon==-1)
                {
                    board.setMove(computer.compMove(board.gridArray), computer.compPiece);
                }
                else
                {
                    if(board.gameWon ==1)
                    {
                        Console.WriteLine("You Won!!");
                    }
                    else
                    {
                        Console.WriteLine("The Game is a Draw");
                    }
                    Console.ReadLine();
                    break;
                }
                
                board.isWon();
                if (board.gameWon!=-1)
                {

                    if (board.gameWon == 1)
                    {
                        Console.WriteLine("Game Over, You Lose!");
                    }
                    else
                    {
                        Console.WriteLine("The Game is a Draw");
                    }
                    
                    Console.ReadLine();
                    break;
             }
            }
           
        }
    }

    public class GameBoard
    {
        public List<string> gridArray = new List<string>();
        
        //verifies if game is won -1 is set by constructor
        //1 if if the game is won and 0 is if tied
        public int gameWon { get; private set; }

        public void isWon()
        {
            StringBuilder check = new StringBuilder();
                        
            //add rows to checker
            for(int i=0;i<9;i+=3)
            {
                check.Append(gridArray[i]);
                check.Append(gridArray[i+1]);
                check.Append(gridArray[i+2]);
                check.Append(", ");
            }

            //check columns
            for (int i = 0; i < 3; i++)
            {
                check.Append(gridArray[i]);
                check.Append(gridArray[i + 3]);
                check.Append(gridArray[i + 6]);
                check.Append(", ");
            }

            //add diagnols to checker
                check.Append(gridArray[0]);
                check.Append(gridArray[4]);
                check.Append(gridArray[8]);
                check.Append(", ");
                check.Append(gridArray[2]);
                check.Append(gridArray[4]);
                check.Append(gridArray[6]);
            

            //check for XXX or OOO in check string
            if(check.ToString().Contains("XXX")||check.ToString().Contains("OOO"))
            {
                gameWon = 1;
            }
            else
            {
                int usedSpaces=0;
                //count number of used spaces
                foreach(String n in gridArray)
                {
                    if(n.Equals("X")||n.Equals("O"))
                    {
                        usedSpaces++;
                    }
                }
                if(usedSpaces==9 && gameWon==-1)
                {
                    gameWon = 0;
                }

            }
        }

        public void setMove(int index, String move)
        {
            gridArray[index] = move;
            drawBoard();
        }
        public GameBoard()
        {
            gameWon = -1;
            for(int i = 0;i<9;i++)
            {
                gridArray.Add((i+1).ToString());
            }
        }

        public void drawBoard()
        {
            Console.Clear();
            Console.WriteLine(
                "{0}|{1}|{2}\n-----\n{3}|{4}|{5}\n-----\n{6}|{7}|{8}\n",
                gridArray.ElementAt(0), 
                gridArray.ElementAt(1),
                gridArray.ElementAt(2),
                gridArray.ElementAt(3),
                gridArray.ElementAt(4),
                gridArray.ElementAt(5),
                gridArray.ElementAt(6),
                gridArray.ElementAt(7),
                gridArray.ElementAt(8));
        }
    }
    
    public class Computer
    {
        public String compPiece { get; set; }

        public int compMove(List<String> board)
        {
            List<String> copy = new List<String>();
            int myMove, bestNumber=0;
            Random random = new Random();
            List<int> bestMoves = new List<int>();
            
            //copy board to copy 
            for (int i = 0, n = board.Count(); i < n;i++)
            {
                copy.Add(board[i]);
            }

                //loop through each element of the Board and evaluate
                for (int i = 0, n = copy.Count(); i < n; i++)
                {
                    //if not X or O
                    if (!(copy[i].Equals("X") || copy[i].Equals("O")))
                    {
                        copy[i] = compPiece;
                        copy[i] = evaluateMove(copy).ToString();
                    }
                }

            
            //find best answer(s)
            for (int i = 0, n = copy.Count(); i < n; i++)
            {
                //if not X or O
                if (!(copy[i].Equals("X") || copy[i].Equals("O")))
                {
                    int numberToCompare = Convert.ToInt32(copy[i]);
                    if (bestNumber == 0)
                    {
                        bestNumber = numberToCompare;
                    }
                    else
                    {
                        if(compPiece.Equals("X"))
                        {
                            if (bestNumber < numberToCompare) 
                            {
                                bestNumber = numberToCompare;
                            }
                        }
                        else
                        {
                            if (bestNumber > numberToCompare)
                            {
                                bestNumber = numberToCompare;
                            }
                        }
                    }
                }
            }

            for (int i = 0, n = copy.Count(); i < n; i++)
            {
                
                //if not X or O
                if (!(copy[i].Equals("X") || copy[i].Equals("O")))
                {
                    int numberToCompare = Convert.ToInt32(copy[i]);
                    if(numberToCompare == bestNumber)
                    {
                        bestMoves.Add(i);
                    }
                }
            }
            myMove = bestMoves[random.Next(0, bestMoves.Count())];
            return myMove;
        }
        
        private int evaluateMove(List<String> board)
        {
            
            //x=#Rows r, Columns c, Diagnols d that have 2 Xs and 0 Os
            //o=#Rows r, Columns c, Diagnols d that have 1 Xs and 0 Os
            //y=#Rows r, Columns c, Diagnols d that have 0 Xs and 2 Os
            //z=#Rows r, Columns c, Diagnols d that have 0 Xs and 1 Os
            int a=0,b=0,x=0, o=0, y=0, z=0;
            Dictionary<String, int> accumulator= new Dictionary<String,int>();
            accumulator.Add("XX", 0);
            accumulator.Add("X", 0);
            accumulator.Add("OO", 0);
            accumulator.Add("O", 0);
            accumulator.Add("XO", 0);
            accumulator.Add("OX", 0);
            accumulator.Add("XXO", 0);
            accumulator.Add("XOX", 0);
            accumulator.Add("OXX", 0);
            accumulator.Add("OXO", 0);
            accumulator.Add("OOX", 0);
            accumulator.Add("XOO", 0);
            accumulator.Add("OOO", 0);
            accumulator.Add("XXX", 0);
            accumulator.Add("", 0);
            StringBuilder check = new StringBuilder();

            //add rows to checker
            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i + j].Equals("X") || board[i + j].Equals("O"))
                    {
                        check.Append(board[i + j]);
                    }   
                }

                accumulator[check.ToString()] = accumulator[check.ToString()]+1;
                check.Clear();
            }

            //check columns
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j+=3)
                {
                    if (board[i + j].Equals("X") || board[i + j].Equals("O"))
                    {
                        check.Append(board[i + j]);
                    }
                }
                accumulator[check.ToString()] = accumulator[check.ToString()] + 1;
                check.Clear();
            }

            //add diagnols to checker
            if(board[0].Equals("X")||board[0].Equals("O")) check.Append(board[0]);
            if (board[4].Equals("X") || board[4].Equals("O")) check.Append(board[4]);
            if (board[8].Equals("X") || board[8].Equals("O")) check.Append(board[8]);

            accumulator[check.ToString()] = accumulator[check.ToString()] + 1;
            check.Clear();

            if (board[2].Equals("X") || board[2].Equals("O")) check.Append(board[2]);
            if (board[4].Equals("X") || board[4].Equals("O")) check.Append(board[4]);
            if (board[6].Equals("X") || board[6].Equals("O")) check.Append(board[6]);

            accumulator[check.ToString()] = accumulator[check.ToString()] + 1;
            check.Clear();

            a = accumulator["XXX"];
            x = accumulator["XX"];
            o = accumulator["X"];
            b = accumulator["OOO"];
            y = accumulator["OO"];
            z = accumulator["O"];
            return (6*a)+(3*x)+o-((6*b)+(3*y)+z);
        }
    }
}

