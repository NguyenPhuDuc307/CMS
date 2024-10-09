using CMS.Models.VNPay;
using CMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    public class PaysController : Controller
    {
        private readonly IVNPayService _vnPayService;
        public PaysController(IVNPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public static Dictionary<string, string> vnp_TransactionStatus = new Dictionary<string, string>()
        {
            {"00","Giao dịch thành công" },
            {"01","Giao dịch chưa hoàn tất" },
            {"02","Giao dịch bị lỗi" },
            {"04","Giao dịch đảo (Khách hàng đã bị trừ tiền tại Ngân hàng nhưng GD chưa thành công ở VNPAY)" },
            {"05","VNPAY đang xử lý giao dịch này (GD hoàn tiền)" },
            {"06","VNPAY đã gửi yêu cầu hoàn tiền sang Ngân hàng (GD hoàn tiền)" },
            {"07","Giao dịch bị nghi ngờ gian lận" },
            {"09","GD Hoàn trả bị từ chối" }
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Pay(CheckoutViewModel request)
        {
            if (ModelState.IsValid)
            {
                if (request.PaymentMethod == "VNPay")
                {
                    string cleanedAmount = request.Amount!.Replace(".", "");
                    var vnPayModel = new VNPaymentRequestModel
                    {
                        // Fill Amount 
                        Amount = Double.Parse(cleanedAmount),
                        CreatedDate = DateTime.Now,
                        Description = $"{request.FullName} {request.PhoneNumber}",
                        FullName = request.FullName,
                        OrderId = new Random().Next(1000, 100000)
                    };
                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }

                // Processed

                return View();
            }
            return View(request);
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }

        public IActionResult PaymentFail()
        {
            return View();
        }

        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response.VNPayResponseCode == "00")
            {
                // Processed successfully
                return RedirectToAction(nameof(PaymentSuccess));
            }

            // Get the message corresponding to VNPayResponseCode from the dictionary
            if (vnp_TransactionStatus.TryGetValue(response.VNPayResponseCode!, out var message))
            {
                TempData["Message"] = $"Payment error: {message}";
            }
            else
            {
                TempData["Message"] = $"Unknown payment error: {response.VNPayResponseCode}";
            }

            return RedirectToAction(nameof(PaymentFail));
        }
    }
}