namespace RobotPigs {
public interface Repr {
  void Loses(object? s, Pig p);
  void PosChangePlr1(object? s, Pig p);
  void PosChangePlr2(object? s, Pig p);
  // Purely visual
  void fire(object? s, Pig p);
  // Purely visual
  void hit(object? s, Pig p);
  void HpChangePlr1(object? s, Pig p);
  void HpChangePlr2(object? s, Pig p);
}
}
