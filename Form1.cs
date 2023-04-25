using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int time;
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public string ASCII = "ASCII";
        public string UTF8 = "UTF-8";
        Socket socketSend;
        public int i1 = 0;
        public int i2 = 0;
        public int i3 = 0;//发送次数计数
        public int i1_0 = 0;
        public int i2_0 = 0;
        public int i3_0 = 0;//上次发送次数计数
        public string str1 = "";
        public string str2 = "";
        public string str3 = "";//发送字符串显示
        private void showStuIfo1(string str, string str1)  //本例中的线程要通过这个方法来访问主线程中的控件
        {
          
        }
        public delegate void stuInfoDelegate1(string a, string str1);
        void ShowMsg(string str, string str1)
        {


            Invoke(new stuInfoDelegate1(showStuIfo1), new object[] { str, str1 });
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(txtServer.Text);
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //获得要连接的远程服务器应用程序的IP地址和端口号
                socketSend.Connect(point);
                ShowMsg("连接成功", "");


                //开启一个新的线程不停的接收服务端发来的消息
                //Thread th = new Thread(Recive);
                //th.IsBackground = false;
                //th.Start();
                //txtServer.Text = " 连接成功";
                btnStart.Enabled = false;
            }
            catch
            {
              
            }
        }
        void Recive()
        {
            while (true)
            {
                try
                {
                    //客户端连接成功后，服务器应该接受客户端发来的消息
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    //实际接受到的有效字节数
                    int r = socketSend.Receive(buffer);

                    string str = Encoding.UTF8.GetString(buffer, 0, r);

                    ShowMsg(socketSend.RemoteEndPoint + ":", str);
                }
                catch
                { }

            }
        }
        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", " ");
                }
            }
            return str.ToLower();
        }
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
                throw new ArgumentException("hex is not a valid number!", "hex");
            }
            // 需要将 hex 转换成 byte 数组。
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message.
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes, 0, hex.Length / 2);
        }
        public bool kaiguan;
        private void button2_Click(object sender, EventArgs e)
        {
            kaiguan = true;
            Task.Run(() =>
            {
                while (kaiguan)
                { 
                    Thread.Sleep(time);
                    string str1 = File.ReadAllText(@"C:\Users\liusheng\Desktop\数据传输极限速度测试\线程一发送信息.txt");
                    //int i_int = Convert.ToInt16(Math.Ceiling(str1.Length / 1000.00));
                    byte[] byteArray = Encoding.UTF8.GetBytes(str1);//Str转为byte值
                    socketSend.Send(byteArray); //发送数据    
                    //int j = 0;
                    //while (i_int > 0)
                    //{
                    //    i_int--;
                    //    if (i_int == 0)
                    //    {
                    //        byte[] byteArray = Encoding.UTF8.GetBytes(str1.Substring((j) * 1000, str1.Length - (j) * 1000));//Str转为byte值
                    //        socketSend.Send(byteArray); //发送数据        
                    //    }
                    //    else
                    //    {
                    //        byte[] byteArray = Encoding.UTF8.GetBytes(str1.Substring(j * 1000, 1000));//Str转为byte值
                    //        socketSend.Send(byteArray); //发送数据
                    //    }
                    //    j++;
                    //}
                    i1 = i1 + 1;
                }
            }
            );
           // Task.Run(() =>
           // {
           //     while (kaiguan)
           //     {
           //         str2 = File.ReadAllText(@"C:\Users\liusheng\Desktop\数据传输极限速度测试\线程二发送信息.txt");
           //         int i_int = Convert.ToInt16(Math.Ceiling(str2.Length / 1000.00));

           //         int j = 0;
           //         while (i_int > 0)
           //         {
           //             i_int--;

           //             if (i_int == 0)
           //             {
           //                 byte[] byteArray = Encoding.UTF8.GetBytes(str2.Substring((j) * 1000, str1.Length - (j) * 1000));//Str转为byte值
           //                 socketSend.Send(byteArray);//发送数据
           //             }
           //             else
           //             {
           //                 byte[] byteArray = Encoding.UTF8.GetBytes(str2.Substring(j * 1000, 1000));//Str转为byte值
           //                 socketSend.Send(byteArray);//发送数据
           //             }
           //             j++;
           //         }
           //         i2 = i2 + 1;
           //     }
           // }
           // );
           // Task.Run(() =>
           //  {
                 
           //          while (kaiguan)
           //          {
           //              str3 = File.ReadAllText(@"C:\Users\liusheng\Desktop\数据传输极限速度测试\线程三发送信息.txt");
           //              int i_int = Convert.ToInt16(Math.Ceiling(str3.Length / 1000.00));

           //              int j = 0;
           //              while (i_int > 0)
           //              {
           //                  i_int--;
           //                  if (i_int == 0)
           //                  {
           //                      byte[] byteArray = Encoding.UTF8.GetBytes(str3.Substring((j) * 1000, str1.Length - (j) * 1000));//Str转为byte值
           //                      socketSend.Send(byteArray);//发送数据
           //                  }
           //                  else
           //                  {


           //                      byte[] byteArray = Encoding.UTF8.GetBytes(str3.Substring(j * 1000, 1000));//Str转为byte值
           //                      socketSend.Send(byteArray);//发送数据
           //                  }
           //                  j++;

           //              }
           //              i3 = i3 + 1;
           //          }
                
           //  }
           //);

         
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
            int t1 = 0;
            int t2 = 0;
            int t3 = 0;
            t1 = i1 - i1_0;
            t2 = i2 - i2_0;
            t3 = i3 - i3_0;
            i1_0 = i1;
            i2_0 = i2;
            i3_0 = i3;
            textBox2.Text = str1;
            textBox3.Text = str2;
            textBox4.Text = str3;
            textBox5.Text = Convert.ToString(str1.Length);
            textBox13.Text = Convert.ToString(str2.Length);
            textBox10.Text = Convert.ToString(str3.Length);

         
            textBox15.Text = Convert.ToString(i1 + i2 + i3);
            textBox14.Text = Convert.ToString(t1 + t2 + t3);


            textBox7.Text = Convert.ToString(t1);
            textBox11.Text = Convert.ToString(t2);
            textBox8.Text = Convert.ToString(t3);
            textBox6.Text = Convert.ToString(i1);
            textBox12.Text = Convert.ToString(i2);
            textBox9.Text = Convert.ToString(i3);
            //textBox17.Text = Convert.ToString(fi.Length*i1);

            FileInfo fi = new FileInfo(@"C:\Users\liusheng\Desktop\TCP速度测试桌面下使用\线程一发送信息.txt");

            Int64 size = fi.Length; // 文件字节大小
            textBox17.Text = Convert.ToString(fi.Length * i1);

            //FileInfo fi1 = new FileInfo(@"C:\Users\liusheng\Desktop\数据传输极限速度测试\线程一发送信息.txt");

            //Int64 size1 = fi.Length; // 文件字节大小


            //FileInfo fi2 = new FileInfo(@"C:\Users\liusheng\Desktop\数据传输极限速度测试\线程一发送信息.txt");

            //Int64 size2 = fi.Length; // 文件字节大小
            // Int64 size3 = str1.Length *  + str2.Length * i2 + str3.Length * i3;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            kaiguan = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            i1 = 0;
            i2 = 0;
            i3 = 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                socketSend.Close();//断开连接
         
                btnStart.Enabled = true;
            }
            catch
            {
            
                btnStart.Enabled = true;
            }
        }
    }
}
