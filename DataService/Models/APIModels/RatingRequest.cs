using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace DataService.Models.APIModels
{
    [ModelBinder(typeof(SkyModelBinder))]
    public class RatingRequest<T> : BaseRequest<T> 
    {

    }
}
