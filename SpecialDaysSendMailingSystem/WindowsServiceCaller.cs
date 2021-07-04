using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SpecialDaysSendMailingSystem
{
    public partial class WindowsServiceCaller : ServiceBase
    {
        IWindowsServiceContract _winSrv;
        public WindowsServiceCaller(IWindowsServiceContract winSrv)
        {
            _winSrv = winSrv;
        }
        private void InitializeComponent()
        {
            this.ServiceName = "WindowsServiceWithEntityFramework_AsisPortal";
        }
        protected override void OnStart(string[] args)
        {
            _winSrv.OnStart(args);
        }
        protected override void OnStop()
        {
            _winSrv.OnStop();
        }
        public void OnDebug()
        {
            _winSrv.OnStart(null);
        }
    }
}
