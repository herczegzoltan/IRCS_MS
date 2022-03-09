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

        public MeasureModeViewModel MV { get; set; }

        public UIElementCollectionHelper(MeasureModeViewModel mv)
        {
            MV = mv;
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
            MV.CmdConnectIsEnabled = connectButton;
            MV.CmdDisConnectIsEnabled = disconnectButton;
            MV.CmdMeasureOnIsEnabled = measureOnButton;
            MV.CmdMeasureOffIsEnabled = measureOffButton;
            MV.CmdRunIsEnabled = runButton;
            MV.CmdCardTypeIsEnabled = cardAndMeasureType;
            MV.CmdMeasureTypeIsEnabled = cardAndMeasureType;
            MV.ReportCheckBoxEnabled = reportField;
            MV.CmdServiceModeIsEnabled = serviceButton;
        }
    }
}
