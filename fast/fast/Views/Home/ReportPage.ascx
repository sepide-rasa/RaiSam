
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>

<%{
      
      //string[] kk=ViewBag.Tables;
      //NewFMS.DataSet.DataSet1 dt = new NewFMS.DataSet.DataSet1();
      //NewFMS.DataSet.DataSet_Com dt_Com = new NewFMS.DataSet.DataSet_Com();
      //NewFMS.DataSet.DataSet_ComTableAdapters.spr_GetDateTableAdapter Date = new NewFMS.DataSet.DataSet_ComTableAdapters.spr_GetDateTableAdapter();
      //NewFMS.DataSet.DataSet_ComTableAdapters.spr_tblFileSelectTableAdapter Filee = new NewFMS.DataSet.DataSet_ComTableAdapters.spr_tblFileSelectTableAdapter();
      //NewFMS.DataSet.DataSet1TableAdapters.spr_SelectNameKarbarRptTableAdapter name = new NewFMS.DataSet.DataSet1TableAdapters.spr_SelectNameKarbarRptTableAdapter();
      //NewFMS.DataSet.DataSet1TableAdapters.spr_TarazTableAdapter taraz = new NewFMS.DataSet.DataSet1TableAdapters.spr_TarazTableAdapter();

      
      //Date.Fill(dt_Com.spr_GetDate);
      //Filee.Fill(dt_Com.spr_tblFileSelect, "fldId", ViewBag.LogoId, 0);
      //name.Fill(dt.spr_SelectNameKarbarRpt, ViewBag.UserId, ViewBag.OrganId);
   //   taraz.Fill(dt.spr_Taraz, ViewBag.AzTarikh, ViewBag.TaTarikh, Convert.ToByte(ViewBag.salMali),Convert.ToByte(ViewBag.OrganId),Convert.ToInt32(ViewBag.AzLevel),Convert.ToInt32(ViewBag.TaLevel),Convert.ToInt32(ViewBag.AzSanad),Convert.ToInt32(ViewBag.TaSanad),Convert.ToInt32(ViewBag.StartNodeID), Convert.ToByte(ViewBag.sanadtype));


      //WebReport1.Report.RegisterData(dt_Com, "dataSetAccounting");
      //WebReport1.Report.RegisterData(dt, "dataSetAccounting");
      string Path = ViewBag.Path;
      WebReport1.Report.Load(Path);

      WebReport1.ID = ViewBag.RId;


      WebReport1.Report.SetParameterValue("aztarikh", ViewBag.AzTarikh);
      WebReport1.Report.SetParameterValue("tatarikh", ViewBag.TaTarikh);
      WebReport1.Report.SetParameterValue("salmaliID", ViewBag.salMali);
      WebReport1.Report.SetParameterValue("organid", ViewBag.OrganId);
      WebReport1.Report.SetParameterValue("azLevel", ViewBag.AzLevel);
      WebReport1.Report.SetParameterValue("tanLevel", ViewBag.TaLevel);
      WebReport1.Report.SetParameterValue("azsanad", ViewBag.AzSanad);
      WebReport1.Report.SetParameterValue("tasanad", ViewBag.TaSanad);
      WebReport1.Report.SetParameterValue("StartNodeID", ViewBag.StartNodeID);
      WebReport1.Report.SetParameterValue("sanadtype", ViewBag.sanadtype);
      
     
  
  } %>
<form id="Form1" runat="server" dir="ltr">
    
<%--</div>--%>
    <cc1:WebReport ID="WebReport1" runat="server" Height="100%" Width="100%" />
</form>

