/*
 *  클래스의 생성자까지 사용 문법 실습한다.
 * 
 *  -게임 폴더
 *      -Program.csw
 *      -Unit.cs
 *      
 *      AI코드를 하나의 Main 작성을 하면 가독성이 매우 덜어집니다. Ai가 무엇을 만들었는지 1도 알아볼 수가 없다.
 *      이해 가능한 범위까지 코드를 줄이면 됩니다.
 *      1번째로. Main으로만 게임 만들어. -> 공통된 속성과 기능을 찾아서 클래스로 표현하고 그 클래스를 분할시켜라.
 */

namespace TextRPG
{
    // 1. 몬스터 클래스 정의
    public class Monster
    {
        // 속성 (Properties)
        public string Name { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }        // 선공 순서 및 회피율에 영향
        public int Intelligence { get; set; } // 공격 명중률에 영향

        private static Random rand = new Random();

        public Monster(string name, int hp, int attack, int defense, int speed, int intelligence)
        {
            Name = name;
            MaxHp = hp;
            Hp = hp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            Intelligence = intelligence;
        }

        // 기능: 공격하기 (지능: 명중, 스피드: 회피 반영)
        public string AttackTarget(Monster target)
        {
            // 1) 지능 기반 공격 성공(명중) 판정 (기본 명중률 50% + 지능 * 2%)
            int hitChance = 50 + (this.Intelligence * 2);
            if (rand.Next(1, 101) > hitChance)
            {
                return $"{Name}의 공격이 빗나갔습니다! (지능 부족)";
            }

            // 2) 상대 스피드 기반 회피 판정 (상대 스피드 * 1.5% 확률로 회피)
            int dodgeChance = (int)(target.Speed * 1.5);
            if (rand.Next(1, 101) <= dodgeChance)
            {
                return $"{target.Name}이(가) 재빠르게 {Name}의 공격을 회피했습니다!";
            }

            // 3) 데미지 계산 (공격력 - 상대 방어력, 최소 데미지 1)
            int damage = Math.Max(1, this.Attack - target.Defense);
            target.Hp = Math.Max(0, target.Hp - damage);

            return $"{Name}의 공격 적중! {target.Name}에게 {damage}의 데미지를 입혔습니다.";
        }

        public bool IsDead() => Hp <= 0;
    }

    // 2. 게임 루프 (입력 -> 조작 -> 업데이트 -> 렌더링)
    class Program
    {
        static void Main(string[] args)
        {
            // 몬스터 생성 (이름, 체력, 공격력, 방어력, 스피드, 지능)
            Monster player = new Monster("용사", 100, 20, 5, 15, 20);
            Monster enemy = new Monster("고블린", 80, 18, 3, 12, 10);

            bool isRunning = true;
            string battleLog = "전투가 시작되었습니다! 행동을 선택하세요.";
            ConsoleKey inputKey = ConsoleKey.None;

            // 초기 화면 렌더링
            Render(player, enemy, battleLog);

            // 게임 메인 루프
            while (isRunning)
            {
                // [1. 입력 (Input)]
                inputKey = GetInput();

                // [2. 조작 (Control)] & [3. 업데이트 (Update)]
                Update(inputKey, player, enemy, ref battleLog, ref isRunning);

                // [4. 렌더링 (Render)]
                Render(player, enemy, battleLog);

                // 게임 종료 조건 확인
                if (player.IsDead() || enemy.IsDead())
                {
                    isRunning = false;
                }
            }

            Console.WriteLine("\n=== 전투 종료 ===");
        }

        // [1. 입력] 키보드 입력 받기
        static ConsoleKey GetInput()
        {
            return Console.ReadKey(true).Key; // true: 누른 키를 콘솔에 출력하지 않음
        }

        // [2. 조작 & 3. 업데이트] 입력에 따른 행동 처리 및 전투 상태 갱신
        static void Update(ConsoleKey key, Monster player, Monster enemy, ref string log, ref bool isRunning)
        {
            if (key == ConsoleKey.Q)
            {
                log = "게임을 종료합니다.";
                isRunning = false;
                return;
            }

            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) // 1번: 공격
            {
                log = "";
                // 스피드 비교를 통한 선공 결정
                if (player.Speed >= enemy.Speed)
                {
                    log += $"[선공] {player.AttackTarget(enemy)}\n";
                    if (!enemy.IsDead())
                        log += $"[후공] {enemy.AttackTarget(player)}";
                }
                else
                {
                    log += $"[선공] {enemy.AttackTarget(player)}\n";
                    if (!player.IsDead())
                        log += $"[후공] {player.AttackTarget(enemy)}";
                }
            }
            else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) // 2번: 휴식 (체력 회복)
            {
                int heal = 15;
                player.Hp = Math.Min(player.MaxHp, player.Hp + heal);
                log = $"{player.Name}이(가) 휴식하여 체력을 {heal} 회복했습니다.\n";
                log += enemy.AttackTarget(player); // 휴식 중 적의 공격
            }
            else
            {
                log = "잘못된 입력입니다. 1(공격), 2(회복), Q(종료) 중 하나를 누르세요.";
            }

            // 승패 결과 업데이트
            if (enemy.IsDead()) log += $"\n★ {enemy.Name}을(를) 물리쳤습니다! 승리!";
            else if (player.IsDead()) log += $"\n💀 {player.Name}이(가) 쓰러졌습니다... 패배!";
        }

        // [4. 렌더링] 현재 상태를 화면에 그리기
        static void Render(Monster player, Monster enemy, string log)
        {
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine($" [내 캐릭터] {player.Name}");
            Console.WriteLine($" HP: {player.Hp}/{player.MaxHp} | 공:{player.Attack} 방:{player.Defense} 속:{player.Speed} 지:{player.Intelligence}");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($" [적 몬스터] {enemy.Name}");
            Console.WriteLine($" HP: {enemy.Hp}/{enemy.MaxHp} | 공:{enemy.Attack} 방:{enemy.Defense} 속:{enemy.Speed} 지:{enemy.Intelligence}");
            Console.WriteLine("==========================================\n");

            Console.WriteLine("[전투 로그]");
            Console.WriteLine(log);
            Console.WriteLine("\n------------------------------------------");
            Console.WriteLine("조작키: [1] 공격   [2] 회복   [Q] 종료");
        }
    }
}
