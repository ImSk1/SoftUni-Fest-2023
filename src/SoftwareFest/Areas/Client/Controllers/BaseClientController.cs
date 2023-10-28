namespace SoftwareFest.Areas.Client.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class BaseClientController : Controller
    {
    }
}
