using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LaunchFromDateSelector {
    public partial class TestForm : Form {
        private int textBoxClicks;
        private Timer textBoxClicksTimer, timer;
        private Point location;

        public TestForm(string[] args) {
            textBoxClicks = 0;
            textBoxClicksTimer = new Timer();

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(Tick);

            InitializeComponent();
            Icon = Properties.Resources.Icon;
            Text = Program.GetTitle() + Constants.NDashWithSpaces + Text;

            textBox.Text = string.Join(" ", args);
            timer.Start();
        }

        private void Tick(object sender, EventArgs e) {
            dateTimePicker2.Value = DateTime.Now;
        }

        private void Close(object sender, EventArgs e) {
            Close();
        }

        private void TextBoxMouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) {
                textBoxClicks = 0;
                return;
            }
            TextBox textBox = (TextBox)sender;
            textBoxClicksTimer.Stop();
            if (textBox.SelectionLength > 0) {
                textBoxClicks = 2;
            } else if (textBoxClicks == 0 || Math.Abs(e.X - location.X) < 2 && Math.Abs(e.Y - location.Y) < 2) {
                textBoxClicks++;
            } else {
                textBoxClicks = 0;
            }
            location = e.Location;
            if (textBoxClicks == 3) {
                if (textBox.Multiline) {
                    int selectionEnd = Math.Max(textBox.SelectionStart + textBox.SelectionLength, Math.Min(textBox.Text.IndexOf('\r', textBox.SelectionStart), textBox.Text.IndexOf('\n', textBox.SelectionStart)));
                    int selectionStart = Math.Min(textBox.SelectionStart, selectionEnd);
                    do {
                        selectionStart--;
                    } while (selectionStart > 0 && textBox.Text[selectionStart] != '\n' && textBox.Text[selectionStart] != '\r');
                    textBox.Select(selectionStart, selectionEnd - selectionStart);
                } else {
                    textBox.SelectAll();
                }
                textBoxClicks = 0;
                MouseEvent(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(Cursor.Position.X), Convert.ToUInt32(Cursor.Position.X), 0, 0);
                textBox.Focus();
            } else {
                textBoxClicksTimer.Interval = SystemInformation.DoubleClickTime;
                textBoxClicksTimer.Start();
                textBoxClicksTimer.Tick += new EventHandler((s, t) => {
                    textBoxClicksTimer.Stop();
                    textBoxClicks = 0;
                });
            }
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                e.SuppressKeyPress = true;
                ((TextBox)sender).SelectAll();
            }
        }

        private void OpenHelp(object sender, HelpEventArgs e) {
            try {
                Process.Start(Properties.Resources.Website.TrimEnd('/').ToLowerInvariant() + '/' + Application.ProductName.ToLowerInvariant() + '/');
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        [DllImport("user32.dll", EntryPoint = "mouse_event", SetLastError = true)]
        private static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
    }
}
