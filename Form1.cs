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

namespace _01BASIC
{
    public partial class winform : Form
    {

        private SerialPort serialPort = new SerialPort();
        public winform()
        {
            InitializeComponent();
        }

        private void PortNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("헬로망고?");
            //Console.WriteLine("오브젝트 : " + sender);
            //Console.WriteLine("이벤트알그즈 : " + e);
            ComboBox cb = (ComboBox)sender;

            Console.WriteLine("셀렉티드 인덱스 : " + (cb.SelectedIndex+1)+"번째 인덱스 입니당.");
            Console.WriteLine("셀렉티드 밸류 : " + cb.Items[cb.SelectedIndex]);
            
        }

        private void SerialPort_DataRecv(object sender, SerialDataReceivedEventArgs e)
        {
            String recvData = this.serialPort.ReadExisting();
            Console.WriteLine(recvData);
        }

        private void conn_btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("커넥션 버튼 클릭!" + this.PortNumber.Items[this.PortNumber.SelectedIndex].ToString());
            try
            {
                this.serialPort.PortName = this.PortNumber.Items[this.PortNumber.SelectedIndex].ToString();
                this.serialPort.BaudRate = 9600;
                this.serialPort.DataBits = 8;
                this.serialPort.StopBits = System.IO.Ports.StopBits.One;
                this.serialPort.Parity = System.IO.Ports.Parity.None;
                this.serialPort.Open();
                Console.WriteLine("커넥션 석세스!");
                this.textArea.AppendText("커넥티드..\r\n");
                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                this.serialPort.Close();
                this.textArea.AppendText("페일..." + ex + "\r\n");
            }
        }
        private void SerialPort_DataReceived(object sender, EventArgs e)
        {
            String recvData = this.serialPort.ReadLine();
            Console.Write(recvData);

            //스레드 생성 코드
            //Invoke(new Action(()=> {/*처리로직*/ }));

            //LED 점등 유무 확인 스레드
            if (recvData.StartsWith("LED:"))
            {
                Invoke(new Action(() => { this.textArea.AppendText(recvData+"\r\n"); }));
            }


            //온도 센서 확인 스레드
            if (recvData.StartsWith("TEMP:"))
            {
                Invoke(new Action(() => { this.TEMP_BOX.Text = ""; this.TEMP_BOX.Text = recvData.Replace("TEMP:",""); }));
            }


            //조도 센서 확인 스레드
            if (recvData.StartsWith("SUN:"))
            {
                Invoke(new Action(() => { this.sun_txt.Text = ""; this.sun_txt.Text = recvData.Replace("SUN:", ""); }));
            }

            //초음파 센서 확인 스레드
            if (recvData.StartsWith("DIS:"))
            {
                Invoke(new Action(() => { this.DIS_TXT.Text = ""; this.DIS_TXT.Text = recvData.Replace("DIS:", ""); }));
            }

        }

        private void textArea_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort.Write("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort.Write("0");
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
