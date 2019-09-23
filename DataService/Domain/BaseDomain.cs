using AutoMapper;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public abstract class BaseDomain
    {

        public IConfigurationProvider AutoMapperConfig
        {
            get
            {
                return this.Service<IConfigurationProvider>();
            }
        }

        public T Service<T>() where T : class
        {
            return DependencyUtils.Resolve<T>();
        }

    }
}
