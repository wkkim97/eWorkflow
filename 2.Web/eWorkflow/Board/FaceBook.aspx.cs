using Bayer.eWF.BSL.Configuration;
using Bayer.eWF.BSL.Configuration.Mgr;
using Bayer.eWF.BSL.Configuration.Dao;
using Bayer.eWF.BSL.Configuration.Dto;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using DNSoft.eWF.FrameWork.Data.EF;


public partial class Facebook_in : DNSoft.eWF.FrameWork.Web.PageBase
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            
        }

        


        protected void grdSearch_NeedDataSource_NEW(object sender, EventArgs e)
        {
            
            try
            {
                    using (context = new ConfigurationContext())
                    {
                        SqlParameter[] parameters = new SqlParameter[1];
                        parameters[0] = new SqlParameter("@KEYWOARD", "BKKWK");
                        List<DTO_USER_LIST> beforeApprovers = context.Database.SqlQuery<DTO_USER_LIST>(ConfigurationContext.USP_SELECT_FACEBOOK, parameters).ToList();
                        
                        this.radGrad1.DataSource = beforeApprovers;
                        this.radGrad1.DataBind();
                

                    }
                
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message +"EEEEEEEE";
            }
        }
        public class DTO_USER_LIST
        {
            public string FULL_NAME { get; set; }
            public string CONTACT { get; set; }
            public string USER_ID { get; set; }

        }

        protected WorkflowContext context = null;


        
    }
