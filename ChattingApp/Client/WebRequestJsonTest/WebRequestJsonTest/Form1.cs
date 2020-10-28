using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace WebRequestJsonTest
{
    public partial class Form1 : Form
    {
        #region 변수 설정

        private CanLogin cLogin = null;
        public static string CK = "";
        private WebSocket client = null;

        public static string msg = "";
        Timer msgTimer;

        #endregion

        #region 생성자

        public Form1()
        {
            InitializeComponent();

            textBox1.Text = "http://localhost:8080/";

            button1.Click += request;
            button2.Click += Clear;
            button3.Click += Copy;
            button4.Click += Login;
            button5.Click += WebSocketTest;
            button6.Click += Logout;
            button7.Click += WebSocketClose;
            button8.Click += GetFileData;

            msgTimer = new Timer();
            msgTimer.Interval = 1000;
            msgTimer.Tick += new EventHandler(timer_Tick);
            //msgTimer.Start();
        }

        #endregion

        #region 로그인 및 로그아웃

        private void Login(object sender, EventArgs e)
        {
            if (cLogin == null)
            {
                cLogin = new CanLogin();
            }

            cLogin.userName = txtID.Text;
            cLogin.userPassword = txtPassword.Text;

            string strResponse = string.Empty;

            strResponse = cLogin.Check();

            debugOutput(strResponse);
        }

        private void Logout(object sender, EventArgs e)
        {
            string strResponse = string.Empty;

            strResponse = cLogin.Delete();

            debugOutput(strResponse);

            cLogin = new CanLogin();
            CK = "";
        }

        #endregion

        #region 웹소켓 관련 함수

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                debugOutput(msg);
                msg = "";
            }
        }

        private void WebSocketTest(object sender, EventArgs e)
        {
            client = WebSock.WebConnect(client);

            //if (client == null)
            //{
            //    client = new WebSocket("ws://localhost:8080/websocket");
            //    //client.Connect();
            //}
            //client.Connect();
            //client.Send("Hello");

            if (msgTimer.Enabled == false)
                msgTimer.Start();

            client.OnMessage += GetData;
        }

        private void WebSocketClose(object sender, EventArgs e)
        {
            client.Close();
            client = null;

            if (msgTimer.Enabled == true)
                msgTimer.Stop();
        }

        private void GetData(object sender, MessageEventArgs e)
        {
            msg = e.Data + "\n";
        }

        #endregion

        #region REST API 사용을 위한 함수

        private void request(object sender, EventArgs e)
        {
            //using(WebClient wc = new WebClient())
            //{
            //    var json = new WebClient().DownloadString(textBox1.Text);

            //    textBox2.Text = json + "\n";

            //    JArray array = JArray.Parse(json);

            //    foreach (JObject job in array)
            //    {
            //        textBox2.Text += job["TID"].ToString() + " / " + job["CODE"].ToString() + " / " + job["NAME"].ToString() + "\n";
            //    }
            //}

            RestClient rClient = new RestClient();
            rClient.endPoint = textBox1.Text;
            rClient.userName = txtID.Text;
            rClient.userPassword = txtPassword.Text;

            switch (comboBox1.Text)
            {
                case "GET":
                    rClient.httpMethod = httpVerb.GET;
                    break;
                case "POST":
                    rClient.httpMethod = httpVerb.POST;
                    break;
                case "PUT":
                    rClient.httpMethod = httpVerb.PUT;
                    break;
                case "DELETE":
                    rClient.httpMethod = httpVerb.DELETE;
                    break;
                default:
                    break;
            }

            debugOutput("Rest Client Start!");

            string strResponse = string.Empty;

            strResponse = rClient.makeRequest();

            debugOutput(strResponse);
        }

        #endregion

        #region 파일 관련 함수

        private void GetFileData(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "File Open";
            ofd.Filter = "문서 파일 (*.txt) | *.txt; | 그림 파일 (*.jpg, *.gif, *.bmp) | *.jpg; *.gif; *.bmp; | 모든 파일 (*.*) | *.*;";

            DialogResult dr = ofd.ShowDialog();

            if(dr == DialogResult.OK)
            {
                string fileName = ofd.SafeFileName;
                string fileFullName = ofd.FileName;
                string filePath = fileFullName.Replace(fileName, "");

                debugOutput("파일명 : " + fileName);
                debugOutput("파일경로와 파일명 : " + fileFullName);
                debugOutput("파일경로 : " + filePath);
            }
            else if(dr == DialogResult.Cancel)
            {

            }
        }

        #endregion

        #region 결과 출력을 위한 함수

        private void debugOutput(string strDebugText)
        {
            try
            {
                System.Diagnostics.Debug.Write(strDebugText + Environment.NewLine);
                textBox2.Text += strDebugText + Environment.NewLine;
                textBox2.SelectionStart = textBox2.TextLength;
                textBox2.ScrollToCaret();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message, Environment.NewLine);
            }
        }

        private void Copy(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(textBox2.Text);
        }

        private void Clear(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
        }

        #endregion
    }
}
