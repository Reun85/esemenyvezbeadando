using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test;

using RobotPigs.Model;
using RobotPigs.Pers;

public class Common
{
    /// This is just candy to write out what fails.
    public static void PosEq(Pos p1, Pos p2)
    {
        Assert.AreEqual(p1.x, p2.x);
        Assert.AreEqual(p1.y, p2.y);
    }

    public static void PosEqWithDir(Pos p1, Pos p2)
    {
        Assert.AreEqual(p1.x, p2.x);
        Assert.AreEqual(p1.y, p2.y);
        Assert.AreEqual(p1.dir, p2.dir);
    }
}

[TestClass]
public class BoardTest
{

    [TestMethod]
    public void PlrPosTest1()
    {
        Board b = new Board(6); // No errors
        Common.PosEqWithDir(new Pos(1, 3, Pos.Dir.East), b.Plr1.Pos);
        Common.PosEqWithDir(new Pos(4, 3, Pos.Dir.West), b.Plr2.Pos);
    }

    [TestMethod]
    public void PlrPosTest2()
    {
        Board b = new Board(7); // No errors
        Common.PosEqWithDir(new Pos(2, 3, Pos.Dir.East), b.Plr1.Pos);
        Common.PosEqWithDir(new Pos(4, 3, Pos.Dir.West), b.Plr2.Pos);
    }

    [TestMethod]
    public void PlrPosTest3()
    {
        Board b = new Board(8); // No errors
        Common.PosEqWithDir(new Pos(2, 4, Pos.Dir.East), b.Plr1.Pos);
        Common.PosEqWithDir(new Pos(5, 4, Pos.Dir.West), b.Plr2.Pos);
    }

    [TestMethod]
    public void PerformTest1()
    {
        GameModel m = new GameModel(null); // No errors
        m.NewGame(8);
        String[] inp1 = { "fordulj balra", "fordulj balra", "fordulj balra",
                      "fordulj balra", "fordulj balra" };
        String[] inp2 = { "ütés", "tűz", "előre", "előre", "ütés" };
        m.Board!.Plr1.parse(inp1);
        m.Board!.Plr2.parse(inp2);
        m.PreparetoPerform();
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(3, m.Board!.Plr1.Hp);
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(2, m.Board!.Plr1.Hp);
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(2, m.Board!.Plr1.Hp);
        Common.PosEqWithDir(new Pos(4, 4, Pos.Dir.West), m.Board.Plr2.Pos);
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(2, m.Board!.Plr1.Hp);
        Assert.AreEqual(false, m.PerformNext());
        Assert.AreEqual(1, m.Board!.Plr1.Hp);
        Assert.AreEqual(3, m.Board!.Plr2.Hp);
    }

    [TestMethod]
    public void PerformTest2()
    {
        GameModel m = new GameModel(null); // No errors
        m.NewGame(8);
        String[] inp1 = { "fordulj balra", "előre", "hátra", "fordulj balra",
                      "fordulj balra" };
        String[] inp2 = { "ütés", "tűz", "tűz", "tűz", "ütés" };
        m.Board!.Plr1.parse(inp1);
        m.Board!.Plr2.parse(inp2);
        m.PreparetoPerform();
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(3, m.Board!.Plr1.Hp);
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(3, m.Board!.Plr1.Hp);
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(2, m.Board!.Plr1.Hp);
        Common.PosEqWithDir(new Pos(5, 4, Pos.Dir.West), m.Board.Plr2.Pos);
        Assert.AreEqual(true, m.PerformNext());
        Assert.AreEqual(1, m.Board!.Plr1.Hp);
        Assert.AreEqual(false, m.PerformNext());
        Assert.AreEqual(1, m.Board!.Plr1.Hp);
        Assert.AreEqual(3, m.Board!.Plr2.Hp);
    }
}

[TestClass]
public class PosTest
{
    [TestMethod]
    public void AddRelativeDirTest1()
    {
        Pos.Dir d = Pos.Dir.East;
        Pos.Dir r = Pos.AddRelativeDirections(Pos.MovDir.Forward, d);
        Assert.AreEqual(Pos.Dir.East, r);
    }

    [TestMethod]
    public void AddRelativeDirTest2()
    {
        Pos.Dir d = Pos.Dir.South;
        Pos.Dir r = Pos.AddRelativeDirections(Pos.MovDir.Right, d);
        Assert.AreEqual(Pos.Dir.West, r);
    }

    [TestMethod]
    public void AddRelativeDirTest3()
    {
        Pos.Dir d = Pos.Dir.South;
        Pos.Dir r = Pos.AddRelativeDirections(Pos.MovDir.Back, d);
        Assert.AreEqual(Pos.Dir.North, r);
    }

    [TestMethod]
    public void MoveTest1()
    {
        Board b = new Board(8);
        Pos p = new Pos(5, 6, Pos.Dir.West);
        Pos n = p.move(Pos.MovDir.Left, b.n);
        Common.PosEqWithDir(new Pos(5, 7, Pos.Dir.West), n);
    }

    [TestMethod]
    public void MoveTest2()
    {
        Board b = new Board(8);
        Pos p = new Pos(5, 6, Pos.Dir.East);
        Pos n = p.move(Pos.MovDir.Back, b.n);
        Common.PosEqWithDir(new Pos(4, 6, Pos.Dir.East), n);
    }

    [TestMethod]
    public void MoveTest3()
    {
        Board b = new Board(8);
        Pos p = new Pos(3, 4, Pos.Dir.South);
        Pos n = p.move(Pos.MovDir.Left, b.n);
        Common.PosEqWithDir(new Pos(4, 4, Pos.Dir.South), n);
    }

    [TestMethod]
    public void MoveTest4()
    {
        Board b = new Board(8);
        Pos p = new Pos(3, 4, Pos.Dir.South);
        Pos n = p.move(Pos.MovDir.Forward, b.n);
        Common.PosEqWithDir(new Pos(3, 5, Pos.Dir.South), n);
    }

    [TestMethod]
    public void MoveTestHitWallBottom()
    {
        Board b = new Board(8);
        Pos p = new Pos(5, 7, Pos.Dir.West);
        Pos n = p.move(Pos.MovDir.Left, b.n);
        Common.PosEqWithDir(new Pos(5, 7, Pos.Dir.West), n);
    }

    [TestMethod]
    public void MoveTestHitWallRight()
    {
        Board b = new Board(8);
        Pos p = new Pos(7, 5, Pos.Dir.West);
        Pos n = p.move(Pos.MovDir.Back, b.n);
        Common.PosEqWithDir(new Pos(7, 5, Pos.Dir.West), n);
    }

    [TestMethod]
    public void MoveTestHitWallLeft()
    {
        Board b = new Board(8);
        Pos p = new Pos(0, 7, Pos.Dir.South);
        Pos n = p.move(Pos.MovDir.Right, b.n);
        Common.PosEqWithDir(new Pos(0, 7, Pos.Dir.South), n);
    }

    [TestMethod]
    public void MoveTestHitWallTop()
    {
        Board b = new Board(8);
        Pos p = new Pos(5, 0, Pos.Dir.South);
        Pos n = p.move(Pos.MovDir.Back, b.n);
        Common.PosEqWithDir(new Pos(5, 0, Pos.Dir.South), n);
    }

    [TestMethod]
    public void InviewTest1()
    {
        Pos p = new Pos(5, 0, Pos.Dir.South);
        Pos p2 = new Pos(5, 2);
        Assert.IsTrue(p.inview(p2));
    }

    [TestMethod]
    public void InviewTest2()
    {
        Pos p = new Pos(5, 3, Pos.Dir.South);
        Pos p2 = new Pos(5, 2);
        Assert.IsFalse(p.inview(p2));
    }

    [TestMethod]
    public void InviewTest3()
    {
        Pos p = new Pos(5, 3, Pos.Dir.East);
        Pos p2 = new Pos(5, 2);
        Assert.IsFalse(p.inview(p2));
    }

    [TestMethod]
    public void InviewTest4()
    {
        Pos p = new Pos(5, 3, Pos.Dir.East);
        Pos p2 = new Pos(6, 4);
        Assert.IsFalse(p.inview(p2));
    }

    [TestMethod]
    public void InviewTest5()
    {
        Pos p = new Pos(5, 3, Pos.Dir.East);
        Pos p2 = new Pos(6, 3);
        Assert.IsTrue(p.inview(p2));
    }

    [TestMethod]
    public void InviewTest6()
    {
        Pos p = new Pos(5, 3, Pos.Dir.East);
        Pos p2 = new Pos(4, 3);
        Assert.IsFalse(p.inview(p2));
    }

    [TestMethod]
    public void InviewTest7()
    {
        Pos p = new Pos(5, 3, Pos.Dir.West);
        Pos p2 = new Pos(4, 3);
        Assert.IsTrue(p.inview(p2));
    }

    [TestMethod]
    public void InviewTest8()
    {
        Pos p = new Pos(5, 3, Pos.Dir.West);
        Pos p2 = new Pos(7, 3);
        Assert.IsFalse(p.inview(p2));
    }

    [TestMethod]
    public void InRadiusTest1()
    {
        Pos p = new Pos(5, 3);
        Pos p2 = new Pos(7, 3);
        Assert.IsFalse(p.inradius(p2, 1));
        // Assert.IsTrue(p.inradius(p2, 1));
    }

    [TestMethod]
    public void InRadiusTest2()
    {
        Pos p = new Pos(5, 3);
        Pos p2 = new Pos(6, 3);
        // Assert.IsFalse(p.inradius(p2, 1));
        Assert.IsTrue(p.inradius(p2, 1));
    }

    [TestMethod]
    public void InRadiusTest3()
    {
        Pos p = new Pos(5, 3);
        Pos p2 = new Pos(4, 3);
        // Assert.IsFalse(p.inradius(p2, 1));
        Assert.IsTrue(p.inradius(p2, 1));
    }

    [TestMethod]
    public void InRadiusTest4()
    {
        Pos p = new Pos(9, 4);
        Pos p2 = new Pos(7, 3);
        Assert.IsFalse(p.inradius(p2, 1));
        // Assert.IsTrue(p.inradius(p2, 1));
    }

    [TestMethod]
    public void InRadiusTest5()
    {
        Pos p = new Pos(0, 2);
        Pos p2 = new Pos(0, 1);
        // Assert.IsFalse(p.inradius(p2, 1));
        Assert.IsTrue(p.inradius(p2, 1));
    }
}

[TestClass]
public class PigTest
{
    [TestMethod]
    public void ValidateTest1()
    {
        String[] inp = { "a" };
        try
        {
            Pig.validate(inp);
            Assert.Fail("Should have failed.");
        }
        catch (ArgumentOutOfRangeException)
        {
            // Assert.Fail("Should not have failed. It had enough parameters.");
        }
        catch (ArgumentException)
        {
            Assert.Fail("Should not have failed to parse.");
        }
    }

    [TestMethod]
    public void ValidateTest2()
    {
        String[] inp = { "a", "b" };
        try
        {
            Pig.validate(inp);
            Assert.Fail("Should have failed.");
        }
        catch (ArgumentOutOfRangeException)
        {
            // Assert.Fail("Should not have failed. It had enough parameters.");
        }
        catch (ArgumentException)
        {
            Assert.Fail("Should not have failed to parse.");
        }
    }

    [TestMethod]
    public void ValidateTest3()
    {
        String[] inp = { "a", "b", "c", "d", "e" };
        try
        {
            Pig.validate(inp);
            Assert.Fail("Should have failed.");
        }
        catch (ArgumentOutOfRangeException)
        {
            Assert.Fail("Should not have failed. It had enough parameters.");
        }
        catch (ArgumentException)
        {
            // Assert.Fail("Should not have failed to parse.");
        }
    }

    [TestMethod]
    public void ValidateTest4()
    {
        String[] inp = { "a", "b", "c", "d", "e", "f" };
        try
        {
            Pig.validate(inp);
            Assert.Fail("Should have failed.");
        }
        catch (ArgumentOutOfRangeException)
        {
            // Assert.Fail("Should not have failed. It had enough parameters.");
        }
        catch (ArgumentException)
        {
            Assert.Fail("Should not have failed to parse.");
        }
    }

    [TestMethod]
    public void ValidateTest5()
    {
        String[] inp = { "előre", "hátra", "előre", "hátra", "előre" };
        try
        {
            Pig.validate(inp);
            // Assert.Fail("Should have failed.");
        }
        catch (ArgumentOutOfRangeException)
        {
            Assert.Fail("Should not have failed. It had enough parameters.");
        }
        catch (ArgumentException)
        {
            Assert.Fail("Should not have failed to parse.");
        }
    }

    [TestMethod]
    public void ValidateTest6()
    {
        String[] inp = { "előre", "hátbra", "előre", "hátra", "előre" };
        try
        {
            Pig.validate(inp);
            Assert.Fail("Should have failed.");
        }
        catch (ArgumentOutOfRangeException)
        {
            Assert.Fail("Should not have failed. It had enough parameters.");
        }
        catch (ArgumentException)
        {
            // Assert.Fail("Should not have failed to parse.");
        }
    }

    [TestMethod]
    public void ValidateTest7()
    {
        String[] inp = { "tűz", "fordulj balra", "jobbra", "fordulj balra",
                     "ütés" };
        try
        {
            Pig.validate(inp);
            // Assert.Fail("Should have failed.");
        }
        catch (ArgumentOutOfRangeException)
        {
            Assert.Fail("Should not have failed. It had enough parameters.");
        }
        catch (ArgumentException)
        {
            Assert.Fail("Should not have failed to parse.");
        }
    }
}

[TestClass]
public class RobotDataAccessTest
{
    [TestMethod]
    public void Test1()
    {
        GameModel m = new GameModel(new RobotPigsDataAccess());
        m.NewGame(8);

        m.SaveAsync("Testser").Wait();

        
        
        
        //Make sure
        m = new GameModel(new RobotPigsDataAccess());
        m.LoadGameAsync("Testser").Wait();
        Assert.AreEqual(3, m.Board!.Plr1.Hp);
        Assert.AreEqual(3, m.Board!.Plr2.Hp);
        Common.PosEqWithDir(new Pos(5, 4, Pos.Dir.West), m.Board.Plr2.Pos);
        Common.PosEqWithDir(new Pos(2, 4, Pos.Dir.East), m.Board.Plr1.Pos);
    }

    [TestMethod]
    public void Test2()
    {
        GameModel m = new GameModel(new RobotPigsDataAccess());
        m.NewGame(8);
        String[] inp1 = { "fordulj balra", "előre", "hátra", "fordulj balra",
                      "fordulj balra" };
        String[] inp2 = { "ütés", "tűz", "tűz", "tűz", "előre" };
        m.Board!.Plr1.parse(inp1);
        m.Board!.Plr2.parse(inp2);
        m.PreparetoPerform();
        while (m.PerformNext()) ;
        m.SaveAsync("Testser").Wait();

        // Make sure
        m = new GameModel(new RobotPigsDataAccess());
        m.LoadGameAsync("Testser").Wait();
        Assert.AreEqual(1, m.Board!.Plr1.Hp);
        Assert.AreEqual(3, m.Board!.Plr2.Hp);
        Common.PosEqWithDir(new Pos(4, 4, Pos.Dir.West), m.Board.Plr2.Pos);
        Common.PosEqWithDir(new Pos(2, 4, Pos.Dir.South), m.Board.Plr1.Pos);
    }
}