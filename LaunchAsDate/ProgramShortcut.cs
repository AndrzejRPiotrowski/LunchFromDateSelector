using IWshRuntimeLibrary;

namespace LaunchFromDateSelector {
    public class ProgramShortcut {
        private string shortcutFilePath, targetPath, workingFolderPath, arguments, iconLocation;
        private WshShell wshShell;

        public ProgramShortcut() {
            wshShell = new WshShell();
        }

        public string ShortcutFilePath {
            get {
                return shortcutFilePath;
            }
            set {
                shortcutFilePath = value;
            }
        }

        public string TargetPath {
            get {
                return targetPath;
            }
            set {
                targetPath = value;
            }
        }

        public string WorkingFolder {
            get {
                return workingFolderPath;
            }
            set {
                workingFolderPath = value;
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

        public string IconLocation {
            get {
                return iconLocation;
            }
            set {
                iconLocation = value;
            }
        }

        public void Create() {
            IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(shortcutFilePath);
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = workingFolderPath;
            shortcut.Arguments = arguments;
            shortcut.IconLocation = iconLocation;
            shortcut.Save();
        }
    }
}
