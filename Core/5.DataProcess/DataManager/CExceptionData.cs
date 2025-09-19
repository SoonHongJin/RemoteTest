using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataProcess
{
    public class CExceptionData
    {
        public string Page {  get; set; }
        public string Name { get; set; }
        public string Tag {  get; set; }
        public string Min {  get; set; }
        public string Max { get; set; }
        public string DataType { get; set; }

        public CExceptionData(string _Page, string _Name, string _Tag,string _Min,string _Max,string _DataType ) 
        {
            Page = _Page;
            Name = _Name;
            Tag = _Tag;
            Min = _Min; 
            Max = _Max;
            DataType = _DataType;
        }
    }
}
