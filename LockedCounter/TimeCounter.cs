using LockedCounter.Model;
using Microsoft.Win32;
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
        private static readonly TimeSpan Change = TimeSpan.FromSeconds(1);
        private Timer timer;

        private ScreenState _screenState = ScreenState.Unlocked;

        public ScreenState ScreenState
        {
            get { return _screenState; }
            set { _screenState = value; }
        }


        private TimeSpan _unlockedTime = TimeSpan.Zero;

        public TimeSpan UnlockedTime
        {
            get { return _unlockedTime; }
            set {
                _unlockedTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _lockedTime = TimeSpan.Zero;

        public TimeSpan LockedTime
        {
            get { return _lockedTime; }
            set {
                _lockedTime = value;
                OnPropertyChanged();
            }
        }

        public void Start()
        {
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            timer.Elapsed -= Timer_Elapsed;
            timer.Dispose();
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch(e.Reason)
            {
                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                case SessionSwitchReason.ConsoleConnect:
                case SessionSwitchReason.RemoteConnect:
                    ScreenState = ScreenState.Unlocked;
                    break;
                case SessionSwitchReason.ConsoleDisconnect:
                case SessionSwitchReason.RemoteDisconnect:
                case SessionSwitchReason.SessionLock:
                case SessionSwitchReason.SessionLogoff:
                    ScreenState = ScreenState.Locked;
                    break;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var toChange = (ScreenState == ScreenState.Unlocked ? UnlockedTime : LockedTime);
            if(ScreenState == ScreenState.Unlocked)
            {
                UnlockedTime += Change;
            }
            else
            {
                LockedTime += Change;
            }
        }
    }
}
