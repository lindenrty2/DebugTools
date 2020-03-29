using DebugTools.DataBase;

namespace DebugTools.DBO
{
    public partial class EditConnect
    {
        private DBConnectInfo _connectInfo;
        public DBConnectInfo ConnectInfo
        {
            get
            {
                return _connectInfo;
            }
        }

        public EditConnect()
        {
            InitializeComponent();
            this.btnSave.Click += btnSave_Click;
            this.btnCancel.Click += btnCancel_Click;
        }

        public bool? ShowDialog(DBConnectInfo connectInfo)
        {
            if (connectInfo == null)
                _connectInfo = new DBConnectInfo();
            else
                _connectInfo = connectInfo;
            txtHost.Text = _connectInfo.Host;
            txtDBName.Text = _connectInfo.DataBaseName;
            txtUsername.Text = _connectInfo.UserName;
            txtPassword.Text = _connectInfo.Password;
            cmbDBType.SelectedValue = _connectInfo.Type;
            return base.ShowDialog();
        }

        private void btnSave_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            _connectInfo.Host = txtHost.Text;
            _connectInfo.DataBaseName = txtDBName.Text;
            _connectInfo.UserName = txtUsername.Text;
            _connectInfo.Password = txtPassword.Text;
            _connectInfo.Type = (string)cmbDBType.SelectedValue;
            this.DialogResult = true;
        }

        private void btnCancel_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
