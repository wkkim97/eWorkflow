using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace Bayer.WinSvc.SendNoticeMail
{
    [RunInstaller(true)]
    public class SendNoticeMailServiceInstaller : Installer
    {
        public SendNoticeMailServiceInstaller()
        {
            System.ServiceProcess.ServiceProcessInstaller oProcInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            System.ServiceProcess.ServiceInstaller oServiceInstaller = new System.ServiceProcess.ServiceInstaller();

            oProcInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            oServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Disabled;

            oServiceInstaller.ServiceName = "Bayer.WinSvc.SendNoticeMail";
            oServiceInstaller.DisplayName = "Bayer.WinSvc.SendNoticeMail";
            oServiceInstaller.Description = "장기간 미 결재자 알림 메일 발송 서비스";

            this.Installers.Add(oProcInstaller);
            this.Installers.Add(oServiceInstaller);
        }
    }
}
