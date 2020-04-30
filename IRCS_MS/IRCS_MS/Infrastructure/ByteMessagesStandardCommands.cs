using IRCS_MS.Infrastructure.XmlHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{
    public class ByteMessagesStandardCommands
    {
        private readonly ByteMessages _byteMessages;
        private readonly XmlFilter _xmlData;

        public ByteMessagesStandardCommands()
        {
            _byteMessages = ByteMessages.Instance;
            _xmlData = XmlFilter.Instance;
        }
        private void ResetByteMessages()
        {
            ByteMessageBuilderRepository.ClearByteArray(_byteMessages.MeasureModeOutgoing);
        }

        public void MeasureModeConnectConfigureDevice()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, _xmlData.GetConnect());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, _xmlData.GetEOF());
        }
        
        public void MeasureModeDisConfigureDevice()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, _xmlData.GetDisConnect());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, _xmlData.GetEOF());
        }

        public void MeasureModeMeasureOn()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, _xmlData.GetMeasureOn());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, _xmlData.GetEOF());
        }

        public void MeasureModeMeasureOff()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, _xmlData.GetMeasureOff());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, _xmlData.GetEOF());
        }

        public void MeasureModeSendRun(string cardType, string measureType)
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, _xmlData.GetMeasureOn());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 1, _xmlData.GetSelectedCardTypeValue(cardType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 2, _xmlData.GetSelectedMeasurementValue(cardType, measureType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 3, _xmlData.GetRun());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, _xmlData.GetEOF());
        }
    }
}
