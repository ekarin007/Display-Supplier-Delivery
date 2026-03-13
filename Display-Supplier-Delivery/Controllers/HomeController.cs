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

        public async Task<IActionResult> Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                id = id.ToUpper();
            }

            string todayStr = this.GetTodayNumberBySwitch();
            //string todayStr = "5";
            var getData = await _qrcodeV2contenxt.TbSupplierPlanShippings
                .AsNoTracking()
                .Where(x => x.StrDay == todayStr)
                .ToListAsync();

            // Filter and Sort by Time
            // "End at current time, if exceeds don't bring it" -> StartTime <= Now
            var currentTime = DateTime.Now.TimeOfDay;
            getData = getData
                .Where(x =>
                {
                    if (TimeSpan.TryParse(x.StartTime, out TimeSpan start))
                    {
                        return start <= currentTime;
                    }
                    return false;
                })
                .OrderBy(x =>
                {
                    TimeSpan.TryParse(x.StartTime, out TimeSpan start);
                    return start;
                })
                .ToList();

            string currentPeriod = "";
            var lastPlan = getData.LastOrDefault();
            if (lastPlan != null)
            {
                currentPeriod = $"{lastPlan.StartTime} - {lastPlan.EndTime}";
            }
            ViewBag.CurrentPeriod = currentPeriod;

            // Set Page Title
            ViewBag.PageTitle = (id == "PT") ? "PAINTING RECEIVING LIST" : "RAW MATERIAL RECEIVING LIST";

            var data = new List<ReceivingItemVM>();

            if (getData.Any())
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                // Batch Query 1: Get all relevant suppliers
                // Use a subquery to avoid passing a list of IDs (which causes EF to use OPENJSON/CTE)
                var relevantSupIdsQuery = _qrcodeV2contenxt.TbSupplierPlanShippings
                    .Where(x => x.StrDay == todayStr)
                    .Select(x => x.SupId);

                var suppliersList = await _qrcodeV2contenxt.TbSuppliers
                    .AsNoTracking()
                    .Where(x => relevantSupIdsQuery.Contains(x.SupId))
                    .Select(x => new { x.SupId, x.SupNameTh })
                    .ToListAsync();

                var suppliers = suppliersList
                    .GroupBy(x => x.SupId)
                    .ToDictionary(g => g.Key, g => g.First().SupNameTh);

                // Batch Query 2: Get all relevant Kanbans
                // Filter ONLY by Date in the DB to avoid passing a list of Supplier Names.
                // We will filter by Supplier Name in memory.

                var query = from k in _qrcodeV2contenxt.TbKanbans
                            join p in _qrcodeV2contenxt.TbMstPartNos on k.PartNo equals p.PartNo
                            where k.DatePlan >= today &&
                                  k.DatePlan < tomorrow
                            select new { k, p };

                if (!string.IsNullOrEmpty(id))
                {
                    query = query.Where(x => x.p.Status == id);
                }

                var flatKanbans = await query.Select(x => new
                {
                    SupplierName = x.p.Supplier,
                    x.k.QtyPlanReceive,
                    x.k.QtyReceive,
                    x.k.DateReceive,
                    // Get latest history user
                    UserId = _qrcodeV2contenxt.TbHistories
                        .Where(h => h.Kanban == x.k.Kanban && h.PartNo == x.k.PartNo)
                        .OrderByDescending(h => h.EventDate)
                        .Select(h => h.UserId)
                        .FirstOrDefault()
                }).AsNoTracking().ToListAsync();

                // Get All Employees involved
                var userIds = flatKanbans
                    .Where(x => !string.IsNullOrEmpty(x.UserId))
                    .Select(x => x.UserId).FirstOrDefault();


                var employees = _qrcodeV2contenxt.TbEmployees.Where(x => x.EmpId == userIds).Select(x => x.EmpNameTh).FirstOrDefault();


                // Group in memory
                var groupedKanbans = flatKanbans
                    .GroupBy(x => x.SupplierName)
                    .Select(g => new
                    {
                        SupplierName = g.Key,
                        Count = g.Count(),
                        SumQtyPlan = g.Sum(x => x.QtyPlanReceive),
                        SumQtyReceive = g.Sum(x => x.QtyReceive),
                        MaxReceive = g.Max(x => x.DateReceive),
                        // Get the first receiver found for this supplier group
                        ReceiverId = g.Where(x => !string.IsNullOrEmpty(x.UserId)).Select(x => x.UserId).FirstOrDefault()
                    })
                    .ToList();

                // Convert to dictionary for fast lookup
                var kanbanDict = groupedKanbans.ToDictionary(x => x.SupplierName);

                int i = 1;
                foreach (var item in getData)
                {
                    if (suppliers.TryGetValue(item.SupId, out string supName))
                    {
                        int volumePN = 0;
                        int volumePC = 0;
                        string status = "ON PLAN";
                        string receiverName = "";
                        string materialStatus = "ส่งวัตถุดิบครบ";

                        if (kanbanDict.TryGetValue(supName, out var kData))
                        {
                            volumePN = kData.Count;
                            volumePC = kData.SumQtyPlan;



                            if (!string.IsNullOrEmpty(kData.ReceiverId))
                            {
                                receiverName = employees;
                            }

                            // Material Status Logic
                            if (kData.SumQtyPlan != kData.SumQtyReceive)
                            {
                                materialStatus = "ส่งวัตถุดิบไม่ครบ";
                            }

                            // Delay Check Logic
                            // 1. Check if complete (Plan == Actual)

                            // 2. Check Time
                            string timeStr = $"{item.StartTime}-{item.EndTime}";
                            if (!string.IsNullOrEmpty(timeStr) && timeStr.Contains("-"))
                            {
                                var timeParts = timeStr.Split('-');
                                if (timeParts.Length > 1 && TimeSpan.TryParse(timeParts[1].Trim(), out TimeSpan endTime))
                                {
                                    DateTime deadline = today.Add(endTime);
                                    if (kData.MaxReceive.HasValue && kData.MaxReceive.Value > deadline)
                                    {
                                        status = "DELAY";
                                    }
                                }
                            }


                        }

                        if (volumePN == 0 && volumePC == 0) continue; // Filter out 0 volumes

                        var dataItem = new ReceivingItemVM
                        {
                            ItemNo = i,
                            Supplier = supName,
                            Time = $"{item.StartTime}-{item.EndTime}",
                            VolumePN = volumePN,
                            VolumePC = volumePC,
                            Status = status,
                            Receiver = receiverName,
                            MaterialStatus = materialStatus
                        };
                        data.Add(dataItem);
                        i++;
                    }
                }
            }

            return View(data);
        }

        public string GetTodayNumberBySwitch()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday: return "1";
                case DayOfWeek.Tuesday: return "2";
                case DayOfWeek.Wednesday: return "3";
                case DayOfWeek.Thursday: return "4";
                case DayOfWeek.Friday: return "5";
                case DayOfWeek.Saturday: return "6";
                case DayOfWeek.Sunday: return "7";
                default: return "0";
            }
        }

        public async Task<IActionResult> Details(string supplier, string time)
        {
            ViewBag.Supplier = supplier;
            ViewBag.Time = time;


            if (string.IsNullOrEmpty(supplier))
            {
                return View(new List<PartDetailViewModel>());
            }

            // 1. Get all parts for this supplier
            var parts = await _qrcodeV2contenxt.TbMstPartNos
                .AsNoTracking()
                .Where(x => x.Supplier == supplier)
                .Select(x => x.PartNo)
                .ToListAsync();

            if (!parts.Any())
            {
                return View(new List<PartDetailViewModel>());
            }

            // 2. Query Kanban for these parts and "Today"
            // Borrowing logic from Index: GetTodayNumberBySwitch -> Today's plan
            // Actually, typical logic is DatePlan == Today. 
            // The Index uses: Where(x => x.StrDay == todayStr) for ShipPlan, 
            // and `where k.DatePlan >= today && k.DatePlan < tomorrow` for Kanban.
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var flatKanbans = await (
                from k in _qrcodeV2contenxt.TbKanbans
                join p in _qrcodeV2contenxt.TbMstPartNos on k.PartNo equals p.PartNo
                where p.Supplier == supplier && 
                      ((k.DatePlan >= today && k.DatePlan < tomorrow) || 
                       (k.DateReceive >= today && k.DateReceive < tomorrow))
                select k
            ).AsNoTracking().ToListAsync();

            var kanbanData = flatKanbans
                .GroupBy(k => k.PartNo)
                .Select(g => new
                {
                    PartNo = g.Key,
                    PlanPcs = g.Where(x => x.DatePlan >= today && x.DatePlan < tomorrow).Sum(x => x.QtyPlanReceive),
                    PlanBox = g.Where(x => x.DatePlan >= today && x.DatePlan < tomorrow && x.QtyPlanReceive != 0).Count(),
                    ActualPcs = g.Where(x => x.DateReceive >= today && x.DateReceive < tomorrow).Sum(x => x.QtyReceive),
                    ActualBox = g.Where(x => x.DateReceive >= today && x.DateReceive < tomorrow && x.QtyReceive != 0).Count(),
                    MaxReceive = g.Max(x => x.DateReceive)
                })
                .ToList();

            // 3. Convert to ViewModel directly from kanbanData
            // This ensures we only show parts that have Kanban data for today.
            var viewModel = kanbanData.Select(kData => new PartDetailViewModel
            {
                PartNo = kData.PartNo,
                PlanPcs = kData.PlanPcs,
                PlanBox = kData.PlanBox,
                ActualPcs = kData.ActualPcs,
                ActualBox = kData.ActualBox,
                DiffPcs = kData.PlanPcs - kData.ActualPcs,
                DiffBox = kData.PlanBox - kData.ActualBox
            }).ToList();

            // Logic to check delay
            string status = "ON PLAN";
            if (viewModel.Any())
            {
                var sumDiffPcs = viewModel.Sum(x => x.DiffPcs);
                var sumDiffBox = viewModel.Sum(x => x.DiffBox);

                // If diff with diffbox is 0 (Completed)

                // Check if dateRecieve exceeds today and time
                if (!string.IsNullOrEmpty(time) && time.Contains("-"))
                {
                    var timeParts = time.Split('-');
                    // Handle potential format like "08:00 - 10:00" or "08:00-10:00"
                    if (timeParts.Length > 1 && TimeSpan.TryParse(timeParts[1].Trim(), out TimeSpan endTime))
                    {
                        DateTime deadline = today.Add(endTime);
                        // check max receive time
                        var maxRec = kanbanData.Max(x => x.MaxReceive);

                        if (maxRec.HasValue && maxRec.Value > deadline)
                        {
                            status = "DELAY";
                        }
                    }
                }

            }
            ViewBag.Status = status;

            return View(viewModel);
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
