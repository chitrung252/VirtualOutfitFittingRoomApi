using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;


namespace DataService.Utilities
{
   
    public class SkyModelBinder : IModelBinder
    {

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {

            var model = Activator.CreateInstance(bindingContext.ModelType);
            foreach (var prop in bindingContext.PropertyMetadata)
            {
                // Retrieving attribute
                var attr = bindingContext.ModelType.GetProperty(prop.Key)
                                         .GetCustomAttributes(false)
                                         .OfType<DataMemberAttribute>()/*.First();*/
                                         .FirstOrDefault();
                if (attr != null)
                {
                    // Overwriting name if attribute present
                    var qsParam = (attr != null) ? attr.Name : prop.Key;
                    // Setting value of model property based on query string value
                    if (bindingContext.ValueProvider.GetValue(qsParam) != null)
                    {
                        // get temp value before parse
                        var temp = bindingContext.ValueProvider.GetValue(qsParam).AttemptedValue;
                        // get property need set value
                        var property = bindingContext.ModelType.GetProperty(prop.Key);
                        // get type each property to parse
                        var mtype = property.PropertyType.FullName;

                        #region parse and set value to model
                        if (mtype.Contains("System.Int32"))
                        {
                            var value = Int32.Parse(temp);
                            property.SetValue(model, value);
                        }
                        else if(mtype.Contains("System.Boolean"))
                        {
                            var value = Boolean.Parse(temp);
                            property.SetValue(model, value);
                        }
                        else if (mtype.Contains("System.DateTime"))
                        {
                            var value = DateTime.Parse(temp);
                            property.SetValue(model, value);
                        }
                        else
                        {
                            
                            property.SetValue(model, temp);
                        }
                        #endregion

                    }
                }
               
            }
            // reset model
            bindingContext.Model = model;
            
            return true;
            
        }
    }
}

