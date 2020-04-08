using IRCS_MS.Model;
using IRCS_MS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure.Message
{

    public class IncomingMessageProcessor
    {
        private readonly MainViewModel _mainViewModel;

        public IncomingMessageProcessor(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }



    }
}
