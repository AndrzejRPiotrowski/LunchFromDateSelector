using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LaunchFromDateSelector {
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args) {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
                MessageBox.Show(Properties.Resources.MessageApplicationCannotRun, GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Settings settings = new Settings();
            if (!settings.DisableThemes) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                settings.RenderWithVisualStyles = Application.RenderWithVisualStyles;
            }
            ArgumentParser argumentParser = new ArgumentParser();
            try {
                argumentParser.Arguments = args;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                MessageBox.Show(exception.Message, GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (argumentParser.HasArguments) {
                if (argumentParser.IsHelp) {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine1.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine2.Replace("\\t", "\t"));
                    stringBuilder.AppendLine(Properties.Resources.HelpLine3.Replace("\\t", "\t"));
                    stringBuilder.AppendLine(Properties.Resources.HelpLine4.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine5.Replace("\\t", "\t"));
                    stringBuilder.AppendLine(Properties.Resources.HelpLine6.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine7.Replace("\\t", "\t"));
                    stringBuilder.AppendLine(Properties.Resources.HelpLine8.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine9.Replace("\\t", "\t"));
                    stringBuilder.AppendLine(Properties.Resources.HelpLine10.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine11.Replace("\\t", "\t"));
                    stringBuilder.AppendLine(Properties.Resources.HelpLine12.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine13.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine14.Replace("\\t", "\t"));
                    stringBuilder.AppendLine(Properties.Resources.HelpLine15.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine16.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine17.Replace("\\t", "\t")).AppendLine();
                    stringBuilder.AppendLine(Properties.Resources.HelpLine18.Replace("\\t", "\t"));
                    MessageBox.Show(stringBuilder.ToString(), GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionHelp, MessageBoxButtons.OK, MessageBoxIcon.Question);
                } else if (argumentParser.IsTest) {
                    try {
                        Application.Run(new TestForm(args));
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                        MessageBox.Show(exception.Message, GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(Properties.Resources.MessageApplicationError, GetTitle(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                } else if (argumentParser.IsThisTest) {
                    try {
                        Application.Run(new ArgumentParserForm());
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                        MessageBox.Show(exception.Message, GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(Properties.Resources.MessageApplicationError, GetTitle(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                } else {
                    try {
                        LaunchFromDateSelector launcherAsDate = new LaunchFromDateSelector() {
                            ApplicationFilePath = argumentParser.ApplicationFilePath,
                            DateTime = argumentParser.DateTime.Value,
                            Arguments = argumentParser.ApplicationArguments,
                            WorkingFolderPath = argumentParser.WorkingFolderPath,
                            OneInstance = argumentParser.OneInstance,
                            Interval = argumentParser.Interval,
                            DisableTimeCorrection = settings.DisableTimeCorrection,
                            ForceTimeCorrection = settings.ForceTimeCorrection
                        };
                        launcherAsDate.Launch();
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                        MessageBox.Show(exception.Message, GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            } else {
                try {
                    SingleMainForm.Run(new MainForm(settings));
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    MessageBox.Show(exception.Message, GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(Properties.Resources.MessageApplicationError, GetTitle(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static string GetTitle() {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            string title = null;
            if (attributes.Length > 0) {
                AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)attributes[0];
                title = assemblyTitleAttribute.Title;
            }
            return string.IsNullOrEmpty(title) ? Application.ProductName : title;
        }

        public static bool IsDebugging {
            get {
                bool isDebugging = false;
                Debugging(ref isDebugging);
                return isDebugging;
            }
        }

        [Conditional("DEBUG")]
        private static void Debugging(ref bool isDebugging) {
            isDebugging = true;
        }
    }
}
