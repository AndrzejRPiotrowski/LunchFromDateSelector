using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LaunchAsDate {
    public partial class MainForm : Form {
        private int textBoxClicks;
        private Timer textBoxClicksTimer;
        private Point location;
        private Process process;
        private Settings settings;
        private string workingFolderPathTemp, shortcutNameTemp, administratorRegPath;
        private Form dialog;

        public MainForm(Settings settings) {
            Text = Program.GetTitle();
            Icon = Properties.Resources.Icon;
            dialog = null;

            textBoxClicks = 0;
            textBoxClicksTimer = new Timer();

            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            openFileDialog.DefaultExt = Constants.ExtensionExe;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            this.settings = settings;
            textBox1.Text = settings.ApplicationFilePath;
            comboBox1.SelectedIndex = settings.DateIndex < 0 || settings.DateIndex > 1 ? 0 : settings.DateIndex;
            if (settings.DateTime >= dateTimePicker.MinDate && settings.DateTime <= dateTimePicker.MaxDate) {
                dateTimePicker.Value = settings.DateTime;
            }
            numericUpDown1.Minimum = Constants.SpanMinimum;
            numericUpDown1.Maximum = Constants.SpanMaximum;
            numericUpDown1.Value = settings.SpanValue < Constants.SpanMinimum || settings.SpanValue > Constants.SpanMaximum || settings.SpanValue == 0 ? Constants.SpanDefault : settings.SpanValue;
            numericUpDown1.Select(0, numericUpDown1.Text.Length);
            comboBox2.SelectedIndex = settings.SpanIndex < 0 || settings.SpanIndex > 2 ? 0 : settings.SpanIndex;
            textBox2.Text = settings.Arguments;
            textBox3.Text = settings.WorkingFolderPath;
            numericUpDown2.Minimum = Constants.IntervalMinimum;
            numericUpDown2.Maximum = Constants.IntervalMaximum;
            numericUpDown2.Value = settings.Interval < Constants.IntervalMinimum || settings.Interval > Constants.IntervalMaximum ? Constants.IntervalDefault : settings.Interval;
            numericUpDown2.Select(0, numericUpDown2.Text.Length);
            textBox4.Text = settings.ShortcutName;
            checkBox.Checked = settings.OneInstance;
        }

        private void SelectApplication(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(textBox1.Text)) {
                    string directoryPath = Path.GetDirectoryName(textBox1.Text);
                    if (Directory.Exists(directoryPath)) {
                        openFileDialog.InitialDirectory = directoryPath;
                    }
                    if (File.Exists(textBox1.Text)) {
                        openFileDialog.FileName = Path.GetFileName(textBox1.Text);
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    textBox1.Text = openFileDialog.FileName;
                    if (string.IsNullOrWhiteSpace(textBox3.Text) || !Directory.Exists(textBox3.Text) || textBox3.Text == workingFolderPathTemp) {
                        textBox3.Text = Path.GetDirectoryName(textBox1.Text);
                        textBox3.SelectAll();
                        workingFolderPathTemp = textBox3.Text;
                    }
                    if (string.IsNullOrWhiteSpace(textBox4.Text) || textBox4.Text == shortcutNameTemp) {
                        textBox4.Text = Program.GetTitle() + " " + Path.GetFileNameWithoutExtension(textBox1.Text);
                        textBox4.SelectAll();
                        shortcutNameTemp = textBox4.Text;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            } finally {
                textBox1.Focus();
                textBox1.SelectAll();
            }
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

        private void TextBoxKeyPress(object sender, KeyPressEventArgs e) {
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

        private void NumericUpDownKeyPress(object sender, KeyPressEventArgs e) {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            if (IsKeyLocked(Keys.Insert) && char.IsDigit(e.KeyChar) && !numericUpDown.ReadOnly) {
                FieldInfo fieldInfo = numericUpDown.GetType().GetField("upDownEdit", BindingFlags.Instance | BindingFlags.NonPublic);
                TextBox textBox = (TextBox)fieldInfo.GetValue(numericUpDown);
                if (textBox.SelectionLength == 0 && textBox.SelectionStart < textBox.TextLength) {
                    int selectionStart = textBox.SelectionStart;
                    StringBuilder stringBuilder = new StringBuilder(numericUpDown.Text);
                    stringBuilder[textBox.SelectionStart] = e.KeyChar;
                    e.Handled = true;
                    textBox.Text = stringBuilder.ToString();
                    textBox.SelectionStart = selectionStart + 1;
                }
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBox1.SelectedIndex > 0) {
                dateTimePicker.Enabled = false;
                numericUpDown1.Enabled = true;
                comboBox2.Enabled = true;
            } else {
                dateTimePicker.Enabled = true;
                numericUpDown1.Enabled = false;
                comboBox2.Enabled = false;
            }
        }

        private void SelectFolder(object sender, EventArgs e) {
            try {
                if (!string.IsNullOrEmpty(textBox3.Text)) {
                    if (Directory.Exists(textBox3.Text)) {
                        folderBrowserDialog.SelectedPath = textBox3.Text;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
            try {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                    if (textBox3.Text != folderBrowserDialog.SelectedPath) {
                        textBox3.Text = folderBrowserDialog.SelectedPath;
                        workingFolderPathTemp = textBox3.Text;
                    }
                }
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            } finally {
                textBox3.Focus();
                textBox3.SelectAll();
            }
        }

        private void Launch(object sender, EventArgs e) {
            try {
                administratorRegPath = Application.CommonAppDataRegistry.Name;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, Properties.Resources.MessageRunAsAdministrator, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Shield);
                dialog.ShowDialog();
                return;
            }
            try {
                List<string> arguments = BuildArguments();

                if (!settings.WarningOk) {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine(Properties.Resources.MessageLaunchWarning1).AppendLine().AppendLine(Properties.Resources.MessageLaunchWarning2).AppendLine().AppendLine(Properties.Resources.MessageLaunchWarning3);
                    dialog = new MessageForm(this, stringBuilder.ToString(), null, MessageForm.Buttons.YesNo, MessageForm.BoxIcon.Warning, MessageForm.DefaultButton.Button2);
                    if (dialog.ShowDialog() == DialogResult.Yes) {
                        settings.WarningOk = true;
                        SaveSettings();
                    }
                }
                if (settings.WarningOk) {
                    process = new Process();
                    process.StartInfo.FileName = Application.ExecutablePath;
                    process.StartInfo.Arguments = string.Join(" ", arguments);
                    process.StartInfo.WorkingDirectory = Application.StartupPath;
                    process.Start();
                    SaveSettings();
                }
            } catch (ApplicationException exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private List<string> BuildArguments() {
            bool setTestArgument = false;
            List<string> arguments = new List<string>();
            string applicationFilePath = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(applicationFilePath) && !File.Exists(applicationFilePath)) {
                throw new ApplicationException(Properties.Resources.MessageApplicationNotFound);
            }
            if (!string.IsNullOrWhiteSpace(applicationFilePath)) {
                arguments.Add("/i");
                arguments.Add(ArgumentParser.EscapeArgument(applicationFilePath));
            } else if (Program.IsDebugging) {
                arguments.Add("/i");
                arguments.Add(ArgumentParser.EscapeArgument(Application.ExecutablePath));
                setTestArgument = true;
            } else {
                throw new ApplicationException(Properties.Resources.MessageApplicationNotSet);
            }
            if (comboBox1.SelectedIndex > 0) {
                string[] spanUnit = new string[] { "day", "month", "year" };
                if (numericUpDown1.Value > 0) {
                    arguments.Add("/r");
                    arguments.Add("+" + Math.Abs(numericUpDown1.Value).ToString("#") + spanUnit[comboBox2.SelectedIndex]);
                } else if (numericUpDown1.Value < 0) {
                    arguments.Add("/r");
                    arguments.Add("-" + Math.Abs(numericUpDown1.Value).ToString("#") + spanUnit[comboBox2.SelectedIndex]);
                } else {
                    throw new ApplicationException(Properties.Resources.ExceptionMessageZ);
                }
            } else {
                arguments.Add("/d");
                arguments.Add(dateTimePicker.Value.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrWhiteSpace(textBox2.Text)) {
                arguments.Add("/a");
                arguments.Add(ArgumentParser.EscapeArgument(textBox2.Text));
            } else if (setTestArgument) {
                arguments.Add("/a");
                arguments.Add("\"/t\"");
            }
            if (Directory.Exists(textBox3.Text)) {
                arguments.Add("/w");
                arguments.Add(ArgumentParser.EscapeArgument(textBox3.Text));
            }
            if (checkBox.Checked) {
                arguments.Add("/o");
            }
            arguments.Add("/s");
            arguments.Add(numericUpDown2.Value.ToString());
            return arguments;
        }

        private void MainFormDragDrop(object sender, DragEventArgs e) {
            try {
                textBox1.Text = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
                if (string.IsNullOrWhiteSpace(textBox3.Text) || !Directory.Exists(textBox3.Text) || textBox3.Text == workingFolderPathTemp) {
                    textBox3.Text = Path.GetDirectoryName(textBox1.Text);
                    textBox3.SelectAll();
                    workingFolderPathTemp = textBox3.Text;
                }
                if (string.IsNullOrWhiteSpace(textBox4.Text) || textBox4.Text == shortcutNameTemp) {
                    textBox4.Text = Program.GetTitle() + " " + Path.GetFileNameWithoutExtension(textBox1.Text);
                    textBox4.SelectAll();
                    shortcutNameTemp = textBox4.Text;
                }
                textBox1.Focus();
                textBox1.SelectAll();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void MainFormDragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false)) {
                e.Effect = DragDropEffects.All;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Close(object sender, EventArgs e) {
            Close();
        }

        private void ShowAbout(object sender, EventArgs e) {
            dialog = new AboutForm();
            dialog.ShowDialog();
        }

        private void CreateShortcut(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(textBox4.Text)) {
                dialog = new MessageForm(this, Properties.Resources.MessageShortcutNameNotSet, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
                textBox4.Focus();
                textBox4.SelectAll();
                return;
            }
            try {
                string shortcutFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), textBox4.Text);
                if (!shortcutFilePath.EndsWith(Constants.ExtensionLnk, StringComparison.OrdinalIgnoreCase)) {
                    shortcutFilePath += Constants.ExtensionLnk;
                }
                if (File.Exists(shortcutFilePath)) {
                    dialog = new MessageForm(this, Properties.Resources.MessageShortcutAlreadyExists, null, MessageForm.Buttons.YesNo, MessageForm.BoxIcon.Warning, MessageForm.DefaultButton.Button2);
                    if (dialog.ShowDialog() != DialogResult.Yes) {
                        textBox4.Focus();
                        textBox4.SelectAll();
                        return;
                    }
                }
                List<string> arguments = BuildArguments();
                ProgramShortcut programShortcut = new ProgramShortcut() {
                    ShortcutFilePath = shortcutFilePath,
                    TargetPath = Application.ExecutablePath,
                    WorkingFolder = Application.StartupPath,
                    Arguments = string.Join(" ", arguments),
                    IconLocation = textBox1.Text
                };
                programShortcut.Create();
            } catch (ApplicationException exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, null, MessageForm.Buttons.OK, MessageForm.BoxIcon.Exclamation);
                dialog.ShowDialog();
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                e.SuppressKeyPress = true;
                if (sender is TextBox) {
                    ((TextBox)sender).SelectAll();
                } else if (sender is NumericUpDown) {
                    NumericUpDown numericUpDown = (NumericUpDown)sender;
                    numericUpDown.Select(0, numericUpDown.Text.Length);
                }
            }
        }

        private void ValueChanged(object sender, EventArgs e) {
            label6.Text = numericUpDown2.Value > 1 ? Properties.Resources.CaptionSeconds : Properties.Resources.CaptionSecond;
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e) {
            SaveSettings();
        }

        private void FormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OpenHelp(object sender, HelpEventArgs hlpevent) {
            try {
                Process.Start(Properties.Resources.Website.TrimEnd('/').ToLowerInvariant() + '/' + Application.ProductName.ToLowerInvariant() + '/');
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
                dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                dialog.ShowDialog();
            }
        }

        private void SaveSettings() {
            settings.ApplicationFilePath = textBox1.Text;
            settings.DateIndex = comboBox1.SelectedIndex;
            settings.DateTime = dateTimePicker.Value;
            settings.SpanValue = (int)numericUpDown1.Value;
            settings.SpanIndex = comboBox2.SelectedIndex;
            settings.Arguments = textBox2.Text;
            settings.WorkingFolderPath = textBox3.Text;
            settings.Interval = (int)numericUpDown2.Value;
            settings.ShortcutName = textBox4.Text;
            settings.OneInstance = checkBox.Checked;
            settings.Save();
        }

        [DllImport("user32.dll", EntryPoint = "mouse_event", SetLastError = true)]
        private static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

       
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
    }
}
