using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Xml.Linq;
using hely;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates; 
namespace Chatting
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }
        public string contentHead = " <style>  body {    background-color: {color};  } </style><script>jssubject={subject};setsubject =function(a){jssubject=a;alert('OK'+jssubject)}</script>";
        public string content = "";
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Bitmap.FromFile("bg.png");
            //异步建立连接回调
            accesstoken = lion.ReadString("app", "accesstoken", "555");
            //subject = lion.ReadString("app", "subject", "0");
            webBrowser1.DocumentText = contentHead.Replace("{subject}", "0").Replace("{color}", "#F3F3F3") ;
           // lion.IniWriteValue("app", "host", "http://127.0.0.1:5000");
            hostUrl = lion.ReadString("app", "host", "https://127.0.0.1:5001");
        }

     
        public string accesstoken = "123456";
       public string subject = "0";
        public string hostUrl = "https://127.0.0.1:5233";
        bool isSubject = false;
            
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            string rst = "";
            string message = txtSendMsg.Text.Trim();

            
            try
            {
                subject = webBrowser1.Document.InvokeScript("eval", new object[] { "jssubject" }).ToString();
            }
            catch {
               
            }
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("message is null !", "error");
                txtSendMsg.Focus();
                return;
            }
          if (message.ToLower().StartsWith("faq"))
            {
                rst = "<a onclick=setsubject(1) href='#'>1.PKI FAQ</a><br />";
                rst = rst + "<a onclick=setsubject(2)  href='#' >2.Certificate management</a><br /> ";
                rst = rst + "<a onclick=setsubject(3)  href='#' >3.Keyfactor FAQ</a><br /> ";
                rst = rst + "<a onclick=setsubject(4)  href='#'>4.CMS FAQ </a><br />...";
                isSubject = true;
            }
            else if (message.ToLower().StartsWith("token:"))
            {
                accesstoken = message.Split(':')[1];
                rst = "ok";
            }
            else if (message.ToLower().StartsWith("subject:"))
            {
                subject = message.Split(':')[1];
                rst = "ok";
            }
            else if (message.ToLower().StartsWith("help?")|| message.ToLower().StartsWith("help？"))
            {
               // subject = message.Split(':')[1];
                rst = "current ：\r\n"+ "   token：" + accesstoken+"\r\n    subject:"+subject;
            }
            else if (message.ToLower().StartsWith("clear") )
            {
                //txtGetMsg.Text = "";
                // subject = message.Split(':')[1];
                rst = "";
            }
            else
            {
                try
                {
                    rst = httpGet(message, subject);
                }
                catch (Exception ecp)
                {
                    MessageBox.Show(ecp.Message, "error");
                    return;
                }
            }
            /*
             
包括api，包括validation，authentication authorization, error handling, logging 
             */
            UpdateGetMsgTextBox(true, message, Color.Blue,subject);
            txtSendMsg.Text = "";
            UpdateGetMsgTextBox(false, rst, Color.Red, subject);



        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开
            return true;
        } 
        public string httpGet(string msg,string jssubject) {
            string url = hostUrl+"/question?question=" + msg + "&subject="+ jssubject + "&accesstoken="+ accesstoken;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback =
         new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult); 

            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseBody = "";
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                responseBody = streamReader.ReadToEnd();
            }
           return (responseBody);
        }
        private void UpdateGetMsgTextBox(bool sendMsg, string message, Color color,string  subject)
        {
            string appendText;
            if (sendMsg == true)
            {
                appendText = "Me:           \r\n" + System.DateTime.Now.ToString()
                    + Environment.NewLine
                    + message + Environment.NewLine;
                 string contentHeadv = contentHead.Replace("{subject}", subject).Replace("{color}", "#F0F0F0"); ;
                content =   content + "<div style='background:#EEE; color:#0F0'><em> Me:           <br />" + System.DateTime.Now.ToString()+ "<br /></em> <div style='background:#EEE; color:#F0F'>" + message + "</div><div><br />" ;

                webBrowser1.DocumentText = contentHeadv +  content;
            }
            else
            {
                appendText = "chatBot:           \r\n" + System.DateTime.Now.ToString()
                    + Environment.NewLine
                    + message + Environment.NewLine;
                string contentHeadv = contentHead.Replace("{subject}", subject).Replace("{color}", "#F3F3F3"); ;
                content = content   + "<div style='background:#BBB; color:#FFF'><em> chatBot:           <br />" + System.DateTime.Now.ToString() + "<br /></em><div style='background:#BBB; color:#00f'>" + message + "</div></div><br />";
                webBrowser1.DocumentText = contentHeadv +  content;

            }
           // int txtGetMsgLength = txtGetMsg.Text.Length;
          //  txtGetMsg.AppendText(appendText);
           // txtGetMsg.Select(txtGetMsgLength, appendText.Length - Environment.NewLine.Length*2 -message.Length);
           // txtGetMsg.SelectionColor = color;

           // txtGetMsg.ScrollToCaret();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSendMsg.Text = "token:";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtSendMsg.Text = "subject:";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtSendMsg.Text = "faq";
            btnSendMsg.PerformClick();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
            webBrowser1.Document.Window.ScrollTo(new Point(0, webBrowser1.Document.Body.ScrollRectangle.Height));
            
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK) { 
            this.BackColor = colorDialog.Color;
             }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            content = "";
            webBrowser1.DocumentText = contentHead.Replace("{subject}", subject).Replace("{color}", "#F3F3F3") + content;
        }
    }
}