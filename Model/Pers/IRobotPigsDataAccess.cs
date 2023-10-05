
using System;
using System.Threading.Tasks;

namespace RobotPigs.Pers {
/// <summary>
/// Sudoku fájl kezelő felülete.
/// </summary>
public interface IRobotPigsDataAccess {
  /// <summary>
  /// Fájl betöltése.
  /// </summary>
  /// <param name="path">Elérési útvonal.</param>
  /// <returns>A fájlból beolvasott játéktábla.</returns>
  Task<Board> LoadAsync(String path);

  /// <summary>
  /// Fájl mentése.
  /// </summary>
  /// <param name="path">Elérési útvonal.</param>
  /// <param name="table">A fájlba kiírandó játéktábla.</param>
  Task SaveAsync(String path, Board table);
}
}
