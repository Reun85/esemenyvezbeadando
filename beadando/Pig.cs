namespace RobotPigs {
public class Pig {
  public String[] orders { get; private set; }
  public bool ready = false;
  public static String[] allowed = { "előre",         "hátra",
                                     "balra",         "jobbra",
                                     "fordulj balra", "fordulj jobbra",
                                     "tűz",           "ütés" };
  Board board;
  Pos pos;

  public event EventHandler<Pig>? posChange;
  public event EventHandler<Pig>? hpChange;
  public event EventHandler<Pig>? fire;
  public event EventHandler<Pig>? hit;
  private int hp;
  public Pos Pos {
    get => pos;
    set {
      pos = value;
      if (this.posChange != null)
        this.posChange(this, this);
    }
  }

  public int Hp {
    get => hp;
    set {
      hp = value;
      if (hpChange != null)
        hpChange(this, this);
    }
  }

  public Pig(Board board, Pos pos) {
    this.orders = new String[5];
    this.board = board;
    this.Pos = pos;
    this.Hp = 3;
  }
  public Pig(Board board, Pos pos, EventHandler<Pig>? poschange,
             EventHandler<Pig>? hpchange, EventHandler<Pig>? fire,
             EventHandler<Pig>? hit) {
    this.posChange += poschange;
    this.hpChange += hpchange;
    this.fire += fire;
    this.hit += hit;
    this.orders = new String[5];
    this.board = board;
    this.Pos = pos;
    this.Hp = 3;
  }
  public static void validate(String[] inp) {
    if (inp.Length != 5) {
      throw new ArgumentOutOfRangeException("Not enough lines.");
    }
    for (int i = 0; i < 5; i++) {
      if (!Pig.allowed.Contains(inp[i])) {
        throw new ArgumentException($"\"{inp[i]}\" could not be parsed.");
      }
    }
  }
  /// orders must be set!
  public Action createaction(int ind) {
    // These default should never be necessary, but just to be
    // sure.
    Pos newpos = this.Pos;
    Action.ActionType type = Action.ActionType.Fire;
    switch (this.orders[ind]) {
    case "előre":
      newpos = this.Pos.move(Pos.MovDir.Forward, this.board);
      type = Action.ActionType.Move;
      break;
    case "hátra":
      newpos = this.Pos.move(Pos.MovDir.Back, this.board);
      type = Action.ActionType.Move;
      break;
    case "balra":
      newpos = this.Pos.move(Pos.MovDir.Left, this.board);
      type = Action.ActionType.Move;
      break;
    case "jobbra":
      newpos = this.Pos.move(Pos.MovDir.Right, this.board);
      type = Action.ActionType.Move;
      break;
    case "fordulj balra":
      newpos = this.Pos.turn(Pos.MovDir.Left);
      type = Action.ActionType.Turn;
      break;
    case "fordulj jobbra":
      newpos = this.Pos.turn(Pos.MovDir.Right);
      type = Action.ActionType.Turn;
      break;
    case "tűz":
      type = Action.ActionType.Fire;
      break;
    case "ütés":
      type = Action.ActionType.Hit;
      break;
    }
    return new Action(type, newpos);
  }
  public void perform(Action act, Pig rhs) {
    if (act.type == Action.ActionType.Fire) {
      if (this.fire != null)
        this.fire(this, this);
      if (this.pos.inview(rhs.pos))
        rhs.takedmg();
    }
    if (act.type == Action.ActionType.Hit) {
      if (this.hit != null)
        this.hit(this, this);
      if (this.pos.inradius(rhs.pos)) {
        rhs.takedmg();
      }
    }
  }
  public void takedmg() {
    this.Hp -= 1;
    if (Hp == 0)
      board.lost(this);
  }

  public void setPosFromAction(Action a) { this.Pos = a.newpos; }

  /// <throws>
  /// ArgumentOutOfRangeException => Not enough lines
  /// ArgumentException => what text could not be parsed.
  /// </throws>
  public void parse(String[] inp) {
    validate(inp);
    orders = inp;
    ready = true;
  }
}
public class Action {
  public enum ActionType { Move, Turn, Hit, Fire }
  public ActionType type;
  public Pos newpos;
  public Action(ActionType t, Pos p) {
    this.type = t;
    this.newpos = p;
  }
}
}
