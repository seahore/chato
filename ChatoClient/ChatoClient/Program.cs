﻿using System;
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
        static Socket clientSocket = null;
        static IPAddress ip = null;
        static IPEndPoint point = null;

        static MainForm form = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
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

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                //进行连接
                clientSocket.Connect(point);
                form.SetConnectionStatusLabel(true, point.ToString());
                form.Println($"连接 {point} 的服务器。");

                //不停的接收服务器端发送的消息
                Thread thread = new Thread(Receive);
                thread.IsBackground = true;
                thread.Start(clientSocket);

            }
            catch (Exception ex) {
                form.Println("错误：" + ex.Message);
            }
        }
        static void Receive(object so)
        {
            Socket send = so as Socket;
            while (true)
            {
                try {
                    //获取发送过来的消息
                    byte[] buf = new byte[1024 * 1024 * 2];
                    int len = send.Receive(buf);
                    if (len == 0) break;
                    string s = Encoding.UTF8.GetString(buf, 0, len);
                    form.Println("服务器：" + s);
                }
                catch (Exception e) {
                    form.SetConnectionStatusLabel(false);
                    form.Println("服务器已中断连接。");
                    break;
                }
            }
        }

        static void SendMsg(object sender, EventArgs e)
        {
            string msg = form.GetMsgText();
            byte[] sendee = Encoding.UTF8.GetBytes(msg);
            clientSocket.Send(sendee);
            form.Println(msg);
            form.ClearMsgText();
        }
    }
}