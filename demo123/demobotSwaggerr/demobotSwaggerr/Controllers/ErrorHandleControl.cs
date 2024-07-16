using demoBot.Controllers;
using hely;
using Jint;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System.Net;

namespace demobotSwaggerr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorHandleControl : ControllerBase
    {
       
        [HttpGet(Name = "ErrorHandle")]
        public  string  errorHandling(string user, string question, string times, Exception ex)
        {
            lion.Write2(user + "\t" + question + "\t" + ex.Message.ToString() + "\r\n", "log.txt");
            try { 
            

                ATH.logging(user, question, ex.Message.ToString(), times);
            }
            catch { }
            return "OK";
        }



    }
}