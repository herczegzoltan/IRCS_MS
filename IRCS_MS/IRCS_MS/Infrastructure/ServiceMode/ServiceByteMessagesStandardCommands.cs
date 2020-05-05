using IRCS_MS.Infrastructure.XmlHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure.ServiceMode
{
    public static class ServiceByteMessagesStandardCommands
    {
        public const string SRVMODON = "ServiceOn";
        public const string PSUON = "PsuOn";
        public const string PSUOFF = "PsuOff";


        private static void ResetByteMessages(byte[] array)
        {
            ByteMessageBuilderRepository.ClearArray(array);
        }

        public static void SetValueToOutGoingMassage(byte[] array, int index, string filterName)
        {
            ResetByteMessages(array);
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.ServiceModeOutgoing, 0, XmlFilterServiceMode.Instance.GetDefaultValueByName(SRVMODON));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.ServiceModeOutgoing, index, XmlFilterServiceMode.Instance.GetDefaultValueByName(filterName));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.ServiceModeOutgoing, 11, XmlFilterServiceMode.Instance.GetEOF());
        }

        //public void SystemBusPsuOn()
        //{
        //    ResetByteMessages();
        //    ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.ServiceModeOutgoing, 0, XmlFilterServiceMode.Instance.GetValueByName());
        //    //ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        //}

        //public void SystemBusPsuOff()
        //{
        //    ResetByteMessages();
        //}

        //public void SystemBusResetOn()
        //{
        //    ResetByteMessages();
        //}

        //public void SystemBusResetOff()
        //{
        //    ResetByteMessages();
        //}

        //public void SystemBusModulInit()
        //{
        //    ResetByteMessages();
        //}

        //public void SystemBusChange()
        //{
        //    ResetByteMessages();
        //}
        //public void SystemBusWrite()
        //{
        //    ResetByteMessages();
        //}
        //public void SystemBusRead()
        //{
        //    ResetByteMessages();
        //}

        ////Function Generator
        //public void FuncGenOn()
        //{

        //}

        //public void FuncGenOff()
        //{

        //}

        ////Analyser Generator
        //public void AnalyserGenRun()
        //{

        //}


    }
}
