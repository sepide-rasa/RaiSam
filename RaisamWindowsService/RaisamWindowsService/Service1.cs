using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace RaisamWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timScheduledTask = new System.Timers.Timer();

            timScheduledTask.Interval = 24 * 60 * 60 * 1000;    

            timScheduledTask.Enabled = true;

            timScheduledTask.Elapsed += new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);

        }
        void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\a.txt", true);
            //sw.WriteLine("vorood   ");
            //sw.Flush();
            //sw.Close();
            
            //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Uploaded\a.txt", "vorood   "+ "\n");
                try
                {
                RaiSamEntities m = new RaiSamEntities();
                var g = m.prs_GetDate().FirstOrDefault();
                // sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\a.txt", true);
                //sw.WriteLine("raft to: " + g.fldDateTime);
                //sw.Flush();
                //sw.Close();

              //  System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Uploaded\a.txt", "raft to: " + g.fldDateTime + "\n");
                    getSajamData();
                    //Select
                    var sms=m.prs_tblSafSMSSelect("Pass3Day", "", 0).ToList();
                    foreach (var item in sms)
                    {
                    //ersal payam
                        CallWebServiceSms(item.fldMatn, item.fldMobile);
                    //Update
                        m.prs_tblSafSMSUpdate(2, item.fldAshkhasId, item.fldCherkheFirstEghdamId, 1);
                        //insert mojadad hamun record
                        m.prs_tblSafSMSInsert(item.fldMatn,1,item.fldAshkhasId,item.fldCherkheFirstEghdamId, 1);
                        
                    }
                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;

                   //sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\a.txt", true);
                   //sw.WriteLine("errrr:   " + InnerException);
                   // sw.Flush();
                   // sw.Close();
                    RaiSamEntities m = new RaiSamEntities();
                    var g = m.prs_GetDate().FirstOrDefault();

                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));

                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, 1, "تاریخ: " + g.fldTarikh + "(sms For Cartabl)");
                }
                
            //sv.GetGroupPersonelInfo("1", 1);
        }

        protected override void OnStop()
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", true);
            sw.WriteLine("stopppp   ");
            sw.Flush();
            sw.Close();
        }

        void getSajamData()
        {
            try
            {
            RaiSamEntities m = new RaiSamEntities();
            var g = m.prs_GetDate().FirstOrDefault();

                var kk = "";
                SejamService.SejamService sv = new SejamService.SejamService();
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                try
                {
                    var MoveTran = sv.GetMoveTranDetails(g.fldTarikh, g.fldTarikh, null, null);
                  //  var MoveTran = GetMoveTranDetails(g.fldTarikh, g.fldTarikh, null, null);

                    List<MoveTranDetails> a = new List<MoveTranDetails>();
                    if (MoveTran.Err == 0)
                    {
                        foreach (var item in MoveTran.data)
                        {

                            if (/*item.PelakNo != 0 &&*/ item.SalonNo != 0)
                                m.prs_InsertFromWebServiceSajam(item.TranNo, item.MoveDate, item.MoveTime, item.PelakNo, item.SalonNo, item.SourceStation, item.TargetStation);

                        }
                        //sv.CallWebServiceRaja();
                    }
                    else
                    {
                        m.prs_tblErrorInsert(ErrorId, MoveTran.Msg, DateTime.Now, 1, "تاریخ: " + g.fldTarikh + "(SajamWinService1)");
                    }
                }
                catch (Exception x)
                {
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId1 = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    m.prs_tblErrorInsert(ErrorId1, InnerException, DateTime.Now, 1, "تاریخ: " + g.fldTarikh + "(SajamWinService2)");
                }



                m.Dispose();
               // sv.Dispose();
            }
            catch (Exception x)
            {
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                //StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\a.txt", true);
                //sw.WriteLine("errrr2:   " + InnerException);
                //sw.Flush();
                //sw.Close();

                RaiSamEntities m = new RaiSamEntities();

                System.Data.Entity.Core.Objects.ObjectParameter ErrorId2 = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));

                m.prs_tblErrorInsert(ErrorId2, InnerException, DateTime.Now, 1, "SajamWinService3");
                m.Dispose();
            }
        }
        public void CallWebServiceSms(string Matn, string Mobile)
        {

            RaiSamEntities m = new RaiSamEntities();
            var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();


            //Reading input values from console
            //string a = "s";
            //string b = "e";
            //Calling InvokeService method
            InvokeService(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, Mobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");
        }
        public void InvokeService(string cUserName, string cPassword, string cBody, string cSmsnumber, string cGetid, int nCMessage, int nTypeSent, string m_SchedulDate, string cDomainname, string cFromNumber, int nSpeedsms, int nPeriodmin, string cstarttime, string cEndTime)
        {
            //Calling CreateSOAPWebRequest method
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
             <soap:Body>
                <wbsLogin xmlns=""http://tempuri.org/"">
                    <cUserName>" + cUserName + @"</cUserName>
                      <cPassword>" + cPassword + @"</cPassword>
                      <cBody>" + cBody + @"</cBody>
                      <cSmsnumber>" + cSmsnumber + @"</cSmsnumber>
                      <cGetid>" + cGetid + @"</cGetid>
                      <nCMessage>" + nCMessage + @"</nCMessage>
                      <nTypeSent>" + nTypeSent + @"</nTypeSent>
                      <m_SchedulDate>" + m_SchedulDate + @"</m_SchedulDate>
                      <cDomainname>" + cDomainname + @"</cDomainname>
                      <cFromNumber>" + cFromNumber + @"</cFromNumber>
                      <nSpeedsms>" + nSpeedsms + @"</nSpeedsms>
                      <nPeriodmin>" + nPeriodmin + @"</nPeriodmin>
                      <cstarttime>" + cstarttime + @"</cstarttime>
                      <cEndTime>" + cEndTime + @"</cEndTime>
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
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Uploaded\smsOut.txt", ServiceResult);
                    Console.WriteLine(ServiceResult);
                    Console.ReadLine();
                }
            }
        }
        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request
            //HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://madeh12.rai.ir/SejamService.asmx");
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://sms.rai.ir/webservice/service.asmx");
            //SOAPAction
            //Req.Headers.Add(@"SOAPAction:http://tempuri.org/TestService");
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/SendSms");
            //Content_type
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method
            Req.Method = "POST";
            //return HttpWebRequest
            return Req;
        }

        ///////
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
      
        MoveTranDetails GetMoveTranDetails(string FromDate, string ToDate, int? MoveTranId, int? CompanyId)
        {

            var ErMsg = 0;
            RaiSamEntities m = new RaiSamEntities();
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

                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId2 = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));

                    m.prs_tblErrorInsert(ErrorId2, Details.Msg, DateTime.Now, 1, "SajamWinService4");

                    return Details;
                }

                return Details;
            }
            catch (Exception x)
            {
                Details.Err = 1;
                Details.Msg = "قطع ارتباط با وب سرویس ";
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId2 = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));

                m.prs_tblErrorInsert(ErrorId2, Details.Msg, DateTime.Now, 1, "SajamWinService5");
                return Details;
            }

        }
    }
}
