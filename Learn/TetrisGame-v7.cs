using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TetrisConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TetrisGame game = new TetrisGame();
            game.Run();
        }
    }

    public class TetrisGame
    {
        public class 블록
        {
            public string shape = "■";

            public 블록(string shape)
            {
                this.shape = shape;
            }

            public virtual string GetDisplayShape()
            {
                return shape;
            }
        }

        public class TimerBlock : 블록
        {
            public int deathTimer = 3;
            private double elapsedSeconds = 0;
            private bool isActive = false;
            public bool IsDestroyed { get; private set; }

            public TimerBlock(string shape) : base(shape)
            {
            }

            public void StartTimer()
            {
                isActive = true;
                elapsedSeconds = 0;
                IsDestroyed = false;
            }

            public void UpdateTimer(double deltaSeconds)
            {
                if (!isActive || IsDestroyed)
                {
                    return;
                }

                elapsedSeconds += deltaSeconds;
                if (elapsedSeconds >= deathTimer)
                {
                    TimeDeath();
                }
            }

            public void TimeDeath()
            {
                IsDestroyed = true;
                isActive = false;
            }

            public int RemainingSeconds()
            {
                int remaining = deathTimer - (int)Math.Floor(elapsedSeconds);
                return remaining < 0 ? 0 : remaining;
            }

            public override string GetDisplayShape()
            {
                if (!isActive)
                {
                    return "▣";
                }
                return RemainingSeconds().ToString();
            }
        }

        // 한 줄이 완성됐을 때 폭발하여 주변 블록을 파괴시키는 블록
        public class Boomblock : 블록
        {
            public int BlastRadius { get; set; } = 1;

            public Boomblock(string shape = "※") : base(shape)
            {
            }

            public void Explode(블록[,] board, int centerRow, int centerCol)
            {
                int rows = board.GetLength(0);
                int cols = board.GetLength(1);

                for (int r = centerRow - BlastRadius; r <= centerRow + BlastRadius; r++)
                {
                    for (int c = centerCol - BlastRadius; c <= centerCol + BlastRadius; c++)
                    {
                        if (r >= 0 && r < rows && c >= 0 && c < cols)
                        {
                            board[r, c] = null;
                        }
                    }
                }
            }

            public override string GetDisplayShape()
            {
                return shape;
            }
        }

        // 바닥에 닿기 전까지 매초마다 랜덤으로 전체 모양과 블록 클래스가 바뀌는 블록
        public class Randomblock : 블록
        {
            public Randomblock(string shape = "?") : base(shape)
            {
            }

            public override string GetDisplayShape()
            {
                return shape;
            }
        }

        private const int BoardWidth = 10;
        private const int BoardHeight = 20;

        // 0: I형, 1: O형, 2: T형, 3: Z형, 4: S형, 5: L형, 6: J형(랜덤 블록)
        private static readonly int[][,] Tetrominoes = new int[][,]
        {
            new int[,] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } }, // 0: I형 -> 일반 블록
            new int[,] { { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } }, // 1: O형 -> 일반 블록
            new int[,] { { 1, 0 }, { 0, 1 }, { 1, 1 }, { 2, 1 } }, // 2: T형 -> TimerBlock
            new int[,] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 2, 1 } }, // 3: Z형 -> TimerBlock
            new int[,] { { 1, 0 }, { 2, 0 }, { 0, 1 }, { 1, 1 } }, // 4: S형 -> Boomblock
            new int[,] { { 2, 0 }, { 0, 1 }, { 1, 1 }, { 2, 1 } }, // 5: L형 -> Boomblock
            new int[,] { { 0, 0 }, { 0, 1 }, { 1, 1 }, { 2, 1 } }  // 6: J형 -> Randomblock
        };

        private readonly 블록[,] board = new 블록[BoardHeight, BoardWidth];
        private readonly Random random = new Random();

        private 블록[,] currentPiece;
        private int currentShapeIndex;
        private bool isCurrentRandomPiece;
        private double randomMorphAccumulator;
        private int currentX;
        private int currentY;

        private int score = 0;
        private int linesCleared = 0;
        private bool isGameOver = false;
        private bool quitRequested = false;

        private double fallAccumulator = 0;
        private readonly double fallInterval = 0.55;
        private DateTime lastFrameTime;

        private ConsoleKey? queuedKey = null;

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            ResetGame();
            lastFrameTime = DateTime.UtcNow;

            while (!isGameOver && !quitRequested)
            {
                DateTime now = DateTime.UtcNow;
                double deltaSeconds = (now - lastFrameTime).TotalSeconds;
                lastFrameTime = now;
                if (deltaSeconds > 0.25)
                {
                    deltaSeconds = 0.25;
                }

                Input(deltaSeconds);
                Update(deltaSeconds);
                Render();

                Thread.Sleep(30);
            }

            Render();
            Console.SetCursorPosition(0, BoardHeight + 6);
            Console.CursorVisible = true;
            Console.WriteLine(quitRequested ? "게임을 종료했습니다." : $"게임 오버! 최종 점수: {score}");
        }

        private void ResetGame()
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    board[y, x] = null;
                }
            }

            score = 0;
            linesCleared = 0;
            isGameOver = false;
            quitRequested = false;
            fallAccumulator = 0;
            SpawnNewPiece();
        }

        private 블록 CreateBlockForShape(int shapeIndex)
        {
            switch (shapeIndex)
            {
                case 0: // I형
                case 1: // O형
                    return new 블록("■");
                case 2: // T형
                case 3: // Z형
                    return new TimerBlock("▣");
                case 4: // S형
                case 5: // L형
                    return new Boomblock("※");
                case 6: // J형
                default:
                    return new Randomblock("?");
            }
        }

        private 블록[,] BuildPieceMatrix(int shapeIndex, bool forceConcreteType = false)
        {
            int[,] offsets = Tetrominoes[shapeIndex];

            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;
            for (int i = 0; i < 4; i++)
            {
                minX = Math.Min(minX, offsets[i, 0]);
                maxX = Math.Max(maxX, offsets[i, 0]);
                minY = Math.Min(minY, offsets[i, 1]);
                maxY = Math.Max(maxY, offsets[i, 1]);
            }

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;
            블록[,] matrix = new 블록[height, width];

            for (int i = 0; i < 4; i++)
            {
                int px = offsets[i, 0] - minX;
                int py = offsets[i, 1] - minY;
                if (forceConcreteType && shapeIndex == 6)
                {
                    matrix[py, px] = new 블록("■");
                }
                else
                {
                    matrix[py, px] = CreateBlockForShape(shapeIndex);
                }
            }

            return matrix;
        }

        private void SpawnNewPiece()
        {
            currentShapeIndex = random.Next(Tetrominoes.Length);
            isCurrentRandomPiece = (currentShapeIndex == 6);
            randomMorphAccumulator = 0;

            currentPiece = BuildPieceMatrix(currentShapeIndex);
            int width = currentPiece.GetLength(1);

            currentX = (BoardWidth - width) / 2;
            currentY = 0;

            if (!CanMove(currentPiece, currentX, currentY))
            {
                isGameOver = true;
            }
        }

        // 랜덤 블록 피스가 낙하 중 1초마다 전체 모양과 클래스 타입을 다른 형태로 변환
        private void MorphWholePieceShape()
        {
            List<int> candidates = new List<int>();
            for (int i = 0; i < Tetrominoes.Length; i++)
            {
                if (i != currentShapeIndex)
                {
                    candidates.Add(i);
                }
            }

            int nextShapeIndex = candidates[random.Next(candidates.Count)];
            블록[,] nextMatrix = BuildPieceMatrix(nextShapeIndex);

            int[] kicks = { 0, -1, 1, -2, 2 };
            foreach (int dx in kicks)
            {
                if (CanMove(nextMatrix, currentX + dx, currentY))
                {
                    currentShapeIndex = nextShapeIndex;
                    currentPiece = nextMatrix;
                    currentX += dx;
                    return;
                }
            }
        }

        // 1. 입력
        private void Input(double deltaSeconds)
        {
            queuedKey = null;
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                queuedKey = keyInfo.Key;
            }
        }

        // 2. 갱신
        private void Update(double deltaSeconds)
        {
            if (isGameOver)
            {
                return;
            }

            if (isCurrentRandomPiece)
            {
                randomMorphAccumulator += deltaSeconds;
                if (randomMorphAccumulator >= 1.0)
                {
                    randomMorphAccumulator -= 1.0;
                    MorphWholePieceShape();
                }
            }

            if (queuedKey.HasValue)
            {
                HandleKey(queuedKey.Value);
            }

            fallAccumulator += deltaSeconds;
            if (fallAccumulator >= fallInterval)
            {
                fallAccumulator = 0;
                if (CanMove(currentPiece, currentX, currentY + 1))
                {
                    currentY++;
                }
                else
                {
                    SettleAndSpawnNext();
                }
            }

            bool anyTimerDestroyed = UpdateBoardTimers(deltaSeconds);
            if (anyTimerDestroyed)
            {
                ApplyGravity();
            }
        }

        private void SettleAndSpawnNext()
        {
            // 바닥에 닿을 때는 현재 테트로미노 형체를 그대로 고정 (ApplyGravity 호출 안 함)
            LockPiece();
            // 줄이 완성되었을 때만 폭발 및 빈 공간 중력 낙하 처리
            ClearLinesWithBoom();
            SpawnNewPiece();
        }

        private void HandleKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    if (CanMove(currentPiece, currentX - 1, currentY))
                    {
                        currentX--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    if (CanMove(currentPiece, currentX + 1, currentY))
                    {
                        currentX++;
                    }
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    if (CanMove(currentPiece, currentX, currentY + 1))
                    {
                        currentY++;
                        score += 1;
                    }
                    else
                    {
                        SettleAndSpawnNext();
                    }
                    break;

                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    TryRotatePiece();
                    break;

                case ConsoleKey.Spacebar:
                    while (CanMove(currentPiece, currentX, currentY + 1))
                    {
                        currentY++;
                        score += 2;
                    }
                    SettleAndSpawnNext();
                    break;

                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    quitRequested = true;
                    break;
            }
        }

        private void TryRotatePiece()
        {
            int oldH = currentPiece.GetLength(0);
            int oldW = currentPiece.GetLength(1);
            블록[,] rotated = new 블록[oldW, oldH];

            for (int y = 0; y < oldH; y++)
            {
                for (int x = 0; x < oldW; x++)
                {
                    rotated[x, oldH - 1 - y] = currentPiece[y, x];
                }
            }

            if (CanMove(rotated, currentX, currentY))
            {
                currentPiece = rotated;
            }
            else if (CanMove(rotated, currentX - 1, currentY))
            {
                currentX -= 1;
                currentPiece = rotated;
            }
            else if (CanMove(rotated, currentX + 1, currentY))
            {
                currentX += 1;
                currentPiece = rotated;
            }
        }

        private bool CanMove(블록[,] piece, int targetX, int targetY)
        {
            if (piece == null)
            {
                return false;
            }

            int h = piece.GetLength(0);
            int w = piece.GetLength(1);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (piece[y, x] == null)
                    {
                        continue;
                    }

                    int bx = targetX + x;
                    int by = targetY + y;

                    if (bx < 0 || bx >= BoardWidth || by >= BoardHeight)
                    {
                        return false;
                    }

                    if (by >= 0 && board[by, bx] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // 현재 피스가 수직으로 떨어졌을 때 착지할 최종 Y 좌표 계산 (고스트 블록용)
        private int GetGhostY()
        {
            int ghostY = currentY;
            while (CanMove(currentPiece, currentX, ghostY + 1))
            {
                ghostY++;
            }
            return ghostY;
        }

        private void LockPiece()
        {
            int h = currentPiece.GetLength(0);
            int w = currentPiece.GetLength(1);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    블록 cell = currentPiece[y, x];
                    if (cell == null)
                    {
                        continue;
                    }

                    int bx = currentX + x;
                    int by = currentY + y;

                    if (by >= 0 && by < BoardHeight && bx >= 0 && bx < BoardWidth)
                    {
                        // 착지 시점까지 J형(Randomblock) 상태였다면 일반 블록으로 확정 안착
                        if (cell is Randomblock)
                        {
                            cell = new 블록("■");
                        }

                        if (cell is TimerBlock tb)
                        {
                            tb.StartTimer();
                        }

                        board[by, bx] = cell;
                    }
                    else
                    {
                        isGameOver = true;
                    }
                }
            }
        }

        private void ClearLinesWithBoom()
        {
            List<int> completedRows = new List<int>();
            for (int y = BoardHeight - 1; y >= 0; y--)
            {
                bool isFull = true;
                for (int x = 0; x < BoardWidth; x++)
                {
                    if (board[y, x] == null)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    completedRows.Add(y);
                }
            }

            if (completedRows.Count == 0)
            {
                return;
            }

            List<(Boomblock boom, int r, int c)> boomsToTrigger = new List<(Boomblock, int, int)>();
            foreach (int row in completedRows)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    if (board[row, x] is Boomblock boom)
                    {
                        boomsToTrigger.Add((boom, row, x));
                    }
                    board[row, x] = null;
                }
            }

            foreach (var item in boomsToTrigger)
            {
                item.boom.Explode(board, item.r, item.c);
                score += 150;
            }

            linesCleared += completedRows.Count;
            score += completedRows.Count * 100;

            // 줄이 삭제되거나 폭발로 빈 공간이 생겼을 때만 위쪽 블록을 아래로 내림
            ApplyGravity();
        }

        private bool UpdateBoardTimers(double deltaSeconds)
        {
            bool destroyedAny = false;

            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    if (board[y, x] is TimerBlock tb)
                    {
                        tb.UpdateTimer(deltaSeconds);
                        if (tb.IsDestroyed)
                        {
                            board[y, x] = null;
                            destroyedAny = true;
                        }
                    }
                }
            }

            return destroyedAny;
        }

        private void ApplyGravity()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                int writeY = BoardHeight - 1;
                for (int y = BoardHeight - 1; y >= 0; y--)
                {
                    if (board[y, x] != null)
                    {
                        if (y != writeY)
                        {
                            board[writeY, x] = board[y, x];
                            board[y, x] = null;
                        }
                        writeY--;
                    }
                }
            }
        }

        // 3. 렌더링
        private void Render()
        {
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=== 특수 블록 테트리스 ===");
            sb.AppendLine($"점수: {score} | 삭제 라인: {linesCleared}");
            sb.AppendLine("I/O: ■ 일반 | T/Z: ▣ 타이머(3초) | S/L: ※ 폭발 | J: ? 랜덤(1초마다 모양변신)");
            sb.AppendLine("착지 위치 가이드: ┆ | 조작: ←/→ 이동, ↑ 회전, ↓ 연타, Space 즉시낙하, Q 종료");

            sb.Append("┌");
            for (int x = 0; x < BoardWidth; x++)
            {
                sb.Append("──");
            }
            sb.AppendLine("┐");

            string[,] display = new string[BoardHeight, BoardWidth];
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    display[y, x] = board[y, x] != null ? FormatCell(board[y, x].GetDisplayShape()) : "  ";
                }
            }

            if (currentPiece != null && !isGameOver)
            {
                int h = currentPiece.GetLength(0);
                int w = currentPiece.GetLength(1);

                // 1) 착지 예상 위치(고스트 블록) 먼저 표시
                int ghostY = GetGhostY();
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (currentPiece[y, x] != null)
                        {
                            int bx = currentX + x;
                            int by = ghostY + y;
                            if (by >= 0 && by < BoardHeight && bx >= 0 && bx < BoardWidth && board[by, bx] == null)
                            {
                                display[by, bx] = "┆ ";
                            }
                        }
                    }
                }

                // 2) 현재 조작 중인 블록 덮어쓰기
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (currentPiece[y, x] != null)
                        {
                            int bx = currentX + x;
                            int by = currentY + y;
                            if (by >= 0 && by < BoardHeight && bx >= 0 && bx < BoardWidth)
                            {
                                display[by, bx] = FormatCell(currentPiece[y, x].GetDisplayShape());
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < BoardHeight; y++)
            {
                sb.Append("│");
                for (int x = 0; x < BoardWidth; x++)
                {
                    sb.Append(display[y, x]);
                }
                sb.AppendLine("│");
            }

            sb.Append("└");
            for (int x = 0; x < BoardWidth; x++)
            {
                sb.Append("──");
            }
            sb.AppendLine("┘");

            Console.Write(sb.ToString());
        }

        private string FormatCell(string raw)
        {
            if (string.IsNullOrEmpty(raw))
            {
                return "  ";
            }
            if (raw.Length == 1)
            {
                return raw + " ";
            }
            return raw.Substring(0, 2);
        }
    }
}
