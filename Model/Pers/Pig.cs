#nullable enable
using System.Text;

namespace RobotPigs.Pers {
public class Pig {

  public const int HP = 3;
  public const int ORDERSIZE = 5;
  public String[] Orders {
    get => _orders;
  private
    set => _orders = value;
  }
        private bool _ready = false;
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
        public bool Ready { get => _ready; set => _ready = value; }

        public Pig(Pos pos) {
    Pos = pos;
    Hp = HP;
  }

  public void setPos(Pos p) { Pos = p; }

  public static void validate(String[] inp) {
    if (inp.Length != ORDERSIZE) {
      throw new ArgumentOutOfRangeException("Not the right amount of lines.");
    }
    for (int i = 0; i < ORDERSIZE; i++) {
      if (!Pig.allowed.Contains(inp[i])) {
        throw new ArgumentException($"\"{inp[i]}\" nem egy értelmes parancs!"+Environment.NewLine + "Használható parancsok: "+allcommands());
      }
    }
  }
        public static String allcommands()
        {
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < allowed.Length-1; i++)
            {
                ret.Append(allowed[i]);
                ret.Append(",");
            }
            if(allowed.Length>1)
            {
                ret.Append(allowed[allowed.Length - 1]);
            }
            return ret.ToString();
        }
  public void takedmg() { Hp -= 1; }

  /// <throws>
  /// ArgumentOutOfRangeException => Not enough lines
  /// ArgumentException => what text could not be parsed.
  /// </throws>
  public void parse(String[] inp) {
    validate(inp);
    Orders = inp;
    _ready = true;
  }
}
public class Action {
  public enum ActionType { Move, Turn, Hit, Fire }
        private ActionType _type;
        private Pos _newpos;

        public ActionType Type { get => _type; set => _type = value; }
        public Pos NewPos { get => _newpos; set => _newpos = value; }

        public Action(ActionType t, Pos p) {
    this._type = t;
    this._newpos = p;
  }
}
}
