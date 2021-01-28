using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace LaunchFromDateSelector {
    public class LauncherAsDate {
        private string timeZoneInformationKey, applicationFilePath, arguments, workingFolderPath, mutexId;
        private bool oneInstance, disableTimeCorrection, forceTimeCorrection;
        private int interval;
        private Mutex mutex;
        private Process process;
        private DateTime dateTime, currentDateTime;

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        private static extern void GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        private static extern bool SetSystemTime(ref SystemTime sysTime);

        public LauncherAsDate() {
            timeZoneInformationKey = @"SYSTEM\CurrentControlSet\Control\TimeZoneInformation";
            mutexId = Path.Combine("Local", Application.CompanyName + "_" + Application.ProductName);
            process = new Process();
        }

        public void Launch() {
            if (oneInstance) {
                if (FocusApplication.SwitchToRunningInstance(applicationFilePath)) {
                    return;
                }
            }
            bool createdNew = false;
            mutex = new Mutex(true, mutexId, out createdNew);
            if (!createdNew) {
                return;
            }
            process.StartInfo.FileName = applicationFilePath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = workingFolderPath;
            currentDateTime = GetSystemTime().AddSeconds(interval);
            if (!disableTimeCorrection) {
                if (forceTimeCorrection || !IsRealTimeUniversal() && !IsDynamicDaylightTimeDisabled()) {
                    dateTime = dateTime.Add(TimeZone.CurrentTimeZone.GetUtcOffset(currentDateTime)).Subtract(TimeZone.CurrentTimeZone.GetUtcOffset(dateTime));
                }
            }
            if (currentDateTime.Year != dateTime.Year || currentDateTime.Month != dateTime.Month || currentDateTime.Day != dateTime.Day) {
                SetSystemTime(dateTime);
                process.Start();
                Thread.Sleep(interval * 1000);
                SetSystemTime(currentDateTime);
            } else {
                process.Start();
            }
        }

        public static DateTime GetSystemTime() {
            SystemTime systemTime = new SystemTime();
            GetSystemTime(ref systemTime);
            return new DateTime(systemTime.Year, systemTime.Month, systemTime.Day, systemTime.Hour, systemTime.Minute, systemTime.Second);
        }

        private static void SetSystemTime(DateTime dateTime) {
            SystemTime systemTime = new SystemTime() {
                Year = (ushort)dateTime.Year,
                Month = (ushort)dateTime.Month,
                Day = (ushort)dateTime.Day,
                Hour = (ushort)dateTime.Hour,
                Minute = (ushort)dateTime.Minute,
                Second = (ushort)dateTime.Second
            };
            SetSystemTime(ref systemTime);
        }

        public string ApplicationFilePath {
            get {
                return applicationFilePath;
            }
            set {
                applicationFilePath = value;
            }
        }

        public DateTime DateTime {
            get {
                return dateTime;
            }
            set {
                dateTime = value;
            }
        }

        public string Arguments {
            get {
                return arguments;
            }
            set {
                arguments = value;
            }
        }

        public string WorkingFolderPath {
            get {
                return workingFolderPath;
            }
            set {
                workingFolderPath = value;
            }
        }

        public bool OneInstance {
            get {
                return oneInstance;
            }
            set {
                oneInstance = value;
            }
        }

        public int Interval {
            get {
                return interval;
            }
            set {
                interval = value;
            }
        }

        public bool DisableTimeCorrection {
            get {
                return disableTimeCorrection;
            }
            set {
                disableTimeCorrection = value;
            }
        }

        public bool ForceTimeCorrection {
            get {
                return forceTimeCorrection;
            }
            set {
                forceTimeCorrection = value;
            }
        }

        private bool IsRealTimeUniversal() {
            RegistryKey registryKey = null;
            int value = 0;
            try {
                registryKey = Registry.LocalMachine.OpenSubKey(timeZoneInformationKey);
                value = (int)registryKey.GetValue("RealTimeIsUniversal", 0);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return value != 0;
        }

        private bool IsDynamicDaylightTimeDisabled() {
            RegistryKey registryKey = null;
            int value = 0;
            try {
                registryKey = Registry.LocalMachine.OpenSubKey(timeZoneInformationKey);
                value = (int)registryKey.GetValue("DynamicDaylightTimeDisabled", 0);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            return value != 0;
        }

        private struct SystemTime {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;
        }
    }
}
