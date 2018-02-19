using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChatoServer
{
    public partial class MainForm : Form
    {
        public MainForm(EventHandler b1Click, EventHandler b2Click)
        {
            InitializeComponent();
            this.button1.Click += b1Click;
            this.button2.Click += b2Click;
        }
        
        public string GetIPText()
        {
            return this.textBox1.Text;
        }
        
        public int GetPort()
        {
            return (int)this.numericUpDown1.Value;
        }

        public string GetMsgText()
        {
            return this.textBox3.Text.Trim();
        }

        public void ClearMsgText()
        {
            this.textBox3.Clear();
        }

        delegate void VoidString(string s);
        public void Println(string s)
        {
            if (this.textBox2.InvokeRequired) {
                VoidString println = Println;
                this.textBox2.Invoke(println, s);
            }
            else {
                this.textBox2.AppendText(s + Environment.NewLine);
            }
        }

        delegate void VoidBoolString(bool b, string s);
        public void SetConnectionStatusLabel(bool isConnect, string point = null)
        {
            if (this.label3.InvokeRequired) {
                VoidBoolString scsl = SetConnectionStatusLabel;
                this.label3.Invoke(scsl, isConnect, point);
            }
            else {
                if (isConnect) {
                    this.label3.ForeColor = Color.Green;
                    this.label3.Text = point;
                }
                else {
                    this.label3.ForeColor = Color.Red;
                    this.label3.Text = "尚未连接";
                }
            }
        }

    }
}
