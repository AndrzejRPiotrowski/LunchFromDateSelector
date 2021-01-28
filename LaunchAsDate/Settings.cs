using FSTools;
using System;

namespace LaunchFromDateSelector {
    public class Settings {
        private PersistentSettings persistentSettings;

        public Settings() {
            persistentSettings = new PersistentSettings();
            Load();
        }

        public string ApplicationFilePath { get; set; }

        public int DateIndex { get; set; }

        public DateTime DateTime { get; set; }

        public int SpanValue { get; set; }

        public int SpanIndex { get; set; }

        public string Arguments { get; set; }

        public string WorkingFolderPath { get; set; }

        public int Interval { get; set; }

        public string ShortcutName { get; set; }

        public bool OneInstance { get; set; }

        public bool WarningOk { get; set; }

        public bool DisableTimeCorrection { get; set; }

        public bool ForceTimeCorrection { get; set; }

        public bool DisableThemes { get; set; }

        private void Load() {
            ApplicationFilePath = persistentSettings.Load("Path", ApplicationFilePath);
            DateIndex = persistentSettings.Load("DateIndex", DateIndex);
            DateTime dateTime = DateTime.Now;
            DateTime.TryParse(persistentSettings.Load("DateTime", dateTime.ToString("yyyy-MM-dd")), out dateTime);
            DateTime = dateTime;
            SpanValue = persistentSettings.Load("Span", SpanValue);
            SpanIndex = persistentSettings.Load("SpanIndex", SpanIndex);
            Arguments = persistentSettings.Load("Arguments", Arguments);
            WorkingFolderPath = persistentSettings.Load("Folder", WorkingFolderPath);
            Interval = persistentSettings.Load("Interval", Interval);
            ShortcutName = persistentSettings.Load("Shortcut", ShortcutName);
            OneInstance = persistentSettings.Load("OneInstance", OneInstance);
            WarningOk = persistentSettings.Load("WarningOk", WarningOk);
            DisableTimeCorrection = persistentSettings.Load("DisableTimeCorr", DisableTimeCorrection);
            ForceTimeCorrection = persistentSettings.Load("ForceTimeCorr", ForceTimeCorrection);
            DisableThemes = persistentSettings.Load("DisableThemes", DisableThemes);
        }

        public void Save() {
            persistentSettings.Save("Path", ApplicationFilePath);
            persistentSettings.Save("DateIndex", DateIndex);
            persistentSettings.Save("DateTime", IsToday(DateTime) ? "0000-00-00" : DateTime.ToString("yyyy-MM-dd"));
            persistentSettings.Save("Span", SpanValue);
            persistentSettings.Save("SpanIndex", SpanIndex);
            persistentSettings.Save("Arguments", Arguments);
            persistentSettings.Save("Folder", WorkingFolderPath);
            persistentSettings.Save("Interval", Interval);
            persistentSettings.Save("Shortcut", ShortcutName);
            persistentSettings.Save("OneInstance", OneInstance);
            persistentSettings.Save("WarningOk", WarningOk);
            persistentSettings.Save("DisableTimeCorr", DisableTimeCorrection);
            persistentSettings.Save("ForceTimeCorr", ForceTimeCorrection);
            persistentSettings.Save("DisableThemes", DisableThemes);
        }

        public bool RenderWithVisualStyles { get; set; }

        private bool IsToday(DateTime dateTime) {
            return dateTime.Day == DateTime.Now.Day && dateTime.Month == DateTime.Now.Month && dateTime.Year == DateTime.Now.Year;
        }
    }
}
