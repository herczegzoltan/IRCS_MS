using IRCS_MS.Infrastructure.XmlHandler;

namespace IRCS_MS.Infrastructure.MeasureMode
{
    public static class MeasureModeByteMessagesStandardCommands
    {
        private static void ResetByteMessages()
        {
            ByteMessageBuilderRepository.ClearArray(ByteMessages.Instance.MeasureModeOutgoing);
        }

        public static void ConnectConfigureDevice()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetConnect());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }
        
        public static void DisConfigureDevice()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetDisConnect());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public static void MeasureOn()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetMeasureOn());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public static void MeasureOff()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetMeasureOff());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public static void ResetOk()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetResetOk());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public static void ResetNok()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetResetNok());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public static void SendRun(string cardType, string measureType)
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetMeasureOn());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 1, XmlFilter.Instance.GetSelectedCardTypeValue(cardType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 2, XmlFilter.Instance.GetSelectedMeasurementValue(cardType, measureType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 3, XmlFilter.Instance.GetRun());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public static void UdpUartTransmitStart(string cardType, string measureType,char data)
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetUdpUartTransmitStart());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 1, XmlFilter.Instance.GetSelectedCardTypeValue(cardType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 2, XmlFilter.Instance.GetSelectedMeasurementValue(cardType, measureType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 3, ConverterRepository.ConvertCharToByte(data));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public static void UdpUartTransmitStop(string cardType, string measureType)
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 0, XmlFilter.Instance.GetUdpUartTransmitStop());
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 1, XmlFilter.Instance.GetSelectedCardTypeValue(cardType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 2, XmlFilter.Instance.GetSelectedMeasurementValue(cardType, measureType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(ByteMessages.Instance.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }
    }
}
