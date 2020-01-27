using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Brierley.TestAutomation.Core.Utilities;
using Brierley.LoyaltyWare.ClientLib.DomainModel;
using System.Collections;

namespace Hertz.API.Utilities
{
    public class LWAttributeSet : Attribute 
    {
        public Type _Type { get; set; }
        public LWAttributeSet()
        {

        }
    }

    public class LODConvert
    {
        public static T FromLW<T>(object obj)
        {
            T outputBase = (T)Activator.CreateInstance(typeof(T));

            //Get all of our output data types properties which are marked as coming from an LWAttributeSet
            IEnumerable<PropertyInfo> attributeSets = outputBase.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(LWAttributeSet)));

            //Convert the base object over to our output type.
            outputBase = DataConverter.ConvertTo<T>(obj);

            if (obj is LWAttributeSetContainer lwObj)
            {
                //Get attribute sets from the input object and convert them over to our output type.
                foreach (var attributeProperty in attributeSets)
                {
                    LWAttributeSet attr = attributeProperty.GetCustomAttribute<LWAttributeSet>();

                    var lwChildren = lwObj.GetAttributeSets(attr._Type.Name);
                    if (lwChildren?.Count > 0)
                    {
                        if (IsPropertyACollection(attributeProperty))
                        {
                            var listObj = Activator.CreateInstance(attributeProperty.PropertyType);

                            foreach (var lwChild in lwChildren)
                            {
                                var setObj = FromLw(lwChild, attributeProperty.PropertyType.GetGenericArguments()[0]);
                                listObj.GetType().GetMethod("Add").Invoke(listObj, new[] { setObj });
                            }
                            attributeProperty.SetValue(outputBase, listObj);
                        }
                        else
                        {
                            var lwChild = lwChildren.FirstOrDefault();
                            var setObj = FromLw(lwChild, attributeProperty.PropertyType);
                            attributeProperty.SetValue(outputBase, setObj);
                        }
                    }
                }
            }

            return outputBase;
        }

        public static T ToLW<T>(object obj)
        {
            T outputBase = (T)Activator.CreateInstance(typeof(T));

            outputBase = DataConverter.ConvertTo<T>(obj);

            if(outputBase is LWAttributeSetContainer lwObj)
            {
                IEnumerable<PropertyInfo> attributeSets = obj.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(LWAttributeSet)));
                foreach (var attributeProperty in attributeSets)
                {
                    LWAttributeSet attr = attributeProperty.GetCustomAttribute<LWAttributeSet>();

                    var modelObj = attributeProperty.GetValue(obj);
                    if (modelObj != null)
                    {
                        if (IsPropertyACollection(attributeProperty))
                        {
                            var modelList = modelObj as IEnumerable;
                            foreach (var val in modelList)
                            {
                                var lwAttrObj = ToLw(val, attr._Type);
                                lwObj.Add(lwAttrObj as LWAttributeSetContainer);
                            }
                        }
                        else
                        {
                            var lwAttrObj = ToLw(modelObj, attr._Type);
                            lwObj.Add(lwAttrObj as LWAttributeSetContainer);
                        }
                    }
                }
            }

            return outputBase;
        }

        private static object ToLw(object modelIn, Type lwType)
        {
            object outputBase = Activator.CreateInstance(lwType);

            outputBase = DataConverter.ConvertObject(modelIn, lwType);

            if (outputBase is LWAttributeSetContainer lwObj)
            {
                IEnumerable<PropertyInfo> attributeSets = modelIn.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(LWAttributeSet)));
                foreach (var attributeProperty in attributeSets)
                {
                    LWAttributeSet attr = attributeProperty.GetCustomAttribute<LWAttributeSet>();

                    var modelObj = attributeProperty.GetValue(modelIn);
                    if (modelObj != null)
                    {
                        if (IsPropertyACollection(attributeProperty))
                        {
                            var modelList = modelObj as IEnumerable;
                            foreach (var val in modelList)
                            {
                                var lwAttrObj = ToLw(val, attr._Type);
                                lwObj.Add(lwAttrObj as LWAttributeSetContainer);
                            }
                        }
                        else
                        {
                            var lwAttrObj = ToLw(modelObj, attr._Type);
                            lwObj.Add(lwAttrObj as LWAttributeSetContainer);
                        }
                    }
                }
            }
            return outputBase;
        }
        private static object FromLw(object lwIn, Type modelOut)
        {
            var outputBase = Activator.CreateInstance(modelOut);

            //Get all of our output data types properties which are marked as coming from an LWAttributeSet
            IEnumerable<PropertyInfo> attributeSets = outputBase.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(LWAttributeSet)));

            //Convert the base object over to our output type.
            outputBase = DataConverter.ConvertObject(lwIn, modelOut);

            if (lwIn is LWAttributeSetContainer lwObj)
            {
                //Get attribute sets from the input object and convert them over to our output type.
                foreach (var attributeProperty in attributeSets)
                {
                    LWAttributeSet attr = attributeProperty.GetCustomAttribute<LWAttributeSet>();

                    var lwChildren = lwObj.GetAttributeSets(attr._Type.Name);
                    if (lwChildren?.Count > 0)
                    {
                        if (IsPropertyACollection(attributeProperty))
                        {
                            var listObj = Activator.CreateInstance(attributeProperty.PropertyType);

                            foreach (var lwChild in lwChildren)
                            {
                                var setObj = FromLw(lwChild, attributeProperty.PropertyType.GetGenericArguments()[0]);
                                listObj.GetType().GetMethod("Add").Invoke(listObj, new[] { setObj });
                            }
                            attributeProperty.SetValue(outputBase, listObj);
                        }
                        else
                        {
                            var lwChild = lwChildren.FirstOrDefault();
                            var setObj = FromLw(lwChild, attributeProperty.PropertyType);
                            attributeProperty.SetValue(outputBase, setObj);
                        }
                    }
                }
            }

            return outputBase;
        }

        private static bool IsPropertyACollection(PropertyInfo property)
        {
            return property.PropertyType.GetInterface(typeof(IList<>).FullName) != null;
        }
    }


}
