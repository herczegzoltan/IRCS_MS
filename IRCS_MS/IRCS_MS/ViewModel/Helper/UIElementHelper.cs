using IRCS_MS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.ViewModel.Commands
{
    public class UIElementCollectionHelper
    {

        public MeasureModeViewModel MMV { get; set; }
        public ServiceModeViewModel SMV { get; set; }

        public UIElementCollectionHelper(MeasureModeViewModel mmv)
        {
            MMV = mmv;
        }
        public UIElementCollectionHelper(ServiceModeViewModel smv)
        {
            SMV = smv;
        }

        public void UIElementVisibilityUpdater(UIElementStateVariations uev)
        {

            switch (uev)
            {
                case UIElementStateVariations.ConnectBeforeClick:
                    //--------------------(Con,  Disc,  M_On,  M_Off, Run,   C&M,   Report, SM
                    UIElementUpdaterHelper(true, false, false, false, false, false, false, false);
                    break;
                case UIElementStateVariations.ConnectAfterClick:
                    UIElementUpdaterHelper(false, true, false, false, false, true, false, true);
                    break;
                case UIElementStateVariations.DisConnectBase:
                    UIElementUpdaterHelper(false, true, false, false, false, false, false, true);
                    break;
                case UIElementStateVariations.DisConnectClick:
                    UIElementUpdaterHelper(true, false, false, false, false, false, false, false);
                    break;
                case UIElementStateVariations.CardAndMeasureSelected:
                    UIElementUpdaterHelper(false, true, true, false, false, true, true, false);
                    break;
                case UIElementStateVariations.MeasureOffClick:
                    UIElementUpdaterHelper(false, true, true, false, false, true, true, false);
                    break;
                case UIElementStateVariations.MeasureOnAfterClick:
                    UIElementUpdaterHelper(false, true, false, true, true, false, false, false);
                    break;
                case UIElementStateVariations.MeasureOnAfterClickVoip:
                    UIElementUpdaterHelper(false, true, false, true, false, false, false, false);
                    break;
                default:
                    break;
            }

        }

        

        public void UIElementUpdaterHelper(
         bool connectButton, bool disconnectButton,
         bool measureOnButton, bool measureOffButton,
         bool runButton, bool cardAndMeasureType, bool reportField,bool serviceButton)
        {
            MMV.CmdConnectIsEnabled = connectButton;
            MMV.CmdDisConnectIsEnabled = disconnectButton;
            MMV.CmdMeasureOnIsEnabled = measureOnButton;
            MMV.CmdMeasureOffIsEnabled = measureOffButton;
            MMV.CmdRunIsEnabled = runButton;
            MMV.CmdCardTypeIsEnabled = cardAndMeasureType;
            MMV.CmdMeasureTypeIsEnabled = cardAndMeasureType;
            MMV.ReportCheckBoxEnabled = reportField;
            MMV.CmdServiceModeIsEnabled = serviceButton;
        }


        public void SM_UIElementVisibilityUpdater(UIElementStateVariations uev)
        {

            switch (uev)
            {
                case UIElementStateVariations.PsuBeforeClickOn:
                    
                    SM_UIElementUpdaterHelper(true, false, false, false, false, false, false, false, false,
                       false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                    break;
                case UIElementStateVariations.PsuAfterClickOn:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, true, false, true, false,
                       false, false, true, false, false, false, false, false, false, false,  true, false, false, false, false);
                    break;
                case UIElementStateVariations.ResetAfterClickOn:
                    SM_UIElementUpdaterHelper(false, true, false, true, true, true, false, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                case UIElementStateVariations.ResetAfterClickOff:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, true, false, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                case UIElementStateVariations.SbChannelSelected:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, true, true, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                case UIElementStateVariations.SmCardSelected:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, true, false, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                case UIElementStateVariations.SmFunctionSelected:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, false, false, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                case UIElementStateVariations.SmFreqAmpChannelsSelected:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, false, false, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                case UIElementStateVariations.AnalChannelsSelectedAfterOn:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, false, false, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                case UIElementStateVariations.AnalChannelsSelectedAfterOff:
                    SM_UIElementUpdaterHelper(false, true, true, false, true, false, false, true, false,
                       false, false, true, false, false, false, false, false, false, false, true, false, false, false, false);
                    break;
                default:
                    break;
            }

        }
        public void SM_UIElementUpdaterHelper(
            bool psuOn, bool psuOff, bool resetOn, bool resetOff, bool modulInit, bool sbChannel, bool sbChange, bool card, bool function, bool write, bool read,
            bool freq, bool freqPref, bool amplitude, bool ampPref, bool channel, bool subChannel, bool fgOn, bool fgOff, bool analChannel, bool analSubChannel, 
            bool analOn, bool analOff, bool analRun)
        {
            SMV.CmdPsuOnIsEnabled = psuOn;
            SMV.CmdPsuOffIsEnabled = psuOff;
            SMV.CmdResetOnIsEnabled = resetOn;
            SMV.CmdResetOffIsEnabled = resetOff;
            SMV.CmdModulInitIsEnabled = modulInit;
            SMV.CmdSbComboIsEnabled = sbChannel;
            SMV.CmdSbChangeIsEnabled = sbChange;
            SMV.CmdSmCardIsEnabled = card;
            SMV.CmdSmFunctionIsEnabled = function;
            SMV.CmdWriteButtonIsEnabled = write;
            SMV.CmdReadButtonIsEnabled = read;
            SMV.CmdFgFreqIsEnabled = freq;
            SMV.CmdFgFreqPrefixIsEnabled = freqPref;
            SMV.CmdFgAmpIsEnabled = amplitude;
            SMV.CmdFgAmpPrefixIsEnabled = ampPref;
            SMV.CmdFgChannelIsEnabled = channel;
            SMV.CmdFgSubChannelIsEnabled = subChannel;
            SMV.CmdFgOnIsEnabled = fgOn;
            SMV.CmdFgOffIsEnabled = fgOff;
            SMV.CmdAnChannelIsEnabled = analChannel;
            SMV.CmdAnSubChannelIsEnabled = analSubChannel;
            SMV.CmdAnOnIsEnabled = analOn;
            SMV.CmdAnOffIsEnabled = analOff;
            SMV.CmdAnRunIsEnabled = analRun;

        }
    }
}
