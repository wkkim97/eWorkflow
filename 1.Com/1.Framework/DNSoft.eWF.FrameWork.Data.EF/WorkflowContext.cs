using System.Data.Entity;

namespace DNSoft.eWF.FrameWork.Data.EF
{
    public class WorkflowContext : DbContext
    {
        public WorkflowContext()
            : this(DNSoft.eW.FrameWork.eWBase.GetConfig("//ServerInfo/SQLServer/ConnectionString"))
        {
        }

        public WorkflowContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
