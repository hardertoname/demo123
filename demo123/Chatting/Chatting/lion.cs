using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace hely
{
    internal class lion
	{

        public static string root = "...222";

		public static string root1 = "";

		public static string domain1 = "m";

		public static string appid = "demo";

		public static string appid_admin = "2";

		public static string appMac = "";
  
		public static string currDir = "";

		public static string server = ReadString("app", "server");

		public static List<string> userPanfu = new List<string>();

		//internal static string headTextA;

		//internal static string headTextB;

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

		public static string ReadString(string section, string key, string def = "")
		{
			return ReadString(section, key, def, getCurrdir());
		}
	
	 

		public static string ReadString(string section, string key, string def, string filePath)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			try
			{
				GetPrivateProfileString(section, key, def, stringBuilder, 1024, filePath);
			}
			catch
			{

			}
			if (stringBuilder.ToString().Length > 0)
			{
				return stringBuilder.ToString();
			}
			return "";
        }
   
	 
 
 
		private static string KeyPath;
		internal static bool isMainFormAlive = false;

 

 
        internal static string ClientUserSeq;
         internal static string currIP;
        internal static string clientIsZero;
        internal static string startWeb;
        internal static bool isOpenNewForm;

        // rsa ¹«Ô¿¼ÓÃÜ 


        public static string getCurrdir()
		{
			if (currDir.Length == 0)
			{
				FileInfo f1 = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
				  
				currDir = f1.DirectoryName + "/appConfig.lion";
			}
			return currDir;
		}

		public static string getCurrpath()
		{
			FileInfo f1 = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
			return f1.DirectoryName;
			
		}

		public static void IniWriteValue(string Section, string Key, string Value)
		{
			IniWriteValue(Section, Key, Value, getCurrdir());
		}
 

		public static void IniWriteValue(string Section, string Key, string Value, string filePath)
		{
			try
			{
				WritePrivateProfileString(Section, Key, Value, filePath);
			}
			catch (Exception)
			{
			}
		}

 
		public static string txtRead(string path)
		{
			StreamReader streamReader = new StreamReader(path, Encoding.Default);
			return streamReader.ReadLine().ToString();
		}

		public static string txtReadAll(string path)
		{
			try
			{
				string text = "";
				StreamReader streamReader = new StreamReader(path, Encoding.UTF8);
				text = streamReader.ReadToEnd().ToString();
				streamReader.Close();
				return text.Replace("\n", " ");
			}
			catch (Exception)
			{
				return "";
			}
		}

		public static void Write(string title, string filename)
		{
			try
			{
				FileStream fileStream = new FileStream(filename.Replace("|", "").Replace(":", "").Replace("\"", "")
					.Replace(">", "")
					.Replace("<", ""), FileMode.Append);
				byte[] bytes = Encoding.Default.GetBytes(title);
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush();
				fileStream.Close();
			}
			catch (Exception ex)
			{
				ex.ToString();
				string text = ex.ToString();
			}
		}

		public static void userLog(string log)
		{
			Write2(DateTime.Now.ToString("yyyyMMddHHmmss") + ":" + log+"\r\n", "log.txt");
		}

		public static void Write2(string title, string filename)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(filename);
				if (fileInfo.Exists)
				{
					//fileInfo.Delete();
				}
				else {
					fileInfo.CreateText().Close();

				}
			}
			catch (Exception)
			{
			}
			try
			{
				FileStream fileStream = new FileStream(filename, FileMode.Append);
				byte[] bytes = Encoding.Default.GetBytes(title);
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush();
				fileStream.Close();
			}
			catch (Exception ex2)
			{
				ex2.ToString();
				string text = ex2.ToString();
			}
		}
        public static void WriteTrunc(string title, string filename)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filename);
                if (fileInfo.Exists)
                {
                    //fileInfo.Delete();
                }
                else
                {
                    fileInfo.CreateText().Close();

                }
            }
            catch (Exception)
            {
            }
            try
            {
                FileStream fileStream = new FileStream(filename, FileMode.Truncate);
                byte[] bytes = Encoding.Default.GetBytes(title);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
                fileStream.Close();
            }
            catch (Exception ex2)
            {
                ex2.ToString();
                string text = ex2.ToString();
            }
        }
        public static void Writeline(string title, string filename)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(filename);
				if (!fileInfo.Exists)
				{
					fileInfo.Create();
				}
				FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Append);
				byte[] bytes = Encoding.Default.GetBytes(title);
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush();
				fileStream.Close();
				fileStream.Dispose();
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
		}
        public static string httpGet(string host)
        {
            try
            {
                Encoding encoding = Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
                request.Method = "post";
                request.KeepAlive = true;
                string str = "";
                byte[] buffer = encoding.GetBytes(str);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return "[]";
            }
        }

        public static string httpPost(string host)
        {
            try
            {
                Encoding encoding = Encoding.Default;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
                request.Method = "post";
                request.KeepAlive = true;
                string str = "";
                byte[] buffer = encoding.GetBytes(str);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return "[]";
            }
        }

        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
		{
			byte[] array = new byte[4];
			new RNGCryptoServiceProvider().GetBytes(array);
			Random random = new Random(BitConverter.ToInt32(array, 0));
			string text = null;
			string text2 = custom;
			if (useNum)
			{
				text2 += "0123456789";
			}
			if (useLow)
			{
				text2 += "abcdefghijklmnopqrstuvwxyz";
			}
			if (useUpp)
			{
				text2 += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			}
			if (useSpe)
			{
				text2 += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
			}
			for (int i = 0; i < length; i++)
			{
				text += text2.Substring(random.Next(0, text2.Length - 1), 1);
			}
			return text;
		}

		public static string Base64Encode(string source)
		{
			return Base64Encode(Encoding.UTF8, source);
		}

		public static string Base64Encode(Encoding encodeType, string source)
		{
			string empty = string.Empty;
			byte[] bytes = encodeType.GetBytes(source);
			try
			{
				return Convert.ToBase64String(bytes);
			}
			catch
			{
				return source;
			}
		}

		public static string Base64Decode(string result)
		{
			return Base64Decode(Encoding.UTF8, result);
		}

		public static string ByteArrayToString(byte[] arrInput)
		{
			StringBuilder stringBuilder = new StringBuilder(arrInput.Length);
			for (int i = 0; i < arrInput.Length; i++)
			{
				stringBuilder.Append(arrInput[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		public static string Base64Decode(Encoding encodeType, string result)
		{
			string empty = string.Empty;
			byte[] bytes = Convert.FromBase64String(result);
			try
			{
				return encodeType.GetString(bytes);
			}
			catch
			{
				return result;
			}
		}

		public static string parseSerialFromDeviceID(string deviceId)
		{
			string[] array = deviceId.Split('\\');
			int num = array.Length - 1;
			string[] array2 = array[num].Split('&');
			return array2[0];
		}

		public static string getValueInQuotes(string inValue)
		{
			int num = inValue.IndexOf("\"");
			int num2 = inValue.IndexOf("\"", num + 1);
			return inValue.Substring(num + 1, num2 - num - 1);
		}
	}
}
