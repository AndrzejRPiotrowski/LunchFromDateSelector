using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LaunchAsDate {
    public partial class MessageForm : Form {
        private const int SC_CLOSE = 0xF060;
        public const int defaultWidth = 420;

        private Form parent;
        private string text, caption;
        private Buttons buttons;
        private BoxIcon icon;
        private DefaultButton defaultButton;
        private int maxWidth;
        private bool noWrap;
        private ClickedButton clickedButton;

        public MessageForm(string text) : this(null, text, null, Buttons.OK, BoxIcon.None, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(Form parent, string text) : this(parent, text, null, Buttons.OK, BoxIcon.None, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(string text, string caption) : this(null, text, caption, Buttons.OK, BoxIcon.None, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(Form parent, string text, string caption) : this(parent, text, caption, Buttons.OK, BoxIcon.None, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(string text, string caption, Buttons buttons) : this(null, text, caption, buttons, BoxIcon.None, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(Form parent, string text, string caption, Buttons buttons) : this(parent, text, caption, buttons, BoxIcon.None, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(string text, string caption, Buttons buttons, BoxIcon icon) : this(null, text, caption, buttons, icon, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(Form parent, string text, string caption, Buttons buttons, BoxIcon icon) : this(parent, text, caption, buttons, icon, DefaultButton.Button1, false, false, 0, false) { }

        public MessageForm(string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton) : this(null, text, caption, buttons, icon, defaultButton, false, false, 0, false) { }

        public MessageForm(Form parent, string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton) : this(parent, text, caption, buttons, icon, defaultButton, false, false, 0, false) { }

        public MessageForm(string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen) : this(null, text, caption, buttons, icon, defaultButton, centerScreen, false, 0, false) { }

        public MessageForm(Form parent, string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen) : this(parent, text, caption, buttons, icon, defaultButton, centerScreen, false, 0, false) { }

        public MessageForm(string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen, bool displayHelpButton) : this(null, text, caption, buttons, icon, defaultButton, centerScreen, displayHelpButton, 0, false) { }

        public MessageForm(Form parent, string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen, bool displayHelpButton) : this(parent, text, caption, buttons, icon, defaultButton, centerScreen, displayHelpButton, 0, false) { }

        public MessageForm(string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen, bool displayHelpButton, int maxWidth) : this(null, text, caption, buttons, icon, defaultButton, centerScreen, displayHelpButton, maxWidth, false) { }

        public MessageForm(Form parent, string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen, bool displayHelpButton, int maxWidth) : this(parent, text, caption, buttons, icon, defaultButton, centerScreen, displayHelpButton, maxWidth, false) { }

        public MessageForm(string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen, bool displayHelpButton, int maxWidth, bool noWrap) : this(null, text, caption, buttons, icon, defaultButton, centerScreen, displayHelpButton, maxWidth, noWrap) { }

        public MessageForm(Form parent, string text, string caption, Buttons buttons, BoxIcon icon, DefaultButton defaultButton, bool centerScreen, bool displayHelpButton, int maxWidth, bool noWrap) {
            this.parent = parent;
            this.text = text;
            this.caption = caption;
            this.buttons = buttons;
            this.icon = icon;
            this.defaultButton = defaultButton;
            StartPosition = centerScreen ? FormStartPosition.CenterScreen : FormStartPosition.CenterParent;
            HelpButton = displayHelpButton;
            this.maxWidth = (maxWidth > 0 ? maxWidth : defaultWidth) - 20;
            this.noWrap = noWrap;

            InitializeComponent();

            SetCaption();
            SetButtons();
            SetIcon();
            SetText();

            label.ContextMenu = new ContextMenu();
            label.ContextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopy, new EventHandler(Copy)));
        }

        private void SetCaption() {
            Text = string.IsNullOrWhiteSpace(caption) ? Program.GetTitle() : caption.Trim();
        }

        private void SetText() {
            if (string.IsNullOrEmpty(text)) {
                return;
            }
            List<string> lines = new List<string>();
            StringReader stringReader = new StringReader(text);
            for (string line; (line = stringReader.ReadLine()) != null;) {
                string[] words = line.Split(' ');
                StringBuilder stringBuilder = new StringBuilder();
                foreach (string word in words) {
                    if (stringBuilder.Length == 0) {
                        stringBuilder.Append(word);
                    } else if (TextRenderer.MeasureText(stringBuilder.ToString() + " " + word, label.Font).Width <= (icon == BoxIcon.None ? maxWidth : maxWidth - 50) || noWrap) {
                        stringBuilder.Append(" " + word);
                    } else {
                        lines.Add(stringBuilder.ToString());
                        stringBuilder = new StringBuilder();
                        stringBuilder.Append(word);
                    }
                }
                lines.Add(stringBuilder.ToString());
            }
            if (lines.Count == 2) {
                label.Location = new Point(label.Left, 18);
            } else if (lines.Count > 2) {
                label.Location = new Point(label.Left, 12);
            } else {
                label.Location = new Point(label.Left, 25);
            }
            label.Text = string.Join(Environment.NewLine, lines);
        }

        private void SetButtons() {
            switch (buttons) {
                case Buttons.OKCancel:
                    MinimumSize = new Size(196, 136);
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Text = Properties.Resources.ButtonOK;
                    button4.Text = Properties.Resources.ButtonCancel;
                    button3.DialogResult = DialogResult.OK;
                    button4.DialogResult = DialogResult.Cancel;
                    CancelButton = button4;
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        if (DialogResult == DialogResult.OK) {
                            clickedButton = ClickedButton.Button1;
                        } else {
                            clickedButton = ClickedButton.Button2;
                        }
                    });
                    break;
                case Buttons.AbortRetryIgnore:
                    MinimumSize = new Size(277, 136);
                    button1.Visible = false;
                    button2.Text = Properties.Resources.ButtonAbort;
                    button3.Text = Properties.Resources.ButtonRetry;
                    button4.Text = Properties.Resources.ButtonIgnore;
                    button2.DialogResult = DialogResult.Abort;
                    button3.DialogResult = DialogResult.Retry;
                    button4.DialogResult = DialogResult.Ignore;
                    DisableCloseButton();
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        if (DialogResult == DialogResult.Abort) {
                            clickedButton = ClickedButton.Button1;
                        } else if (DialogResult == DialogResult.Retry) {
                            clickedButton = ClickedButton.Button2;
                        } else if (DialogResult == DialogResult.Ignore) {
                            clickedButton = ClickedButton.Button3;
                        }
                    });
                    break;
                case Buttons.YesNoCancel:
                    MinimumSize = new Size(277, 136);
                    button1.Visible = false;
                    button2.Text = Properties.Resources.ButtonYes;
                    button3.Text = Properties.Resources.ButtonNo;
                    button4.Text = Properties.Resources.ButtonCancel;
                    button2.DialogResult = DialogResult.Yes;
                    button3.DialogResult = DialogResult.No;
                    button4.DialogResult = DialogResult.Cancel;
                    CancelButton = button4;
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        if (DialogResult == DialogResult.Yes) {
                            clickedButton = ClickedButton.Button1;
                        } else if (DialogResult == DialogResult.No) {
                            clickedButton = ClickedButton.Button2;
                        } else {
                            clickedButton = ClickedButton.Button3;
                        }
                    });
                    break;
                case Buttons.YesNo:
                    MinimumSize = new Size(196, 136);
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Text = Properties.Resources.ButtonYes;
                    button4.Text = Properties.Resources.ButtonNo;
                    button3.DialogResult = DialogResult.Yes;
                    button4.DialogResult = DialogResult.No;
                    DisableCloseButton();
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        if (DialogResult == DialogResult.Yes) {
                            clickedButton = ClickedButton.Button1;
                        } else if (DialogResult == DialogResult.No) {
                            clickedButton = ClickedButton.Button2;
                        }
                    });
                    break;
                case Buttons.RetryCancel:
                    MinimumSize = new Size(196, 136);
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Text = Properties.Resources.ButtonRetry;
                    button4.Text = Properties.Resources.ButtonCancel;
                    button3.DialogResult = DialogResult.Retry;
                    button4.DialogResult = DialogResult.Cancel;
                    CancelButton = button4;
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        if (DialogResult == DialogResult.Retry) {
                            clickedButton = ClickedButton.Button1;
                        } else {
                            clickedButton = ClickedButton.Button2;
                        }
                    });
                    break;
                case Buttons.YesAllNoCancel:
                    MinimumSize = new Size(358, 136);
                    button1.Text = Properties.Resources.ButtonYes;
                    button2.Text = Properties.Resources.ButtonYesAll;
                    button3.Text = Properties.Resources.ButtonNo;
                    button4.Text = Properties.Resources.ButtonCancel;
                    button1.DialogResult = DialogResult.Yes;
                    button2.DialogResult = DialogResult.Yes;
                    button3.DialogResult = DialogResult.No;
                    button4.DialogResult = DialogResult.Cancel;
                    CancelButton = button4;
                    button1.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button1;
                    });
                    button2.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button2;
                    });
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        if (DialogResult == DialogResult.No) {
                            clickedButton = ClickedButton.Button3;
                        } else if (DialogResult == DialogResult.Cancel) {
                            clickedButton = ClickedButton.Button4;
                        }
                    });
                    break;
                case Buttons.DeleteAllSkipCancel:
                    MinimumSize = new Size(358, 136);
                    button1.Text = Properties.Resources.ButtonDelete;
                    button2.Text = Properties.Resources.ButtonAll;
                    button3.Text = Properties.Resources.ButtonSkip;
                    button4.Text = Properties.Resources.ButtonCancel;
                    button1.DialogResult = DialogResult.Yes;
                    button2.DialogResult = DialogResult.Yes;
                    button3.DialogResult = DialogResult.No;
                    button4.DialogResult = DialogResult.Cancel;
                    CancelButton = button4;
                    button1.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button1;
                    });
                    button2.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button2;
                    });
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        if (DialogResult == DialogResult.No) {
                            clickedButton = ClickedButton.Button3;
                        } else if (DialogResult == DialogResult.Cancel) {
                            clickedButton = ClickedButton.Button4;
                        }
                    });
                    break;
                case Buttons.YesAllNoAll:
                    MinimumSize = new Size(358, 136);
                    button1.Text = Properties.Resources.ButtonYes;
                    button2.Text = Properties.Resources.ButtonYesAll;
                    button3.Text = Properties.Resources.ButtonNo;
                    button4.Text = Properties.Resources.ButtonNoAll;
                    button1.DialogResult = DialogResult.Yes;
                    button2.DialogResult = DialogResult.Yes;
                    button3.DialogResult = DialogResult.No;
                    button4.DialogResult = DialogResult.No;
                    DisableCloseButton();
                    button1.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button1;
                    });
                    button2.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button2;
                    });
                    button3.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button3;
                    });
                    button4.Click += new EventHandler((sender, e) => {
                        clickedButton = ClickedButton.Button4;
                    });
                    break;
                default:
                    MinimumSize = new Size(146, 136);
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Visible = false;
                    button4.Text = Properties.Resources.ButtonOK;
                    button4.DialogResult = DialogResult.OK;
                    button4.KeyDown += new KeyEventHandler((sender, e) => {
                        if (e.KeyCode == Keys.Escape) {
                            Close();
                        }
                    });
                    FormClosed += new FormClosedEventHandler((sender, e) => {
                        DialogResult = DialogResult.OK;
                        clickedButton = ClickedButton.Button1;
                    });
                    break;
            }
        }

        private void SetIcon() {
            switch (icon) {
                case BoxIcon.Hand:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Hand.ToBitmap();
                    break;
                case BoxIcon.Stop:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Hand.ToBitmap();
                    break;
                case BoxIcon.Error:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Error.ToBitmap();
                    break;
                case BoxIcon.Question:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Question.ToBitmap();
                    break;
                case BoxIcon.Exclamation:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Exclamation.ToBitmap();
                    break;
                case BoxIcon.Warning:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Warning.ToBitmap();
                    break;
                case BoxIcon.Asterisk:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Asterisk.ToBitmap();
                    break;
                case BoxIcon.Information:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Information.ToBitmap();
                    break;
                case BoxIcon.OK:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = Properties.Resources.OK.ToBitmap();
                    break;
                case BoxIcon.Shield:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Shield.ToBitmap();
                    break;
                case BoxIcon.ShieldError:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = Properties.Resources.ShieldError.ToBitmap();
                    break;
                case BoxIcon.ShieldQuestion:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = Properties.Resources.ShieldQuestion.ToBitmap();
                    break;
                case BoxIcon.ShieldQuestionRed:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = Properties.Resources.ShieldQuestionRed.ToBitmap();
                    break;
                case BoxIcon.ShieldWarning:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = Properties.Resources.ShieldWarning.ToBitmap();
                    break;
                case BoxIcon.ShieldOK:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = Properties.Resources.ShieldOK.ToBitmap();
                    break;
                case BoxIcon.WinLogo:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.WinLogo.ToBitmap();
                    break;
                case BoxIcon.Application:
                    label.Location = new Point(60, label.Top);
                    pictureBox.Image = SystemIcons.Application.ToBitmap();
                    break;
                default:
                    pictureBox.Visible = false;
                    break;
            }
        }

        public ClickedButton MessageBoxClickedButton {
            get {
                return clickedButton;
            }
        }

        private void OnFormLoad(object sender, EventArgs e) {
            if (Location == Point.Empty && (parent == null || !parent.Visible)) {
                Location = new Point((SystemInformation.PrimaryMonitorSize.Width - Width) / 2, (SystemInformation.PrimaryMonitorSize.Height - Height) / 2);
            }
            List<Button> buttons = new List<Button>();
            foreach (Control control in Controls) {
                if (control is Button && control.Visible) {
                    buttons.Add((Button)control);
                }
            }
            int index = buttons.Count - 1 - (int)defaultButton;
            if (index < 0 || index > buttons.Count - 1) {
                index = buttons.Count - 1;
            }
            buttons[index].Select();
            buttons[index].Focus();
        }

        private void Copy(object sender, EventArgs e) {
            try {
                Clipboard.SetText(((Label)((MenuItem)sender).GetContextMenu().SourceControl).Text);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void DisableCloseButton() {
            EnableMenuItem(GetSystemMenu(Handle, false), SC_CLOSE, 1);
        }

        private void EnableCloseButton() {
            EnableMenuItem(GetSystemMenu(Handle, false), SC_CLOSE, 0);
        }

        public enum BoxIcon {
            None, Hand, Stop, Error, Question, Exclamation, Warning, Asterisk, Information, OK, Shield, ShieldError, ShieldQuestion, ShieldQuestionRed, ShieldWarning, ShieldOK, WinLogo, Application
        }

        public enum Buttons {
            OK, OKCancel, AbortRetryIgnore, YesNoCancel, YesNo, RetryCancel, YesAllNoCancel, DeleteAllSkipCancel, YesAllNoAll
        }

        public enum DefaultButton {
            Button1, Button2, Button3, Button4
        }

        public enum ClickedButton {
            Button1, Button2, Button3, Button4
        }

        [DllImport("user32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint itemId, uint uEnable);
    }
}
