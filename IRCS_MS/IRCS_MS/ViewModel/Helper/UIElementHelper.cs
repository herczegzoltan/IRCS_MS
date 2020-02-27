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

        public MainViewModel MV { get; set; }

        public UIElementCollectionHelper(MainViewModel mv)
        {
            MV = mv;
        }

        public void UIElementVisibilityUpdater(UIElementStateVariations uev)
        {

            switch (uev)
            {
                case UIElementStateVariations.ConnectBeforeClick:
                    UIElementUpdaterHelper(true, false, false, false, false, false, false);
                    break;
                case UIElementStateVariations.ConnectAfterClick:
                    UIElementUpdaterHelper(false, true, false, false, false, true, false);
                    break;
                case UIElementStateVariations.DisConnectBase:
                    UIElementUpdaterHelper(false, true, false, false, false, false, false);
                    break;
                case UIElementStateVariations.DisConnectClick:
                    UIElementUpdaterHelper(true, false, false, false, false, false, false);
                    break;
                case UIElementStateVariations.CardAndMeasureSelected:
                    UIElementUpdaterHelper(false, true, true, false, false, true, false);
                    break;
                case UIElementStateVariations.MeasureOffClick:
                    UIElementUpdaterHelper(false, true, true, false, false, true, false);
                    break;
                case UIElementStateVariations.MeasureOnAfterClick:
                    UIElementUpdaterHelper(false, true, false, true, true, false, false);
                    break;
                default:
                    break;
            }

        }

        

        public void UIElementUpdaterHelper(
         bool connectButton, bool disconnectButton,
         bool measureOnButton, bool measureOffButton,
         bool runButton, bool cardAndMeasureType, bool reportField)
        {
            MV.CmdConnectIsEnabled = connectButton;
            MV.CmdDisConnectIsEnabled = disconnectButton;
            MV.CmdMeasureOnIsEnabled = measureOnButton;
            MV.CmdMeasureOffIsEnabled = measureOffButton;
            MV.CmdRunIsEnabled = runButton;
            MV.CmdCardTypeIsEnabled = cardAndMeasureType;
            MV.CmdMeasureTypeIsEnabled = cardAndMeasureType;
            //ReportFieldState = reportField;
        }
    }
}
