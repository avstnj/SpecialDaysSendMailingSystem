using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SpecialDaysSendMailingSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG

            General general = new General();
            WindowsServiceCaller wsc = new WindowsServiceCaller(general);
            wsc.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            General general = new General();
            if (Environment.UserInteractive)
                ServiceStarter.Run(
                    new string[]
                    {
                        ConfigurationManager.AppSettings["path"]
                    }, general
                    );
            else
                ServiceBase.Run(new WindowsServiceCaller(general));
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new WindowsServiceCaller(deviceStatus)
            //};
            //ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
