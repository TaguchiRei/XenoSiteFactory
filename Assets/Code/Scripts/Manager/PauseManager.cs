using System.Collections.Generic;
using Interface;

namespace Manager
{
    public class PauseManager : ManagerBase<PauseManager>
    {
        private List<IPauseable> _pausables = new();

        public void AddPauseObject(IPauseable pausable)
        {
            _pausables.Add(pausable);
        }

        public void Pause()
        {
            foreach (var pausable in _pausables)
            {
                pausable.Pause();
            }
        }

        public void Resume()
        {
            foreach (var pausable in _pausables)
            {
                pausable.Resume();
            }
        }

        public override void Initialize()
        {
            
        }

        private void Awake()
        {
            Register();
        }
    }
}
