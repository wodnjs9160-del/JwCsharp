// AI를 활용하여 배운 문법을 작성하는 페이지 입니다.

// 클래스. 생성자.
// 클래스로 무엇을 할래?

// RPG 턴제. 전투.
// '플레이어'(똑같은 녀석) '적' (도감에 있는 모든 몬스터)
// Unit - 전투를 한다. 플레이어도하고 상대방도 똑같은 방식으로 한다.

// AI 읽을 수 있는 범위 만큼만 만들어달라.
// 클래스 한개만 만들어라.
// 내가 어떻게 만들건지 문법을 설명할거다.

// AI. 정해지지 않은 내용은 자기가 알아서 만든다. 다음 똑같이 프롬프트를 입력해도 결과가 다르게 나온다.
// 클래스를 직접 설계 하신다음에.
// 그 클래스로 while 반복문 입력, 조작, 업데이트, 렌더링 으로 만들어 줘

public class Unit
{
    // 속성
    public int Hp { get; set; }      // 체력
    public int Attack { get; set; }  // 공격력
    public int Defense { get; set; } // 방어력
    public int Speed { get; set; }   // 속도

    // 생성자
    public Unit(int hp, int attack, int defense, int speed)
    {
        Hp = hp;
        Attack = attack;
        Defense = defense;
        Speed = speed;
    }

    // 기능: 체력과 공격력과 방어력과 속도를 사용해서 전투하는 기능
    public void Battle(Unit target)
    {
        // 속도가 빠른 유닛이 선공
        Unit first = (this.Speed >= target.Speed) ? this : target;
        Unit second = (first == this) ? target : this;

        while (this.Hp > 0 && target.Hp > 0)
        {
            // 선공 유닛의 공격 (최소 데미지 1)
            int damage1 = Math.Max(1, first.Attack - second.Defense);
            second.Hp = Math.Max(0, second.Hp - damage1);

            if (second.Hp <= 0) break;

            // 후공 유닛의 반격 (최소 데미지 1)
            int damage2 = Math.Max(1, second.Attack - first.Defense);
            first.Hp = Math.Max(0, first.Hp - damage2);

            Console.WriteLine($"공격자가{damage1} 피해를 입혔습니다. 남은체력{second.Hp}");
        }
    }
}
