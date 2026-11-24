namespace WaveShooterGame;

public class Monster
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsAlive { get; set; } = true;
    public char Symbol => 'M';

    private int _moveCounter = 0;

    public Monster(int startX, int startY)
    {
        X = startX;
        Y = startY;
    }

    public void Update()
    {
        _moveCounter++;
        if (_moveCounter >= 10)
        {
            Y++;
            _moveCounter = 0;
        }

        if (Y >= GameManager.MapHeight - 1)
        {
            GameManager.IsGameOver = true;
        }
    }
}
