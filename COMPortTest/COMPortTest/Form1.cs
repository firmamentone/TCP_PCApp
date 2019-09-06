//Sep 06 2019 T.I.: refactored data members in the "Form1" class

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;



namespace COMPortTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ComPort = new SerialPort();
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;

            ArrayComPortsNames = SerialPort.GetPortNames();
            do
            {
                index += 1;

                comboBox1.Items.Add(ArrayComPortsNames[index]);
            }
            while (!((ArrayComPortsNames[index] == ComPortName) ||
                                (index == ArrayComPortsNames.GetUpperBound(0))));



        }

        private void button3_Click(object sender, EventArgs e)
        {
            ComPort.Write(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InputData = String.Empty;

            ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_1);
            ComPort.BaudRate = Convert.ToInt32(comboBox2.Text);
            ComPort.Parity = Parity.None;
            ComPort.StopBits = StopBits.One;
            ComPort.DataBits = 8;
            ComPort.Handshake = Handshake.None;
            ComPort.RtsEnable = true;

            ComPort.PortName = comboBox1.Text;
            ComPort.Open();
     

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ComPort.Close();
        }

        delegate void SetTextCallback(string text);

        private void port_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            InputData = ComPort.ReadExisting();
            if (InputData != String.Empty)
            {
                SetTextCallback invoke = new SetTextCallback(SetText);
                this.BeginInvoke(invoke, new object[] { InputData });

            }
        }

        private void SetText(string text)
        {
            this.textBox2.Text += text;
        }

        private SerialPort ComPort;
        private string InputData;
    }
}