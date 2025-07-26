namespace Template
{
    public class Singleton<I, T> where T : I, new() where I : class
    {
        private static I instance = null;
        public static I Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    (instance as Singleton<I, T>).Init();
                }
                return instance;
            }
        }
        protected static bool _initDone;

        public void Init()
        {
            if (_initDone) return;

            _initDone = true;
            OnInit();
        }

        protected virtual void OnInit() { }
    }
}

