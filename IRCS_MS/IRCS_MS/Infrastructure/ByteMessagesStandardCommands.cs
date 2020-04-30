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

        public ByteMessagesStandardCommands()
        {
            _byteMessages = ByteMessages.Instance;
        }
        private void ResetByteMessages()
        {
            ByteMessageBuilderRepository.ClearArray(_byteMessages.MeasureModeOutgoing);
        }

        public void MeasureModeConnectConfigureDevice()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, XmlFilter.Instance.GetConnect());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }
        
        public void MeasureModeDisConfigureDevice()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, XmlFilter.Instance.GetDisConnect());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public void MeasureModeMeasureOn()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, XmlFilter.Instance.GetMeasureOn());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public void MeasureModeMeasureOff()
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, XmlFilter.Instance.GetMeasureOff());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }

        public void MeasureModeSendRun(string cardType, string measureType)
        {
            ResetByteMessages();
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 0, XmlFilter.Instance.GetMeasureOn());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 1, XmlFilter.Instance.GetSelectedCardTypeValue(cardType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 2, XmlFilter.Instance.GetSelectedMeasurementValue(cardType, measureType));
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 3, XmlFilter.Instance.GetRun());
            ByteMessageBuilderRepository.SetByteArrayByIndex(_byteMessages.MeasureModeOutgoing, 4, XmlFilter.Instance.GetEOF());
        }
    }
}
