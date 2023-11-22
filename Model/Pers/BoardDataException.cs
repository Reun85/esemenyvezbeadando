
namespace RobotPigs.Persistence
{
    public class BoardDataException : IOException
    {

        public BoardDataException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
