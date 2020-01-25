using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.API.DataModels
{
    public class ExpectedPointEvent
    {
        public string PointEventName { get; set; }
        public decimal PointAmount { get; set; }
        public ExpectedPointEvent(string name, decimal amount)
        {
            this.PointEventName = name;
            this.PointAmount = amount;
        }
    }
}
