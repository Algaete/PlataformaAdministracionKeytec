using LogsAdms.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LogsAdms.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly ILog logger;

        public DownloadController(ILog logger)
        {
            this.logger = logger;
        }
        // GET: Home 
       
        [HttpGet("/ftp")]
        public ActionResult<string> DownloadFile()
        {
            logger.Information("Nlog funcionando");
            string FtpServer = "ftp://waws-prod-cq1-015.ftp.azurewebsites.windows.net/site/wwwroot/LogsPrueba/";
            string username = @"key-api-1-qa\$key-api-1-qa";
            string password = "lL8P5bxja2WsvWwEenpxzaqXuDNFJ8q8pENoF2Kru80vqcBdzaqkvXvt1i0n";
            string localpath = @"C:\Users\agaet\source\repos\LogsAdms\LogsAdms\DescargaFTP/descarga.txt";
            var returnValue = "Archivo ftp descargado \n";
            Console.Write("hola");

            try { 
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FtpServer);
                request.Credentials = new NetworkCredential(username, password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fileName3 = streamReader.ReadLine();
                List<string> directories = new List<string>();

                while (fileName3 != null && fileName3 != "testing12")
                {
                    //cambiar a la extension que quieras
                    if (Path.GetExtension(fileName3) == ".txt") //or .xlsx// .png // .jpg etc.
                    {
                        directories.Add(fileName3);
                    }

                    fileName3 = streamReader.ReadLine();

                }

                streamReader.Close();


                using (WebClient ftpClient = new WebClient())
                {
                    ftpClient.Credentials = new NetworkCredential(username, password);

                    for (int i = 0; i <= directories.Count - 1; i++)
                    {
                        if (directories[i].Contains("."))
                        {

                            string path = FtpServer + directories[i].ToString();
                            string trnsfrpth = localpath + directories[i].ToString();
                            ftpClient.DownloadFile(path, trnsfrpth);
                            if (!string.IsNullOrEmpty(trnsfrpth))
                            {
                                StreamReader sr = new StreamReader(trnsfrpth);
                                //string contenido = sr.ReadToEnd();
                                string contenidoErrores = sr.ReadLine();
                                while(!string.IsNullOrEmpty(contenidoErrores))
                                {

                                }
                                sr.Close();
                                return contenidoErrores;
                            }
                            return returnValue;
                        }
                    }
                }
            }catch(Exception ex)
            {
                logger.Error("Error al conectar al servidor FTP" + ex.Message);
                return returnValue;
            }
            return returnValue;
        }
    }
}

/*public ActionResult DownLoadFile()
{
    try { 
        // objeto que sera usado para comunicarse con el servidor FTP
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://waws-prod-cq1-015.ftp.azurewebsites.windows.net/site/wwwroot/LogsPrueba/2020-02-11_Info.txt");
        request.Method = WebRequestMethods.Ftp.DownloadFile;

        // This example assumes the FTP site uses anonymous logon.
        request.Credentials = new NetworkCredential("key-api-1-qa\$key-api-1-qa", "lL8P5bxja2WsvWwEenpxzaqXuDNFJ8q8pENoF2Kru80vqcBdzaqkvXvt1i0n");

        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        Stream responseStream = response.GetResponseStream();

        StreamReader reader = new StreamReader(responseStream);


        Console.WriteLine(reader.ReadToEnd());
        FileStream fileStream = new FileStream(localDestinationFilePath, FileMode.Create);

        Console.WriteLine($"Download Complete, status {response.StatusDescription}");


        reader.Close();
        response.Close();
        return View(new List<LogersModel>());
    }
    catch
    {
        logger.Error("No conecta con servidor FTP");
    }
}*/
