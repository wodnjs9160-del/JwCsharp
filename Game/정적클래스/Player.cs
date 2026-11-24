namespace WaveShooterGame;

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public char Symbol => '▲';

    public Player(int startX, int startY)
    {
        X = startX;
        Y = startY;
    }

    public void Move(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W:
                if (Y > 1) Y--;
                break;
            case ConsoleKey.S:
                if (Y < GameManager.MapHeight - 2) Y++;
                break;
            case ConsoleKey.A:
                if (X > 1) X--;
                break;
            case ConsoleKey.D:
                if (X < GameManager.MapWidth - 2) X++;
                break;
        }
    }

    public Bullet Shoot()
    {
        return new Bullet(X, Y - 1);
    }
}
