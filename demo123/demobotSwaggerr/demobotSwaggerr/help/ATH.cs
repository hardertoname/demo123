using hely;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Xml.Linq;

namespace demoBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ATH
    {
  
        public  static void logging(string user, string question, object solve, string times)
        {
            List<MySqlParameter> par = new List<MySqlParameter>();
            MySqlParameter p1 = new MySqlParameter("@user", MySqlDbType.VarChar);
            p1.Value = user;
            MySqlParameter p2 = new MySqlParameter("@question", MySqlDbType.VarChar);
            p2.Value = question;

            MySqlParameter p3 = new MySqlParameter("@solve", MySqlDbType.VarChar);
            p3.Value = solve;

            MySqlParameter p4 = new MySqlParameter("@times", MySqlDbType.VarChar);
            p4.Value = times;


            MySqlParameter p5 = new MySqlParameter("@operdt", MySqlDbType.VarChar);
            p5.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            par.Add(p1); par.Add(p2); par.Add(p3); par.Add(p4); par.Add(p5);


            var tab = MysqlClient.MySqlHelper.GetDataSet(
                MysqlClient.MySqlHelper.Conn,
                System.Data.CommandType.Text,
                "insert into questionlog(username,question,solve,operdt,lefttimes) values(@user,@question,@solve,@operdt,@times)",
             par.ToArray());

        }
     
        // 获取权限
        public static   bool checkAuth(string accesstoken, ref string times, ref string user)
        {
            List<MySqlParameter> par = new List<MySqlParameter>();
            MySqlParameter p1 = new MySqlParameter("@authtoken", MySqlDbType.VarChar);
            p1.Value = accesstoken;
            MySqlParameter p2 = new MySqlParameter("@dt2", MySqlDbType.VarChar);
            p2.Value = DateTime.Now.ToString("yyyy-MM-dd");



            par.Add(p1); par.Add(p2);
            var tab = MysqlClient.MySqlHelper.GetDataSet(
                MysqlClient.MySqlHelper.Conn,
                System.Data.CommandType.Text,
                "select * from usertab where authtoken = @authtoken and endtime>@dt2 and times>=0",
             par.ToArray());

            try
            {
                if (tab.Tables[0].Rows.Count > 0)
                { //数据库有完整的返回数据 
                    MysqlClient.MySqlHelper.ExecuteNonQuery(
              MysqlClient.MySqlHelper.Conn,
              System.Data.CommandType.Text,
              "update  usertab set times= times-1 where  authtoken = @authtoken and endtime>@dt2", par.ToArray());
                    times = tab.Tables[0].Rows[0].ItemArray[3].ToString();
                    user = tab.Tables[0].Rows[0].ItemArray[1].ToString();
                    return true;
                }
                else
                {

                    return false;
                }

            }
            catch
            {
                return false;

            }
        }
      
        public static void errorHandling(string user, string question, string times, Exception ex) {
            lion.Write2(user + "\t" + question + "\t" + ex.Message.ToString() + "\r\n", "log.txt");
            try
            {


                logging(user, question, ex.Message.ToString(), times);
            }
            catch { }

            }
        
        public static bool validation(string  name,string pwd) {
            List<MySqlParameter> par = new List<MySqlParameter>();
            MySqlParameter p1 = new MySqlParameter("@name", MySqlDbType.VarChar);
            p1.Value = name;
            MySqlParameter p2 = new MySqlParameter("@pwd", MySqlDbType.VarChar);
            p2.Value = pwd;

            par.Add(p1);
            par.Add(p2);
            var tab = MysqlClient.MySqlHelper.GetDataSet(
                MysqlClient.MySqlHelper.Conn,
                System.Data.CommandType.Text,
                "select * from usertab where name = @name and pwd= @pwd",
             par.ToArray());

            try
            {
                if (tab.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }

        }
    }
}
