using System;
using System.Collections.Generic;

namespace StarCraftOOP
{
    public enum MovementType { Ground, Air }
    public enum UnitAttribute { Biological, Mechanical }

    public interface ICloakable
    {
        bool IsCloaked { get; }
        void ToggleCloak();
    }

    public interface IDetector
    {
        void Detect(List<Unit> units);
    }

    public interface ITransport
    {
        void LoadUnit(Unit unit);
        void UnloadAll();
    }

    public interface IWorker
    {
        void GatherResource();
    }

    public interface IBurrowable
    {
        bool IsBurrowed { get; }
        void ToggleBurrow();
    }

    public abstract class Unit
    {
        public string Name { get; protected set; }
        public int Hp { get; protected set; }
        public int MaxHp { get; protected set; }
        public MovementType MoveType { get; protected set; }
        public UnitAttribute Attribute { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }

        protected Unit(string name, int maxHp, MovementType moveType, UnitAttribute attribute)
        {
            Name = name;
            MaxHp = maxHp;
            Hp = maxHp;
            MoveType = moveType;
            Attribute = attribute;
        }

        public virtual void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
            Console.WriteLine($"[{Name}] ({MoveType}) 이동 -> ({X}, {Y})");
        }

        public virtual void TakeDamage(int damage)
        {
            Hp = Math.Max(0, Hp - damage);
            Console.WriteLine($"[{Name}] {damage} 피해 입음! (남은 체력: {Hp}/{MaxHp})");
        }

        public abstract void Update();

        public virtual void Render()
        {
            Console.WriteLine($"[{Name}] 체력: {Hp}/{MaxHp} | 타입: {MoveType}/{Attribute} | 위치: ({X}, {Y})");
        }
    }

    public abstract class TerranUnit : Unit
    {
        protected TerranUnit(string name, int maxHp, MovementType moveType, UnitAttribute attribute)
            : base(name, maxHp, moveType, attribute) { }

        public override void Update()
        {
        }
    }

    public abstract class ZergUnit : Unit
    {
        protected ZergUnit(string name, int maxHp, MovementType moveType, UnitAttribute attribute)
            : base(name, maxHp, moveType, attribute) { }

        public override void Update()
        {
            if (Hp > 0 && Hp < MaxHp)
            {
                Hp = Math.Min(MaxHp, Hp + 2);
                Console.WriteLine($"[{Name}] 저그 생체 재생 (+2) -> 현재 체력: {Hp}/{MaxHp}");
            }
        }
    }

    public abstract class ProtossUnit : Unit
    {
        public int Shield { get; protected set; }
        public int MaxShield { get; protected set; }

        protected ProtossUnit(string name, int maxHp, int maxShield, MovementType moveType, UnitAttribute attribute)
            : base(name, maxHp, moveType, attribute)
        {
            MaxShield = maxShield;
            Shield = maxShield;
        }

        public override void TakeDamage(int damage)
        {
            if (Shield > 0)
            {
                int absorbed = Math.Min(Shield, damage);
                Shield -= absorbed;
                damage -= absorbed;
                Console.WriteLine($"[{Name}] 쉴드가 {absorbed} 피해 흡수 (남은 쉴드: {Shield}/{MaxShield})");
            }
            if (damage > 0)
            {
                base.TakeDamage(damage);
            }
        }

        public override void Update()
        {
            if (Hp > 0 && Shield < MaxShield)
            {
                Shield = Math.Min(MaxShield, Shield + 3);
                Console.WriteLine($"[{Name}] 프로토스 쉴드 재생 (+3) -> 현재 쉴드: {Shield}/{MaxShield}");
            }
        }

        public override void Render()
        {
            Console.WriteLine($"[{Name}] 체력: {Hp}/{MaxHp} | 쉴드: {Shield}/{MaxShield} | 타입: {MoveType}/{Attribute} | 위치: ({X}, {Y})");
        }
    }

    public class SCV : TerranUnit, IWorker
    {
        public SCV() : base("SCV", 60, MovementType.Ground, UnitAttribute.Mechanical) { }

        public void GatherResource()
        {
            Console.WriteLine($"[{Name}] 미네랄과 가스를 채취합니다.");
        }
    }

    public class Wraith : TerranUnit, ICloakable
    {
        public bool IsCloaked { get; private set; }

        public Wraith() : base("레이스", 120, MovementType.Air, UnitAttribute.Mechanical) { }

        public void ToggleCloak()
        {
            IsCloaked = !IsCloaked;
            Console.WriteLine($"[{Name}] 은신 상태: {(IsCloaked ? "활성화" : "해제")}");
        }

        public override void Render()
        {
            base.Render();
            if (IsCloaked) Console.WriteLine("   └ 상태: 은신 중");
        }
    }

    public class Dropship : TerranUnit, ITransport
    {
        private readonly List<Unit> _cargo = new List<Unit>();

        public Dropship() : base("드랍쉽", 150, MovementType.Air, UnitAttribute.Mechanical) { }

        public void LoadUnit(Unit unit)
        {
            if (unit.MoveType == MovementType.Ground)
            {
                _cargo.Add(unit);
                Console.WriteLine($"[{Name}] {unit.Name} 탑승 완료 (탑승 인원: {_cargo.Count})");
            }
            else
            {
                Console.WriteLine($"[{Name}] 공중 유닛({unit.Name})은 태울 수 없습니다.");
            }
        }

        public void UnloadAll()
        {
            Console.WriteLine($"[{Name}] 모든 유닛({_cargo.Count}기) 하차!");
            _cargo.Clear();
        }
    }

    public class Zergling : ZergUnit, IBurrowable
    {
        public bool IsBurrowed { get; private set; }

        public Zergling() : base("저글링", 35, MovementType.Ground, UnitAttribute.Biological) { }

        public void ToggleBurrow()
        {
            IsBurrowed = !IsBurrowed;
            Console.WriteLine($"[{Name}] 버로우(잠복) 상태: {(IsBurrowed ? "잠복 완료" : "지상 복귀")}");
        }

        public override void Move(int dx, int dy)
        {
            if (IsBurrowed)
            {
                Console.WriteLine($"[{Name}] 잠복 상태에서는 이동할 수 없습니다.");
                return;
            }
            base.Move(dx, dy);
        }

        public override void Render()
        {
            base.Render();
            if (IsBurrowed) Console.WriteLine("   └ 상태: 잠복 중");
        }
    }

    public class Overlord : ZergUnit, IDetector, ITransport
    {
        private readonly List<Unit> _cargo = new List<Unit>();

        public Overlord() : base("오버로드", 200, MovementType.Air, UnitAttribute.Biological) { }

        public void Detect(List<Unit> units)
        {
            Console.WriteLine($"[{Name}] 디텍팅 시야 가동!");
            foreach (var unit in units)
            {
                if (unit is ICloakable cloakable && cloakable.IsCloaked)
                    Console.WriteLine($"   -> 은신 유닛 감지: {unit.Name}");
                if (unit is IBurrowable burrowable && burrowable.IsBurrowed)
                    Console.WriteLine($"   -> 잠복 유닛 감지: {unit.Name}");
            }
        }

        public void LoadUnit(Unit unit)
        {
            if (unit.MoveType == MovementType.Ground)
            {
                _cargo.Add(unit);
                Console.WriteLine($"[{Name}] {unit.Name} 수송 탑승");
            }
        }

        public void UnloadAll()
        {
            Console.WriteLine($"[{Name}] 수송 유닛 전체 하차");
            _cargo.Clear();
        }
    }

    public class Observer : ProtossUnit, ICloakable, IDetector
    {
        public bool IsCloaked { get; private set; } = true;

        public Observer() : base("옵저버", 40, 20, MovementType.Air, UnitAttribute.Mechanical) { }

        public void ToggleCloak()
        {
            Console.WriteLine($"[{Name}] 영구 은신 유닛입니다.");
        }

        public void Detect(List<Unit> units)
        {
            Console.WriteLine($"[{Name}] 관측선 탐지 시작!");
            foreach (var unit in units)
            {
                if (unit is ICloakable c && c.IsCloaked && unit != this)
                    Console.WriteLine($"   -> 은신 유닛 감지: {unit.Name}");
                if (unit is IBurrowable b && b.IsBurrowed)
                    Console.WriteLine($"   -> 잠복 유닛 감지: {unit.Name}");
            }
        }
    }

    public class DarkTemplar : ProtossUnit, ICloakable
    {
        public bool IsCloaked { get; private set; } = true;

        public DarkTemplar() : base("다크 템플러", 80, 40, MovementType.Ground, UnitAttribute.Biological) { }

        public void ToggleCloak()
        {
            Console.WriteLine($"[{Name}] 영구 은신 상태입니다.");
        }
    }

    public class Program
    {
        private static readonly List<Unit> Units = new List<Unit>();
        private static bool _isRunning = true;

        public static void Main(string[] args)
        {
            Init();

            while (_isRunning)
            {
                Render();
                Input();
                Update();
            }
        }

        private static void Init()
        {
            Units.Add(new SCV());
            Units.Add(new Wraith());
            Units.Add(new Dropship());
            Units.Add(new Zergling());
            Units.Add(new Overlord());
            Units.Add(new Observer());
            Units.Add(new DarkTemplar());
        }

        private static void Input()
        {
            Console.WriteLine("=== 명령을 선택하세요 ===");
            Console.WriteLine("1. 전체 이동 (+1, +1)");
            Console.WriteLine("2. 전체 피격 테스트 (데미지 30)");
            Console.WriteLine("3. 특수 기능 실행 (다형성 인터페이스 호출)");
            Console.WriteLine("4. 턴 넘기기 (체력/쉴드 재생 갱신)");
            Console.WriteLine("0. 종료");
            Console.Write("입력 > ");

            string input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    foreach (var unit in Units)
                        unit.Move(1, 1);
                    break;

                case "2":
                    foreach (var unit in Units)
                        unit.TakeDamage(30);
                    break;

                case "3":
                    ExecuteSpecialAbilities();
                    break;

                case "4":
                    Console.WriteLine("턴을 진행합니다...");
                    break;

                case "0":
                    _isRunning = false;
                    Console.WriteLine("프로그램을 종료합니다.");
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }

        private static void ExecuteSpecialAbilities()
        {
            foreach (var unit in Units)
            {
                if (unit is IWorker worker)
                    worker.GatherResource();

                if (unit is ICloakable cloakable)
                    cloakable.ToggleCloak();

                if (unit is IBurrowable burrowable)
                    burrowable.ToggleBurrow();

                if (unit is ITransport transport)
                    transport.LoadUnit(Units[0]);

                if (unit is IDetector detector)
                    detector.Detect(Units);
            }
        }

        private static void Update()
        {
            if (!_isRunning) return;

            Console.WriteLine("--- [Update: 상태 갱신] ---");
            foreach (var unit in Units)
            {
                unit.Update();
            }
        }

        private static void Render()
        {
            Console.WriteLine("============================== [Render: 현재 유닛 상태] ==============================");
            foreach (var unit in Units)
            {
                unit.Render();
            }
            Console.WriteLine("======================================================================================");
        }
    }
}
