using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hertz.FileProcessing.Utilities
{
    public abstract class DataFeedSerializer
    {
        public string Delimiter = "|";
        public override string ToString()
        {
            List<PropertyInfo> properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            List<string> lineItems = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    DateTime obj = (DateTime)property.GetValue(this);
                    lineItems.Add(obj.ToString("yyyyMMdd"));
                }
                else
                    lineItems.Add(property.GetValue(this).ToString());
            }
            return String.Join(Delimiter, lineItems);
        }
        public string GenerateHeader()
        {
            List<PropertyInfo> properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            List<string> lineItems = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                lineItems.Add(property.Name);
            }
            return String.Join(Delimiter, lineItems);
        }
    }
}
