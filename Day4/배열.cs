// 배열

// 왜 쓸까요?
// 반복문. 갯수가 정해졌을 때 쓰면 'for'반복문

// 어떻게 배열을 사용하면 n개를 하나의 코드로 표현할 수 있을까요?
// 배열의 구현 원리를 알면 설명할 수 있다. => 자료구조.

int[] numbers = new int[10];

for (int i =0; i < 10; i++)
{
    numbers[i] = i;
    Console.WriteLine($"현재 값은 : {numbers[i]}");
}

// 배열은 같은 자료형을 한번에 표현할 수 있다.
// 갯수를 정하면 시작부터 끝까지 데이터를 저장하고 언제든지 꺼내서 쓸 수 있다.

// 가격은 가격인데... (xx)가격 너무 많은데? price 마음껏 써도 되나?

 static void Main(string[] args)
{
    const int ITEM_MAX_COUNT = 10;

    // 1. 전통 RPG 아이템 이름 배열 추가
    string[] names = new string[ITEM_MAX_COUNT]
    {
            "빨간 물약",
            "파란 물약",
            "해독제",
            "귀환 주문서",
            "단검",
            "강철 검",
            "가죽 갑옷",
            "사슬 갑옷",
            "나무 방패",
            "엘릭서"
    };

    // 2. 가격 및 아이템 개수 배열 선언
    int[] prices = new int[ITEM_MAX_COUNT];
    int[] counts = new int[ITEM_MAX_COUNT] { 15, 10, 5, 3, 1, 1, 1, 1, 2, 1 };

    // 3. 테이블 헤더 출력
    Console.WriteLine("=======================================================");
    Console.WriteLine("| 번호 |    아이템 이름    |   가격   |    수량    |");
    Console.WriteLine("=======================================================");

    // 4. 데이터 계산 및 테이블 행 출력
    for (int i = 0; i < ITEM_MAX_COUNT; i++)
    {
        prices[i] = 10 + 10 * i;

        // 한글/영문 너비 차이를 고려해 정렬 서식 적용
        Console.WriteLine($"| {i + 1,4} | {names[i],-13} | {prices[i],6} G | {counts[i],7} 개 |");
    }

    Console.WriteLine("=======================================================");
}