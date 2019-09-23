using AutoMapper.QueryableExtensions;
using DataService.APIViewModels;
using DataService.Models;
using DataService.Models.APIModels;
using DataService.Models.Entities;
using DataService.Models.Entities.Services;
using DataService.Models.Identities;
using DataService.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataService.Domain
{
    public interface ICustomerDomain
    {
        CustomerAPIViewModel GetCustomerById(int id);
        CustomerAPIViewModel GetCustomersByPhonenumber(string phonenumber, int brandId);
        CustomerAPIViewModel GetCustomerByBrandIdAndFbId(int brandId, string fbId);
        int AddCustomer(CustomerAPIViewModel customer);
        BaseResponse<CustomerAPIViewModel> UpdateCustomer(CustomerAPIViewModel customer);
        List<CustomerAPIViewModel> GetAllCustomer();
        List<CustomerAPIViewModel> GetCustomerByRequest(BaseRequest<string> request);
        CustomerAPIViewModel CreateWithContrans(string cardcode, CustomerAPIViewModel customer);
        int getCount();
    }
    public class CustomerDomain : BaseDomain, ICustomerDomain
    {

        public int AddCustomer(CustomerAPIViewModel customer)
        {
            var customerService = this.Service<ICustomerService>();
            var entity = customer.ToEntity();
            return customerService.CreateCustomer(entity);
        }

        public CustomerAPIViewModel CreateWithContrans(string cardcode, CustomerAPIViewModel customer)
        {
            #region call service
            var customerService = this.Service<ICustomerService>();
            var mbsCardService = this.Service<IMembershipService>();
            var accountService = this.Service<IAccountService>();
            var cardService = this.Service<ICardService>();
            #endregion
            int id;
            var customerVM = new CustomerAPIViewModel();
            using (var trans = new TransactionScope(TransactionScopeOption.Required))
            {


                var entity = customer.ToEntity();
                try
                {
                    #region Check phone and email exist
                    // check phone exist //
                    if (!string.IsNullOrEmpty(customer.AccountPhone))
                    {
                        var customerCheck = customerService.GetCustomersByAccountPhone(customer.AccountPhone, customer.BrandId.Value);
                        if (customerCheck != null)
                        {
                            throw ApiException.Get(false, ConstantManager.MES_CUSTOMER_PHONE_EXIST, ResultEnum.CreateFail, HttpStatusCode.BadRequest);
                        }
                    }

                    // check email exist
                    if (!string.IsNullOrEmpty(customer.Email))
                    {
                        var customerCheck = customerService.GetCustomerByEmail(customer.Email, customer.BrandId.Value);
                        if (customerCheck != null)
                        {
                            throw ApiException.Get(false, ConstantManager.MES_CUSTOMER_EMAIL_EXIST, ResultEnum.CreateFail, HttpStatusCode.BadRequest);
                        }
                    }
                    #endregion

                    // Create Customer
                    id = customerService.CreateCustomer(entity);
                    // create Membership
                    var mbsCard = mbsCardService.AddMembership(id, (int)MembershipCardTypeEnum.Newbie);
                    // create Card
                    var newCard = cardService.AddCard(cardcode, customer.BrandId.Value, mbsCard, (int)CardTypeEnum.MobieCard);
                    // create Account Creadit and Point Member
                    accountService.CreateAccountByMemCard(mbsCard.MembershipCode, 0, customer.BrandId.Value, mbsCard.Id, (int)AccountTypeEnum.CreditAccount);
                    accountService.CreateAccountByMemCard(mbsCard.MembershipCode, 0, customer.BrandId.Value, mbsCard.Id, (int)AccountTypeEnum.PointAccount);
                    // End Transaction
                    trans.Complete();
                    trans.Dispose();

                }

                catch (Exception ex)
                {
                    // roll back transaction
                    trans.Dispose();
                    Console.WriteLine("Error Create : " + ex);
                    if (ex is ApiException)
                    {
                        throw ex;
                    }
                    else
                    {
                        throw ApiException.Get(false, ex.ToString(), ResultEnum.CreateFail, HttpStatusCode.InternalServerError);
                    }

                }
               


            }
            //customerVM = customerService.GetCustomerById(id);
            //if (customerVM == null)
            //{
            //    throw ApiException.Get(false, ConstantManager.MES_LOGIN_FAIL, ResultEnum.CreateFail, HttpStatusCode.BadRequest);
            //}
            customerVM = customerService.GetCustomerById(id);
            if(customerVM != null)
            {
                return customerVM;
            }
            return null;
            //return customerVM;
        }

        public List<CustomerAPIViewModel> GetAllCustomer()
        {
            var customerService = this.Service<ICustomerService>();
            return customerService.GetAllCustomer();
        }

        public int getCount()
        {
            var customerService = this.Service<ICustomerService>();
            return customerService.GetCount();
        }


        //public CustomerAPIViewModel CreateCustomerWithConstraint(CustomerAPIViewModel model)
        //{
        //    var customerService = this.Service<ICustomerService>();

        //}

        public CustomerAPIViewModel GetCustomerByBrandIdAndFbId(int brandId, string fbId)
        {
            var customerService = this.Service<ICustomerService>();

            var customer = customerService.GetCustomerByBrandIdAndFbId(brandId, fbId);
            //var list =  customerService.GetCustomerByBrandId(brandId);
            // var customer = list.Where(q => q.FacebookId.Equals(fbId)).FirstOrDefault();

            return customer;
        }

        public CustomerAPIViewModel GetCustomerById(int id)
        {
            var customerService = this.Service<ICustomerService>();
            return customerService.GetCustomerById(id);
        }

        public List<CustomerAPIViewModel> GetCustomerByRequest(BaseRequest<string> request)
        {
            var customerService = this.Service<ICustomerService>();
            return customerService.GetCustomerByRequest(request);
        }

        public CustomerAPIViewModel GetCustomersByPhonenumber(string phonenumber, int brandId)
        {
            var customerService = this.Service<ICustomerService>();
            return customerService.GetCustomersByAccountPhone(phonenumber, brandId);

        }

        public BaseResponse<CustomerAPIViewModel> UpdateCustomer(CustomerAPIViewModel customer)
        {
            var customerService = this.Service<ICustomerService>();
            try
            {
                var customerVM = customerService.UpdateCustomer(customer);
                if (customerVM == null)
                {
                    throw ApiException.Get(false, ConstantManager.MES_UPDATE_FAIL, ResultEnum.UpdateFail, HttpStatusCode.InternalServerError);
                }
                var account = customerVM.MembershipVM.AccountVMs;
                foreach (var item in account)
                {
                    if (item.Type == (int)AccountTypeEnum.CreditAccount)
                    {
                        customerVM.Balance = item.Balance == null ? 0 : item.Balance.Value;
                    }
                    if (item.Type == (int)AccountTypeEnum.PointAccount)
                    {
                        customerVM.Point = item.Balance == null ? 0 : item.Balance.Value;
                    }
                }
                return BaseResponse<CustomerAPIViewModel>.Get(true, ConstantManager.MES_UPDATE_SUCCESS, customerVM, ResultEnum.Success);
            }
            catch (Exception e)
            {
                if(e is ApiException)
                {
                    throw e;
                }
                else
                {
                    throw ApiException.Get(false, e.ToString(), ResultEnum.InternalError, HttpStatusCode.InternalServerError);
                }
                
            }
            
        }
    }
}
