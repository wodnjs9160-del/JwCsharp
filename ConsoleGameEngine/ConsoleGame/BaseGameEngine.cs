namespace ConsoleTetris
{
    // 1. 최상위 추상 게임 엔진
    public abstract class BaseGameEngine
    {
        public abstract void Input();
        public abstract void Update();
        public abstract void Rendering();
    }
}
