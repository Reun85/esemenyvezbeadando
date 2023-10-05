#nullable enable
namespace RobotPigs.Pers {
public class Pig {

  public const int HP = 3;
  public const int ORDERSIZE = 5;
  public String[] Orders {
    get => _orders;
  private
    set => _orders = value;
  }
  public bool ready = false;
  public static readonly String[] allowed = { "előre",         "hátra",
                                              "balra",         "jobbra",
                                              "fordulj balra", "fordulj jobbra",
                                              "tűz",           "ütés" };
  private int _hp;
  private Pos _pos;
  private string[] _orders = new String[0];

  public Pos Pos {
    get => _pos;
  private
    set => _pos = value;
  }
  public int Hp {
    get => _hp;
    set => _hp = value;
  }

  public Pig(Pos pos) {
    Pos = pos;
    Hp = HP;
  }

  public void setPos(Pos p) { Pos = p; }

  public static void validate(String[] inp) {
    if (inp.Length != ORDERSIZE) {
      throw new ArgumentOutOfRangeException("Not enough lines.");
    }
    for (int i = 0; i < ORDERSIZE; i++) {
      if (!Pig.allowed.Contains(inp[i])) {
        throw new ArgumentException($"\"{inp[i]}\" could not be parsed.");
      }
    }
  }
  public void takedmg() { Hp -= 1; }

  /// <throws>
  /// ArgumentOutOfRangeException => Not enough lines
  /// ArgumentException => what text could not be parsed.
  /// </throws>
  public void parse(String[] inp) {
    validate(inp);
    Orders = inp;
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
