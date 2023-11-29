using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MPSDataMananger.library.DataAccess;
using MPSDataMananger.library.Models;

namespace MPSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _config;
        public InventoryController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData(_config);
            return data.GetInventory();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(InventoryModel invenory)
        {
            InventoryData data = new InventoryData(_config);
            data.SaveInventoryRecord(invenory);
        }
    }
}
