using System;
using System.Linq;
using System.Collections.Generic;

namespace FirstWeekWorkTest.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public int findRepeatEmail(string args)
        {
            var count = this.All().Where(p => p.Email == args).Count();
            return count;
        }

        public IQueryable<客戶資料> SearchFilterQuery(string args, int? category)
        {
            
            var result = base.All();
            if (!string.IsNullOrEmpty(args))
            {
                result = result.Where(p => p.客戶名稱.Contains(args));
            }

            if (category.HasValue)
            {
                result = result.Where(c => c.客戶分類 == category);
            }
            
            return result;
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }

        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(c => c.是否已刪除 == false);
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}