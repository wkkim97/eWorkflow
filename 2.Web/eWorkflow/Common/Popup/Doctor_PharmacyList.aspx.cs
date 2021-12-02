using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNSoft.eWF.FrameWork.Web;
using DNSoft.eW.FrameWork;
using Bayer.eWF.BSL.Common.Dao;
using Bayer.eWF.BSL.Common.Mgr;
using Bayer.eWF.BSL.Common.Dto;

public partial class Common_Popup_Doctor_PharmacyList : DNSoft.eWF.FrameWork.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var bu = Request["bu"].NullObjectToEmptyEx();

            this.hddBu.Value = bu;
            if (hddBu.Value != "CC")
            {
                this.RadrdoPharmacy.Visible = false;
                this.RadrdoDoctor.Checked = true;               
            }
            RadioEvent();
        }
       
    }

    protected void RadrdoDoctor_CheckedChanged(object sender, EventArgs e)
    {
        RadioEvent();       
    }

    protected void RadGrdPharmacy_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.RadGrdPharmacy.DataSource = GetPharmacyList(this.radTxtKeyword.Text);
    }

    protected void RadGrdDoctor_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.RadGrdDoctor.DataSource = GetDoctorList(this.radTxtKeyword.Text);
        
    }

    private void RadioEvent()
    {
        if (this.RadrdoDoctor.Checked == true)
        {
            SelectDoctor("");        
             this.docter.Attributes.CssStyle.Add("Display", "block");
             this.pharmacy.Attributes.CssStyle.Add("Display", "none");
             this.hddRadioValue.Value = RadrdoDoctor.Value;
        }

        if (this.RadrdoPharmacy.Checked == true)
        {
            selectPharmacy("");
            this.docter.Attributes.CssStyle.Add("Display", "none");
            this.pharmacy.Attributes.CssStyle.Add("Display", "block");
            this.hddRadioValue.Value = RadrdoPharmacy.Value;
        }        
    }

    #region Pharmacy
    private void selectPharmacy(string keyword)
    {
        try
        {
            this.RadGrdPharmacy.DataSource = GetPharmacyList(keyword);
            this.RadGrdPharmacy.DataBind();
        }
        catch (Exception ex)
        {
            this.errorMessage = ex.ToString();
        }
    }

    private List<DTO_PHARMACY> GetPharmacyList(string keyword)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                return mgr.SelectPharmacy(keyword);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    #endregion

    #region Doctor
    private void SelectDoctor(string keyword)
    {
        try
        {
            this.RadGrdDoctor.DataSource = GetDoctorList(keyword);
            this.RadGrdDoctor.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private List<DTO_DOCTOR> GetDoctorList(string keyword)
    {
        try
        {
            using (CommonMgr mgr = new CommonMgr())
            {
                return mgr.SelectDoctor(keyword);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   
    #endregion

    protected void radBtnSearch_Click(object sender, EventArgs e)
    {
        if (this.RadrdoDoctor.Checked == true)
            SelectDoctor(this.radTxtKeyword.Text);
        else if (this.RadrdoPharmacy.Checked == true)
            selectPharmacy(this.radTxtKeyword.Text);
        else
            this.informationMessage = "Please Select Term";
    }
}