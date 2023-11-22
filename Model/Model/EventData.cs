namespace RobotPigs.Model
{
    public class EventData
    {
        private Persistence.Pig? _p;
        private Persistence.Pos? _newPos;
        private int _id;

        public Persistence.Pig? P
        {
            get => _p;
            private
              set => _p = value;
        }

        public Persistence.Pos? NewPos
        {
            get => _newPos;
            private
              set => _newPos = value;
        }

        /// <summary>
        /// 1 or 2.
        /// </summary>
        public int Id
        {
            get => _id;
            private
              set => _id = value;
        }

        public EventData(Persistence.Pig? p, int id)
        {
            _p = p;
            _id = id;
            _newPos = p?.Pos;
        }

        public EventData(Persistence.Pig? p, int id, Persistence.Pos pos)
        {
            _p = p;
            _id = id;
            _newPos = pos;
        }
    }
}
