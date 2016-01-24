using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Documents;
using Microsoft.Win32;

namespace WPFSerialAssistant
{
    public partial class MainWindow : Window
    {

        private string GetSaveDataPath()
        {
            string path = @"data.txt";

            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Title = "选择存储数据的路径...";
            sfd.FileName = string.Format("数据{0}", DateTime.Now.ToString("yyyyMdHHMMss"));
            sfd.Filter = "文本文件|*.txt";

            if (sfd.ShowDialog() == true)
            {
                path = sfd.FileName;
            }

            return path;
        }

        private void SaveData(string path)
        {
            try
            {
                using (System.IO.StreamWriter sr = new StreamWriter(path))
                {
                    string text = (new TextRange(recvDataRichTextBox.Document.ContentStart, recvDataRichTextBox.Document.ContentEnd)).Text;

                    sr.Write(text);

                    Information(string.Format("成功保存数据到{0}", path));
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
        }
    }
}
