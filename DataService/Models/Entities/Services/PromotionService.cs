using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.Entities.Services
{
    public partial interface IPromotionService
    {
        Promotion GetByPromoCode(string code);
        Promotion GetPromotionByDateAndId(int id);
        new IEnumerable<Promotion> GetActive();
        new IEnumerable<Promotion> GetActive(Expression<Func<Promotion, bool>> expr);
    }

    public partial class PromotionService
    {
        public Promotion GetPromotionByDateAndId(int id)
        {
            var result = Repository
                .Get(q => q.Active == true && q.PromotionID == id && DateTime.Now >= q.FromDate && DateTime.Now <= q.ToDate)
                .FirstOrDefault();
            return result;
        }
        public Promotion GetByPromoCode(string code)
        {
            //var promotion = this.GetByPromoCode(code);
            var promotion = Repository.FirstOrDefaultActive(p => p.PromotionCode == code);
            if (promotion == null)
            {
                return null;
            }
            else
            {
                return promotion;
            }
        }
        public new IEnumerable<Promotion> GetActive()
        {
            return this.repository.GetActive();
        }

        public new IEnumerable<Promotion> GetActive(Expression<Func<Promotion, bool>> expr)
        {
            return this.repository.GetActive(expr);
        }
    }

}
