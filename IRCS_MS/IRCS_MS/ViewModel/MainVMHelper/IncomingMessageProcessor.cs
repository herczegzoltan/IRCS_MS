using IRCS_MS.Infrastructure.Message;
using IRCS_MS.Model;
using IRCS_MS.ViewModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.ViewModel.MainVMHelper
{
    public class IncomingMessageProcessor
    {
        private readonly MainViewModel _mainViewModel;
        private readonly SerialPort _serialPort;
        private const int NUMBEROFINBYTES = 2;
        private int NumberOfInBytesCounter = 0;

        public IncomingMessageProcessor(MainViewModel mainViewModel, SerialPort serialPort)
        {
            _mainViewModel = mainViewModel;
            _serialPort = serialPort;
        }

        public void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            ReadAndProcessIncomingMessage();
        }


        //todo: increase counter
        private void ReadAndProcessIncomingMessage()
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    string incomingByte = _serialPort.ReadByte().ToString();

                    ByteMessageBuilder.SetByteIncomingArray(NumberOfInBytesCounter, incomingByte); //34 0 13

                    if (NumberOfInBytesCounter == NUMBEROFINBYTES)
                    {

                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}

/*
if (COMPort.IsOpen)
{
    try
    {
        string incomingByte = COMPort.ReadByte().ToString();

        ByteMessageBuilder.SetByteIncomingArray(countBytes, incomingByte); //34 0 13

        //all bytes arrived
        if (countBytes == 2)
        {
            if (WasItRun)
            {

                IsRunningNow = GeneralMessageCollection.IsRunningStateChecker(true);

                TimeOutValidator(TimeOutValidatorStates.Reset);

                if (ValidatorIncomingMessage.ValidationEOF(xmlData))
                {
                    _extramessages = xmlData.IsCommonIncluded(SelectedCardType) == true ?
                        xmlData.GetNumberOfExpectedMeasureState(xmlData.GetDefaultName()) * xmlData.DefaultNumbersOfBytes :
                        _extramessages = 0;


                    //if the incoming messages's number is equle with the required number from XML file
                    if (_counterIncomingPackage ==
                        xmlData.GetNumberOfExpectedMeasureState(SelectedCardType) * xmlData.DefaultNumbersOfBytes + _extramessages)
                    {
                        TimeOutValidator(TimeOutValidatorStates.Stop);

                        MessageRecievedText = GeneralMessageCollection.GeneralMessageRecived(" -> Validate OK", xmlData) + MessageRecievedText;
                        //_counterIncomingPackage = 1;
                        _validateFinished = true;
                        GeneralMessageCollection.LoopCounter = 0;
                    }
                    else
                    {
                     //when it is not validated yet.   
                        TimeOutValidator(TimeOutValidatorStates.Reset);
                        MessageRecievedText = GeneralMessageCollection.GeneralMessageRecived("", xmlData) + MessageRecievedText;
                        GeneralMessageCollection.LoopCounter++;
                        _validateFinished = false;
                    }

                    //if incoming message returns with measure ok or not-> negative logic
                    if (ValidatorIncomingMessage.ErrorMessageBack(xmlData, ByteMessageBuilder.GetByteIncomingArray()[1]))
                    {
                        //TimeOutValidator(TimeOutValidatorStates.Reset);
                        TimeOutValidator(TimeOutValidatorStates.Stop);
                        //_counterIncomingPackage = 1;
                        _validateFinished = true;
                        GeneralMessageCollection.LoopCounter = 0;
                    }

                    if (ReportFieldState)
                    {
                        _savedMeasureCounter++;

                        string reportInsertData = xmlData.GetResponseData(
                            ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[1].ToString()));

                        ReportDataCollector.AddToVertical(reportInsertData);

                        if (ReportFieldState && _validateFinished)
                        {
                            ReportDataCollector.AddToVerticalAtIndex(0, SchauerNumber.ToString());
                            ReportDataCollector.AddVerticalToHorizontal();
                            ReportDataCollector.CleanerVertical();
                            PopUpQuestionbox();
                        }
                    }
                }
                else
                {
                    //TimeOutValidator(TimeOutValidatorStates.Reset);
                    TimeOutValidator(TimeOutValidatorStates.Stop);
                    //_counterIncomingPackage = 1;
                    _validateFinished = true;
                    GeneralMessageCollection.LoopCounter = 0; 
                    MessageRecievedText = GeneralMessageCollection.GeneralMessageRecived("Validate Error -> Wrong EoF") + MessageRecievedText;
                }
            }
            else
            {
                MessageRecievedText = "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + " -> " +
                                    xmlData.GetResponseTranslate
                                    (ByteMessageBuilder.GetByteIncomingArray()[0].ToString(),
                                    ByteMessageBuilder.GetByteIncomingArray()[1].ToString(),
                                    ByteMessageBuilder.GetByteIncomingArray()[2].ToString())
                                    + "\n" + MessageRecievedText + "\n";
            }

            countBytes = 0;
            WasItDisconnect();
            ByteMessageBuilder.ResetByteIncomingArray();
        }
        else
        {
            countBytes++;
        }

        if (WasItRun && !_validateFinished)
        {
            _counterIncomingPackage++;
        }
        if(_validateFinished)
        {
            _counterIncomingPackage = 1;
            _validateFinished = false;
                                    IsRunningNow = GeneralMessageCollection.IsRunningStateChecker(false);
        }
    }
    catch (Exception ex)
    {
        //throw;
        MessageBox.Show(GeneralMessageCollection.LogIntoFile(ex));
    }
}

*/