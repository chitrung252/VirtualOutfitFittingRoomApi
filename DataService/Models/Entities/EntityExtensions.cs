using DataService.BaseConnect;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.Entities
{
    public static class EntityExtensions
    {

        public static TViewModel ToViewModel<TEntity, TViewModel>(this TEntity entity)
            where TEntity : IEntity, new()
            where TViewModel : BaseEntityViewModel<TEntity>, new()
        {
            var result = new TViewModel();
            result.CopyFromEntity(entity);
            return result;
        }

    }
}
