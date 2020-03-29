namespace DebugTools.Package
{

    public class PackageInfo
    {
        string _name;
        string Name
        {
            get { return _name; }
        }

        private string _path;
        public PackageInfo(string path, string name)
        {
            _path = path;
            _name = name;
        }

    }

}