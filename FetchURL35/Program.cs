using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using FetchURL35.Properties;

namespace FetchURL35
{
    class Program
    {
        private static Settings opc;

        static void Main(string[] args)
        {
            try
            {
                opc = Properties.Settings.Default;
                if (args.Length <= 0) throw new Exception("Falta la url en el argumento.");
                string url = string.Concat(args[0], "?key=true");

                HttpWebRequest defaultCredentials = (HttpWebRequest)WebRequest.Create(url);
                defaultCredentials.MaximumAutomaticRedirections = 4;
                defaultCredentials.MaximumResponseHeadersLength = 4;
                defaultCredentials.Timeout = 600000;

                if (opc.Dominio != "" && opc.usuario != "" && opc.password != "")
                    defaultCredentials.Credentials = new NetworkCredential(opc.usuario, opc.password, opc.Dominio);
                else
                    defaultCredentials.Credentials = CredentialCache.DefaultCredentials;

                using (HttpWebResponse response = (HttpWebResponse)defaultCredentials.GetResponse())
                {
                    Console.WriteLine("<!-- Content length is {0} -->", response.ContentLength);
                    Console.WriteLine("<!-- Content type is {0} -->", response.ContentType);
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string str1 = streamReader.ReadToEnd().ToString();                    
                        Console.WriteLine("<!-- Response stream received. -->");
                        Console.WriteLine(str1);
                        File.WriteAllText("log.txt", str1);
                    }
                }//fin using response            

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());                
            }



        }//fin Main
    }
}
