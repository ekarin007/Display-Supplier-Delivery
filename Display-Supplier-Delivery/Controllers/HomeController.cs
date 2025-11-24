using System.Diagnostics;
using Display_Supplier_Delivery.Data;
using Display_Supplier_Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Display_Supplier_Delivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static QrcodeV2Context _qrcodeV2contenxt;

        public HomeController(ILogger<HomeController> logger, QrcodeV2Context qrcodeV2contenxt)
        {
            _logger = logger;
            _qrcodeV2contenxt = qrcodeV2contenxt;
        }

        public async Task<IActionResult> Index()
        {
            var getData = await _qrcodeV2contenxt.TbSupplierPlanShippings.Where(x => x.StrDay == "1").ToListAsync();

            var data = new List<ReceivingItemVM>();

            if (getData.Count > 0)
            {
                int i = 1;
                foreach (var item in getData)
                {
                    var getSupName = _qrcodeV2contenxt.TbSuppliers.Where(x => x.SupId == item.SupId).FirstOrDefault();
                    var dataItem = new ReceivingItemVM { ItemNo = i, Supplier = $"{getSupName.SupNameTh}", Time = $"{item.StartTime}-{item.EndTime}", VolumePN = 0, VolumePC = 0, Status = "ON PLAN", };
                    i++;
                    data.Add(dataItem);
                }


            }


            return View(data);
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
