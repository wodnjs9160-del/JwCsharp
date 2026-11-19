// 플레이어 hp 100, 몬스터 hp 80
// 플레이어 speed 10, 몬스터 speed 10
// 플레이어 공격력 10, 몬스터 공격력 8
// 매 턴마다 플레이어, 몬스터가 서로 공격한다.
// 스피드가 높은 엔티티가 먼저 공격을 한다.단 스피드가 같을시 랜덤으로 먼저 공격한다
// 공격을 받았을 때 스피드가 낮아질 확률 50%
// 공격을 받았을 때 회피할 확률 30%
// 둘 중 하나라도 hp가 0이 될 경우 게임이 끝난다.
// 플레이어가 hp가 0 됐을 경우 패배
// 몬스터 hp가 0이 됐을 경우 승리 문구를 띄운다
// 매 턴마다 누가 때렸고, 회피를 했는지, 플레이어, 몬스터의 hp가 실시간으로 계속 띄워줘
// 반드시 내가 주석으로 지정한 표현 방식만 쓰고 너가 추가적으로 기능을 구현하지는 말아라
// 만약에 그 상태에서 실행이 안 된다면 안 되는 이유를 나에게 말해라.

using System;

class Program
{
    static void Main()
    {
        // 플레이어 및 몬스터 초기 스탯
        int 플레이어_hp = 100;
        int 몬스터_hp = 80;

        int 플레이어_speed = 10;
        int 몬스터_speed = 10;

        int 플레이어_공격력 = 10;
        int 몬스터_공격력 = 8;

        Random rand = new Random();

        // 둘 중 하나라도 hp가 0 이하가 될 때까지 반복
        while (플레이어_hp > 0 && 몬스터_hp > 0)
        {
            bool 플레이어_선공 = false;

            // 스피드가 높은 엔티티가 먼저 공격, 같을 시 랜덤 선공
            if (플레이어_speed > 몬스터_speed)
            {
                플레이어_선공 = true;
            }
            else if (플레이어_speed < 몬스터_speed)
            {
                플레이어_선공 = false;
            }
            else
            {
                플레이어_선공 = (rand.Next(0, 2) == 0);
            }

            if (플레이어_선공)
            {
                // 1. 플레이어 공격
                if (rand.Next(0, 100) < 30) // 회피 확률 30%
                {
                    Console.WriteLine("플레이어 공격 -> 몬스터 회피!");
                }
                else
                {
                    몬스터_hp -= 플레이어_공격력;
                    Console.WriteLine("플레이어 공격 -> 몬스터 피격!");

                    if (rand.Next(0, 100) < 50) // 스피드 낮아질 확률 50%
                    {
                        몬스터_speed -= 1;
                    }
                }
                Console.WriteLine("플레이어 HP: " + 플레이어_hp + " / 몬스터 HP: " + 몬스터_hp);

                if (몬스터_hp <= 0)
                {
                    break;
                }

                // 2. 몬스터 반격
                if (rand.Next(0, 100) < 30) // 회피 확률 30%
                {
                    Console.WriteLine("몬스터 공격 -> 플레이어 회피!");
                }
                else
                {
                    플레이어_hp -= 몬스터_공격력;
                    Console.WriteLine("몬스터 공격 -> 플레이어 피격!");

                    if (rand.Next(0, 100) < 50) // 스피드 낮아질 확률 50%
                    {
                        플레이어_speed -= 1;
                    }
                }
                Console.WriteLine("플레이어 HP: " + 플레이어_hp + " / 몬스터 HP: " + 몬스터_hp);
            }
            else
            {
                // 1. 몬스터 선공
                if (rand.Next(0, 100) < 30) // 회피 확률 30%
                {
                    Console.WriteLine("몬스터 공격 -> 플레이어 회피!");
                }
                else
                {
                    플레이어_hp -= 몬스터_공격력;
                    Console.WriteLine("몬스터 공격 -> 플레이어 피격!");

                    if (rand.Next(0, 100) < 50) // 스피드 낮아질 확률 50%
                    {
                        플레이어_speed -= 1;
                    }
                }
                Console.WriteLine("플레이어 HP: " + 플레이어_hp + " / 몬스터 HP: " + 몬스터_hp);

                if (플레이어_hp <= 0)
                {
                    break;
                }

                // 2. 플레이어 반격
                if (rand.Next(0, 100) < 30) // 회피 확률 30%
                {
                    Console.WriteLine("플레이어 공격 -> 몬스터 회피!");
                }
                else
                {
                    몬스터_hp -= 플레이어_공격력;
                    Console.WriteLine("플레이어 공격 -> 몬스터 피격!");

                    if (rand.Next(0, 100) < 50) // 스피드 낮아질 확률 50%
                    {
                        몬스터_speed -= 1;
                    }
                }
                Console.WriteLine("플레이어 HP: " + 플레이어_hp + " / 몬스터 HP: " + 몬스터_hp);
            }
        }

        // 종료 문구 출력
        if (플레이어_hp <= 0)
        {
            Console.WriteLine("패배");
        }
        else if (몬스터_hp <= 0)
        {
            Console.WriteLine("승리");
        }
    }
}
