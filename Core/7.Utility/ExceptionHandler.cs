using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataProcess;
using Core.UI;
using Tsukikage.SharpJson;

namespace Core.Utility
{
    public class ParameterExceptionHandler : CExceptionData, IDisposable
    {
        private string value = string.Empty;
        private double checkData = double.NaN;
        private char[] invalidChars = new char[] { '\\', '/', ':', '?', '"', '<', '>', '|', '*' };
        private WarningDialog dialog = new WarningDialog();
        public ParameterExceptionHandler(string Page, string Name,string Tag ,string Min, string Max, string DataType, string _value) : base(Page, Name,Tag ,Min, Max, DataType)
        {
            value = _value;
        }
        /// <summary>
        /// 25.01.23 NWT Parameter Input Data Check
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            bool result;
            if (DataType == "Text")
            {
                if (ContainsInvalidChars(value))
                {
                    dialog.Content = "Input value must not include '\\', '/', ':', '?', '?', '<', '>', '|', '*'.";
                    dialog.ShowDialog();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (double.TryParse(value, out double temp))
                {
                    if (temp > Convert.ToDouble(Max) || temp < Convert.ToDouble(Min))
                    {
                        var dialog = new WarningDialog()
                        {
                            Content = string.Format("Input value must be between {0} and {1}.", Min, Max)
                        };
                        dialog.ShowDialog();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    var dialog = new WarningDialog()
                    {
                        Content = "Input Value must be a number."
                    };
                    dialog.ShowDialog();
                    return false;
                }
            }
        }
        

        private bool ContainsInvalidChars(string text)
        {
            return text.Any(ch => invalidChars.Contains(ch));  // 특수 문자가 포함되었는지 확인
        }

        public void Dispose()
        {
        }
    }
}
