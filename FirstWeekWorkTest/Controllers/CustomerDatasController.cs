using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using FirstWeekWorkTest.Enum;
using FirstWeekWorkTest.Models;

namespace FirstWeekWorkTest.Controllers
{
    public class CustomerDatasController : Controller
    {
        客戶資料Repository customerdatasRepo;

        public CustomerDatasController()
        {
            customerdatasRepo = RepositoryHelper.Get客戶資料Repository();
        }


        // GET: CustomerDatas
        public ActionResult Index()
        {
            var customerdata = customerdatasRepo.All();
            SetCategoryDDLListItem();
            return View(customerdata.Take(10));
        }

        public void SetCategoryDDLListItem()
        {
            // Must Change! 
            var dictionary = new Dictionary<int, string>();

            var category = customerdatasRepo.All().Select(c=>c.客戶分類).Distinct().ToList();

            foreach (var item in category)
            {
                dictionary.Add(item, ((客戶分類type)item).ToString());                
            }            
            ViewBag.category = new SelectList(dictionary, "Key", "Value");  
        }


        public ActionResult SearchForCustomerName(string SearchForCustomer, int? Category)
        {
            var customerdata = customerdatasRepo.SearchFilterQuery(SearchForCustomer, Category);
            SetCategoryDDLListItem();
            return View("Index", customerdata);
        }






        // GET: CustomerDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 customerData = customerdatasRepo.Find(1);
            if (customerData == null)
            {
                return HttpNotFound();
            }
            return View(customerData);
        }

        // GET: CustomerDatas/Create
        public ActionResult Create()
        {
            SetCategoryDDLListItem();
            return View();
        }

        // POST: CustomerDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                customerdatasRepo.Add(客戶資料);
                customerdatasRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: CustomerDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            SetCategoryDDLListItem();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Use Id.Value cause id?
            客戶資料 customerData = customerdatasRepo.Find(id.Value);
            if (customerData == null)
            {
                return HttpNotFound();
            }
            return View(customerData);
        }

        // POST: CustomerDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            SetCategoryDDLListItem();
            if (ModelState.IsValid)
            {
                var db = customerdatasRepo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: CustomerDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerdatasRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: CustomerDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 customerData = customerdatasRepo.Find(id);
            customerdatasRepo.Delete(customerData);
            customerdatasRepo.UnitOfWork.Commit();


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerdatasRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }




        /// <summary>
        /// expoert 客戶資料 excel.
        /// </summary>
        /// <returns></returns>
        public ActionResult CusDataExport()
        {           
            using (XLWorkbook wb = new XLWorkbook())
            {                
                var data = customerdatasRepo.All().Select(c => new { c.客戶名稱, c.客戶分類, c.統一編號, c.電話,c.傳真,c.地址,c.Email });
                var ws = wb.Worksheets.Add("cusdata", 1);                
                ws.Cell(1, 1).InsertData(data);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);                    
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    // application/vnd.ms-excel
                    return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "客戶資料.xlsx");
                }
            }
        }




    }
}
