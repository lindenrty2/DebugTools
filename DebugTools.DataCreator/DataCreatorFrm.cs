using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DebugTools.Common;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DebugTools.DataCreator
{ 

    public partial class DataCreatorFrm : Form  
    { 
        public DataCreatorFrm()
        {
            InitializeComponent(); 
            if (System.Environment.GetCommandLineArgs().Length > 1)
            {
                string targetPath = System.Environment.GetCommandLineArgs()[1];
                this.selectPathControl1.SelectedPath = targetPath;
                SetObject(targetPath);
            }
        }

        private void selectPathControl1_PathChanged(object sender, Common.Window.PathChangedEventArgs args)
        {
            SetObject(args.AfterPath);
        }

        private object _targetObject;

        public void SetObject(string targetPath)
        {
            if(string.IsNullOrEmpty(targetPath) ) return;


            FileStream fileStream = new FileStream(targetPath, FileMode.Open);
            byte[] assemblyHeader = new byte[1024];
            fileStream.Read(assemblyHeader, 0, 1024);

            byte[] typeHeader = new byte[1024];
            fileStream.Read(typeHeader, 0, 1024);

            string assemblyLocation = UTF8Encoding.UTF8.GetString(assemblyHeader).Trim('\0');
            string typeName = UTF8Encoding.UTF8.GetString(typeHeader).Trim('\0');

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Assembly ass = Assembly.LoadFrom(assemblyLocation); 
            Type type = ass.GetType(typeName);
            BinaryFormatter b = new BinaryFormatter();
            _targetObject = b.Deserialize(fileStream);

            propertyGrid1.SelectedObject = _targetObject;

            fileStream.Close();
            //CXmlFile xmlFile = new CXmlFile(targetPath);
            //CXmlNode xmlNode = xmlFile.GetNode<CXmlNode>("configuration");
            //string path = xmlNode.GetAttributeValue("AssemblyPath");
            //string classNs = xmlNode.GetAttributeValue("ClassNameSpace");
            //Assembly assembly = Assembly.LoadFrom(path);
            //Type type = assembly.GetType(classNs);
            //_targetObject = assembly.CreateInstance(classNs, true, BindingFlags.Default, null, new object[] { targetPath }, CultureInfo.CurrentCulture, null);
            //propertyGrid1.SelectedObject = _targetObject;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x=>x.FullName.Equals(args.Name )) ;
        } 

        private void btnSave_Click(object sender, EventArgs e)
        {
            //_targetObject.GetType().GetMethod("Save").Invoke(_targetObject,null);


        }




    }
}
