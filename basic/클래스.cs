// 클래스 사용하기
Game lol = new Game("롤", 1.01f);     // (1) 어떤 클래스 쓸지 정하기. Game형태를 저장할 수 있고 이름이 lol이다. 공간은 아직 x
// lol.title = "롤";                  // (2) 메모리에 공간 만들기new [타입]. [타입] 메모리 크기만큼 공간을 만들어준다.
// lol.version = 1.01f;               // (3) 공간에 데이터 처음 넣기 "초기화 Initialize"
lol.Run();

Game val = new Game("발로란트", 1.11f);
// val.title = "발로란트";
// val.version = 1.11f;
val.Run();

// 클래스 만들기
// 실습 예제. 게임에 버전을 실수형으로 정의를 해서 실행할때 {title} v{version}을 실행합니다.
// 생성자.
class Game
{
    // 속성. 게임이라는 개념이 무엇을 가지고 있어야 하나.
    public string title;
    // 버전
    public float version;

    public Game(string title, float version)
    {
        this.title = title;
        this.version = version;
    }

    // 실행한다. 모든 게임이 실행하나요?

    // 함수 : 기능
    public void Run()
    {
        Console.WriteLine($"{title}v{version}을 실행합니다.");
    }
}

// 클래스 왜 만들어요?
// 발로란트를 실행합니다.
// 롤을 실행합니다.
// 공통적인 기능과 속성을 가진 개념을 하나의 설계도로 만드는 방법을 클래스라고 합니다.
// 기능 : 함수
// 속성 : 변수

