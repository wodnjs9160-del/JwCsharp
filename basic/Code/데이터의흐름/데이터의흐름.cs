using System;
using System.Collections.Generic;
using System.Text;

// 반드시 내가 주석으로 지정한 표현 방식만 쓰고 너가 추가적으로 기능을 구현하지는 말아라
// 만약에 그 상태에서 실행이 안 된다면 안 되는 이유를 나에게 말해라.
// 가위 바위 보를 조건문으로 만들때 조건을 내가 만든다.

// 나 : 가위, 남 : 바위 조건으로 표현해서 이긴 사람은 점수를 1점 얻는다.
//              상대(가위, 바위, 보)
// 3x3 코드를 만들어서.
// 내가 가위 입력을 하면 어떤 결과가 나오는지 점수판에 나오게 구연한다.

namespace basic
{
    internal class 데이터의흐름
    {
        static void Main()
        {
            // if, else if, if
            bool GameFinish = true;
            int Score = 65;  // 0~100

            if (GameFinish)
            {
                // 60점 이상 80점 미만  B를 출력하라
                if (60 <= Score && 80 > Score)
                {
                    Console.WriteLine("B");
                }
                // 80점 이상 100점 미만 A를 출력하라
                else if (80 <= Score && 100 > Score)
                {
                    Console.WriteLine("A");
                }
                else
                {
                    Console.WriteLine("미정의");
                }

            }

            string name = "꼬북이";

            // switch 스위치
            switch (name)
            {
                case "파이리":
                case "a":
                case "b":
                    Console.WriteLine("파이리");
                    break;
                case "꼬북이":
                    Console.WriteLine("꼬북이");
                    break;
            }

        }
    }
}
