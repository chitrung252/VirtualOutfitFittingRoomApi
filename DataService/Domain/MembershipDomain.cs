using DataService.APIViewModels;
using DataService.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Domain
{
    public interface IMembershipDomain
    {
          Task<MembershipAPIViewModel> GetMembershipActiveByCustomerIdAsync(int customerId);
        MembershipAPIViewModel AddMembership(int customerId, int membershipTypeId);
    }
    public class MembershipDomain : BaseDomain,IMembershipDomain
    {
        public MembershipAPIViewModel AddMembership( int customerId, int membershipTypeId)
        {
            var mbsCardService = this.Service<IMembershipService>();

           return mbsCardService.AddMembership(customerId, membershipTypeId);
        }

        public async Task<MembershipAPIViewModel> GetMembershipActiveByCustomerIdAsync(int customerId)
        {
            var mbsCardService = this.Service<IMembershipService>();
           var mbsCard = await mbsCardService.GetMembershipActiveByCustomerIdAsync(customerId);
            return mbsCard;
        }
    }
}
