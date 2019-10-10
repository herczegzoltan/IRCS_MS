using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunicator.Infrastructure
{
    public class Measurement
    {
        public string Name { get; set; }

        public List<string> SchauerNumber = new List<string>() {};

        public List<string> ResultOfMeasurement = new List<string>() { };

        public List<string> MeasureType = new List<string>() { };



        public void AddSchauerNumber(string _sNumber)
        {
            if (SchauerNumber.Count == 0)
            {
                SchauerNumber.Add(_sNumber);
            }else
            {

            }
        }

        public void AddResultOfMeasurement(string _resultOfMes)
        {
            ResultOfMeasurement.Add(_resultOfMes);
        }

        public void AddMeasureType(string _measureType)
        {
            MeasureType.Add(_measureType);
        }

        public void ClearLists()
        {
            SchauerNumber.Clear();

            ResultOfMeasurement.Clear();


            MeasureType.Clear();
        }
    }
}
