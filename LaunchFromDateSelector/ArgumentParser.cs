using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LaunchFromDateSelector {
    public class ArgumentParser {
        private List<string> arguments;
        private string argumentString, applicationFilePath, applicationArguments, workingFolderPath;
        private int interval;
        private DateTime? dateTime;
        private bool expectingFilePath, expectingDate, expectingSpan, expectingArguments, expectingInterval, expectingFolderPath, filePathSet, dateSet, spanSet, argumentsSet, intervalSet, folderPathSet, testSet, helpSet, hasArguments, oneInstanceSet, thisTestSet;
        private Regex spanRegex;

        public ArgumentParser() {
            spanRegex = new Regex(@"^([-+])(\d+)(day|days|month|months|year|years)$", RegexOptions.IgnoreCase);
            Reset();
        }

        private void Reset() {
            applicationFilePath = string.Empty;
            applicationArguments = string.Empty;
            workingFolderPath = string.Empty;
            interval = 0;
            dateTime = null;
            expectingFilePath = false;
            expectingDate = false;
            expectingSpan = false;
            expectingArguments = false;
            expectingInterval = false;
            expectingFolderPath = false;
            filePathSet = false;
            dateSet = false;
            spanSet = false;
            argumentsSet = false;
            intervalSet = false;
            folderPathSet = false;
            testSet = false;
            helpSet = false;
            hasArguments = false;
            oneInstanceSet = false;
            thisTestSet = false;
        }

        private void Evaluate() {
            DateTime systemDateTime = LaunchFromDateSelector.GetSystemTime();
            foreach (string arg in arguments) {
                string argument = arg;
                hasArguments = true;
                if (argument == "-i" || argument == "/i") {             //Input file path: Application to launch.
                    if (filePathSet || expectingFilePath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageI);
                    }
                    if (expectingDate || expectingSpan || expectingArguments || expectingInterval || expectingFolderPath || testSet || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingFilePath = true;
                } else if (argument == "-d" || argument == "/d") {      //Absolute date in format yyyy-mm-dd.
                    if (dateSet || expectingDate) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageD);
                    }
                    if (dateTime.HasValue) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageT);
                    }
                    if (expectingFilePath || expectingSpan || expectingArguments || expectingInterval || expectingFolderPath || testSet || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingDate = true;
                } else if (argument == "-r" || argument == "/r") {      //Relative time span in format for example -9day, -1year, +2month.
                    if (spanSet || expectingSpan) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageQ);
                    }
                    if (dateTime.HasValue) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageT);
                    }
                    if (expectingFilePath || expectingDate || expectingArguments || expectingInterval || expectingFolderPath || testSet || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingSpan = true;
                } else if (argument == "-a" || argument == "/a") {      //Arguments passed to the launched application.
                    if (argumentsSet || expectingArguments) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageA);
                    }
                    if (expectingFilePath || expectingDate || expectingSpan || expectingInterval || expectingFolderPath || testSet || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingArguments = true;
                } else if (argument == "-s" || argument == "/s") {      //Interval in seconds to return to the current date.
                    if (intervalSet || expectingInterval) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageS);
                    }
                    if (expectingFilePath || expectingDate || expectingSpan || expectingArguments || expectingFolderPath || testSet || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingInterval = true;
                } else if (argument == "-w" || argument == "/w") {      //Working folder path.
                    if (folderPathSet || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageW);
                    }
                    if (expectingFilePath || expectingDate || expectingSpan || expectingArguments || expectingInterval || testSet || helpSet || thisTestSet) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    expectingFolderPath = true;
                } else if (argument == "-o" || argument == "/o") {      //Allows only one instance.
                    if (oneInstanceSet || testSet || helpSet || thisTestSet || expectingFilePath || expectingDate || expectingSpan || expectingArguments || expectingInterval || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    oneInstanceSet = true;
                } else if (argument == "-t" || argument == "/t") {      //Test mode (will show form with the date launched).
                    if (filePathSet || dateSet || spanSet || argumentsSet || intervalSet || oneInstanceSet || testSet || helpSet || thisTestSet || expectingFilePath || expectingDate || expectingSpan || expectingArguments || expectingInterval || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    testSet = true;
                } else if (argument == "-h" || argument == "/h" || argument == "-?" || argument == "/?") {      //Will show help.
                    if (filePathSet || dateSet || spanSet || argumentsSet || intervalSet || oneInstanceSet || testSet || helpSet || thisTestSet || expectingFilePath || expectingDate || expectingSpan || expectingArguments || expectingInterval || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    helpSet = true;
                } else if (argument == "-T" || argument == "/T") {      //Test mode (ArgumentParser test).
                    if (filePathSet || dateSet || spanSet || argumentsSet || intervalSet || oneInstanceSet || testSet || helpSet || thisTestSet || expectingFilePath || expectingDate || expectingSpan || expectingArguments || expectingInterval || expectingFolderPath) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                    }
                    thisTestSet = true;
                } else if (expectingFilePath) {
                    applicationFilePath = argument;
                    expectingFilePath = false;
                    filePathSet = true;
                } else if (expectingDate) {
                    if (dateTime.HasValue) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageT);
                    }
                    dateTime = System.DateTime.Parse(argument).Add(systemDateTime.TimeOfDay);
                    expectingDate = false;
                    dateSet = true;
                } else if (expectingSpan) {
                    if (dateTime.HasValue) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageT);
                    }
                    if (!spanRegex.IsMatch(argument)) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageF);
                    }
                    string[] span = spanRegex.Split(argument);
                    if (int.Parse(span[2]) == 0) {
                        throw new ApplicationException(Properties.Resources.ExceptionMessageZ);
                    }
                    if (string.Compare(span[3], "year", true) == 0 || string.Compare(span[3], "years", true) == 0) {
                        dateTime = systemDateTime.AddYears(span[1] == "-" ? 0 - int.Parse(span[2]) : int.Parse(span[2]));
                    } else if (string.Compare(span[3], "month", true) == 0 || string.Compare(span[3], "months", true) == 0) {
                        dateTime = systemDateTime.AddMonths(span[1] == "-" ? 0 - int.Parse(span[2]) : int.Parse(span[2]));
                    } else {
                        dateTime = systemDateTime.AddDays(span[1] == "-" ? 0 - double.Parse(span[2]) : double.Parse(span[2]));
                    }
                    expectingSpan = false;
                    spanSet = true;
                } else if (expectingArguments) {
                    applicationArguments = argument;
                    expectingArguments = false;
                    argumentsSet = true;
                } else if (expectingFolderPath) {
                    workingFolderPath = argument;
                    expectingFolderPath = false;
                    folderPathSet = true;
                } else if (expectingInterval) {
                    interval = int.Parse(argument);
                    if (interval < Constants.IntervalMinimum || interval > Constants.IntervalMaximum) {
                        throw new ApplicationException(string.Format(Properties.Resources.ExceptionMessageN, Constants.IntervalMaximum));
                    }
                    expectingInterval = false;
                    intervalSet = true;
                } else if (argument.StartsWith("-") || argument.StartsWith("/")) {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageU);
                } else {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageM);
                }
            }
            if (expectingFilePath || expectingDate || expectingSpan || expectingArguments || expectingInterval || expectingFolderPath) {
                throw new ApplicationException(Properties.Resources.ExceptionMessageM);
            }
            if (hasArguments && !testSet && !helpSet && !thisTestSet) {
                if (!dateTime.HasValue && interval == 0) {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageJ);
                }
                if (!dateTime.HasValue) {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageK);
                }
                if (interval == 0) {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageL);
                }
            }
        }

        public static string EscapeArgument(string argument) {
            argument = Regex.Replace(argument, @"(\\*)" + "\"", @"$1$1\" + "\"");
            return "\"" + Regex.Replace(argument, @"(\\+)$", @"$1$1") + "\"";
        }

        public bool HasArguments {
            get {
                return hasArguments;
            }
        }

        public bool OneInstance {
            get {
                return oneInstanceSet;
            }
        }

        public bool IsTest {
            get {
                return testSet;
            }
        }

        public bool IsHelp {
            get {
                return helpSet;
            }
        }

        public bool IsThisTest {
            get {
                return thisTestSet;
            }
        }

        public string ApplicationFilePath {
            get {
                return applicationFilePath;
            }
        }

        public string WorkingFolderPath {
            get {
                return workingFolderPath;
            }
        }

        public int Interval {
            get {
                return interval;
            }
        }

        public DateTime? DateTime {
            get {
                return dateTime;
            }
        }

        public string ApplicationArguments {
            get {
                return applicationArguments;
            }
        }

        public string[] Arguments {
            get {
                return arguments.ToArray();
            }
            set {
                Reset();
                arguments = new List<string>(value.Length);
                arguments.AddRange(value);
                try {
                    Evaluate();
                } catch (Exception exception) {
                    Reset();
                    throw exception;
                }
            }
        }

        public string ArgumentString {
            get {
                if (string.IsNullOrEmpty(argumentString) && arguments.Count > 0) {
                    return string.Join(" ", arguments);
                }
                return argumentString;
            }
            set {
                Reset();
                argumentString = value;
                arguments = Parse(argumentString);
                try {
                    Evaluate();
                } catch (Exception exception) {
                    Reset();
                    throw exception;
                }
            }
        }

        private static List<string> Parse(string str) {
            List<string> arguments = new List<string>();
            StringBuilder c = new StringBuilder();
            bool e = false, d = false, s = false;
            for (int i = 0; i < str.Length; i++) {
                if (!s) {
                    if (str[i] == ' ') {
                        continue;
                    }
                    d = str[i] == '"';
                    s = true;
                    e = false;
                    if (d) {
                        continue;
                    }
                }
                if (d) {
                    if (str[i] == '\\') {
                        if (i + 1 < str.Length && str[i + 1] == '"') {
                            c.Append(str[++i]);
                        } else {
                            c.Append(str[i]);
                        }
                    } else if (str[i] == '"') {
                        if (i + 1 < str.Length && str[i + 1] == '"') {
                            c.Append(str[++i]);
                        } else {
                            d = false;
                            e = true;
                        }
                    } else {
                        c.Append(str[i]);
                    }
                } else if (s) {
                    if (str[i] == ' ') {
                        s = false;
                        arguments.Add(e ? c.ToString() : c.ToString().TrimEnd(' '));
                        c = new StringBuilder();
                    } else if (!e) {
                        c.Append(str[i]);
                    }
                }
            }
            if (c.Length > 0) {
                arguments.Add(c.ToString());
            }
            return arguments;
        }
    }
}
