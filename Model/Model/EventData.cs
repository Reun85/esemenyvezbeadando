namespace RobotPigs.Model {
public class EventData {
  private Pers.Pig _p;
  private int _id;
  public Pers.Pig P {
    get => _p;
  private
    set => _p = value;
  }
  /// 1 or 2.
  public int Id {
    get => _id;
  private
    set => _id = value;
  }
  public EventData(Pers.Pig p, int id) {
    _p = p;
    _id = id;
  }
}
}
