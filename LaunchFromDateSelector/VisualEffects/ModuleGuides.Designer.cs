namespace DevExpress.ApplicationUI.Demos.VisualEffects {
    partial class ModuleGuides {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModuleGuides));
            this.normalBackColorRepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.adornerUIManager = new DevExpress.Utils.VisualEffects.AdornerUIManager(this.components);
            this.guide1 = new DevExpress.Utils.VisualEffects.Guide();
            this.badge1 = new DevExpress.Utils.VisualEffects.Badge();
            this.sample = new DevExpress.XtraEditors.GroupControl();
            this.label = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.tabNavigation = new DevExpress.XtraBars.Navigation.TabPane();
            this.styleNavigationPage = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.appearanceAccordionControl = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionContentContainer1 = new DevExpress.XtraBars.Navigation.AccordionContentContainer();
            this.lbBackColor = new DevExpress.XtraEditors.LabelControl();
            this.lbForeColor = new DevExpress.XtraEditors.LabelControl();
            this.cbBackColor = new DevExpress.XtraEditors.ColorEdit();
            this.cbForeColor = new DevExpress.XtraEditors.ColorEdit();
            this.appearanceItem = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.settingsNavigationPage = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.textAccordionControl = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionContentContainer2 = new DevExpress.XtraBars.Navigation.AccordionContentContainer();
            this.lbText = new DevExpress.XtraEditors.LabelControl();
            this.lbFontSizeDelta = new DevExpress.XtraEditors.LabelControl();
            this.teText = new DevExpress.XtraEditors.TextEdit();
            this.seFontSizeDelta = new DevExpress.XtraEditors.SpinEdit();
            this.textItem = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            ((System.ComponentModel.ISupportInitialize)(this.normalBackColorRepositoryItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adornerUIManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sample)).BeginInit();
            this.sample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).BeginInit();
            this.xtraTabControl.SuspendLayout();
            this.xtraTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabNavigation)).BeginInit();
            this.tabNavigation.SuspendLayout();
            this.styleNavigationPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appearanceAccordionControl)).BeginInit();
            this.appearanceAccordionControl.SuspendLayout();
            this.accordionContentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbBackColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbForeColor.Properties)).BeginInit();
            this.settingsNavigationPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textAccordionControl)).BeginInit();
            this.textAccordionControl.SuspendLayout();
            this.accordionContentContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seFontSizeDelta.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // normalBackColorRepositoryItem
            // 
            this.normalBackColorRepositoryItem.AutoHeight = false;
            this.normalBackColorRepositoryItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.normalBackColorRepositoryItem.Name = "normalBackColorRepositoryItem";
            // 
            // adornerUIManager
            // 
            this.adornerUIManager.Elements.Add(this.guide1);
            this.adornerUIManager.Elements.Add(this.badge1);
            this.adornerUIManager.GuideProperties.ShowGuidesShortcut = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F2);
            this.adornerUIManager.Owner = this;
            // 
            // guide1
            // 
            this.guide1.Tag = "";
            // 
            // badge1
            // 
            this.badge1.Properties.Location = System.Drawing.ContentAlignment.TopRight;
            this.badge1.Properties.Offset = new System.Drawing.Point(-60, 14);
            this.badge1.Properties.Text = "Start Tutorial";
            this.badge1.Properties.TextMargin = new System.Windows.Forms.Padding(5);
            this.badge1.TargetElement = this.sample;
            this.badge1.Click += new System.EventHandler(this.OnBadgeClick);
            // 
            // sample
            // 
            this.sample.Controls.Add(this.label);
            this.sample.Location = new System.Drawing.Point(24, 93);
            this.sample.Name = "sample";
            this.sample.Size = new System.Drawing.Size(271, 171);
            this.sample.TabIndex = 9;
            this.sample.Text = "Sample";
            // 
            // label
            // 
            this.label.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.label.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.label.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label.Location = new System.Drawing.Point(2, 20);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(267, 149);
            this.label.TabIndex = 3;
            this.label.Text = "Label1";
            // 
            // xtraTabControl
            // 
            this.xtraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl.Location = new System.Drawing.Point(5, 0);
            this.xtraTabControl.Name = "xtraTabControl";
            this.xtraTabControl.SelectedTabPage = this.xtraTabPage;
            this.xtraTabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl.Size = new System.Drawing.Size(654, 378);
            this.xtraTabControl.TabIndex = 4;
            this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage});
            this.xtraTabControl.TabStop = false;
            // 
            // xtraTabPage
            // 
            this.xtraTabPage.Controls.Add(this.sample);
            this.xtraTabPage.Controls.Add(this.tabNavigation);
            this.xtraTabPage.Name = "xtraTabPage";
            this.xtraTabPage.Size = new System.Drawing.Size(648, 372);
            this.xtraTabPage.Text = "xtraTabPage1";
            // 
            // tabNavigation
            // 
            this.tabNavigation.Controls.Add(this.styleNavigationPage);
            this.tabNavigation.Controls.Add(this.settingsNavigationPage);
            this.tabNavigation.Location = new System.Drawing.Point(361, 93);
            this.tabNavigation.Name = "tabNavigation";
            this.tabNavigation.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.styleNavigationPage,
            this.settingsNavigationPage});
            this.tabNavigation.RegularSize = new System.Drawing.Size(259, 169);
            this.tabNavigation.SelectedPage = this.settingsNavigationPage;
            this.tabNavigation.Size = new System.Drawing.Size(259, 169);
            this.tabNavigation.TabIndex = 0;
            // 
            // styleNavigationPage
            // 
            this.styleNavigationPage.Caption = "Style changed";
            this.styleNavigationPage.Controls.Add(this.appearanceAccordionControl);
            this.styleNavigationPage.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("styleNavigationPage.ImageOptions.SvgImage")));
            this.styleNavigationPage.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            this.styleNavigationPage.Name = "styleNavigationPage";
            this.styleNavigationPage.Size = new System.Drawing.Size(241, 121);
            // 
            // appearanceAccordionControl
            // 
            this.appearanceAccordionControl.Controls.Add(this.accordionContentContainer1);
            this.appearanceAccordionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appearanceAccordionControl.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.appearanceItem});
            this.appearanceAccordionControl.Location = new System.Drawing.Point(0, 0);
            this.appearanceAccordionControl.Name = "appearanceAccordionControl";
            this.appearanceAccordionControl.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Hidden;
            this.appearanceAccordionControl.Size = new System.Drawing.Size(241, 121);
            this.appearanceAccordionControl.TabIndex = 18;
            // 
            // accordionContentContainer1
            // 
            this.accordionContentContainer1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.accordionContentContainer1.Appearance.Options.UseBackColor = true;
            this.accordionContentContainer1.Controls.Add(this.lbBackColor);
            this.accordionContentContainer1.Controls.Add(this.lbForeColor);
            this.accordionContentContainer1.Controls.Add(this.cbBackColor);
            this.accordionContentContainer1.Controls.Add(this.cbForeColor);
            this.accordionContentContainer1.Name = "accordionContentContainer1";
            this.accordionContentContainer1.Size = new System.Drawing.Size(241, 76);
            this.accordionContentContainer1.TabIndex = 1;
            // 
            // lbBackColor
            // 
            this.lbBackColor.Location = new System.Drawing.Point(3, 17);
            this.lbBackColor.Name = "lbBackColor";
            this.lbBackColor.Size = new System.Drawing.Size(51, 13);
            this.lbBackColor.TabIndex = 14;
            this.lbBackColor.Text = "BackColor:";
            // 
            // lbForeColor
            // 
            this.lbForeColor.Location = new System.Drawing.Point(3, 47);
            this.lbForeColor.Name = "lbForeColor";
            this.lbForeColor.Size = new System.Drawing.Size(51, 13);
            this.lbForeColor.TabIndex = 15;
            this.lbForeColor.Text = "ForeColor:";
            // 
            // cbBackColor
            // 
            this.cbBackColor.EditValue = System.Drawing.Color.Empty;
            this.cbBackColor.Location = new System.Drawing.Point(86, 14);
            this.cbBackColor.Name = "cbBackColor";
            this.cbBackColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbBackColor.Size = new System.Drawing.Size(125, 20);
            this.cbBackColor.TabIndex = 17;
            this.cbBackColor.ColorChanged += new System.EventHandler(this.OnBackColorChanged);
            // 
            // cbForeColor
            // 
            this.cbForeColor.EditValue = System.Drawing.Color.Empty;
            this.cbForeColor.Location = new System.Drawing.Point(86, 44);
            this.cbForeColor.Name = "cbForeColor";
            this.cbForeColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbForeColor.Size = new System.Drawing.Size(125, 20);
            this.cbForeColor.TabIndex = 16;
            this.cbForeColor.ColorChanged += new System.EventHandler(this.OnForeColorChanged);
            // 
            // appearanceItem
            // 
            this.appearanceItem.ContentContainer = this.accordionContentContainer1;
            this.appearanceItem.Name = "appearanceItem";
            this.appearanceItem.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.appearanceItem.Text = "Appearance";
            // 
            // settingsNavigationPage
            // 
            this.settingsNavigationPage.Caption = "Text changed";
            this.settingsNavigationPage.Controls.Add(this.textAccordionControl);
            this.settingsNavigationPage.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("settingsNavigationPage.ImageOptions.SvgImage")));
            this.settingsNavigationPage.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            this.settingsNavigationPage.Name = "settingsNavigationPage";
            this.settingsNavigationPage.Size = new System.Drawing.Size(241, 121);
            // 
            // textAccordionControl
            // 
            this.textAccordionControl.Controls.Add(this.accordionContentContainer2);
            this.textAccordionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textAccordionControl.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.textItem});
            this.textAccordionControl.Location = new System.Drawing.Point(0, 0);
            this.textAccordionControl.Name = "textAccordionControl";
            this.textAccordionControl.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Hidden;
            this.textAccordionControl.Size = new System.Drawing.Size(241, 121);
            this.textAccordionControl.TabIndex = 19;
            // 
            // accordionContentContainer2
            // 
            this.accordionContentContainer2.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.accordionContentContainer2.Appearance.Options.UseBackColor = true;
            this.accordionContentContainer2.Controls.Add(this.lbText);
            this.accordionContentContainer2.Controls.Add(this.lbFontSizeDelta);
            this.accordionContentContainer2.Controls.Add(this.teText);
            this.accordionContentContainer2.Controls.Add(this.seFontSizeDelta);
            this.accordionContentContainer2.Name = "accordionContentContainer2";
            this.accordionContentContainer2.Size = new System.Drawing.Size(241, 76);
            this.accordionContentContainer2.TabIndex = 1;
            // 
            // lbText
            // 
            this.lbText.Location = new System.Drawing.Point(3, 16);
            this.lbText.Name = "lbText";
            this.lbText.Size = new System.Drawing.Size(26, 13);
            this.lbText.TabIndex = 14;
            this.lbText.Text = "Text:";
            // 
            // lbFontSizeDelta
            // 
            this.lbFontSizeDelta.Location = new System.Drawing.Point(3, 46);
            this.lbFontSizeDelta.Name = "lbFontSizeDelta";
            this.lbFontSizeDelta.Size = new System.Drawing.Size(70, 13);
            this.lbFontSizeDelta.TabIndex = 15;
            this.lbFontSizeDelta.Text = "FontSizeDelta:";
            // 
            // teText
            // 
            this.teText.EditValue = "label1";
            this.teText.Location = new System.Drawing.Point(93, 13);
            this.teText.Name = "teText";
            this.teText.Size = new System.Drawing.Size(125, 20);
            this.teText.TabIndex = 17;
            this.teText.EditValueChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // seFontSizeDelta
            // 
            this.seFontSizeDelta.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seFontSizeDelta.Location = new System.Drawing.Point(93, 43);
            this.seFontSizeDelta.Name = "seFontSizeDelta";
            this.seFontSizeDelta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seFontSizeDelta.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.seFontSizeDelta.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.seFontSizeDelta.Properties.MaxValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.seFontSizeDelta.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.seFontSizeDelta.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.seFontSizeDelta.Size = new System.Drawing.Size(125, 20);
            this.seFontSizeDelta.TabIndex = 16;
            this.seFontSizeDelta.ValueChanged += new System.EventHandler(this.OnFontSizeDeltaChanged);
            // 
            // textItem
            // 
            this.textItem.ContentContainer = this.accordionContentContainer2;
            this.textItem.Name = "textItem";
            this.textItem.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.textItem.Text = "Text";
            // 
            // ModuleGuides
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl);
            this.Name = "ModuleGuides";
            this.Size = new System.Drawing.Size(659, 383);
            ((System.ComponentModel.ISupportInitialize)(this.normalBackColorRepositoryItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adornerUIManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sample)).EndInit();
            this.sample.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).EndInit();
            this.xtraTabControl.ResumeLayout(false);
            this.xtraTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabNavigation)).EndInit();
            this.tabNavigation.ResumeLayout(false);
            this.styleNavigationPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.appearanceAccordionControl)).EndInit();
            this.appearanceAccordionControl.ResumeLayout(false);
            this.accordionContentContainer1.ResumeLayout(false);
            this.accordionContentContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbBackColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbForeColor.Properties)).EndInit();
            this.settingsNavigationPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textAccordionControl)).EndInit();
            this.textAccordionControl.ResumeLayout(false);
            this.accordionContentContainer2.ResumeLayout(false);
            this.accordionContentContainer2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seFontSizeDelta.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.VisualEffects.AdornerUIManager adornerUIManager;
        private DevExpress.Utils.VisualEffects.Guide guide1;
        private XtraTab.XtraTabControl xtraTabControl;
        private XtraTab.XtraTabPage xtraTabPage;
        private XtraEditors.Repository.RepositoryItemColorEdit normalBackColorRepositoryItem;
        private XtraBars.Navigation.TabPane tabNavigation;
        private XtraBars.Navigation.TabNavigationPage styleNavigationPage;
        private XtraBars.Navigation.TabNavigationPage settingsNavigationPage;
        private XtraBars.Navigation.AccordionControl appearanceAccordionControl;
        private XtraBars.Navigation.AccordionContentContainer accordionContentContainer1;
        private XtraEditors.LabelControl lbBackColor;
        private XtraEditors.LabelControl lbForeColor;
        private XtraBars.Navigation.AccordionControlElement appearanceItem;
        private XtraEditors.ColorEdit cbBackColor;
        private XtraEditors.ColorEdit cbForeColor;
        private XtraBars.Navigation.AccordionControl textAccordionControl;
        private XtraBars.Navigation.AccordionContentContainer accordionContentContainer2;
        private XtraEditors.LabelControl lbText;
        private XtraEditors.LabelControl lbFontSizeDelta;
        private XtraBars.Navigation.AccordionControlElement textItem;
        private XtraEditors.TextEdit teText;
        private XtraEditors.SpinEdit seFontSizeDelta;
        private XtraEditors.GroupControl sample;
        private XtraEditors.LabelControl label;
        private Utils.VisualEffects.Badge badge1;   
    }
}
