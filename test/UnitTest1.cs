using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test;
using RobotPigs;

public class Common {
  public static void PosEq(Pos p2, Pos p1) {

    Assert.AreEqual(p1.x, p2.x);
    Assert.AreEqual(p1.y, p2.y);
  }
  public static void PosEqWithDir(Pos p2, Pos p1) {

    Assert.AreEqual(p1.x, p2.x);
    Assert.AreEqual(p1.y, p2.y);
    Assert.AreEqual(p1.dir, p2.dir);
  }
}

[TestClass]
public class BoardTest {
  [TestMethod]
  public void BasicTest1() {
    Board b = new Board(6); // No errors
  }

  [TestMethod]
  public void PlrPosTest1() {
    Board b = new Board(6); // No errors
    Common.PosEqWithDir(b.Plr1.Pos, new Pos(2, 3, Pos.Dir.East));
    Common.PosEq(b.Plr2.Pos, new Pos(4, 3, Pos.Dir.West));
  }
  [TestMethod]
  public void PlrPosTest2() {
    Board b = new Board(7); // No errors
    Common.PosEq(b.Plr1.Pos, new Pos(2, 3, Pos.Dir.East));
    Common.PosEq(b.Plr2.Pos, new Pos(4, 3, Pos.Dir.West));
  }
  [TestMethod]
  public void PlrPosTest3() {
    Board b = new Board(8); // No errors
    Common.PosEq(b.Plr1.Pos, new Pos(3, 4, Pos.Dir.East));
    Common.PosEq(b.Plr2.Pos, new Pos(5, 4, Pos.Dir.West));
  }
  [TestMethod]
  public void PerformTest1() {
    Board b = new Board(8); // No errors
    String[] inp1 = { "fordulj balra", "fordulj balra", "fordulj balra",
                      "fordulj balra", "fordulj balra" };
    String[] inp2 = { "ütés", "tűz", "előre", "fordulj balra", "ütés" };
    b.Plr1.parse(inp1);
    b.Plr2.parse(inp2);
    b.preparetoperform();
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(3, b.Plr1.Hp);
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(2, b.Plr1.Hp);
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(2, b.Plr1.Hp);
    Common.PosEqWithDir(b.Plr2.Pos, new Pos(4, 4, Pos.Dir.West));
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(2, b.Plr1.Hp);
    Assert.AreEqual(false, b.performnext());
    Assert.AreEqual(1, b.Plr1.Hp);
    Assert.AreEqual(3, b.Plr2.Hp);
  }

  [TestMethod]
  public void PerformTest2() {
    Board b = new Board(8); // No errors
    String[] inp1 = { "fordulj balra", "előre", "hátra", "fordulj balra",
                      "fordulj balra" };
    String[] inp2 = { "ütés", "tűz", "tűz", "tűz", "ütés" };
    b.Plr1.parse(inp1);
    b.Plr2.parse(inp2);
    b.preparetoperform();
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(3, b.Plr1.Hp);
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(3, b.Plr1.Hp);
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(2, b.Plr1.Hp);
    Assert.AreEqual(true, b.performnext());
    Assert.AreEqual(1, b.Plr1.Hp);
    Assert.AreEqual(false, b.performnext());
    Assert.AreEqual(1, b.Plr1.Hp);
    Assert.AreEqual(3, b.Plr2.Hp);
  }
}
[TestClass]
public class PosTest {
  [TestMethod]
  public void AddRelativeDirTest1() {
    Pos.Dir d = Pos.Dir.East;
    Pos.Dir r = Pos.AddRelativeDirections(Pos.MovDir.Forward, d);
    Assert.AreEqual(r, Pos.Dir.East);
  }
  [TestMethod]
  public void AddRelativeDirTest2() {
    Pos.Dir d = Pos.Dir.South;
    Pos.Dir r = Pos.AddRelativeDirections(Pos.MovDir.Right, d);
    Assert.AreEqual(r, Pos.Dir.West);
  }
  [TestMethod]
  public void AddRelativeDirTest3() {
    Pos.Dir d = Pos.Dir.South;
    Pos.Dir r = Pos.AddRelativeDirections(Pos.MovDir.Back, d);
    Assert.AreEqual(r, Pos.Dir.North);
  }

  [TestMethod]
  public void MoveTest1() {
    Board b = new Board(8);
    Pos p = new Pos(5, 6, Pos.Dir.West);
    Pos n = p.move(Pos.MovDir.Left, b);
    Common.PosEq(n, new Pos(5, 7));
  }
  [TestMethod]
  public void MoveTest2() {
    Board b = new Board(8);
    Pos p = new Pos(5, 6, Pos.Dir.East);
    Pos n = p.move(Pos.MovDir.Back, b);
    Common.PosEq(n, new Pos(4, 6));
  }
  [TestMethod]
  public void MoveTest3() {
    Board b = new Board(8);
    Pos p = new Pos(3, 4, Pos.Dir.South);
    Pos n = p.move(Pos.MovDir.Left, b);
    Common.PosEq(n, new Pos(4, 4));
  }
  [TestMethod]
  public void MoveTest4() {
    Board b = new Board(8);
    Pos p = new Pos(3, 4, Pos.Dir.South);
    Pos n = p.move(Pos.MovDir.Forward, b);
    Common.PosEq(n, new Pos(3, 5));
  }
  [TestMethod]
  public void MoveTestHitWallBottom() {
    Board b = new Board(8);
    Pos p = new Pos(5, 7, Pos.Dir.West);
    Pos n = p.move(Pos.MovDir.Left, b);
    Common.PosEq(n, new Pos(5, 7));
  }
  [TestMethod]
  public void MoveTestHitWallRight() {
    Board b = new Board(8);
    Pos p = new Pos(7, 5, Pos.Dir.West);
    Pos n = p.move(Pos.MovDir.Back, b);
    Common.PosEq(n, new Pos(7, 5));
  }
  [TestMethod]
  public void MoveTestHitWallLeft() {
    Board b = new Board(8);
    Pos p = new Pos(0, 7, Pos.Dir.South);
    Pos n = p.move(Pos.MovDir.Right, b);
    Common.PosEq(n, new Pos(0, 7));
  }
  [TestMethod]
  public void MoveTestHitWallTop() {
    Board b = new Board(8);
    Pos p = new Pos(5, 0, Pos.Dir.South);
    Pos n = p.move(Pos.MovDir.Back, b);
    Common.PosEq(n, new Pos(5, 0));
  }

  [TestMethod]
  public void InviewTest1() {
    Pos p = new Pos(5, 0, Pos.Dir.South);
    Pos p2 = new Pos(5, 2);
    Assert.IsTrue(p.inview(p2));
  }
  [TestMethod]
  public void InviewTest2() {
    Pos p = new Pos(5, 3, Pos.Dir.South);
    Pos p2 = new Pos(5, 2);
    Assert.IsFalse(p.inview(p2));
  }
  [TestMethod]
  public void InviewTest3() {
    Pos p = new Pos(5, 3, Pos.Dir.East);
    Pos p2 = new Pos(5, 2);
    Assert.IsFalse(p.inview(p2));
  }
  [TestMethod]
  public void InviewTest4() {
    Pos p = new Pos(5, 3, Pos.Dir.East);
    Pos p2 = new Pos(6, 4);
    Assert.IsFalse(p.inview(p2));
  }
  [TestMethod]
  public void InviewTest5() {
    Pos p = new Pos(5, 3, Pos.Dir.East);
    Pos p2 = new Pos(6, 3);
    Assert.IsTrue(p.inview(p2));
  }
  [TestMethod]
  public void InviewTest6() {
    Pos p = new Pos(5, 3, Pos.Dir.East);
    Pos p2 = new Pos(4, 3);
    Assert.IsFalse(p.inview(p2));
  }
  [TestMethod]
  public void InviewTest7() {
    Pos p = new Pos(5, 3, Pos.Dir.West);
    Pos p2 = new Pos(4, 3);
    Assert.IsTrue(p.inview(p2));
  }
  [TestMethod]
  public void InviewTest8() {
    Pos p = new Pos(5, 3, Pos.Dir.West);
    Pos p2 = new Pos(7, 3);
    Assert.IsFalse(p.inview(p2));
  }
  [TestMethod]
  public void InRadiusTest1() {
    Pos p = new Pos(5, 3);
    Pos p2 = new Pos(7, 3);
    Assert.IsFalse(p.inradius(p2, 1));
    // Assert.IsTrue(p.inradius(p2, 1));
  }
  [TestMethod]
  public void InRadiusTest2() {
    Pos p = new Pos(5, 3);
    Pos p2 = new Pos(6, 3);
    // Assert.IsFalse(p.inradius(p2, 1));
    Assert.IsTrue(p.inradius(p2, 1));
  }
  [TestMethod]
  public void InRadiusTest3() {
    Pos p = new Pos(5, 3);
    Pos p2 = new Pos(4, 3);
    // Assert.IsFalse(p.inradius(p2, 1));
    Assert.IsTrue(p.inradius(p2, 1));
  }
  [TestMethod]
  public void InRadiusTest4() {
    Pos p = new Pos(9, 4);
    Pos p2 = new Pos(7, 3);
    Assert.IsFalse(p.inradius(p2, 1));
    // Assert.IsTrue(p.inradius(p2, 1));
  }
  [TestMethod]
  public void InRadiusTest5() {
    Pos p = new Pos(0, 2);
    Pos p2 = new Pos(0, 1);
    // Assert.IsFalse(p.inradius(p2, 1));
    Assert.IsTrue(p.inradius(p2, 1));
  }
}
[TestClass]
public class PigTest {

  [TestMethod]
  public void ValidateTest1() {
    String[] inp = { "a" };
    try {
      Pig.validate(inp);
      Assert.Fail("Should have failed.");
    } catch (ArgumentOutOfRangeException) {
      // Assert.Fail("Should not have failed. It had enough parameters.");
    } catch (ArgumentException) {
      Assert.Fail("Should not have failed to parse.");
    }
  }
  [TestMethod]
  public void ValidateTest2() {
    String[] inp = { "a", "b" };
    try {
      Pig.validate(inp);
      Assert.Fail("Should have failed.");
    } catch (ArgumentOutOfRangeException) {
      // Assert.Fail("Should not have failed. It had enough parameters.");
    } catch (ArgumentException) {
      Assert.Fail("Should not have failed to parse.");
    }
  }
  [TestMethod]
  public void ValidateTest3() {
    String[] inp = { "a", "b", "c", "d", "e" };
    try {
      Pig.validate(inp);
      Assert.Fail("Should have failed.");
    } catch (ArgumentOutOfRangeException) {
      Assert.Fail("Should not have failed. It had enough parameters.");
    } catch (ArgumentException) {
      // Assert.Fail("Should not have failed to parse.");
    }
  }
  [TestMethod]
  public void ValidateTest4() {
    String[] inp = { "a", "b", "c", "d", "e", "f" };
    try {
      Pig.validate(inp);
      Assert.Fail("Should have failed.");
    } catch (ArgumentOutOfRangeException) {
      // Assert.Fail("Should not have failed. It had enough parameters.");
    } catch (ArgumentException) {
      Assert.Fail("Should not have failed to parse.");
    }
  }
  [TestMethod]
  public void ValidateTest5() {
    String[] inp = { "előre", "hátra", "előre", "hátra", "előre" };
    try {
      Pig.validate(inp);
      // Assert.Fail("Should have failed.");
    } catch (ArgumentOutOfRangeException) {
      Assert.Fail("Should not have failed. It had enough parameters.");
    } catch (ArgumentException) {
      Assert.Fail("Should not have failed to parse.");
    }
  }
  [TestMethod]
  public void ValidateTest6() {
    String[] inp = { "előre", "hátbra", "előre", "hátra", "előre" };
    try {
      Pig.validate(inp);
      Assert.Fail("Should have failed.");
    } catch (ArgumentOutOfRangeException) {
      Assert.Fail("Should not have failed. It had enough parameters.");
    } catch (ArgumentException) {
      // Assert.Fail("Should not have failed to parse.");
    }
  }
  [TestMethod]
  public void ValidateTest7() {
    String[] inp = { "tűz", "fordulj balra", "jobbra", "fordulj balra",
                     "ütés" };
    try {
      Pig.validate(inp);
      // Assert.Fail("Should have failed.");
    } catch (ArgumentOutOfRangeException) {
      Assert.Fail("Should not have failed. It had enough parameters.");
    } catch (ArgumentException) {
      Assert.Fail("Should not have failed to parse.");
    }
  }
}
