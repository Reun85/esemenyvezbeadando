#nullable enable
using System;
namespace RobotPigs.Pers {
/// <summary>
/// Sudoku fájlkezelő típusa.
/// </summary>
public class RobotPigsDataAccess : IRobotPigsDataAccess {
  /// <summary>
  ///  Load a file.
  /// </summary>
  /// <param name="path">Path to file.</param>
  /// <returns>The loaded Board.</returns>
  public async Task<Board> LoadAsync(String path) {
    try {
      using (StreamReader reader = new StreamReader(path)) // fájl megnyitása
      {
        String line = await reader.ReadLineAsync() ?? String.Empty;
        String[] numbers = line.Split(' ');
        Int32 boardSize = System.Int32.Parse(numbers[0]);
        Board table = new Board(boardSize);

        Pig[] plrs = new Pig[2];
        int[] buff = new int[4];
        for (int i = 0; i < 2; i++) {
          line = await reader.ReadLineAsync() ?? String.Empty;
          numbers = line.Split(' ');
          for (int j = 0; j < 4; j++) {
            buff[j] = System.Int32.Parse(numbers[0]);
          }
          plrs[i] = new Pers.Pig(
              new Pers.Pos(buff[0], buff[1], (Pers.Pos.Dir)buff[2]));
          plrs[i].Hp = buff[3];
        }

        table.Plr1 = plrs[0];
        table.Plr2 = plrs[1];
        return table;
      }
    } catch {
      throw new BoardDataException();
    }
  }

  /// <summary>
  /// Save into a file.
  /// </summary>
  /// <param name="path">Path to the file.</param>
  /// <param name="board">The board to save..</param>
  public async Task SaveAsync(String path, Board board) {
    try {
      using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
      {
        await writer.WriteLineAsync(Convert.ToString(board.n));

        for (int i = 0; i < 2; i++) {
          await writer.WriteAsync(Convert.ToString(board.Plr1.Pos.x));
          await writer.WriteAsync(Convert.ToString(board.Plr1.Pos.y));
          await writer.WriteAsync(Convert.ToString((int)board.Plr1.Pos.dir));
          await writer.WriteAsync(Convert.ToString(board.Plr1.Hp));
          await writer.WriteLineAsync();
        }
      }
    } catch {
      throw new BoardDataException();
    }
  }
}
}
