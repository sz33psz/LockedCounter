using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LockedCounter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            Debug.Write(DateTime.Now);
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                Debug.WriteLine("Locked");
            } else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                Debug.WriteLine("Unlocked");
            }
        }
    }
}
