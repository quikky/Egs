using System;
using System.IO;
using System.Windows;
using System.Net;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

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
                var EgsData = sr.ReadToEnd();

                //EgsDataをJSONパース
                var result = new DataContractJsonSerializer(typeof(RootObject));
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(EgsData)))
                    {
                    //出力
                    var data = (RootObject)result.ReadObject(ms);
                    Console.WriteLine(data.shorten);
                    Console.WriteLine(data.original);
                    label1.Content  = "短縮URL：" + data.shorten   + "\r\n";
                    label1.Content += "展開URL：" + data.original  + "\r\n";
                }

                //レスポンスの終了？
                sr.Close();
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

        public class RootObject
        {
            public string shorten { get; set; }
            public string original { get; set; }
        }
    }
}
