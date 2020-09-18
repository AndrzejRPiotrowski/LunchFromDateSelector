namespace LaunchAsDate {
    public static class Constants {
        public const int IntervalDefault = 5;
        public const int IntervalMinimum = 1;
        public const int IntervalMaximum = 60;
        public const int SpanDefault = -1;
        public const int SpanMinimum = -999999;
        public const int SpanMaximum = 999999;

        public const string ExtensionExe = ".exe";
        public const string ExtensionLnk = ".lnk";

        public const string ErrorLog = "error.log";

        public const string ErrorLogTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public const string NDashWithSpaces = " – ";

        public const string ExampleApplicationFilePath = "C:\\Program Files\\Example Application\\example.exe";
        public const string ExampleWorkingFolderPath = "C:\\Program Files\\Example Application";
        public const string ExampleApplicationArguments = "msiexec /i \"C:\\Program Files\\Example Application\\example.msi\" INSTALLLEVEL=3 /l* msi.log PROPERTY=\"Embedded \"\"Quotes\"\" White Space\"";
    }
}
