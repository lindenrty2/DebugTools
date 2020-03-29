using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using DebugTools.Common.Helper;

namespace DebugTools.Common.Hook.ExceptionHook
{
    public partial class ExceptionViewer : UserControl
    {

        private Exception _exception;
        public ExceptionViewer()
        {
            InitializeComponent();
        }

        public void SetExecption(Exception ex)
        {
            if (ex == null) return;
            _exception = ex;
            WriteErrorInformation(ex);
        }

        private static string DISPLAY_TEMPLATE = @"
<Html>
    <Body>
        <p>
            <b>Execptionタイプ</b><br/>
            {0}
        </p>
        <p>
            <b>メッセージ</b><br/>
            {1}
        </p>
        <p>
            <b>InnerExecption</b><br/>
            <a href='#' onclick='OpenInnerExecption();' >{2}</a>
        </p>
        <p>
            <b>異常源</b><br/>
            {3}
        </p>
        <p>
            <b>追加データ</b><br/>
            {4}
        </p>
        <p>
            <b>StackTrace</b><br/>
            {5}
        </p>
    </Body>
    <Script>
        function OpenInnerExecption(){{
            window.open('InnerExecption');
        }}
    </Script>
</Html>
";

        private void WriteErrorInformation(Exception ex)
        {   
            wbInformation.Navigate("about:blank");

            //string execptionName = ex.GetType().FullName;
            //string message = ex.Message;
            //string innerExecption = GetInnerExecption(ex);
            //string stack = ex.StackTrace.Replace("\r\n", "<br/>");
            //string source = ex.Source;
            //string data = GetData(ex); 
            //wbInformation.Document.Write (
            //    string.Format(DISPLAY_TEMPLATE, execptionName, message, innerExecption, source, data, stack));
        }

        private string GetInnerExecption(Exception ex)
        {
            string result = string.Empty;
            if (ex.InnerException == null)
            {
                return result;
            }
            result =ex.InnerException.GetType().FullName ;
            return result;
        }

        private string GetData(Exception ex)
        {
            string result = string.Empty;
            if (ex.Data != null)
            {
                DictionaryEntry de;
                IEnumerator enumObj = ex.Data.GetEnumerator();
                while (enumObj.MoveNext())
                {
                    de = (DictionaryEntry)enumObj.Current; 
                    result += string.Format("{0},{1}<br>", de.Key , de.Value );
                }
            }
            return result;
        }

        private void btnExportReport_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "Html";
            if (dialog.ShowDialog() == DialogResult.Cancel )
            {
                return;
            }
            path = dialog.FileName;
            if (FileSystemHelper.IsValidFile(path))
            {
                File.WriteAllText(path, GetDisplayHtml());
                MessageBox.Show("レポートを出力しました。");
            }
            else
            {
                MessageBox.Show("有効なパスを入力してください。");
            }
        }

        private void wbInformation_NewWindow(object sender, CancelEventArgs e)
        {
            if (_exception.InnerException == null) return;
            ExceptionViewWindow window = new ExceptionViewWindow();
            window.SetExecption(_exception.InnerException );
            window.ShowDialog();
            e.Cancel = true; 
        }

        private string GetDisplayHtml()
        {
            string execptionName = _exception.GetType().FullName;
            string message = _exception.Message;
            string innerExecption = GetInnerExecption(_exception);
            string stack = _exception.StackTrace.Replace("\r\n", "<br/>");
            string source = _exception.Source;
            string data = GetData(_exception);
            return string.Format(DISPLAY_TEMPLATE, execptionName, message, innerExecption, source, data, stack);
        }

        private void wbInformation_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            wbInformation.Document.Write(GetDisplayHtml());
        }
    }
}
