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

        public void ComboBoxAddItem(string s)
        {
            if (this.comboBox1.InvokeRequired) {
                VoidString cbAddItem = ComboBoxAddItem;
                this.textBox2.Invoke(cbAddItem, s);
            }
            else {
                this.comboBox1.Items.Add(s);
            }
        }
        public void ComboBoxRemoveItem(string s)
        {
            if (this.comboBox1.InvokeRequired) {
                VoidString cbRmItem = ComboBoxRemoveItem;
                this.textBox2.Invoke(cbRmItem, s);
            }
            else {
                this.comboBox1.Items.Remove(s);
            }
        }

    }
}
