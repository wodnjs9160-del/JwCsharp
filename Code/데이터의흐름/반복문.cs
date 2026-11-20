// 반복문
// for반복문
// 언제 for 반복문을 쓰면 더 좋을까요? 반복하는 횟수가 정해져있을 때, 명확할 때
// while반복문
// 왜 써야하는가?

int a = 1;
int a1 = 2;
//                       i +=2
for (int i = 1; i<= 100 ; i++ )
{
    Console.WriteLine(i);
}

//  while 반복문
// 왜 써야하는가? 무한 반복문 표현. 무한히 반복할 때 표현.
// 어떤 로직이던 간에 일단 이 형태로 다 출발할 수 있다.

int hp = 0;
bool gamefinish = false;
while (true)
{

    // 게임이 종료되는 조건1
    if (hp <= 0) gamefinish = true;
    if(gamefinish)
    {
        break;
    }
}

// 게임은 항상 실행을 해야한다.
// 게임 특성상 while 반복문이 최상단에 쓰여있다.


while (true)
{
    // 입력 조건을 만들고

    // 로직

    // 렌더링 실행한다.
    Input();
    Logic();
    Rendering();

}

// 반복문을 사용하는 게임 추천 예제.

