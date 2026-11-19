using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=====================");
        Console.WriteLine("  동전 던지기 게임");
        Console.WriteLine("=====================");
        Console.WriteLine();

        int 나_점수 = 0;
        int 연속_정답 = 0;

        Random rand = new Random();

        while (true)
        {
            Console.Write("앞 또는 뒤를 입력하세요: ");
            string 예측 = Console.ReadLine();

            // "앞" 또는 "뒤"가 아닌 값을 입력했을 때 라운드 실행 안 함
            if (예측 != "앞" && 예측 != "뒤")
            {
                Console.WriteLine("잘못된 입력입니다. '앞' 또는 '뒤'만 입력하세요.");
                Console.WriteLine();
                continue;
            }

            int 랜덤 = rand.Next(0, 2);
            string 동전 = "";

            if (랜덤 == 0)
            {
                동전 = "앞";
            }
            else
            {
                동전 = "뒤";
            }

            if (예측 == 동전)
            {
                연속_정답 = 연속_정답 + 1;

                if (연속_정답 >= 2)
                {
                    나_점수 = 나_점수 + 2;
                }
                else
                {
                    나_점수 = 나_점수 + 1;
                }
            }
            else
            {
                연속_정답 = 0;
            }

            Console.WriteLine();
            Console.WriteLine("동전 결과 : " + 동전);
            Console.WriteLine("연속 정답 : " + 연속_정답);
            Console.WriteLine("나 점수   : " + 나_점수);
            Console.WriteLine("---------------------");
            Console.WriteLine();
        }
    }
}
