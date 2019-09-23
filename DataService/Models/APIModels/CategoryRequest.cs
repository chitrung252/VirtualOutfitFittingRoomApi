using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace DataService.Models.APIModels
{
    [ModelBinder(typeof(SkyModelBinder))]
    public class CategoryRequest<T> : BaseRequest<T>
    {
        [DataMember(Name = "is_parent")]
        public bool IsParent { get; set; } = false;
    }
}
