
using System.Runtime.Serialization;

namespace RobotPigs.Pers
{
    public class BoardDataException : IOException
    {


        public BoardDataException(string? message) : base(message)
        {
        }

        public BoardDataException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}