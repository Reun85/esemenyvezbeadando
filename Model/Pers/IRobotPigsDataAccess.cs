namespace RobotPigs.Persistence
{
    public interface IRobotPigsDataAccess
    {
        Task<Board> LoadAsync(string path);

        Task SaveAsync(string path, Board table);
    }
}