using System;
using System.IO;
using System.Windows;
using System.Net;
using System.Web;
using System.Text;

namespace EgsApp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            // API叩く用URL
            var url = string.Format("{0}{1}"
                , "http://api.hitonobetsu.com/surl/open?url="
                , textBox1.Text);

            // リクエスト
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            // 速度対策
            req.Proxy = null;

            HttpWebResponse response = null;
            try
            {
                //レスポンスの取得
                response = (HttpWebResponse)req.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(resStream);
                string EgsData = sr.ReadToEnd();
                sr.Close();
                Console.WriteLine(EgsData);
                label1.Content = EgsData.ToString();
                resStream.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.ToString());
                return;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }


    }
}
