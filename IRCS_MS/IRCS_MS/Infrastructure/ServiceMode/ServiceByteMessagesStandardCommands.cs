using IRCS_MS.Infrastructure.XmlHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure.ServiceMode
{
    public class ServiceByteMessagesStandardCommands
    {
        private void ResetByteMessages()
        {
            ByteMessageBuilderRepository.ClearArray(ByteMessages.Instance.MeasureModeOutgoing);
        }

        public void SystemBusPsuOn()
        {
            ResetByteMessages();
        }
        
        public void SystemBusPsuOff()
        {
            ResetByteMessages();
        }

        public void SystemBusResetOn()
        {
            ResetByteMessages();
        }

        public void SystemBusResetOff()
        {
            ResetByteMessages();
        }

        public void SystemBusModulInit()
        {
            ResetByteMessages();
        }

        public void SystemBusChange()
        {
            ResetByteMessages();
        }
        public void SystemBusWrite()
        {
            ResetByteMessages();
        }
        public void SystemBusRead()
        {
            ResetByteMessages();
        }

        //Function Generator
        public void FuncGenOn()
        {

        }
        
        public void FuncGenOff()
        {

        }
        
        //Analyser Generator
        public void AnalyserGenRun()
        {

        }


    }
}
