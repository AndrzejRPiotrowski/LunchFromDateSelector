using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using DevExpress.Utils.VisualEffects;

namespace DevExpress.ApplicationUI.Demos.VisualEffects {
    public partial class ModuleValidationHints : TutorialControl {
        public ModuleValidationHints() {
            InitializeComponent();
            this.CausesValidation = false;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            tePassword.Validating += OnPasswordValidating;
            tePhone.Validating += OnPhoneValidating;
            teAge.Validating += OnAgeValidating;
            teName.Tag = vhName;
            teUserName.Tag = vhUserName;
            tePassword.Tag = vhPassword;
            teAddress.Tag = vhAddress;
            teAge.Tag = vhAge;
            teEMail.Tag = vhEMail;
            tePhone.Tag = vhPhone;
        }
        void OnAgeValidating(object sender, CancelEventArgs e) {
            vhAge.Properties.State = CalcTextEditValidationState(teAge, e);
        }
        ValidationHintState CalcTextEditValidationState(DevExpress.XtraEditors.TextEdit edit, CancelEventArgs e) {
            if(edit.EditValue == null || string.IsNullOrEmpty(edit.Text))
                return ValidationHintState.Indeterminate;
            return e.Cancel ? ValidationHintState.Invalid : ValidationHintState.Valid;
        }
        void OnPhoneValidating(object sender, CancelEventArgs e) {
            vhPhone.Properties.State = CalcTextEditValidationState(tePhone, e);
        }
        void OnCheckButtonClick(object sender, EventArgs e) {
            dataLayoutControl.ValidateChildren();
        }
        void OnInvalidValue(object sender, XtraEditors.Controls.InvalidValueExceptionEventArgs e) {
            Control editor = sender as Control;
            if(editor == null) return;
            if(editor == tePassword)
                OnInvalidPasswordValue(e);
            if(editor == teEMail)
                OnInvalidEMailValue(e);
            if(editor == tePhone)
                OnInvalidPhoneValue(e);
            ValidationHint hint = editor.Tag as ValidationHint;
            if(hint != null) {
                hint.Properties.InvalidState.Text = e.ErrorText;
                e.ErrorText = null;
            }
        }
        void OnPasswordValidating(object sender, CancelEventArgs e) {
            if(tePassword.EditValue != null && tePassword.EditValue.ToString().Length < 8)
                e.Cancel = true;
        }
        void OnInvalidPhoneValue(XtraEditors.Controls.InvalidValueExceptionEventArgs e) {
            if(e.ErrorText == "Invalid Value")
                e.ErrorText = "Invalid phone number.";
        }
        void OnInvalidEMailValue(XtraEditors.Controls.InvalidValueExceptionEventArgs e) {
            if(e.ErrorText == "Invalid Value")
                e.ErrorText = "Invalid e-mail.";
        }
        void OnInvalidPasswordValue(XtraEditors.Controls.InvalidValueExceptionEventArgs e) {
            if(e.ErrorText == "Invalid Value")
                e.ErrorText = "Your password must be at least 8 characters.";
        }
    }
    public class Customer {
        public Customer() { }

        [Required(ErrorMessage = "This field is required.")]
        public string Name {
            get;
            set;
        }
        [Required(ErrorMessage = "This field is required.")]
        public string UserName {
            get;
            set;
        }
        [DataType(DataType.Password, ErrorMessage = "Invalid password.")]
        [Required(ErrorMessage = "This field is required.")]
        public string Password {
            get;
            set;
        }
        [Range(20, 120, ErrorMessage = "Enter the age between 20 and 100.")]
        public int Age {
            get;
            set;
        }
        [Required(ErrorMessage = "This field is required.")]
        public string EMail {
            get;
            set;
        }
        public string Phone {
            get;
            set;
        }
        public string Address {
            get;
            set;
        }
    }
}
