// 아이템은 어떤 종류가 있을 까요? 열거형을 이용해 아이템의 종류를 만들어줘
// 갯수는 몇개가 있을 까요? 각 종류마다 10개
// 이를 콘솔에서 어떻게 보여지게 할거에요? 깔끔하게

// '조건문, 반복문, 배열, 열거형'

// 아이템 10개. 10개를 어떻게 포현할까? 배열의 선언

// 아이템 1개씩 데이터를 넣어야 한다. 배열의 초기화

// 반복적으로 코드를 그려야 한다.

// 입력 - 업데이트 - 렌더링 방식으로 상점 기능이 실행이 되게 만들어주세요.

// 어떤 입력을 해서 무엇이 변경해야하고 그것을 어떻게 보여줘야 한다.

using System;

class Program
{
    enum ItemType
    {
        Weapon = 0,     // 무기
        Armor = 1,      // 방어구
        Consumable = 2, // 소비아이템
        Count = 3       // 종류의 총 개수
    }

    static void Main(string[] args)
    {
        const int TYPE_COUNT = (int)ItemType.Count;
        const int ITEM_MAX_COUNT = 10;

        string[,] itemNames = new string[TYPE_COUNT, ITEM_MAX_COUNT];
        int[,] itemPrices = new int[TYPE_COUNT, ITEM_MAX_COUNT];
        int[,] itemCounts = new int[TYPE_COUNT, ITEM_MAX_COUNT];

        // [무기 10개]
        itemNames[(int)ItemType.Weapon, 0] = "나무 몽둥이"; itemPrices[(int)ItemType.Weapon, 0] = 100; itemCounts[(int)ItemType.Weapon, 0] = 5;
        itemNames[(int)ItemType.Weapon, 1] = "녹슨 단검"; itemPrices[(int)ItemType.Weapon, 1] = 250; itemCounts[(int)ItemType.Weapon, 1] = 3;
        itemNames[(int)ItemType.Weapon, 2] = "청동 검"; itemPrices[(int)ItemType.Weapon, 2] = 500; itemCounts[(int)ItemType.Weapon, 2] = 4;
        itemNames[(int)ItemType.Weapon, 3] = "철제 도끼"; itemPrices[(int)ItemType.Weapon, 3] = 800; itemCounts[(int)ItemType.Weapon, 3] = 2;
        itemNames[(int)ItemType.Weapon, 4] = "사냥용 활"; itemPrices[(int)ItemType.Weapon, 4] = 1100; itemCounts[(int)ItemType.Weapon, 4] = 3;
        itemNames[(int)ItemType.Weapon, 5] = "강철 장검"; itemPrices[(int)ItemType.Weapon, 5] = 1500; itemCounts[(int)ItemType.Weapon, 5] = 2;
        itemNames[(int)ItemType.Weapon, 6] = "마력의 지팡이"; itemPrices[(int)ItemType.Weapon, 6] = 2000; itemCounts[(int)ItemType.Weapon, 6] = 1;
        itemNames[(int)ItemType.Weapon, 7] = "은빛 레이피어"; itemPrices[(int)ItemType.Weapon, 7] = 2800; itemCounts[(int)ItemType.Weapon, 7] = 2;
        itemNames[(int)ItemType.Weapon, 8] = "미스릴 대검"; itemPrices[(int)ItemType.Weapon, 8] = 4000; itemCounts[(int)ItemType.Weapon, 8] = 1;
        itemNames[(int)ItemType.Weapon, 9] = "드래곤 슬레이어"; itemPrices[(int)ItemType.Weapon, 9] = 9900; itemCounts[(int)ItemType.Weapon, 9] = 1;

        // [방어구 10개]
        itemNames[(int)ItemType.Armor, 0] = "천 옷"; itemPrices[(int)ItemType.Armor, 0] = 80; itemCounts[(int)ItemType.Armor, 0] = 5;
        itemNames[(int)ItemType.Armor, 1] = "가죽 조끼"; itemPrices[(int)ItemType.Armor, 1] = 200; itemCounts[(int)ItemType.Armor, 1] = 4;
        itemNames[(int)ItemType.Armor, 2] = "나무 방패"; itemPrices[(int)ItemType.Armor, 2] = 350; itemCounts[(int)ItemType.Armor, 2] = 3;
        itemNames[(int)ItemType.Armor, 3] = "청동 투구"; itemPrices[(int)ItemType.Armor, 3] = 600; itemCounts[(int)ItemType.Armor, 3] = 3;
        itemNames[(int)ItemType.Armor, 4] = "사슬 갑옷"; itemPrices[(int)ItemType.Armor, 4] = 1000; itemCounts[(int)ItemType.Armor, 4] = 2;
        itemNames[(int)ItemType.Armor, 5] = "철제 방패"; itemPrices[(int)ItemType.Armor, 5] = 1400; itemCounts[(int)ItemType.Armor, 5] = 2;
        itemNames[(int)ItemType.Armor, 6] = "기사의 건틀릿"; itemPrices[(int)ItemType.Armor, 6] = 1800; itemCounts[(int)ItemType.Armor, 6] = 2;
        itemNames[(int)ItemType.Armor, 7] = "판금 갑옷"; itemPrices[(int)ItemType.Armor, 7] = 2600; itemCounts[(int)ItemType.Armor, 7] = 1;
        itemNames[(int)ItemType.Armor, 8] = "미스릴 흉갑"; itemPrices[(int)ItemType.Armor, 8] = 3800; itemCounts[(int)ItemType.Armor, 8] = 1;
        itemNames[(int)ItemType.Armor, 9] = "성기사의 성갑"; itemPrices[(int)ItemType.Armor, 9] = 8500; itemCounts[(int)ItemType.Armor, 9] = 1;

        // [소비아이템 10개]
        itemNames[(int)ItemType.Consumable, 0] = "빨간 물약"; itemPrices[(int)ItemType.Consumable, 0] = 50; itemCounts[(int)ItemType.Consumable, 0] = 20;
        itemNames[(int)ItemType.Consumable, 1] = "파란 물약"; itemPrices[(int)ItemType.Consumable, 1] = 70; itemCounts[(int)ItemType.Consumable, 1] = 15;
        itemNames[(int)ItemType.Consumable, 2] = "해독제"; itemPrices[(int)ItemType.Consumable, 2] = 100; itemCounts[(int)ItemType.Consumable, 2] = 10;
        itemNames[(int)ItemType.Consumable, 3] = "안약"; itemPrices[(int)ItemType.Consumable, 3] = 100; itemCounts[(int)ItemType.Consumable, 3] = 10;
        itemNames[(int)ItemType.Consumable, 4] = "귀환 주문서"; itemPrices[(int)ItemType.Consumable, 4] = 150; itemCounts[(int)ItemType.Consumable, 4] = 8;
        itemNames[(int)ItemType.Consumable, 5] = "주황 물약"; itemPrices[(int)ItemType.Consumable, 5] = 200; itemCounts[(int)ItemType.Consumable, 5] = 10;
        itemNames[(int)ItemType.Consumable, 6] = "맑은 물약"; itemPrices[(int)ItemType.Consumable, 6] = 250; itemCounts[(int)ItemType.Consumable, 6] = 10;
        itemNames[(int)ItemType.Consumable, 7] = "만병통치약"; itemPrices[(int)ItemType.Consumable, 7] = 500; itemCounts[(int)ItemType.Consumable, 7] = 5;
        itemNames[(int)ItemType.Consumable, 8] = "하얀 포션"; itemPrices[(int)ItemType.Consumable, 8] = 600; itemCounts[(int)ItemType.Consumable, 8] = 5;
        itemNames[(int)ItemType.Consumable, 9] = "엘릭서"; itemPrices[(int)ItemType.Consumable, 9] = 3000; itemCounts[(int)ItemType.Consumable, 9] = 2;

        ItemType currentCategory = ItemType.Weapon;
        int playerGold = 5000;
        string systemMessage = "상점에 오신 것을 환영합니다! 원하는 탭이나 아이템 번호를 입력하세요.";
        bool isRunning = true;

        while (isRunning)
        {
            // [1. 렌더링]
            Console.Clear();
            Console.WriteLine("========================================================");
            Console.WriteLine($" [전통 RPG 상점]                    보유 골드: {playerGold,6} G");
            Console.WriteLine("========================================================");

            string tabWeapon = (currentCategory == ItemType.Weapon) ? "[▶ Q.무기]" : "   Q.무기 ";
            string tabArmor = (currentCategory == ItemType.Armor) ? "[▶ W.방어구]" : "   W.방어구 ";
            string tabConsumable = (currentCategory == ItemType.Consumable) ? "[▶ E.소비]" : "   E.소비 ";
            Console.WriteLine($" 카테고리 전환 :  {tabWeapon}   {tabArmor}   {tabConsumable}");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine(" | 번호 |    아이템 이름     |    가격    | 남은 재고 |");
            Console.WriteLine("--------------------------------------------------------");

            int typeIndex = (int)currentCategory;
            for (int i = 0; i < ITEM_MAX_COUNT; i++)
            {
                // 실제 화면 가로 폭 계산 (한글 2칸, 나머지 1칸)
                string currentName = itemNames[typeIndex, i];
                int visualWidth = 0;

                for (int j = 0; j < currentName.Length; j++)
                {
                    if (currentName[j] >= '가' && currentName[j] <= '힣')
                    {
                        visualWidth += 2;
                    }
                    else
                    {
                        visualWidth += 1;
                    }
                }

                // 전체 칸 너비 18칸에서 실제 글자 너비를 뺀 만큼 스페이스(' ') 추가
                int targetWidth = 18;
                string paddedName = currentName + new string(' ', targetWidth - visualWidth);

                string stockText;
                if (itemCounts[typeIndex, i] > 0)
                {
                    stockText = $"{itemCounts[typeIndex, i],6} 개";
                }
                else
                {
                    stockText = "     품절";
                }

                Console.WriteLine($" | {i + 1,4} | {paddedName} | {itemPrices[typeIndex, i],8} G | {stockText} |");
            }

            Console.WriteLine("========================================================");
            Console.WriteLine($" 안내: {systemMessage}");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine(" [조작법] Q:무기 | W:방어구 | E:소비 | 1~10:구매 | 0:종료");
            Console.Write(" 입력 > ");

            // [2. 입력]
            string input = Console.ReadLine().ToUpper();

            // [3. 업데이트]
            if (input == "0")
            {
                isRunning = false;
                Console.WriteLine("상점을 이용해주셔서 감사합니다. 프로그램을 종료합니다.");
            }
            else if (input == "Q")
            {
                currentCategory = ItemType.Weapon;
                systemMessage = "[무기] 카테고리로 이동했습니다.";
            }
            else if (input == "W")
            {
                currentCategory = ItemType.Armor;
                systemMessage = "[방어구] 카테고리로 이동했습니다.";
            }
            else if (input == "E")
            {
                currentCategory = ItemType.Consumable;
                systemMessage = "[소비아이템] 카테고리로 이동했습니다.";
            }
            else if (int.TryParse(input, out int itemNumber) && itemNumber >= 1 && itemNumber <= 10)
            {
                int selectedIdx = itemNumber - 1;
                int price = itemPrices[typeIndex, selectedIdx];
                int stock = itemCounts[typeIndex, selectedIdx];
                string name = itemNames[typeIndex, selectedIdx];

                if (stock <= 0)
                {
                    systemMessage = $"[구매 실패] '{name}' 아이템은 품절되었습니다.";
                }
                else if (playerGold < price)
                {
                    systemMessage = $"[구매 실패] 골드가 부족합니다. (필요: {price} G)";
                }
                else
                {
                    playerGold -= price;
                    itemCounts[typeIndex, selectedIdx]--;
                    systemMessage = $"[구매 완료] '{name}' 1개를 구매했습니다! (남은 재고: {itemCounts[typeIndex, selectedIdx]}개)";
                }
            }
            else
            {
                systemMessage = "[입력 오류] Q, W, E 또는 1~10 사이의 번호를 입력해주세요.";
            }
        }
    }
}
