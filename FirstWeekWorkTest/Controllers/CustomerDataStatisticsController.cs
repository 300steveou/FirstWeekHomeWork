using FirstWeekWorkTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace FirstWeekWorkTest.Controllers
{
    public class CustomerDataStatisticsController : Controller
    {
        客戶聯絡人Repository customerContactRepo;
        客戶資料Repository customerDataRepo;
        客戶銀行資訊Repository customerBankRepo;

        private int pageSize = 5;


        public CustomerDataStatisticsController()
        {
            customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
            customerDataRepo = RepositoryHelper.Get客戶資料Repository();
            customerBankRepo = RepositoryHelper.Get客戶銀行資訊Repository();
        }


        // 要放在邏輯曾
        // GET: CustomerDataStatistics
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            CustomerDatastatistics model = new CustomerDatastatistics();

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "客戶Id" : sortOrder;

            var customerDatas = customerDataRepo.All().Select(
                c =>
                new CustomerDatastatistics
                {
                    ID = c.Id,
                    客戶名稱 = c.客戶名稱,
                }).ToList();


            foreach (var a in customerDatas)
            {
                var customerContactDatas = customerContactRepo.All().Where(c => c.客戶Id == a.ID).Count();

                var customerBankDatas = customerBankRepo.All().Where(c => c.客戶Id == a.ID).Count();

                a.聯絡人數量 = customerContactDatas;
                a.銀行帳戶數量 = customerBankDatas;
            }

            var pageNumeber = page ?? 1;
            var result = customerDatas.OrderBy(x => x.ID);
            var onePageOfResult = result.ToPagedList(pageNumeber, pageSize);
            //return View(onePageOfResult);


            //https://www.c-sharpcorner.com/UploadFile/rahul4_saxena/paging-and-sorting-in-mvc4-using-pagedlist/

            IPagedList<CustomerDatastatistics> emp = null;
            switch (sortOrder)
            {
                case "客戶名稱":
                    if (sortOrder.Equals(CurrentSort))
                        emp = onePageOfResult.OrderByDescending
                                (m => m.客戶名稱).ToPagedList(pageIndex, pageSize);
                    else
                        emp = onePageOfResult.OrderBy
                                (m => m.客戶名稱).ToPagedList(pageIndex, pageSize);
                    break;
                case "銀行帳戶數量":
                    if (sortOrder.Equals(CurrentSort))
                        emp = onePageOfResult.OrderByDescending
                                 (m => m.銀行帳戶數量).ToPagedList(pageIndex, pageSize);
                    else
                        emp = onePageOfResult.OrderBy
                                (m => m.銀行帳戶數量).ToPagedList(pageIndex, pageSize);
                    break;

                case "聯絡人數量":
                    if (sortOrder.Equals(CurrentSort))
                        emp = onePageOfResult.OrderByDescending
                                (m => m.聯絡人數量).ToPagedList(pageIndex, pageSize);
                    else
                        emp = onePageOfResult.OrderBy
                                (m => m.聯絡人數量).ToPagedList(pageIndex, pageSize);
                    break;

                case "客戶Id":
                    emp = onePageOfResult.OrderByDescending
                                 (m => m.ID).ToPagedList(pageIndex, pageSize);
                    break;
            }
            return View(emp);
        }

    }  
}