using GameData;

// '구현 전에 허락을 맡아라'


// 게임이 되려면 무엇이 필요할까?
// 1. 입력
// 2. 렌더링
// 3. Update





    class Program
    {
        static void Main()
        {
        // 상성 효과 수치 약속
        const int HealAmount = 20;
        const int DamageAmount = 30;
        const int GoalPosition = 5;

        // 몬스터 초기 상태
        MonsterType myMonster = MonsterType.FIRE;
        int monsterHp = 100;
        int playerPos = 1; // 현재 위치 (1 ~ 5)
        Tile currentTile = Tile.EARTH;
        string statusMessage = "방향키(←, →)를 눌러 이동하세요!";
        bool isGameOver = false;

        // ==========================================
        // 게임 메인 루프 (렌더링 -> 입력 -> 업데이트)
        // ==========================================
        while (!isGameOver)
        {
            // ------------------------------------------
            // 1. 렌더링 (Render) : 현재 게임 상태를 화면에 그림
            // ------------------------------------------
            Console.Clear(); // 이전 프레임 화면 지우기
            Console.WriteLine("==========================================");
            Console.WriteLine($" 몬스터: {myMonster} | 체력: {monsterHp} | 위치: {playerPos} / {GoalPosition}");
            Console.WriteLine("==========================================");

            // 맵(타일)과 플레이어 위치 시각화 (배열 없이 반복문으로 출력)
            Console.Write(" 맵: ");
            for (int i = 1; i <= GoalPosition; i++)
            {
                if (i == playerPos) Console.Write("[★] "); // 현재 몬스터 위치
                else if (i == 1) Console.Write("[땅] ");
                else if (i == 2 || i == 5) Console.Write("[불] ");
                else Console.Write("[물] ");
            }
            Console.WriteLine("\n------------------------------------------");
            Console.WriteLine($" 현재 밟은 타일 : {currentTile}");
            Console.WriteLine($" 알림 : {statusMessage}");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine(" 조작: [←] 왼쪽 이동  [→] 오른쪽 이동  [ESC] 종료");

            // ------------------------------------------
            // 2. 입력 (Input) : 플레이어의 키보드 입력 감지
            // ------------------------------------------
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); // 입력한 키가 화면에 찍히지 않게 처리
            int moveDirection = 0;

            if (keyInfo.Key == ConsoleKey.RightArrow) moveDirection = 1;
            else if (keyInfo.Key == ConsoleKey.LeftArrow) moveDirection = -1;
            else if (keyInfo.Key == ConsoleKey.Escape) break; // 게임 강제 종료
            else continue; // 다른 키를 누르면 업데이트 생략 후 다시 대기

            // ------------------------------------------
            // 3. 업데이트 (Update) : 입력에 따른 데이터 및 규칙 계산
            // ------------------------------------------
            // 위치 이동 (1 ~ 5 범위 제한)
            int nextPos = playerPos + moveDirection;
            if (nextPos < 1 || nextPos > GoalPosition)
            {
                statusMessage = "더 이상 그쪽으로 이동할 수 없습니다!";
                continue;
            }
            playerPos = nextPos;

            // 이동한 위치의 타일 속성 결정
            if (playerPos == 1) currentTile = Tile.EARTH;
            else if (playerPos == 2 || playerPos == 5) currentTile = Tile.FIRE;
            else currentTile = Tile.WATER;

            // 불타입 몬스터와 타일 상성 계산
            if (myMonster == MonsterType.FIRE)
            {
                switch (currentTile)
                {
                    case Tile.FIRE:
                        monsterHp += HealAmount;
                        statusMessage = $"용암 타일! 체력이 {HealAmount} 회복되었습니다.";
                        break;
                    case Tile.WATER:
                        monsterHp -= DamageAmount;
                        statusMessage = $"물 타일! 상성 데미지 {DamageAmount}을 입었습니다!";
                        break;
                    case Tile.EARTH:
                        statusMessage = "평범한 땅 타일입니다. 변화가 없습니다.";
                        break;
                }
            }

            // 게임 종료 조건(승리/패배) 판정
            if (monsterHp <= 0)
            {
                monsterHp = 0;
                isGameOver = true;
            }
            else if (playerPos == GoalPosition)
            {
                isGameOver = true;
            }
        }

        // 최종 결과 화면 렌더링
        Console.Clear();
        if (monsterHp <= 0)
            Console.WriteLine("\n💀 몬스터의 체력이 0이 되어 쓰러졌습니다... [GAME OVER]");
        else if (playerPos == GoalPosition)
            Console.WriteLine($"\n🎉 목적지에 무사히 도착했습니다! (남은 체력: {monsterHp}) [GAME CLEAR]");
        else
            Console.WriteLine("\n게임을 종료했습니다.");
    }
}