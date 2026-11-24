namespace WaveShooterGame;

public class Bullet
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsActive { get; set; } = true;
    public char Symbol => '|';

    public Bullet(int startX, int startY)
    {
        X = startX;
        Y = startY;
    }

    public void Update()
    {
        Y--;
        if (Y <= 0)
        {
            IsActive = false;
        }
    }
}
