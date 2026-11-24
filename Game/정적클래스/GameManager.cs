namespace WaveShooterGame;

public static class GameManager
{
    public static int CurrentWave { get; private set; } = 0;
    public static int AliveMonstersCount { get; private set; } = 0;
    public static int Score { get; private set; } = 0;
    public static bool IsGameOver { get; set; } = false;

    public const int MapWidth = 40;
    public const int MapHeight = 20;

    public static List<Monster> StartNextWave()
    {
        CurrentWave++;
        int spawnCount = CurrentWave * 3;
        AliveMonstersCount = spawnCount;

        var newMonsters = new List<Monster>();
        Random rand = new Random();

        for (int i = 0; i < spawnCount; i++)
        {
            int x = rand.Next(2, MapWidth - 2);
            int y = rand.Next(1, 5);
            newMonsters.Add(new Monster(x, y));
        }

        return newMonsters;
    }

    public static void OnMonsterKilled()
    {
        AliveMonstersCount--;
        Score += 100;
    }
}
