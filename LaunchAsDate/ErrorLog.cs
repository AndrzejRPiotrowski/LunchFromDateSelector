using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LaunchAsDate {
    public static class ErrorLog {
        public static void WriteLine(Exception exception) {
            try {
                using (StreamWriter streamWriter = File.AppendText(Path.Combine(Application.LocalUserAppDataPath, Constants.ErrorLog))) {
                    StringBuilder stringBuilder = new StringBuilder(DateTime.Now.ToString(Constants.ErrorLogTimeFormat));
                    stringBuilder.Append('\t');
                    stringBuilder.Append(exception.TargetSite.Name);
                    stringBuilder.Append('\t');
                    stringBuilder.Append(exception.GetType().FullName);
                    stringBuilder.Append('\t');
                    stringBuilder.Append(exception.Message);
                    string[] stackTrace = SplitStackTrace(exception.StackTrace);
                    if (stackTrace.Length > 0) {
                        stringBuilder.Append('\t');
                        stringBuilder.Append(stackTrace[stackTrace.Length - 1]);
                    }
                    streamWriter.WriteLine(stringBuilder.ToString());
                }
            } catch (Exception e) {
                Debug.WriteLine(e);
            }
        }

        private static string[] SplitStackTrace(string stackTrace) {
            StringReader stringReader = new StringReader(stackTrace);
            List<string> lines = new List<string>();
            for (string line; (line = stringReader.ReadLine()) != null;) {
                lines.Add(line);
            }
            return lines.ToArray();
        }
    }
}
