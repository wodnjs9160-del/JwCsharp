/*
 *  2026-11-23 
 *  개념 정리
 * '프로그램의 흐름'
 *  static void Main() - 메인 함수에서 프로그램을 시작한다.
 */

int a = -4;
int b = 8;
Console.WriteLine((float)a / b); // (타입) 형변환.
Console.WriteLine("hello"); // 아무 코드가 없는 페이지 2이상 있으면. Main이 두개가 있는거와 같다.

// 반복문에서 continue를 만나면 반복 시작점으로 돌아간다
// break를 만나면 루프를 종료한다.
for(int i=0; i <5; i++)
{
    if (i == 0) continue;
    if (i == 3) break;
    Console.WriteLine(i);
}