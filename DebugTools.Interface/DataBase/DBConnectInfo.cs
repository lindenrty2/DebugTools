using System;

namespace DebugTools.DataBase
{
    [Serializable()]
    public class DBConnectInfo
    {

        // Data Source=testsv01;Initial Catalog=AcceptanceMarooonR3_2;Persist Security Info=True;User ID=marooon;Password=marooon

        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        private string _host;
        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }

        private string _databaseName;
        public string DataBaseName
        {
            get
            {
                return _databaseName;
            }
            set
            {
                _databaseName = value;
            }
        }

        private string _username;
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return string.Format("主机：{0}  数据库：{1}  用户：{2}  类型：{3}", this.Host, this.DataBaseName, this.UserName, this.Type);
            }
        }

        public string Key
        {
            get
            {
                return string.Format("{0}_{1}_{2}", this.Type, this.Host, this.DataBaseName);
            }
        }

        public DBConnectInfo()
        {
        }

        public DBConnectInfo(string type, string host, string dbname, string username, string password)
        {
            _type = type;
            _host = host;
            _databaseName = dbname;
            _username = username;
            _password = password;
        }
         
    }
     
    [Serializable()]
    public class NewDBConnectInfo : DBConnectInfo
    {
    }
}
