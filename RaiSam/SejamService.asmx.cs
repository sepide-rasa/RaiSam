using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RaiSam.Models;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;

namespace RaiSam
{
    /// <summary>
    /// Summary description for SejamService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SejamService : System.Web.Services.WebService
    {
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];


        [WebMethod]
        public string TestService(string username, string password)
        {
            if (username == "s" && password == "e")
                return "Helloo";
            else
                return "Nooo";

        }
        static string GetToken()
        {
            string requestUri = "https://externalapi.rai.ir/api/v1/Users/TokenBody";
            var bodyContent = new
            {
                username = "Made12_PCS",
                password = "Made12_PCS.IT.14020708.583.PO2!qaS32a1Yu6h#v9bg$ed330",
                grant_type = "password"
            };
            var myJson = JsonConvert.SerializeObject(bodyContent);


            HttpClient client = new HttpClient();
            var result = new StringContent(myJson.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(requestUri, result).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            var token = json.access_token;
            return token;
        }
        [WebMethod]
        public MoveTranDetails GetMoveTranDetails(string FromDate, string ToDate, int? MoveTranId, int? CompanyId)
        {

            var ErMsg = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            MoveTranDetails Details = new MoveTranDetails();
            try
            {

                string requestUri = "https://externalapi.rai.ir/api/v1/PCS/GetMoveTranDetails?FromDate=" + FromDate + "&ToDate=" + ToDate;
                MoveTranDetails Result = new MoveTranDetails();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", GetToken()));
                HttpResponseMessage response = client.GetAsync(requestUri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                ErMsg = 1;
                Result = Newtonsoft.Json.JsonConvert.DeserializeObject<MoveTranDetails>(content);
                ErMsg = 2;
                if (Result.isSuccess)
                {
                    ErMsg = 3;
                    Details = Result;
                    Details.Err = 0;
                }
                else
                {
                    ErMsg = 4;
                    Details.Err = 1;
                    Details.Msg = "-خطایی با شماره " + Result.statusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                    if (Result.statusCode == 1)
                        Details.Msg = Details.Msg + "(نامعتبر بودن ورودی یا رهگیری یا منقضی شدن کد رهگیری)";
                    return Details;
                }

                return Details;
            }
            catch (Exception x)
            {
                Details.Err = 1;
                Details.Msg = "قطع ارتباط با وب سرویس ";
                return Details;
            }

        }
        [WebMethod]
        public static MoveTranDetails GetMoveTranDetailsInApp(string FromDate, string ToDate, int? MoveTranId, int? CompanyId)
        {

            var ErMsg = 0;
            Models.RaiSamEntities m = new RaiSamEntities();
            MoveTranDetails Details = new MoveTranDetails();
            try
            {

                string requestUri = "https://externalapi.rai.ir/api/v1/PCS/GetMoveTranDetails?FromDate=" + FromDate + "&ToDate=" + ToDate;
                MoveTranDetails Result = new MoveTranDetails();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", GetToken()));
                HttpResponseMessage response = client.GetAsync(requestUri).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                ErMsg = 1;
                Result = Newtonsoft.Json.JsonConvert.DeserializeObject<MoveTranDetails>(content);
                ErMsg = 2;
                if (Result.isSuccess)
                {
                    ErMsg = 3;
                    Details = Result;
                    Details.Err = 0;
                }
                else
                {
                    ErMsg = 4;
                    Details.Err = 1;
                    Details.Msg = "-خطایی با شماره " + Result.statusCode + " اتفاق افتاده است. لطفا جهت برطرف شدن خطا با پشتیبانی تماس بگیرید.";
                    if (Result.statusCode == 1)
                        Details.Msg = Details.Msg + "(نامعتبر بودن ورودی یا رهگیری یا منقضی شدن کد رهگیری)";
                    return Details;
                }

                return Details;
            }
            catch (Exception x)
            {
                Details.Err = 1;
                Details.Msg = "قطع ارتباط با وب سرویس ";
                return Details;
            }

        }
        [WebMethod]
        public void CallWebServiceRaja()
        {



            //Reading input values from console
            string a = "railway";
            string b = "321987";
            //string a = "s";
            //string b = "e";
            //Calling InvokeService method
            InvokeService(a, b);
        }
        public void InvokeService(string a, string b)
        {
            //Calling CreateSOAPWebRequest method
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
             <soap:Body>
                <wbsLogin xmlns=""http://tempuri.org/"">
                  <username>" + a + @"</username>
                  <password>" + b + @"</password>
                </wbsLogin>
              </soap:Body>
            </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Uploaded\rajaOut.txt", ServiceResult);
                    Console.WriteLine(ServiceResult);
                    Console.ReadLine();
                }
            }
        }
        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request
            //HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://madeh12.rai.ir/SejamService.asmx");
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://webservices.raja.ir/online2Services.asmx");
            //SOAPAction
            //Req.Headers.Add(@"SOAPAction:http://tempuri.org/TestService");
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/wbsLogin");
            //Content_type
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method
            Req.Method = "POST";
            //return HttpWebRequest
            return Req;
        }
    }
}
