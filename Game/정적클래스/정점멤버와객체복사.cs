Enemy boss = new Enemy("보스", 300);
boss.Loot.Add("왕관");
Enemy shallow = boss.ShallowCopy();
Enemy deep = boss.DeepCopy();
shallow.Loot.Add("열쇠");
deep.Loot.Add("물약");
// string.Join: 리스트를 쉼표로 이어 붙인다
Console.WriteLine("보스: " + string.Join(", ", boss.Loot));
Console.WriteLine("깊은 복사본: " + string.Join(", ", deep.Loot));

// record 특수 문법. <- shawllowcopy
// 정적 클래스
// static 사용한 클래스

// 개별로 자신의 속성과 기능을 가질 수 있다.
// Enemy 몇마리 소환이 되었나요? => Enemy ... = new (); 몇번 호출이 세면 된다.
// Enemy 생성될 때 자동으로 몇마리 생성됐는지 파악을 하고 싶다.

// 클래스 이름 .(점)을 붙여서 안에 있는 데이터를 쓸 수 있는 방법이다.
// Class 이름 = new Class(); 형태 없이 쓸 수 있는 문법.

// static을 쓰면은 클래스 이름으로 바로 쓸 수 있다. -> Enemy.@@@@ static이 붙은 개념이다.
// static은 단일로 존재하는 녀석이어야 한다. -> 싱글톤.(sigleton) 'static'

Console.WriteLine($"몬스터가 몇마리 소환되었나요? {Enemy.Count} 마리");

class Enemy
{
    public static int Count;   // 클래스에 하나
    public string Name;        // 객체마다 하나

    public List<string> Loot = new List<string>();
    public int Hp;

    public Enemy(string name, int hp)
    {
        Name = name;
        Hp = hp;
        Count++;
    }
    public Enemy ShallowCopy()
    {
        return (Enemy)MemberwiseClone();
    }

    public Enemy DeepCopy()
    {
        Enemy copy = new Enemy(Name, Hp);
        copy.Loot = new List<string>(Loot);
        return copy;
    }
}