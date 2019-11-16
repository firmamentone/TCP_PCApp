//Nov 17 2019 T.I.: Fixed "#1 init failed if user start app without comport connect"
//                  Added temperature display
//                  Added comport Paser framework
//                  Added graphic sample
//Nov 09 2019 T.I.: Timer1 test with "F101, F102"
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
using System.Threading;


namespace COMPortTest
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;
        float tempC;
        float tmpADC;
        public Form1()
        {
            InitializeComponent();

            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ComPort = new SerialPort();
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;
            int comprotOpenFailed=0;


             // Get a list of serial port names.
             ArrayComPortsNames = SerialPort.GetPortNames();

             if (0<ArrayComPortsNames.Length)
             {

                do
                {
                    index += 1;

                    comboBox1.Items.Add(ArrayComPortsNames[index]);
                }
                while (!((ArrayComPortsNames[index] == ComPortName) ||
                                   (index == ArrayComPortsNames.GetUpperBound(0))));


            }
            


            Graphics g;
            g = Graphics.FromImage(DrawArea);

            Pen mypen = new Pen(Brushes.Black);
            g.DrawLine(mypen, 0, 0, 200, 200);
            g.Clear(Color.White);
            g.Dispose();

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
            timer1.Enabled = false;
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
            int s1 = 0;
            string cmdRec;
            string parRec0;
            

            InputDataBuffer += text;

            s1 = InputDataBuffer.IndexOf("\r\n");
            if (s1 > -1)
            {
                this.textBox2.Text += InputDataBuffer;
                cmdRec = InputDataBuffer.Substring(0, 4);

                if("F101"== cmdRec)
                {
                    //-5.5417x+67.6
                    
                    parRec0 = InputDataBuffer.Substring(5, 3);
                    tmpADC = Convert.ToSingle(parRec0);
                    tempC = (0.1153F * tmpADC) - 34.629F;

                }
               


                InputDataBuffer = null;


            }

        }
        private float ADCtoTmpC(int ADCValue)
        {

            return 1;
        }

        private SerialPort ComPort;
        private string InputData;
        private string InputDataBuffer;
        private string strHeatingValue="000";

        private void button4_Click(object sender, EventArgs e)
        {
            strHeatingValue = "000";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            strHeatingValue = "255";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            strHeatingValue = "170";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            strHeatingValue = "085";
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            InputDataBuffer = null;

            ComPort.Write("F101");

            Thread.Sleep(100);

            InputDataBuffer = null;
            ComPort.Write("F102Q"+ strHeatingValue);

            label2.Text = tempC.ToString();

            /*
                        Graphics g;
                        g = Graphics.FromImage(DrawArea);

                        Pen mypen = new Pen(Color.Black);

                        g.DrawLine(mypen, 0, 0, 200, 150);

                        pictureBox1.Image = DrawArea;

                        g.Dispose();

            */

            /*
            Graphics g;
            g = Graphics.FromImage(DrawArea);

            Pen mypen = new Pen(Color.Black);

            //g.DrawLine(mypen, 0, 0, 200, 150);
            g.DrawEllipse(mypen, 0, 0, 20, 20);
            pictureBox1.Image = DrawArea;

            g.Dispose();
            */
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}