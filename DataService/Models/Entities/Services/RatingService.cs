using DataService.APIViewModels;
using DataService.Models.Entities.Repositories;
using DataService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models.Entities.Services
{
    public partial interface IRatingService
    {
        List<RatingAPIViewModel> GetRatingByProductId(int product_id);
        List<RatingAPIViewModel> GetRatingByOrderId(int orderId);
        RatingAPIViewModel CreateRatingProduct(RatingAPIViewModel rating);
        RatingAPIViewModel UpdateRatingProduct(RatingAPIViewModel rating);
        List<RatingAPIViewModel> GetRatingProductId(int id);
    }
    public partial class RatingService : IRatingService
    {
        public List<RatingAPIViewModel> GetRatingByOrderId(int orderId)
        {
            var repo = DependencyUtils.Resolve<IRatingRepository>();
            var list = repo.GetActive(r => r.OrderId == orderId).ToList().Select(r => new RatingAPIViewModel(r)).ToList();
            if (list.Count > 0)
            {
                return list;
            }
            return null;
        }
        public List<RatingAPIViewModel> GetRatingByProductId(int product_id)
        {
            var repo = DependencyUtils.Resolve<IRatingRepository>();
            var list = repo.GetActive(p => p.ProductId == product_id).AsEnumerable().Select(p => new RatingAPIViewModel(p)).ToList();
            if (list.Count > 0)
            {
                return list;
            }
            return null;
        }

        public List<RatingAPIViewModel> GetRatingProductId(int id)
        {
            var repo = DependencyUtils.Resolve<IRatingRepository>();
            var list = repo.GetActive(p => p.Id == id).AsEnumerable().Select(p => new RatingAPIViewModel(p)).ToList();
            if (list.Count > 0)
            {
                return list;
            }
            return null;
        }
        public RatingAPIViewModel CreateRatingProduct(RatingAPIViewModel rating)
        {
            var RatingProductE = rating.ToEntity();
            
                RatingProductE.UserId = null;
            this.Create(RatingProductE);
            return new RatingAPIViewModel(RatingProductE);
        }
        public RatingAPIViewModel UpdateRatingProduct(RatingAPIViewModel rating)        {            var ratingE = Repository.FirstOrDefault(c => c.Id== rating.Id);            if (ratingE != null)            {                var ratingET = rating.ToEntity();                Repository.Edit(ratingET);                Repository.Save();                rating = new RatingAPIViewModel(ratingET);                return rating;            }            return null;        }
    }
}
