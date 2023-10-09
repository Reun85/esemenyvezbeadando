namespace RobotPigs.Model
{
    public class EventData
    {
        private Pers.Pig? _p;
        private Pers.Pos? _pos;
        private int _id;

        public Pers.Pig? P
        {
            get => _p;
            private
              set => _p = value;
        }

        public Pers.Pos? Pos
        {
            get => _pos;
            private
              set => _pos = value;
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

        public EventData(Pers.Pig? p, int id)
        {
            _p = p;
            _id = id;
            _pos = p?.Pos;
        }

        public EventData(Pers.Pig? p, int id, Pers.Pos pos)
        {
            _p = p;
            _id = id;
            _pos = pos;
        }
    }
}