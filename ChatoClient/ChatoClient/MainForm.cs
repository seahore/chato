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
            this.buttonConnect.Click += b1Click;
            this.buttonSend.Click += b2Click;
        }
        
        public string GetIPText()
        {
            return this.textBoxIP.Text;
        }
        
        public int GetPort()
        {
            return (int)this.numericUpDownPort.Value;
        }

        public string GetMsgText()
        {
            return this.textBoxSendee.Text.Trim();
        }

        public void ClearMsgText()
        {
            this.textBoxSendee.Clear();
        }

        delegate void VoidString(string s);
        public void Println(string s)
        {
            if (this.textBoxMsg.InvokeRequired) {
                VoidString println = Println;
                this.textBoxMsg.Invoke(println, s);
            }
            else {
                this.textBoxMsg.AppendText(s + Environment.NewLine);
            }
        }

        delegate void VoidBoolString(bool b, string s);
        public void SetConnectionStatusLabel(bool isConnect, string point = null)
        {
            if (this.labelStatus.InvokeRequired) {
                VoidBoolString scsl = SetConnectionStatusLabel;
                this.labelStatus.Invoke(scsl, isConnect, point);
            }
            else {
                if (isConnect) {
                    this.labelStatus.ForeColor = Color.Green;
                    this.labelStatus.Text = point;
                }
                else {
                    this.labelStatus.ForeColor = Color.Red;
                    this.labelStatus.Text = "尚未连接";
                }
            }
        }

        delegate void VoidBool(bool b);
        public void SetButtonSendEnabled(bool enabled)
        {
            if (this.buttonSend.InvokeRequired)
            {
                VoidBool sbse = SetButtonSendEnabled;
                this.textBoxMsg.Invoke(sbse, enabled);
            }
            else
            {
                this.buttonSend.Enabled = enabled;
            }
        }

    }
}
