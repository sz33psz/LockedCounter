using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LockedCounter
{
    public class TimeCounter: VMBase
    {
        private Timer timer;

        private TimeSpan _unlockedTime;

        public TimeSpan UnlockedTime
        {
            get { return _unlockedTime; }
            set {
                _unlockedTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _lockedTime;

        public TimeSpan LockedTime
        {
            get { return _lockedTime; }
            set {
                _lockedTime = value;
                OnPropertyChanged();
            }
        }

    }
}
