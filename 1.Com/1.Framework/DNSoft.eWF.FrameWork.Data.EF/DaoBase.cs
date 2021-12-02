using System;

namespace DNSoft.eWF.FrameWork.Data.EF
{
    /// <summary>
    /// Data Access Object 최상위 클래스
    /// </summary>
    public class DaoBase : IDisposable
    {
        /// <summary>
        /// WorkflowContext instance
        /// </summary>
        protected WorkflowContext context = null;

        /// <summary>
        /// Implement IDisposable
        /// </summary>
        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }
    }
}
