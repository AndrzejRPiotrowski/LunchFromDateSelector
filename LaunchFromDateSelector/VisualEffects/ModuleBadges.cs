using DevExpress.Skins;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Svg;
using DevExpress.Utils.VisualEffects;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevExpress.ApplicationUI.Demos.VisualEffects {
    public partial class ModuleBadges : TutorialControl {
        Color unreadTextColor;
        TileItem current;
        public ModuleBadges() {
            InitializeComponent();
            LookAndFeel.StyleChanged += LookAndFeel_StyleChanged;
        }
        private void LookAndFeel_StyleChanged(object sender, EventArgs e) {
            unreadTextColor = CommonColors.GetQuestionColor(LookAndFeel);
            InitTileItems();
            SelectPage(current);
        }
        delegate void InvokeMethod();
        private void ModuleBadges_Load(object sender, EventArgs e) {
            dashboardItem.Tag = navigationPage1;
            calendarItem.Tag = navigationPage2;
            mailItem.Tag = navigationPage3;
            unreadTextColor = CommonColors.GetQuestionColor(LookAndFeel);
            InitBadges();
            navigationTileControl.Paint += OnPaint;
            InitTileItems();
            InitAppointments();
            InitScheduller();
            schedulerControl1.Start = DateTime.Now;
            schedulerControl1.DayView.TopRowTime = new TimeSpan(8, 0, 0);
            current = dashboardItem;
            SelectPage(current);
            gridControl1.DataSource = SourceHelper.GetMessages();
        }
        private void InitBadges() {
            InitBadge(dashMainBadge, "3", dashboardItem);
            InitBadge(dashClockBadge, "2", clockItem);
            InitBadge(dashSettingsBadge, "1", settingsItem);
            InitBadge(calendarMainBadge, "4", calendarItem);
            InitBadge(mailMainBadge, "6", mailItem);
        }
        private void InitBadge(Badge badge, string text, TileItem target) {
            badge.Properties.BeginUpdate();
            badge.Properties.PaintStyle = BadgePaintStyle.Critical;
            badge.Properties.Location = ContentAlignment.TopRight;
            badge.TargetElement = target;
            badge.Properties.Text = text;
            badge.Properties.EndUpdate();
        }
        private void InitScheduller() {
            schedulerControl1.HandleCreated += SchedulerControl1_HandleCreated;
            schedulerControl1.SizeChanged += (sender, e) => schedulerControl1.DateNavigationBar.Panel.Invalidate();
        }
        private void SchedulerControl1_HandleCreated(object sender, EventArgs e) {
            schedulerControl1.DateNavigationBar.Panel = new SchedulerCustomPanel(schedulerControl1) { ImageCollection = svgImageCollection1 };
        }
        private void SelectPage(TileItem tile) {
            current = tile;
            UpdateSelection();
            navigationFrame1.SelectedPage = (INavigationPage)tile.Tag;
        }
        private void UpdateSelection() {
            UpdateTileContent(dashboardItem, current == dashboardItem, "Dashboard");
            UpdateTileContent(calendarItem, current == calendarItem, "Calendar");
            UpdateTileContent(mailItem, current == mailItem, "Mail");
            UpdateTileContent(notesItem, current == notesItem, "Notes");
        }
        private void UpdateTileContent(TileItem item, bool highlight, string imageKey) {
            item.ImageOptions.SvgImageColorizationMode = SvgImageColorizationMode.Default;
            item.ImageOptions.SvgImage = null;
            Color foreColor = highlight ? CommonColors.GetSystemColor("HighlightText") : GetSkinColor("ControlText");
            Color backColor = highlight ? GetSkinColor("Question") : Color.Transparent;
            item.AppearanceItem.Normal.ForeColor = foreColor;
            item.AppearanceItem.Normal.BackColor = backColor;
            item.AppearanceItem.Normal.BorderColor = backColor;
            if(highlight) {
                imageKey += "H";
                item.ImageOptions.SvgImageColorizationMode = SvgImageColorizationMode.None;
            }
            item.ImageOptions.SvgImage = svgImageCollection2[imageKey];
        }
        private void OnSizeChanged(object sender, EventArgs e) {
            int tileHeight = (dashTiles.Height - ScaleDPI.ScaleVertical(dashTiles.IndentBetweenItems)) / 3;
            int vertPadding = tileHeight / 2;
            int horzPadding = (dashTiles.Width - tileHeight * 3 - 2 * dashTiles.IndentBetweenItems) / 2;
            dashTiles.ItemSize = (int)(tileHeight / ScaleDPI.ScaleFactorVert);
            dashTiles.Padding = new Padding(horzPadding, vertPadding, horzPadding, vertPadding);
            UpdateBadges();
        }
        private void OnPaint(object sender, EventArgs e) {
            UpdateBadges();
        }
        private void InitTileItems() {
            Color backColor = CommonColors.GetQuestionColor(LookAndFeel);
            InitTileItem(dashboardItem, 9F, 10);
            InitTileItem(calendarItem, 9F, 10);
            InitTileItem(mailItem, 9F, 10);
            InitTileItem(notesItem, 9F, 10);
            InitDashboardTileItem(clockItem, "Alarms", backColor);
            InitDashboardTileItem(calculatorItem, "Calculator", backColor);
            InitDashboardTileItem(weatherItem, "Weather", backColor);
            InitDashboardTileItem(photosItem, "Photos", backColor);
            InitDashboardTileItem(mapsItem, "Map", backColor);
            InitDashboardTileItem(settingsItem, "settings", backColor);
        }
        private void InitDashboardTileItem(TileItem tile, string imageKey, Color backColor) {
            InitTileItem(tile, 10F, 15);
            tile.AppearanceItem.Normal.BackColor = backColor;
            tile.AppearanceItem.Normal.BorderColor = backColor;
            tile.ImageOptions.SvgImageColorizationMode = SvgImageColorizationMode.None;
            tile.ImageOptions.SvgImage = svgImageCollection2[imageKey];
            tile.ImageOptions.SvgImageSize = new Size(48, 48);
        }
        private void InitTileItem(TileItem tile, float textSize, int indent) {
            tile.ImageToTextIndent = indent;
            tile.AppearanceItem.Normal.Font = new Font("Segoe UI", textSize);
        }
        private void UpdateBadges() {
            SetBadgeOffset(dashClockBadge, clockItem, ScaleDPI.ScaleVertical(4), ScaleDPI.ScaleVertical(5));
            SetBadgeOffset(dashSettingsBadge, settingsItem, 0, ScaleDPI.ScaleVertical(9));
            SetBadgeOffset(dashMainBadge, dashboardItem, 0, ScaleDPI.ScaleVertical(9));
            SetBadgeOffset(calendarMainBadge, calendarItem, 0, ScaleDPI.ScaleVertical(9));
            SetBadgeOffset(mailMainBadge, mailItem, 0, ScaleDPI.ScaleVertical(9));
        }
        private void SetBadgeOffset(Badge badge, TileItem tile, int deltaX, int deltaY) {
            int delta = ScaleDPI.ScaleSize(tile.ImageOptions.SvgImageSize).Width / 2;
            int x = ((ISupportAdornerElement)tile).Bounds.Width / 2 - delta;
            int y = ((ISupportAdornerElement)tile).Bounds.Height / 2 - delta;
            badge.Properties.Offset = new Point(-x - deltaX, y - deltaY);
        }
        private Color GetSkinColor(string name) {
            return CommonSkins.GetSkin(LookAndFeel).Colors[name];
        }
        private void DashboardItemClick(object sender, TileItemEventArgs e) {
            SelectPage(dashboardItem);
        }
        private void CalendarItemClick(object sender, TileItemEventArgs e) {
            SelectPage(calendarItem);
        }
        private void MailItemClick(object sender, TileItemEventArgs e) {
            SelectPage(mailItem);
        }
        private void NotesItemClick(object sender, TileItemEventArgs e) {
        }
        private void InitAppointments() {
            AppointmentMappingInfo mappings = this.schedulerDataStorage1.Appointments.Mappings;
            mappings.Start = "StartTime";
            mappings.End = "EndTime";
            mappings.Subject = "Subject";
            mappings.AllDay = "AllDay";
            mappings.Description = "Description";
            mappings.Label = "Label";
            mappings.Location = "Location";
            mappings.RecurrenceInfo = "RecurrenceInfo";
            mappings.ReminderInfo = "ReminderInfo";
            mappings.ResourceId = "OwnerId";
            mappings.Status = "Status";
            mappings.Type = "EventType";
            schedulerDataStorage1.Appointments.DataSource = SourceHelper.GetEvents();
        }
        private void tileView1_ItemCustomize(object sender, XtraGrid.Views.Tile.TileViewItemCustomizeEventArgs e) {
            Badges.Message msg = tileView1.GetRow(e.RowHandle) as Badges.Message;
            if(msg != null) {
                if(!msg.Read) {
                    e.Item["Date"].Appearance.Normal.ForeColor = unreadTextColor;
                    e.Item["Subject"].Appearance.Normal.ForeColor = unreadTextColor;
                    e.Item["Subject"].Appearance.Normal.FontStyleDelta = FontStyle.Bold;
                }
            }
        }
    }
}