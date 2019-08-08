using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        #region Game_Variable
        static double[] Player = { 100, 10, 10, 0 ,1}; // healt, atk, heal, exp, level
        static double[] temp_Player = new double[3]; // healt, atk, heal
        static double[] Enemy = { 100, 7, 5 };
        static double[] temp_Enemy = new double[3];
        static bool Encountered_Enemy = false;
        static bool Player_Died = false;
        static int Level_Exp_Cap = 100;
        static string Player_Move = "";
        static string L1 = "";
        static string L2 = "";
        #endregion
        #region NN_Variable
        static bool train = false;
        static bool Set_Weight = false;
        static double[] Input=new double[8];
        static int[] Network = { Input.Length, 1 };
        static double[,] W1 = new double[Network[0], Network[1]];
        static double[,] U1 = new double[Network[0], Network[1]];
        static double[,] V1 = new double[Network[0], Network[1]];
        static string Output = "";
        #endregion
        public static void Main()
        {
            #region Game_Logic
            string Game_Title = "<---Welcome to Ricky's Adventure!--->";
            Console.WriteLine(Game_Title);
            Player_Transision();
            while (!Player_Died)
            {
                Player_Stat();
                Enemy_Stat();
                Console.WriteLine($"{L1}\n{L2}");
                SearchMenu(Encountered_Enemy);
                if (train)
                {
                    NN();
                    Player_Move = Output;
                }
                else
                {
                    Player_Move = Console.ReadLine();
                }
                if (!Encountered_Enemy)
                {
                    if (Player_Move == "1")
                    {
                        Encountered_Enemy = true;
                        Console.WriteLine("\r\n---Player Just Encountered An Enemy.---\r\n");
                        Enemy_Transision();
                    }
                    else if (Player_Move == "2")
                    {
                        Player_Died = true;
                        Console.WriteLine("[Player just gave up on life.]");
                    }
                }
                else
                {
                    if (Player_Move == "1")
                    {
                        temp_Enemy[0] -= temp_Player[1];
                        Console.WriteLine($"\r\n[Player attacked for {temp_Player[1]} DMG.]");
                        if (temp_Enemy[0] < 1)
                        {
                            Encountered_Enemy = false;
                            Player[3] += Enemy[2];
                            Console.WriteLine($"\r\n[Player has slained an enemy. Player earned {Enemy[2]} Exp.]\r\n");
                            while (Player[3] >= Level_Exp_Cap)
                            {
                                Level_Exp_Cap += 100;
                                Player[4]++;
                                Player[0] += 20;
                                Player[1] += 5;
                                Player[2] += 5;
                                Console.WriteLine("***Congratulations! Player Has Leveled Up!***");
                                for (int i = 0; i < temp_Player.Length; i++)
                                {
                                    temp_Player[i] = Player[i];
                                }
                                Enemy[0] += 50;
                                Enemy[1] += 2;
                                Enemy[2] += 5;
                            }
                        }
                        else
                        {
                            EnemyTurn();
                        }
                    }
                    else if (Player_Move == "2")
                    {
                        if ((temp_Player[0] + temp_Player[2]) >= Player[0])
                        {
                            temp_Player[0] = Player[0];
                        }
                        else
                        {
                            temp_Player[0] += temp_Player[2];
                        }
                        Console.WriteLine($"\r\n[Player cast a healing spell. Heal for {temp_Player[2]} HP.]");
                        EnemyTurn();
                    }
                }
            }
            #endregion
            #region (After_Player_Died)Count_Fitness
            fitness = Player[3];
            #endregion
            Console.ReadLine();
        }

        #region SearchMenu_Option
        public static void SearchMenu(bool Encountered_Enemy)
        {
            if (!Encountered_Enemy)
            {
                Console.WriteLine("Select an option:\r\n1)Search\r\n2)Suicide");
            }
            else
            {
                Console.WriteLine("Select an option:\r\n1)Attack\r\n2)Heal");
            }
        }

        #endregion
        #region Player_Transision
        public static void Player_Transision()
        {
            for (int i = 0; i < 3; i++)
            {
                temp_Player[i] = Player[i];
            }
        }

        #endregion
        #region Enemy_Transision
        public static void Enemy_Transision()
        {
            for (int i = 0; i < 3; i++)
            {
                temp_Enemy[i] = Enemy[i];
            }
        }

        #endregion
        #region Player_Stat
        public static void Player_Stat()
        {
            L1 = "";L2 = "";
            L1 += $"Level:{Player[4]}\t\t  Exp:{Player[3]}/{Level_Exp_Cap}";
            L2 += $"HP:{temp_Player[0]}/{Player[0]}  Atk:{Player[1]}  Heal:{Player[2]}";
        }

        #endregion
        #region Enemy_Stat
        public static void Enemy_Stat()
        {
            if (Encountered_Enemy)
            {
                L1 += $"||  <Enemy>";
                L2 += $"||  HP:{temp_Enemy[0]}/{Enemy[0]}\tAtk:{Enemy[1]}";
            }
        }

        #endregion
        #region EnemyTurn
        public static void EnemyTurn()
        {
            temp_Player[0] -= temp_Enemy[1];
            Console.WriteLine($"[Enemy attacked for {temp_Enemy[1]} DMG.]\r\n");
            if (temp_Player[0]<1)
            {
                Player_Died = true;
                Console.WriteLine("[Player DIED. You Lost.]");
            }
        }

        #endregion
        #region Neural_Network
        public static void NN()
        {
            double Encounter = 0.0;
            string P = @"C:\Users\Hans Bk\source\repos\ConsoleApp2";
            if (Encountered_Enemy)
            {
                Encounter = 1.0;
            }
            double[] temp_storage = { temp_Player[0], Player[0], Player[1], Player[2], temp_Enemy[0], Enemy[0], Enemy[1], Encounter };
            #region Normalization
            double Largest_Input= Input[0];
            for (int i = 0; i < Input.Length; i++)
            {
                Input[i] = temp_storage[i];
                if (Largest_Input<Input[i])
                {
                    Largest_Input = Input[i];
                }
            }
            for (int i = 0; i < Input.Length-1; i++)
            {
                Input[i] = Input[i]/Largest_Input;

            }
            #endregion
            #region Set_Weight
            if (!Set_Weight)
            {
                Set_Weight = true;

                int RandomSeed1 = 1;
                Random w1 = new Random(RandomSeed1);
                StreamWriter swX1 = new StreamWriter(Path.Combine(P, "wph11-1.txt"));//寫資料放的位置
                for (int j = 0; j < Network[1]; j++)
                {
                    for (int i = 0; i < Network[0] + 1/*Bias*/; i++)
                    {
                        W1[i, j] = w1.NextDouble();
                        U1[i, j] = W1[i, j];
                        V1[i, j] = 0.0;
                        swX1.WriteLine(W1[i, j] + " " + U1[i, j] + " " + V1[i, j]);
                    }
                }
                swX1.Flush();
                swX1.Close();
            }
            #endregion
            #region Reinitialize_Weights
            StreamReader srW1 = new StreamReader(Path.Combine(P, "wph11-1.txt"));//寫資料放的位置
            string[] SW1 = srW1.ReadToEnd().Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int A = 0;
            for (int j = 0; j < Network[1]; j++)
            {
                for (int i = 0; i < Network[0] + 1; i++)
                {
                    W1[i, j] = Convert.ToDouble(SW1[A++]);
                    U1[i, j] = Convert.ToDouble(SW1[A++]);
                    V1[i, j] = Convert.ToDouble(SW1[A++]);
                }
            }
            srW1.Close();
            #endregion
            if (blbbla > something) Output = "1";
            else Output = "2";
        }
        #endregion
    }
}
