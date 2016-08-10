using LockedCounter.Model;
using LockedCounter.Storage;
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
    public class TimeCounter : VMBase
    {
        private static readonly TimeSpan Change = TimeSpan.FromSeconds(1);
        private Timer _timer;
        public BaseRepository<StateDuration> Repository { get; set; }

        private ScreenState _screenState;

        public ScreenState ScreenState
        {
            get { return _screenState; }
            set
            {
                _screenState = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastStateChange;

        public DateTime LastStateChange
        {
            get { return _lastStateChange; }
            set
            {
                _lastStateChange = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _unlockedTime = TimeSpan.Zero;

        public TimeSpan UnlockedTime
        {
            get { return _unlockedTime; }
            set
            {
                _unlockedTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _lockedTime = TimeSpan.Zero;

        public TimeSpan LockedTime
        {
            get { return _lockedTime; }
            set
            {
                _lockedTime = value;
                OnPropertyChanged();
            }
        }

        public void Start()
        {
            ScreenState = ScreenState.Unlocked;
            LastStateChange = DateTime.Now;
            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            _timer.Start();
        }

        public async Task Stop()
        {
            _timer.Stop();
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Dispose();
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            await SaveData(ScreenState);
        }

        public void Reset()
        {
            LockedTime = TimeSpan.Zero;
            UnlockedTime = TimeSpan.Zero;
        }

        private async void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            var lastState = ScreenState;
            switch (e.Reason)
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
            if (ScreenState != lastState)
            {
                await SaveData(lastState);
                LastStateChange = DateTime.Now;
            }
        }

        private async Task SaveData(ScreenState lastState)
        {
            StateDuration toSave = new StateDuration()
            {
                StartTime = LastStateChange,
                EndTime = DateTime.Now,
                State = lastState
            };
            await Repository.Add(toSave);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ScreenState == ScreenState.Unlocked)
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
