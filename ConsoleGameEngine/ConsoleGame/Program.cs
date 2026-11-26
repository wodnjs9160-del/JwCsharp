// AI 임의로 구성한 내용을 파악하고 이 부분을 AI로 만들어서 만들어지는 결과를 일정한 수준으로 맞추게 유도하는 작업.
using System;
using System.Text;
using System.Threading;
// '상속'
// 내가 만들고 싶은 것을 클래스 만들어주세요.


abstract class BaseGameEngine
{
    abstract public void Input();
    abstract public void Update();
    abstract public void Rendering();
}

class MyGameEngine : BaseGameEngine
{
    private int width = 10;
    private int height = 10;
    private int playerX = 5;
    private int playerY = 5;
    private bool isRunning = true;

    // 화면 출력을 한 번에 모아서 처리할 버퍼 (깜빡임 방지 핵심)
    private StringBuilder screenBuffer = new StringBuilder();

    public MyGameEngine()
    {
        // 깜빡임의 주원인인 콘솔 커서 숨기기
        Console.CursorVisible = false;
    }

    public void Run()
    {
        while (isRunning)
        {
            Input();
            Update();
            Rendering();
            Thread.Sleep(50); // 프레임 속도 조절 (약 20 FPS)
        }
    }

    public override void Input()
    {
        // 키가 눌렸을 때만 입력 처리 (게임 루프가 멈추지 않게 함)
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow: if (playerX > 0) playerX--; break;
                case ConsoleKey.RightArrow: if (playerX < width - 1) playerX++; break;
                case ConsoleKey.UpArrow: if (playerY > 0) playerY--; break;
                case ConsoleKey.DownArrow: if (playerY < height - 1) playerY++; break;
                case ConsoleKey.Escape: isRunning = false; break;
            }
        }
    }

    public override void Update()
    {
        // 충돌 처리, 점수 계산 등 게임 로직 작성 위치
    }

    public override void Rendering()
    {
        // 1. Console.Clear() 대신 커서를 (0, 0)으로 이동시켜 덮어쓰기
        Console.SetCursorPosition(0, 0);
        screenBuffer.Clear();

        // 2. 2칸을 차지하는 특수문자("■", "□")를 사용할 때 
        //    배경과 플레이어 모두 동일한 2칸 문자로 통일하여 밀림 방지
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == playerX && y == playerY)
                {
                    screenBuffer.Append("■"); // 플레이어 (2칸 차지)
                }
                else
                {
                    screenBuffer.Append("□"); // 빈 공간 (2칸 차지)
                }
            }
            screenBuffer.AppendLine(); // 줄바꿈
        }

        screenBuffer.AppendLine("방향키: 이동 / ESC: 종료");

        // 3. 완성된 전체 화면 문자열을 단 한 번에 출력
        Console.Write(screenBuffer.ToString());
    }
}

class TetrisGame : MyGameEngine
{
    private const int Width = 10;
    private const int Height = 20;

    private int[,] board = new int[Height, Width];
    private string[,] displayGrid = new string[Height + 2, Width + 2]; // 테두리 포함

    private readonly int[][,] shapes = new int[][,]
    {
        new int[,] { {1,1,1,1} },                         // I
        new int[,] { {1,1}, {1,1} },                       // O
        new int[,] { {0,1,0}, {1,1,1} },                   // T
        new int[,] { {0,1,1}, {1,1,0} },                   // S
        new int[,] { {1,1,0}, {0,1,1} },                   // Z
        new int[,] { {1,0,0}, {1,1,1} },                   // J
        new int[,] { {0,0,1}, {1,1,1} }                    // L
    };

    private int[,] currentBlock;
    private int curX, curY;
    private int score = 0;
    private bool isGameOver = false;

    private int dropCounter = 0;
    private int dropInterval = 10;
    private Random rand = new Random();

    public TetrisGame() : base()
    {
        SpawnBlock();
    }

    private void SpawnBlock()
    {
        currentBlock = (int[,])shapes[rand.Next(shapes.Length)].Clone();
        curX = Width / 2 - currentBlock.GetLength(1) / 2;
        curY = 0;

        if (!IsValidPosition(currentBlock, curX, curY))
        {
            isGameOver = true;
        }
    }

    private bool IsValidPosition(int[,] block, int nextX, int nextY)
    {
        int rows = block.GetLength(0);
        int cols = block.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (block[r, c] == 0) continue;

                int boardX = nextX + c;
                int boardY = nextY + r;

                if (boardX < 0 || boardX >= Width || boardY >= Height)
                    return false;

                if (boardY >= 0 && board[boardY, boardX] == 1)
                    return false;
            }
        }
        return true;
    }

    private int[,] RotateMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        int[,] rotated = new int[cols, rows];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                rotated[c, rows - 1 - r] = matrix[r, c];
            }
        }
        return rotated;
    }

    private void LockBlock()
    {
        int rows = currentBlock.GetLength(0);
        int cols = currentBlock.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (currentBlock[r, c] == 1)
                {
                    int boardY = curY + r;
                    int boardX = curX + c;
                    if (boardY >= 0 && boardY < Height && boardX >= 0 && boardX < Width)
                    {
                        board[boardY, boardX] = 1;
                    }
                }
            }
        }

        ClearLines();
        SpawnBlock();
    }

    private void ClearLines()
    {
        int linesCleared = 0;

        for (int r = Height - 1; r >= 0; r--)
        {
            bool isFull = true;
            for (int c = 0; c < Width; c++)
            {
                if (board[r, c] == 0)
                {
                    isFull = false;
                    break;
                }
            }

            if (isFull)
            {
                linesCleared++;
                for (int moveRow = r; moveRow > 0; moveRow--)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        board[moveRow, c] = board[moveRow - 1, c];
                    }
                }
                for (int c = 0; c < Width; c++) board[0, c] = 0;
                r++;
            }
        }

        if (linesCleared > 0)
        {
            score += linesCleared * 100;
            dropInterval = Math.Max(2, 10 - (score / 500));
        }
    }

    public override void Input()
    {
        if (!Console.KeyAvailable) return;

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Escape)
        {
            isRunning = false; // 부모 엔진의 실행 플래그 제어
            return;
        }

        if (isGameOver) return;

        switch (keyInfo.Key)
        {
            case ConsoleKey.LeftArrow:
                if (IsValidPosition(currentBlock, curX - 1, curY)) curX--;
                break;
            case ConsoleKey.RightArrow:
                if (IsValidPosition(currentBlock, curX + 1, curY)) curX++;
                break;
            case ConsoleKey.DownArrow:
                if (IsValidPosition(currentBlock, curX, curY + 1))
                {
                    curY++;
                    dropCounter = 0;
                }
                break;
            case ConsoleKey.UpArrow:
                int[,] rotated = RotateMatrix(currentBlock);
                if (IsValidPosition(rotated, curX, curY)) currentBlock = rotated;
                break;
            case ConsoleKey.Spacebar:
                while (IsValidPosition(currentBlock, curX, curY + 1)) curY++;
                LockBlock();
                dropCounter = 0;
                break;
        }
    }

    public override void Update()
    {
        if (isGameOver) return;

        dropCounter++;
        if (dropCounter >= dropInterval)
        {
            dropCounter = 0;
            if (IsValidPosition(currentBlock, curX, curY + 1))
                curY++;
            else
                LockBlock();
        }
    }

    public override void Rendering()
    {
        // 1. 테두리 그리기 (모두 2칸 폭 전각 문자 사용)
        for (int r = 0; r < Height + 2; r++)
        {
            for (int c = 0; c < Width + 2; c++)
            {
                if (r == 0 && c == 0) displayGrid[r, c] = "┏";
                else if (r == 0 && c == Width + 1) displayGrid[r, c] = "┓";
                else if (r == Height + 1 && c == 0) displayGrid[r, c] = "┗";
                else if (r == Height + 1 && c == Width + 1) displayGrid[r, c] = "┛";
                else if (r == 0 || r == Height + 1) displayGrid[r, c] = "━";
                else if (c == 0 || c == Width + 1) displayGrid[r, c] = "┃";
                else displayGrid[r, c] = "ㆍ";
            }
        }

        // 2. 쌓인 블록 배치
        for (int r = 0; r < Height; r++)
        {
            for (int c = 0; c < Width; c++)
            {
                if (board[r, c] == 1)
                    displayGrid[r + 1, c + 1] = "▣";
            }
        }

        // 3. 현재 떨어지는 블록 배치
        if (!isGameOver)
        {
            for (int r = 0; r < currentBlock.GetLength(0); r++)
            {
                for (int c = 0; c < currentBlock.GetLength(1); c++)
                {
                    if (currentBlock[r, c] == 1)
                    {
                        int drawR = curY + r + 1;
                        int drawC = curX + c + 1;
                        if (drawR >= 1 && drawR <= Height && drawC >= 1 && drawC <= Width)
                            displayGrid[drawR, drawC] = "■";
                    }
                }
            }
        }

        // 4. 하단 안내 문구 생성 후 부모 엔진(MyGameEngine)의 렌더러 호출
        string status = isGameOver
            ? $"점수: {score}점 | ====== GAME OVER (ESC: 종료) ======"
            : $"점수: {score}점 | ←→:이동 ↑:회전 ↓:내림 Space:즉시낙하";

        RenderGrid(displayGrid, status);
    }
}

class Program
{
    static void Main()
    {
        // 엔진을 상속받은 테트리스 객체 실행
        BaseGameEngine game = new TetrisGame();
        ((MyGameEngine)game).Run();
    }
}