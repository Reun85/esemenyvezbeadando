using System;
namespace RobotPigs.Pers {

public class Board {
  /// n x n board.
  /// Numbered from top left.
  public int n;
  public Pig Plr1;
  public Pig Plr2;

  public Board(int s) {

    this.n = s;
    int center = this.n / 2;
    this.Plr1 = new Pig(new Pos(center - 1, center, Pos.Dir.East));
    this.Plr2 = new Pig(new Pos(center + 1, center, Pos.Dir.West));
  }
  public bool isReady() { return Plr1.ready && Plr2.ready; }
}
}
