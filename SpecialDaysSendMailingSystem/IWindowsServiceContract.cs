using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialDaysSendMailingSystem
{
    public interface IWindowsServiceContract
    {
        void OnStart(string[] args);
        void OnStop();
    }
}
