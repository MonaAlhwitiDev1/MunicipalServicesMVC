using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVC.Application.Services;

namespace MunicipalServicesMVC.Controllers
{
    [Authorize(Roles = "مدير النظام")]
    public class ServiceRequestsController : Controller
    {
        private readonly IServiceRequestService _requestService;

        public ServiceRequestsController(
            IServiceRequestService requestService)
        {
            _requestService = requestService;
        }

        public IActionResult Index()
        {
            var requests = _requestService.GetAll();

            return View(requests);
        }
    }
}