namespace RobotPigs.Pers
{
    public interface IRobotPigsDataAccess
    {
        Task<Board> LoadAsync(String path);

        Task SaveAsync(String path, Board table);
    }
}