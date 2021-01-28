using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LaunchFromDateSelector {
    public partial class ArgumentParserForm : Form {
        private int textBoxClicks;
        private Timer textBoxClicksTimer;
        private Point location;
        private StatusBarPanel statusBarPanel, statusBarPanelCapsLock, statusBarPanelNumLock, statusBarPanelInsert, statusBarPanelScrollLock;
        private ArgumentParser argumentParser;

        public ArgumentParserForm() {
            Icon = Properties.Resources.Icon;

            textBoxClicks = 0;
            textBoxClicksTimer = new Timer();

            argumentParser = new ArgumentParser();

            InitializeComponent();
            Text = Program.GetTitle() + Constants.NDashWithSpaces + Text;

            statusBarPanel = new StatusBarPanel() {
                BorderStyle = StatusBarPanelBorderStyle.Sunken,
                AutoSize = StatusBarPanelAutoSize.Spring,
                Alignment = HorizontalAlignment.Left
            };
            statusBar.Panels.Add(statusBarPanel);

            statusBarPanelCapsLock = new StatusBarPanel() {
                BorderStyle = StatusBarPanelBorderStyle.Sunken,
                Alignment = HorizontalAlignment.Center,
                Width = 42
            };
            statusBar.Panels.Add(statusBarPanelCapsLock);

            statusBarPanelNumLock = new StatusBarPanel() {
                BorderStyle = StatusBarPanelBorderStyle.Sunken,
                Alignment = HorizontalAlignment.Center,
                Width = 42
            };
            statusBar.Panels.Add(statusBarPanelNumLock);

            statusBarPanelInsert = new StatusBarPanel() {
                BorderStyle = StatusBarPanelBorderStyle.Sunken,
                Alignment = HorizontalAlignment.Center,
                Width = 42
            };
            statusBar.Panels.Add(statusBarPanelInsert);

            statusBarPanelScrollLock = new StatusBarPanel() {
                BorderStyle = StatusBarPanelBorderStyle.Sunken,
                Alignment = HorizontalAlignment.Center,
                Width = 42
            };
            statusBar.Panels.Add(statusBarPanelScrollLock);

            statusBar.ContextMenu = new ContextMenu();
            statusBar.ContextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy, new EventHandler((sender, e) => {
                if (!string.IsNullOrEmpty(statusBarPanel.Text)) {
                    try {
                        Clipboard.SetText(statusBarPanel.Text);
                    } catch (Exception exception) {
                        Debug.WriteLine(exception);
                        ErrorLog.WriteLine(exception);
                    }
                }
            })));
            statusBar.ContextMenu.Popup += new EventHandler((sender, e) => {
                ((ContextMenu)sender).MenuItems[0].Visible = !string.IsNullOrEmpty(statusBarPanel.Text);
            });

            SubscribeEvents();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("/i ");
            stringBuilder.Append(ArgumentParser.EscapeArgument(Constants.ExampleApplicationFilePath));
            stringBuilder.Append(" /d 2019-01-01 /a ");
            stringBuilder.Append(ArgumentParser.EscapeArgument(Constants.ExampleApplicationArguments));
            stringBuilder.Append(" /w ");
            stringBuilder.Append(ArgumentParser.EscapeArgument(Constants.ExampleWorkingFolderPath));
            stringBuilder.Append(" /o /s 5");
            textBox1.Text = stringBuilder.ToString();

            textBoxInput.Text = Constants.ExampleApplicationArguments;
        }

        private void SetValues() {
            SetTextBoxes();
            SetCheckBoxes();
        }

        private void SetTextBoxes() {
            textBox2.Text = argumentParser.ApplicationFilePath;
            textBox3.Text = argumentParser.DateTime.HasValue ? argumentParser.DateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;
            textBox4.Text = argumentParser.ApplicationArguments;
            textBox5.Text = argumentParser.WorkingFolderPath;
            textBox6.Text = argumentParser.Interval.ToString();
        }

        private void SetCheckBoxes() {
            foreach (Control control in Controls) {
                if (control is CheckBox) {
                    ((CheckBox)control).CheckedChanged -= new EventHandler(OnCheckedChanged);
                }
            }
            checkBox1.Checked = argumentParser.OneInstance;
            checkBox2.Checked = argumentParser.IsTest;
            checkBox3.Checked = argumentParser.IsHelp;
            checkBox4.Checked = argumentParser.IsThisTest;
            checkBox5.Checked = argumentParser.HasArguments;
            foreach (Control control in Controls) {
                if (control is CheckBox) {
                    ((CheckBox)control).CheckedChanged += new EventHandler(OnCheckedChanged);
                }
            }
        }

        private void SetStatusBarPanelText(string text) {
            statusBarPanel.Text = text;
        }

        private void EscapeArgument() {
            textBoxOutput.Text = ArgumentParser.EscapeArgument(textBoxInput.Text);
        }

        private static ContextMenu BuildLabelAndCheckBoxContextMenu() {
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy, new EventHandler((sender, e) => {
                try {
                    Clipboard.SetText(((MenuItem)sender).GetContextMenu().SourceControl.Text);
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                }
            })));
            return contextMenu;
        }

        private void SubscribeEvents() {
            foreach (Control control in Controls) {
                if (control is Label) {
                    control.ContextMenu = BuildLabelAndCheckBoxContextMenu();
                } else if (control is TextBox) {
                    TextBox textBox = (TextBox)control;
                    textBox.KeyDown += new KeyEventHandler(KeyDownHandler);
                    textBox.KeyPress += new KeyPressEventHandler(TextBoxKeyPress);
                    textBox.MouseDown += new MouseEventHandler(TextBoxMouseDown);
                } else if (control is CheckBox) {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.ContextMenu = BuildLabelAndCheckBoxContextMenu();
                    checkBox.CheckedChanged += new EventHandler(OnCheckedChanged);
                }
            }
            textBox1.TextChanged += new EventHandler(OnArgumentStringChanged);
            textBoxInput.TextChanged += new EventHandler(OnInputStringChanged);
        }

        private void OnArgumentStringChanged(object sender, EventArgs e) {
            try {
                argumentParser.ArgumentString = ((TextBox)sender).Text;
                SetStatusBarPanelText(Properties.Resources.ButtonOK);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                SetStatusBarPanelText(Properties.Resources.CaptionError + ": " + exception.Message);
            } finally {
                SetValues();
            }
        }

        private void OnInputStringChanged(object sender, EventArgs e) {
            EscapeArgument();
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

        private void KeyDownHandler(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                Close();
            } else if (e.Control && e.KeyCode == Keys.A && sender is TextBox) {
                ((TextBox)sender).SelectAll();
            }
        }

        private void OnCheckedChanged(object sender, EventArgs e) {
            SetCheckBoxes();
        }

        private void OnFormLoad(object sender, EventArgs e) {
            Application.Idle += new EventHandler(ApplicationIdle);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            Application.Idle -= new EventHandler(ApplicationIdle);
        }

        private void ApplicationIdle(object sender, EventArgs e) {
            statusBarPanelCapsLock.Text = IsKeyLocked(Keys.CapsLock) ? Properties.Resources.CaptionCapsLock : string.Empty;
            statusBarPanelNumLock.Text = IsKeyLocked(Keys.NumLock) ? Properties.Resources.CaptionNumLock : string.Empty;
            statusBarPanelInsert.Text = IsKeyLocked(Keys.Insert) ? Properties.Resources.CaptionOverWrite : Properties.Resources.CaptionInsert;
            statusBarPanelScrollLock.Text = IsKeyLocked(Keys.Scroll) ? Properties.Resources.CaptionScrollLock : string.Empty;
        }

        private static void TextBoxKeyPress(object sender, KeyPressEventArgs e) {
            TextBox textBox = (TextBox)sender;
            if (IsKeyLocked(Keys.Insert) && !char.IsControl(e.KeyChar) && !textBox.ReadOnly && textBox.SelectionLength == 0 && textBox.SelectionStart < textBox.TextLength) {
                int selectionStart = textBox.SelectionStart;
                StringBuilder stringBuilder = new StringBuilder(textBox.Text);
                stringBuilder[textBox.SelectionStart] = e.KeyChar;
                e.Handled = true;
                textBox.Text = stringBuilder.ToString();
                textBox.SelectionStart = selectionStart + 1;
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
