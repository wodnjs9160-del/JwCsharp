// 추상 클래스. 공통적인 기능들을 모두 빼낸다.
// 테트리스블록이 공통적으로 가져야 할 요소가 무엇일까요?
// 회전, 떨어질 수 있어야 한다, 한줄이 채워지면 사라져야 한다, 겹치지 않는다, 바닥에 닿기 전까지 좌우로 이동이 가능하다.
// 색깔, 모양

// I미노 is 블록 -> 상속
// 정사각형 -> 회전을 할 수 도 있다. -> 회전가능한 IRollable 인터페이스

class 블록
{
    public string shape = "■";
    public 블록(string shape)
    {
        this.shape = shape;
    }
}

// 블록의 모든 성질을 가지면서 새로운 독특한 기능을 한번 추가하고 싶다.
// 시간 제한이 있어서 일정시간 후에 블록이 파괴된다. 위에 있는 블록은 자동으로 아래로 내려온다.

class Timerblock : 블록
{
    public int deathTimer = 3;
    public Timerblock(string shape) : base(shape)
    {
    }

    public void Timedeath() // 시간이 지났을 때 없어지는 기능을 영어로 바꿔서 ()
    {
        // 몇초뒤에 블록이 사라질지 있어야 한다.
    }
}

// 또 다른 블록 클래스를 만들어서 색다른 테트리스 게임을 만들어보기
// 한 줄이 완성됐을 때 폭발하여 주변 블록을 파괴시키는 블록
// 바닥에 닿기 전까지 매초마다 랜덤으로 다른 클래스 블록으로 바뀌는 블록
class Boomblock : 블록
{

}

class Randomblock : 블록
{

}