namespace RobotPigs {

public class Board {
  /// n x n board.
  /// Numbered from top left.
  public int n;
  public Pig Plr1 { get; private set; }
  public Pig Plr2 { get; private set; }

  public event EventHandler<Pig>? loses;
  public Board(int s) {

    this.n = s;
    int center = this.n / 2;
    this.Plr1 = new Pig(this, new Pos(center - 1, center, Pos.Dir.East));
    this.Plr2 = new Pig(this, new Pos(center + 1, center, Pos.Dir.West));
  }
  public Board(int s, Repr repr) {
    this.n = s;
    int center = this.n / 2;
    this.Plr1 =
        new Pig(this, new Pos(center - 1, center, Pos.Dir.East),
                repr.PosChangePlr2, repr.HpChangePlr2, repr.fire, repr.hit);
    this.Plr2 =
        new Pig(this, new Pos(center + 1, center, Pos.Dir.West),
                repr.PosChangePlr1, repr.HpChangePlr1, repr.fire, repr.hit);
    this.loses += repr.Loses;
  }

  public void Restart(object? s, int newsize) {
    this.n = newsize;
    int center = this.n / 2;
    this.Plr1.Pos = new Pos(center + 1, center, Pos.Dir.East);
    this.Plr2.Pos = new Pos(center - 1, center, Pos.Dir.West);
  }
  public void SaveGame(object? s, String filepath) {}
  public void LoadGame(object? s, String filepath) {}

  // This is done this way to allow our representation to call our code
  // periodically
  private int _performind = 0;
  public bool preparetoperform() {

    if (!Plr1.ready || !Plr2.ready) {
      return false;
    }
    _performind = 0;
    return true;
  }
  public bool performnext() {
    if (!Plr1.ready || !Plr2.ready) {
      throw new ArgumentException(
          "One of the players does not have orders set.");
    }

    Action act1 = Plr1.createaction(_performind);
    Action act2 = Plr2.createaction(_performind);
    if (act1.type == Action.ActionType.Move &&
        act2.type == Action.ActionType.Move &&
        Pos.sameplace(act1.newpos, act2.newpos)) {
      // Attempt to move to same place ignore.
    } else {
      Plr1.setPosFromAction(act1);
      Plr2.setPosFromAction(act2);

      Plr1.perform(act1, Plr2);
      Plr2.perform(act2, Plr1);
    }
    _performind++;
    return _performind < 5; // stop
  }
  public void lost(Pig loser) {
    if (loses != null)
      loses(this, loser);
  }

  public bool contains(Pos p) {
    return p.x >= 0 && p.x < this.n && p.y >= 0 && p.y < this.n;
  }
}
}
