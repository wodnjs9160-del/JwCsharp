- [몬스터](ConsoleApp1/Monster.cs)
- [플레이어](ConsoleApp1/Player.cs)
- [프로그램](ConsoleApp1/Program.cs)
- [웨이브](ConsoleApp1/GameManager.cs)
- [불릿](ConsoleApp1/Bullet.cs)

정적 클래스의 활용을 찾아서 아래에 작성하세요.

AI가 만든 코드를 해석할 때 코드줄 수가 많으면 많을수록 해석하기 어렵다.

## what
    - 몬스터 웨이브 시스템에 살아있는 몬스터 수를 정적(static) 클래스와 변수를 활용해서 구현했다.

## how
    - AI 해석 및 활용을 위해서 어떠헥 하면 클래스안의 코드 수를 줄일 수 있을까?

## 정리
    - 게임을 만들어줘(x)
    - 1. 입력, 업데이트, 렌더링 구현해라
    - 2. Detail 준다. 몬스터 생성되어 있는 갯수를 표현할 때 이런 상황에서는 static을 활용하면 좋더라.

 ``` 
 public static class GameManager
{
    public static int CurrentWave { get; private set; } = 0;
    public static int AliveMonstersCount { get; private set; } = 0;
    public static int Score { get; private set; } = 0;
    public static bool IsGameOver { get; set; } = false;
```