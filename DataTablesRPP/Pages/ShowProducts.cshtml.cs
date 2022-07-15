using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTablesRPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DataTablesRPP.Models.ViewModels;

namespace DataTablesRPP.Pages
{
    public class ShowProductsModel : PageModel
    {
        NorthwindContext _nwctx = null;

        public List<Products> PList { get; private set; }

        public ShowProductsModel(NorthwindContext nwctx)
        {
            _nwctx = nwctx;
            IEnumerable<Products> PList = null;
        }
        public void OnGet()
        {
        }
        public void OnGetEdit(int id)
        {
        }
        public void OnPostDelete(int prodId)
        {
            Products pr = _nwctx.Products.Find(prodId);
            if (pr != null)
            {
                _nwctx.Products.Remove(pr);
                _nwctx.SaveChanges();
            }
        }
        //public void OnPostEdit(int prodId)
        //{
        // var prod = PList.Where(p => p.ProductId == prodId).FirstOrDefault<Products>();
        // if (prod != null)
        // prod.UnitPrice = prod.UnitPrice * 1.10m;
        //}
        public IActionResult OnPostLoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // starting row number 
                var start = Request.Form["start"].FirstOrDefault();
                // page length 10,20 
                var length = Request.Form["length"].FirstOrDefault();
                // sort column Name 
                var sortColumn = Request.Form["columns[" +
                Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // sort column direction (asc, desc) 
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // search text from search textbox 
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                // paging Size (10, 20, 50,100) 
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                // get all Customer data 
                PList = _nwctx.Products.ToList<Products>();
                // sorting 
               /* if (!(string.IsNullOrEmpty(sortColumn) &&
                string.IsNullOrEmpty(sortColumnDirection)))
                {
                    // orderby = requires system.linq.dynamic package
                    PList = (PList.AsQueryable<Products>()).OrderBy(sortColumn + " " +
                    sortColumnDirection).AsEnumerable<Products>();
                }*/
                // search has value
                if (!string.IsNullOrEmpty(searchValue))
                {
                    PList = PList.Where(m =>
                    (m.ProductName.ToUpper().IndexOf(searchValue.ToUpper()) >= 0) || (m.UnitPrice.ToString() ==
                    searchValue)).ToList<Products>();
                }
                //total number of rows counts 
                recordsTotal = PList.Count();
                //Paging 
                var data = PList.Skip(skip).Take(pageSize).ToList();
                return new JsonResult(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public PartialViewResult OnGetProductsModalPartial(int prodid)
        {
            // this handler returns _ProductsModalPartial
            Products pr = _nwctx.Products.Find(prodid);
            ProductEditVM pevm = new ProductEditVM(pr, "");
            return new PartialViewResult
            {
                ViewName = "_ProductEditPartial",
                ViewData = new ViewDataDictionary<ProductEditVM>(ViewData, pevm)
            };
        }
        public PartialViewResult OnPostProductsEditSave(ProductEditVM prod)
        {
            // this handler returns _ProductsModalPartial
            Products pr = _nwctx.Products.Find(prod.ProductId);
            pr.ProductName = prod.ProductName;
            pr.UnitPrice = prod.UnitPrice;
            pr.UnitsInStock = prod.UnitsInStock;
            if (ModelState.IsValid)
            {
                _nwctx.SaveChanges();
                prod.Message = "product updated..";
            }
            else
                prod.Message = "invalid data..";
            return new PartialViewResult
            {
                ViewName = "_ProductEditPartial",
                ViewData = new ViewDataDictionary<ProductEditVM>(ViewData, prod)
            };
        }
    }
}

