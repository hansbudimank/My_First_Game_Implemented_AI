using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static int[] Player = { 100, 10, 10, 0 ,1}; // healt, atk, heal, exp, level
        static int[] temp_Player = new int[3]; // healt, atk, heal
        static int[] Enemy = { 100, 7, 50 };
        static int[] temp_Enemy = new int[3];
        static bool Encountered_Enemy = false;
        static bool Player_Died = false;
        static int Level_Exp_Cap = 100;
        static string Player_Move = "";
        static string L1 = "";
        static string L2 = "";
        static bool train = false;
        public static void Main()
        {
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
                    else if (Player_Move == "2"){
                        Player_Died = true;
                        Console.WriteLine("[Player just gave up on life.]");
                    }
                }
                else{
                    if (Player_Move == "1")
                    {
                        temp_Enemy[0] -= temp_Player[1];
                        Console.WriteLine($"\r\n[Player attacked for {temp_Player[1]} DMG.]");
                        if (temp_Enemy[0] < 1)
                        {
                            Encountered_Enemy = false;
                            Player[3] += Enemy[2];
                            Console.WriteLine($"\r\n[Player has slained an enemy. Player earned {Enemy[2]} Exp.]\r\n");
                            while (Player[3]>= Level_Exp_Cap)
                            {
                                Level_Exp_Cap+=100;
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
                        if ((temp_Player[0]+ temp_Player[2])>=Player[0])
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
            Console.ReadLine();
        }

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
        public static void Player_Transision()
        {
            for (int i = 0; i < 3; i++)
            {
                temp_Player[i] = Player[i];
            }
        }
        public static void Enemy_Transision()
        {
            for (int i = 0; i < 3; i++)
            {
                temp_Enemy[i] = Enemy[i];
            }
        }
        public static void Player_Stat()
        {
            L1 = "";L2 = "";
            L1 += $"Level:{Player[4]}\t\t  Exp:{Player[3]}/{Level_Exp_Cap}";
            L2 += $"HP:{temp_Player[0]}/{Player[0]}  Atk:{Player[1]}  Heal:{Player[2]}";
        }
        public static void Enemy_Stat()
        {
            if (Encountered_Enemy)
            {
                L1 += $"||  <Enemy>";
                L2 += $"||  HP:{temp_Enemy[0]}/{Enemy[0]}\tAtk:{Enemy[1]}";
            }
        }
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
    }
}
