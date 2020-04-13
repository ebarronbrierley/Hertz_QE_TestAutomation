using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.DataModels
{
    public class PointsDetailModel
    {
            public DateTime? DATE { get; set; }
            public int POINTS { get; set; }
            public string DESCRIPTION { get; set; }
            public string RANUMBER { get; set; }
            public string TYPE { get; set; }
            public string NOTES { get; set; }
    }
}
