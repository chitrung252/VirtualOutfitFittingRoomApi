using AutoMapper.QueryableExtensions;
using DataService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.DataAPIGen
{
    public partial class StoreWebSettingApi
    {
        public StoreWebSettingViewModel GetValueByStoreId(int id)
        {
            return (new StoreWebSettingViewModel(this.BaseService.GetValueByStoreId(id)));
        }
    }
}
