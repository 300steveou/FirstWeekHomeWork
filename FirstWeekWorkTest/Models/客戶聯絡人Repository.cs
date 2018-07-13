using System;
using System.Linq;
using System.Collections.Generic;
	
namespace FirstWeekWorkTest.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        public bool IsRepeatEMail(int CustomerId,string CustomerEmail)
        {           

            var _count = this.All().Where(p => p.客戶Id == CustomerId && p.是否已刪除 == false && p.Email == CustomerEmail).Count();

            if(_count > 0)
            {
                return true;
            }

            return false;
        }


       

        public List<客戶聯絡人> AllData()
        {

            客戶聯絡人Repository customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();

            var customerData = RepositoryHelper.Get客戶資料Repository().All().ToList();

            var customerContact = customerContactRepo.All().ToList()
                .Join(customerData, a => a.客戶Id, b => b.Id, (a, b) => new 客戶聯絡人
                {
                    Id = a.Id,
                    客戶Id = a.Id,
                    職稱 = a.職稱,
                    姓名 = a.姓名,
                    Email = a.Email,
                    手機 = a.手機,
                    電話 = a.電話,
                    是否已刪除 = a.是否已刪除,
                    客戶資料 = new 客戶資料 { Id = a.客戶Id, 客戶名稱 = b.客戶名稱 }
                }).Where(c => c.是否已刪除 == false).ToList();
            return customerContact;

        }






    }

    public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}