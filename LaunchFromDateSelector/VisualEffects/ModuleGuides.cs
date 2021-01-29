using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.VisualEffects;
using DevExpress.XtraEditors;

namespace DevExpress.ApplicationUI.Demos.VisualEffects {
    public partial class ModuleGuides : TutorialControl {
        GuideFlyoutPanel panel;
        Color saveBackColor, saveForeColor;
        int saveFontSizeDelta;
        string saveText;
        int countLessons;
        public ModuleGuides() {
            InitializeComponent();
            countLessons = 6;
            panel = new GuideFlyoutPanel(this, countLessons);
            adornerUIManager.QueryGuideFlyoutControl += OnQueryGuideFlyoutControl;
            guide1.TargetElement = sample;            
        }
        void OnQueryGuideFlyoutControl(object sender, DevExpress.Utils.VisualEffects.QueryGuideFlyoutControlEventArgs e) {
            e.Control = panel;
        }
        public void StartTutorial() {
            adornerUIManager.ShowGuides = Utils.DefaultBoolean.True;
            saveFontSizeDelta = (int)seFontSizeDelta.Value;
            saveText = teText.Text;
            saveBackColor = cbBackColor.Color;
            saveForeColor = cbForeColor.Color;
            SetLesson(panel.CurrentLessonIndex);
            badge1.Visible = false;
        }
        public void SetLesson(int index) {
            if(index < 0 || index > countLessons - 1) return;
            switch(index) {
                case 0:
                    FirstLesson(); break;
                case 1:
                    SecondLesson(); break;
                case 2:
                    ThirdLesson(); break;
                case 3:
                    FourthLesson(); break;
                case 4:
                    FifthLesson(); break;
                case 5:
                    SixthLesson(); break;
            }
        }
        void ActivateTextPage() {
            textItem.Expanded = true;
            appearanceItem.Expanded = false;
            tabNavigation.SelectedPage = settingsNavigationPage;
        }
        void ActivateAppearancePage() {
            textItem.Expanded = false;
            appearanceItem.Expanded = true;
            tabNavigation.SelectedPage = styleNavigationPage;
        }
        void FirstLesson() {
            ResetSettings();
            panel.LabelText = "You are about to start a tutorial on customizing LabelControl.";
            guide1.TargetElement = sample;
        }
        void SecondLesson() {
            ResetSettings();
            panel.LabelText = "You can change LabelControl's properties using the Settings panel.";
            guide1.TargetElement = tabNavigation;
            textItem.Expanded = true;
        }
        void ThirdLesson() {
            ActivateTextPage();
            SetSettings("New text label", saveFontSizeDelta, saveBackColor, saveForeColor);
            panel.LabelText = "Use the Text property to change the LabelControl's caption.";
            guide1.TargetElement = teText;
        }
        void FourthLesson() {
            ActivateTextPage();
            SetSettings("New text label", 3, saveBackColor, saveForeColor);
            panel.LabelText = "You can adjust the font size with the FontSizeDelta setting.";
            guide1.TargetElement = seFontSizeDelta;
        }
        void FifthLesson() {
            ActivateAppearancePage();
            SetSettings("New text label", 3, Color.Red, saveForeColor);
            panel.LabelText = "The Style tab provides the BackColor setting to change the LabelControl's background.";
            guide1.TargetElement = cbBackColor;
        }
        void SixthLesson() {
            ActivateAppearancePage();
            SetSettings("New text label", 3, Color.Red, Color.White);
            panel.LabelText = "Use this property to customize the LabelControl's foreground color.";
            guide1.TargetElement = cbForeColor;
        }
        void SetSettings(string text, int fontSizeDelta, Color backColor, Color foreColor) {
            teText.Text = text;
            cbBackColor.Color = backColor;
            cbForeColor.Color = foreColor;
            seFontSizeDelta.Value = fontSizeDelta;
        }
        void ResetSettings() {
            SetSettings(saveText, saveFontSizeDelta, saveBackColor, saveForeColor);
            textItem.Expanded = false;
            appearanceItem.Expanded = false;
            tabNavigation.SelectedPage = settingsNavigationPage;
        }
        public void EndTutorial() {
            adornerUIManager.ShowGuides = Utils.DefaultBoolean.Default;
            badge1.Visible = true;
            ResetSettings();
        }
        void OnBackColorChanged(object sender, System.EventArgs e) {
            label.Appearance.BackColor = cbBackColor.Color;
        }
        void OnForeColorChanged(object sender, System.EventArgs e) {
            label.Appearance.ForeColor = cbForeColor.Color;
        }
        void OnTextChanged(object sender, System.EventArgs e) {
            label.Text = teText.Text;
        }
        void OnFontSizeDeltaChanged(object sender, System.EventArgs e) {
            label.Appearance.FontSizeDelta = (int)seFontSizeDelta.Value;
        }
        void OnBadgeClick(object sender, System.EventArgs e) {
            StartTutorial();
        }
    }
}
