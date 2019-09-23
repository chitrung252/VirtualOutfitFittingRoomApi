using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.Entities.Services
{
    public partial interface IPromotionDetailService
    {
        PromotionDetail GetDetailByPromotionDetailCode(string code);
        IQueryable<PromotionDetail> GetDetailByCode(string code);
        PromotionDetail GetDetailById(int id);
        new IEnumerable<PromotionDetail> GetActive();
        new IEnumerable<PromotionDetail> GetActive(Expression<Func<PromotionDetail, bool>> expr);

    }
    public partial class PromotionDetailService
    {
        public PromotionDetail GetDetailById(int id)
        {
            var promoDetail = Repository.FirstOrDefault(q => q.PromotionDetailID == id);
            if (promoDetail == null)
            {
                return null;
            }
            else
            {
                return promoDetail;
            }
        }
        public IQueryable<PromotionDetail> GetDetailByCode(string code)
        {
            var a = Repository.Get(q => q.PromotionCode == code);
            return a;
        }
        public PromotionDetail GetDetailByPromotionDetailCode(string code)
        {
            return this.GetDetailByPromotionDetailCode(code);
        }
        public new IEnumerable<PromotionDetail> GetActive()
        {
            return this.repository.GetActive();
        }

        public new IEnumerable<PromotionDetail> GetActive(Expression<Func<PromotionDetail, bool>> expr)
        {
            return this.repository.GetActive(expr);
        }
    }

}
