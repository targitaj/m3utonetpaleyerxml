namespace Uma.Eservices.Models.Shared
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Base class for models
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Used to calculate Model filling status in percentage
        /// </summary>
        /// <param name="modelState">Model state that contains information about filled fields</param>
        /// <param name="prefix">Prefix that used to get full path to elemet returned from page</param>
        public int GetProgressPercentage(ModelStateDictionary modelState, string prefix)
        {
            var successCnt = 0;
            var errorCnt = 0;

            var formattedList = this.GetAllPropertyFormattedList(this).Select(s => prefix + s).ToList();

            foreach (var element in formattedList)
            {
                if (modelState.ContainsKey(element))
                {
                    if (modelState[element].Errors.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(modelState[element].Value.AttemptedValue))
                        {
                            successCnt++;
                        }
                    }
                    else
                    {
                        errorCnt++;
                    }
                }
            }

            var totalCnt = successCnt + errorCnt;


            return totalCnt == 0 ? 100 : (successCnt * 100 / totalCnt);
        }

        /// <summary>
        /// Used to generate list of object properties and child properties
        /// </summary>
        /// <param name="object">Object to generate from</param>
        private List<string> GetAllPropertyFormattedList(object @object)
        {
            var res = new List<string>();

            if (@object != null)
            {
                var pType = @object.GetType();

                foreach (var propertyInfo in pType.GetProperties())
                {
                    object[] attrs = propertyInfo.GetCustomAttributes(typeof(SkipModelPropertyAttribute), false);

                    if (attrs.Length == 0)
                    {
                        bool addDot = true;
                        var formatedList = new List<string>();
                        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GenericTypeArguments[0].BaseType == typeof(BaseModel))
                        {
                            var genVal = (IList)propertyInfo.GetValue(@object);

                            if (genVal != null)
                            {
                                addDot = false;

                                for (int i = 0; i < genVal.Count; i++)
                                {
                                    var fl = this.GetAllPropertyFormattedList(genVal[i]);
                                    fl.ForEach(fr => formatedList.Add("[" + i + "]." + fr));
                                }
                            }
                        }
                        else if (@object is BaseModel)
                        {
                            formatedList = this.GetAllPropertyFormattedList(propertyInfo.GetValue(@object));
                        }

                        if (@object is BaseModel)
                        {
                            if (formatedList.Count == 0)
                            {
                                res.Add(propertyInfo.Name);
                            }
                            else
                            {
                                res.AddRange(formatedList.Select(f => propertyInfo.Name + (addDot ? "." : "") + f));
                            }
                        }
                    }
                }
            }

            return res;
        }
    }
}
