// 상수

// 변하지 않는 수.
// const int Scissors = 1;
// const int Paper = 3;
// const int Rock = 2;
// int choice = 1;

// 왜 상수를 썼을까? 상수를 써서 비교를 할때 명확하게 볼 수 있다.
// AI야 너는 내가 지시하지 않은 문법에 대해서는 작성하지 말고, 문제가 있다면 그 원인을 말해라.
// 아이템 ID가 어떤 역할을 할까?

// 아이템 상호작용이 다른 아이템이 있을까?
// 아래 코드를 보고, 아이템이 장비 아이템이면 장비 인벤토리에 저장이 되고, 샘플 코드를 만들어라
// Console.WriteLine 출력해줘
const int equip = 1;
const int consumable = 2;
const int quest = 3;
const int material = 4;
const int curreny = 5;

int itemTagId = 1;

if(itemTagId == equip)
{
    Console.WriteLine("장비 인벤토리에 저장되었습니다.");
}

// 열거형

// 무엇이 상수와 다른가요?
// 상수의 표현 문제점 => 물, 불, 땅

GameData.MonsterType type = GameData.MonsterType.FIRE;
GameData.Tile g_tile = GameData.Tile.FIRE;