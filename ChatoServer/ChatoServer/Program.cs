using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace ChatoServer
{
    static class Program
    {
        static Socket serverSocket = null;
        static IPAddress ip = null;
        static IPEndPoint point = null;

        static Dictionary<string, Socket> allClientSockets = null;

        static MainForm form = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            allClientSockets = new Dictionary<string, Socket>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new MainForm(b1Click, b2Click);
            Application.Run(form);

        }

        static EventHandler b1Click = SetConnection;
        static EventHandler b2Click = SendMsg;

        static void SetConnection(object sender, EventArgs e)
        {
            ip = IPAddress.Parse(form.GetIPText());
            point = new IPEndPoint(ip, form.GetPort());

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                serverSocket.Bind(point);
                serverSocket.Listen(20);
                form.Println($"服务器开始在 {point} 上监听。");

                Thread thread = new Thread(Listen);
                thread.IsBackground = true;
                thread.Start(serverSocket);
            }
            catch (Exception ex) {
                form.Println("错误：" + ex.Message);
            }
        }

        static void Listen(object so)
        {
            Socket serverSocket = so as Socket;
            while (true)
            {
                try {
                    //等待连接并且创建一个负责通讯的socket
                    Socket clientSocket = serverSocket.Accept();
                    //获取链接的IP地址
                    string clientPoint = clientSocket.RemoteEndPoint.ToString();
                    form.Println($"{clientPoint} 上的客户端请求连接。");

                    allClientSockets.Add(clientPoint, clientSocket);
                    form.ComboBoxAddItem(clientPoint);

                    //开启一个新线程不停接收消息
                    Thread thread = new Thread(Receive);
                    thread.IsBackground = true;
                    thread.Start(clientSocket);
                }
                catch(Exception e) {
                    form.Println(e.Message);
                    break;
                }
            }
        }

        static void Receive(object so)
        {
            Socket clientSocket = so as Socket;
            string clientPoint = clientSocket.RemoteEndPoint.ToString();
            while (true) {
                try {
                    //获取发送过来的消息容器
                    byte[] buf = new byte[1024 * 1024 * 2];
                    int len = clientSocket.Receive(buf);
                    //有效字节为0则跳过
                    if (len == 0) break;

                    string s = Encoding.UTF8.GetString(buf, 0, len);
                    form.Println($"{clientPoint}: {s}");

                    foreach (Socket t in allClientSockets.Values) {
                        byte[] sendee = Encoding.UTF8.GetBytes($"{clientPoint}: {s}");
                        t.Send(sendee);
                    }

                    //byte[] sendee = Encoding.UTF8.GetBytes("服务器返回信息");
                    //clientSocket.Send(sendee);
                }
                catch (Exception e) {
                    allClientSockets.Remove(clientPoint);
                    form.ComboBoxRemoveItem(clientPoint);

                    form.Println($"客户端 {clientSocket.RemoteEndPoint} 中断连接： "+e.Message);
                    clientSocket.Close();
                    break;
                }
            }
        }

        static void SendMsg(object sender, EventArgs e)
        {
            string msg = form.GetMsgText();
            if (msg == "") return;
            byte[] sendee = Encoding.UTF8.GetBytes($"服务器：{msg}");
            foreach(Socket s in allClientSockets.Values)
                s.Send(sendee);
            form.Println(msg);
            form.ClearMsgText();
        }

    }
}
