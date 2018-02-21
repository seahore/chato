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
        public MainForm(EventHandler bListenClick, EventHandler bSendClick)
        {
            InitializeComponent();
            this.buttonListen.Click += bListenClick;
            this.buttonSend.Click += bSendClick;
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

        public void ComboBoxAddItem(string s)
        {
            if (this.comboBoxAllClients.InvokeRequired) {
                VoidString cbAddItem = ComboBoxAddItem;
                this.textBoxMsg.Invoke(cbAddItem, s);
            }
            else {
                this.comboBoxAllClients.Items.Add(s);
            }
        }
        public void ComboBoxRemoveItem(string s)
        {
            if (this.comboBoxAllClients.InvokeRequired) {
                VoidString cbRmItem = ComboBoxRemoveItem;
                this.textBoxMsg.Invoke(cbRmItem, s);
            }
            else {
                this.comboBoxAllClients.Items.Remove(s);
            }
        }

    }
}
