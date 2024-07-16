using demoBot.Controllers;
using Jint;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net;

namespace demobotSwaggerr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
       
        private string userSubject = "0";

        [HttpGet(Name = "Question")]
        public string Get(string question, string subject = "0", string accesstoken = "")
        {
            string times = "0";
            string user = "";

            if (checkBlackWord(question))
            {
                ATH.logging(user, question, "black" + accesstoken, times);
                return "black world ";
            }
            if (ATH.checkAuth(accesstoken, ref times, ref user) == false)
            {
                ATH.logging(user, question, "no auth" + accesstoken, times);

                return "no auth ";
            }
            else
            {
                question = question.Replace(" ", "+");
                var engine = new Engine();
                try
                {
                    var result = engine.Invoke("eval", question).AsNumber();
                    ATH.logging(user, question, result, times);
                    return result + "";
                }
                catch (Exception ex)
                {

                }


                string rest = queryDataBase(user, question, subject);
                if (rest.Length >= 1)
                {
                    ATH.logging(user, question, rest, times);
                    return rest;
                }
                else
                {
                    string rst = queryWithSubject(user, question, subject);
                    ATH.logging(user, question, rst, times);
                    return rst;

                }
            }
        }

        private bool checkBlackWord(string question)
        {

            List<MySqlParameter> par = new List<MySqlParameter>();
            MySqlParameter p2 = new MySqlParameter("@question", MySqlDbType.VarChar);
            p2.Value = "%" + question + "%";


            par.Add(p2);
            var tab = MysqlClient.MySqlHelper.GetDataSet(
                MysqlClient.MySqlHelper.Conn,
                System.Data.CommandType.Text,
                "select * from blackTab where   question like @question ",
             par.ToArray());

            try
            {
                if (tab.Tables[0].Rows.Count > 0)
                { //数据库有完整的返回数据 
                    return true;
                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {


            }
            return false;


            // if (question.IndexOf("黑名单") >=0|| question.IndexOf("杀") >= 0 || question.IndexOf("事件") >= 0|| question.IndexOf("国家") >= 0)
            // {
            //     return true;
            // }
            // else {
            //     return false;
            // }
        }


        private string queryDataBase(string user, string question, string subject)
        {
            List<MySqlParameter> par = new List<MySqlParameter>();
            MySqlParameter p1 = new MySqlParameter("@user", MySqlDbType.VarChar);
            p1.Value = user;


            MySqlParameter p2 = new MySqlParameter("@question", MySqlDbType.VarChar);
            p2.Value = "%" + question + "%";

            MySqlParameter p3 = new MySqlParameter("@q3", MySqlDbType.VarChar);
            p3.Value = "" + question + "";

            par.Add(p1);
            par.Add(p2);
            par.Add(p3);
            var tab = MysqlClient.MySqlHelper.GetDataSet(
                MysqlClient.MySqlHelper.Conn,
                System.Data.CommandType.Text,
                "select * from questionTab where userName = @user and question like @question order  by  abs(length(question)-length(@q3)) asc",
             par.ToArray());

            try
            {
                if (tab.Tables[0].Rows.Count > 0)
                { //数据库有完整的返回数据 
                    return tab.Tables[0].Rows[0].ItemArray[3].ToString();
                }
                else
                {

                    return "";
                }

            }
            catch (Exception ex)
            {
                ATH.errorHandling(user, question, "", ex);
                return "";

            }
        }

        private string queryWithSubject(string user, string question, string subject)
        {
            if (subject == "0")
            {

                //  
                return question + "request ：" + question + "";
            }
            else if (subject == "1")
            {
                return question + "request ：" + question.Replace(" ", "    ") + "\t[from-PKI FAQ]";
                //  
            }
            else if (subject == "2")
            {
                return question + "request ：" + question.ToUpper() + "\tfrom-[Certificate management]";
                //  
            }
            else if (subject == "3")
            {
                return question + "request ：" + question.ToLower() + "\tfrom-[Keyfactor FAQ]";
                //  
            }
            else if (subject == "4")
            {
                return question + "request ：" + (question) + "\tfrom-[CMS FAQ]";
                //  
            }
            else
            {
                question = question.Replace("+", " ");
                return " " + question + "?";
            }
        }
        private string httpGet(string wd)
        {

            string url = "https://www.baidu.com/s?wd=" + wd;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseBody = "";
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                responseBody = streamReader.ReadToEnd();
            }
            return (responseBody);
        }

    }
}