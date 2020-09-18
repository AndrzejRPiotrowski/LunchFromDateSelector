using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LaunchAsDate {
    public partial class AboutForm : Form {
        private const int defaultWidth = 420;

        private StringBuilder stringBuilder;
        private Form dialog;

        public AboutForm() {
            InitializeComponent();

            Text = Properties.Resources.CaptionAbout + " " + Program.GetTitle();
            pictureBox.Image = Properties.Resources.Icon.ToBitmap();

            panel1.ContextMenu = new ContextMenu();
            panel1.ContextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyAbout, new EventHandler(CopyAbout)));
            panel2.ContextMenu = new ContextMenu();
            panel2.ContextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyAbout, new EventHandler(CopyAbout)));

            stringBuilder = new StringBuilder();
            stringBuilder.Append(Program.GetTitle());
            stringBuilder.AppendLine();
            StringReader stringReader = new StringReader(Properties.Resources.Description);
            for (string line; (line = stringReader.ReadLine()) != null;) {
                string[] words = line.Split(' ');
                StringBuilder builder = new StringBuilder();
                foreach (string word in words) {
                    if (builder.Length == 0) {
                        builder.Append(word);
                    } else if (TextRenderer.MeasureText(builder.ToString() + " " + word, label1.Font).Width <= defaultWidth - 70) {
                        builder.Append(" " + word);
                    } else {
                        stringBuilder.AppendLine(builder.ToString());
                        builder = new StringBuilder();
                        builder.Append(word);
                    }
                }
                stringBuilder.AppendLine(builder.ToString());
            }
            stringBuilder.AppendLine();
            stringBuilder.Append(Properties.Resources.LabelVersion);
            stringBuilder.Append(' ');
            stringBuilder.Append(Application.ProductVersion);
            stringBuilder.AppendLine();
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length > 0) {
                AssemblyCopyrightAttribute assemblyCopyrightAttribute = (AssemblyCopyrightAttribute)attributes[0];
                stringBuilder.Append(assemblyCopyrightAttribute.Copyright);
                stringBuilder.AppendLine();
            }
            label1.Text = stringBuilder.ToString();
            label2.Text = Properties.Resources.LabelWebsite;

            linkLabel.ContextMenu = new ContextMenu();
            linkLabel.ContextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyLink, new EventHandler(CopyLink)));
            linkLabel.ContextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MenuItemCopyAbout, new EventHandler(CopyAbout)));
            linkLabel.Text = Properties.Resources.Website.TrimEnd('/').ToLowerInvariant() + '/' + Application.ProductName.ToLowerInvariant() + '/';
            toolTip.SetToolTip(linkLabel, Properties.Resources.ToolTipVisit);
            button.Text = Properties.Resources.ButtonClose;
            stringBuilder.AppendLine();
            stringBuilder.Append(label2.Text);
            stringBuilder.Append(' ');
            stringBuilder.Append(linkLabel.Text);
        }

        private void CopyAbout(object sender, EventArgs e) {
            try {
                Clipboard.SetText(stringBuilder.ToString());
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void CopyLink(object sender, EventArgs e) {
            try {
                Clipboard.SetText(((LinkLabel)((MenuItem)sender).GetContextMenu().SourceControl).Text);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                try {
                    Process.Start(((LinkLabel)sender).Text);
                    linkLabel.LinkVisited = true;
                } catch (Exception exception) {
                    Debug.WriteLine(exception);
                    ErrorLog.WriteLine(exception);
                    dialog = new MessageForm(this, exception.Message, Program.GetTitle() + Constants.NDashWithSpaces + Properties.Resources.CaptionError, MessageForm.Buttons.OK, MessageForm.BoxIcon.Error);
                    dialog.ShowDialog();
                }
            }
        }

        private void OnFormActivated(object sender, EventArgs e) {
            if (dialog != null) {
                dialog.Activate();
            }
        }

        private void OnFormLoad(object sender, EventArgs e) {
            linkLabel.Location = new Point(linkLabel.Location.X + label2.Width + 10, linkLabel.Location.Y);
            button.Select();
            button.Focus();
        }
    }
}
