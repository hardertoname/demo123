using demoBot.Controllers;
using Jint;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net;

namespace demobotSwaggerr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidationControl : ControllerBase
    {
        
        [HttpGet(Name = "Validation")]
        public   string validation(string name, string pwd)
        {
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
                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            catch
            {
                return "false";
            }

        }



    }
}