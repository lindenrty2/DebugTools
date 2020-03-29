using DebugTools.DataBase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DebugTools.DBO
{
    public partial class SelectConnect : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        private List<DBConnectInfo> _connects;
        public IEnumerable<DBConnectInfo> Connects
        {
            get
            {
                return _connects;
            }
        }

        private IDataAccessor _selectedDataAccessor;
        public IDataAccessor SelectedDataAccessor
        {
            get
            {
                return _selectedDataAccessor;
            }
        }

        public SelectConnect()
        {
            InitializeComponent();
            ctlConnectList.MouseDoubleClick += ctlConnectList_MouseDoubleClick;
            _connects = ConfigCenter.Instance.GetConnectInfoList().ToList();
            _connects.Add(new NewDBConnectInfo());
            ctlConnectList.ItemsSource = Connects;
        }

        public void Save()
        {
            XmlSerializeHelper.Serialize(@"setting\\connects.xml", _connects.Where(x => !((x) is NewDBConnectInfo)).ToArray());
        }

        private void ctlConnectList_MouseDoubleClick(System.Object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedIndex = ((System.Windows.Controls.ListView)sender).SelectedIndex;
            if (selectedIndex >= (_connects.Count - 1))
                StartNewDBConnectInfo();
            else
            {
                _selectedDataAccessor = AccessorCenter.Instance.GetAccessor(_connects[selectedIndex]);
                if (_selectedDataAccessor == null)
                    return;
                this.DialogResult = true;
            }
        }

        private void StartNewDBConnectInfo()
        {
            EditConnect edit = new EditConnect();
            if (edit.ShowDialog(null) == true)
            {
                _connects.Insert(_connects.Count - 1, edit.ConnectInfo);
                ctlConnectList.ItemsSource = null;
                ctlConnectList.ItemsSource = Connects;
                Save();
            }
            else
            {
            }
        }

        private void StartMainWindow()
        {
        }

        private void menuEdit_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            int selectedIndex = ctlConnectList.SelectedIndex;
            DBConnectInfo selectedConnect = _connects[selectedIndex];
            DBConnectInfo copy = JsonConvert.DeserializeObject<DBConnectInfo>(JsonConvert.SerializeObject(selectedConnect));
            EditConnect edit = new EditConnect();
            if (edit.ShowDialog(copy) == true)
            {
                _connects[selectedIndex] = edit.ConnectInfo;
                ctlConnectList.ItemsSource = null;
                ctlConnectList.ItemsSource = Connects;
                Save();
            }
            else
            {
            }
        }

        private void menuDelete_Click(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedIndex = ctlConnectList.SelectedIndex;
            var selectedConnect = _connects[selectedIndex];
            if ((int)MessageBox.Show(string.Format("「{0}」を削除する、よろしいでしょうか？", selectedConnect.Host), "操作確認", MessageBoxButton.YesNo) != (int)MessageBoxResult.Yes)
                return;
            _connects.RemoveAt(selectedIndex);
            Save();
            ctlConnectList.ItemsSource = null;
            ctlConnectList.ItemsSource = Connects;
        }
    }
}
