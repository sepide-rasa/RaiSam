using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Transports;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Timers;
using RaiSam.Models;

namespace RaiSam
{
    [HubName("S_hub")]
    public class SignalrHub : Hub
    {
        public void ReloadTickets()
        {
            if (Clients != null)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
                context.Clients.All.LoadTickets();
            }
        }
        public void Send(string message)
        {
            if (Clients != null)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
                context.Clients.All.broadcastMessage(message);
            }
        }
        //public void InsertLog(string TransactiontTypeName, int TransactionGroupId, bool Status)
        //{
        //    if (Clients != null)
        //    {
        //        var context = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
        //        context.Clients.All.broadcastInsertLog(TransactiontTypeName, TransactionGroupId, Status);
        //    }
        //}
        public void ReloadLog()
        {
            if (Clients != null)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
                context.Clients.All.broadcastReloadLog();
            }
        }
        public void AddProgress(string Title, decimal Percent, int inputId,int Id, int st)
        {
            if (Clients != null)
            {
              //  Models.RaiSamEntities m = new RaiSamEntities();
              //  m.prs_tblProgressInsert(Title, Percent, inputId);
               
                var context = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
                context.Clients.All.broadcastAddProgress(Title, Percent, Id,st,inputId);
            }
        }
        public void UpdateSmsPanelInfo(string sharj,int Er, int inputId)
        {
            if (Clients != null)
            {
                //  Models.RaiSamEntities m = new RaiSamEntities();
                //  m.prs_tblProgressInsert(Title, Percent, inputId);

                var context = GlobalHost.ConnectionManager.GetHubContext<SignalrHub>();
                context.Clients.All.UpdateSmsPanelInfo(sharj,Er, inputId);
            }
        }
    }

    //[HubName("Ch_hub")]
    //public class SignalrCheckListHub : Hub
    //{
    //    public void ReloadCheckList()
    //    {
    //        if (Clients != null)
    //        {
    //            var context = GlobalHost.ConnectionManager.GetHubContext<SignalrCheckListHub>();
    //            context.Clients.All.LoadCheckList();
    //        }
    //    }
    //}
}