using System.Text;
using WaveShooterGame;

Console.CursorVisible = false;
Console.OutputEncoding = Encoding.UTF8;

Player player = new Player(GameManager.MapWidth / 2, GameManager.MapHeight - 2);
List<Bullet> bullets = new List<Bullet>();
List<Monster> monsters = GameManager.StartNextWave();

while (!GameManager.IsGameOver)
{
    ProcessInput();
    UpdateGame();
    RenderScreen();

    Thread.Sleep(50);
}

ShowGameOver();

void ProcessInput()
{
    if (!Console.KeyAvailable) return;

    ConsoleKey key = Console.ReadKey(true).Key;
    if (key == ConsoleKey.Spacebar)
    {
        bullets.Add(player.Shoot());
    }
    else
    {
        player.Move(key);
    }
}

void UpdateGame()
{
    foreach (var bullet in bullets) bullet.Update();
    foreach (var monster in monsters) monster.Update();

    foreach (var bullet in bullets.Where(b => b.IsActive))
    {
        foreach (var monster in monsters.Where(m => m.IsAlive))
        {
            if (bullet.X == monster.X && bullet.Y == monster.Y)
            {
                bullet.IsActive = false;
                monster.IsAlive = false;
                GameManager.OnMonsterKilled();
                break;
            }
        }
    }

    bullets.RemoveAll(b => !b.IsActive);
    monsters.RemoveAll(m => !m.IsAlive);

    if (GameManager.AliveMonstersCount <= 0)
    {
        monsters = GameManager.StartNextWave();
    }
}

void RenderScreen()
{
    Console.SetCursorPosition(0, 0);
    StringBuilder sb = new StringBuilder();

    sb.AppendLine($"WAVE: {GameManager.CurrentWave} | ALIVE: {GameManager.AliveMonstersCount} | SCORE: {GameManager.Score}      ");
    sb.AppendLine(new string('=', GameManager.MapWidth));

    for (int y = 0; y < GameManager.MapHeight; y++)
    {
        for (int x = 0; x < GameManager.MapWidth; x++)
        {
            if (x == 0 || x == GameManager.MapWidth - 1)
                sb.Append('|');
            else if (x == player.X && y == player.Y)
                sb.Append(player.Symbol);
            else if (monsters.Any(m => m.IsAlive && m.X == x && m.Y == y))
                sb.Append('M');
            else if (bullets.Any(b => b.IsActive && b.X == x && b.Y == y))
                sb.Append('|');
            else
                sb.Append(' ');
        }
        sb.AppendLine();
    }

    sb.AppendLine(new string('=', GameManager.MapWidth));
    sb.AppendLine("조작: [W, A, S, D] 이동 | [Space] 발사");

    Console.Write(sb.ToString());
}

void ShowGameOver()
{
    Console.Clear();
    Console.WriteLine("===========================");
    Console.WriteLine("        GAME OVER          ");
    Console.WriteLine($"   최종 웨이브: {GameManager.CurrentWave}");
    Console.WriteLine($"   최종 점수: {GameManager.Score}");
    Console.WriteLine("===========================");
}
