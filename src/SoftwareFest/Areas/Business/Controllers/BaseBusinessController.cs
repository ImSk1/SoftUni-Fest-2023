using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SoftwareFest.Areas.Business.Controllers
{
    [Area("Business")]
    [Authorize(Roles = "Business")]
    public class BaseBusinessController : Controller
    {

    }
}
