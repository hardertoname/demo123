using demoBot.Controllers;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Xml.Linq;

namespace demobotSwaggerr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
         
        [HttpGet(Name = "auth1")]
        public string Get(string name, string pwd)
        {
            return ATH.validation(name, pwd) + "";
        }
        [HttpPut(Name = "auth2")]
        public string Put(string name, string pwd)
        {

            {
                List<MySqlParameter> par = new List<MySqlParameter>();
                MySqlParameter p1 = new MySqlParameter("@name", MySqlDbType.VarChar);
                p1.Value = name;
                MySqlParameter p2 = new MySqlParameter("@pwd", MySqlDbType.VarChar);
                p2.Value = pwd;

                MySqlParameter p3 = new MySqlParameter("@endtime", MySqlDbType.VarChar);
                p2.Value = pwd;

                MySqlParameter p4 = new MySqlParameter("@authtoken", MySqlDbType.VarChar);
                p2.Value = pwd;


                par.Add(p1);
                par.Add(p2);
                par.Add(p3);
                par.Add(p4);
                MysqlClient.MySqlHelper.ExecuteNonQuery(MysqlClient.MySqlHelper.Conn,
                    System.Data.CommandType.Text,
                    "delete from  usertab where name = @name ", par.ToArray());


                var tab = MysqlClient.MySqlHelper.GetDataSet(
                    MysqlClient.MySqlHelper.Conn,
                    System.Data.CommandType.Text,
                    "insert into usertab(name,pwd,times,endtime,authtoken) values(@name,@pwd,'9999',@endtime,@authtoken)",
                 par.ToArray());

                try
                {
                    if (tab.Tables[0].Rows.Count > 0)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "Error";
                    }

                }
                catch
                {
                    return name + "" + pwd;
                }
            }
        }
    }
}