using AEMAssessment.WebAPI.Model;
using AEMAssessment.WebAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace AEMAssessment.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncDataController : ControllerBase
    {
        private readonly ApiClient _apiClient;
        private readonly SyncDataService _dataSyncService;

        public SyncDataController(ApiClient apiClient, SyncDataService dataSyncService)
        {
            _apiClient = apiClient;
            _dataSyncService = dataSyncService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {
            if (loginModel == null || string.IsNullOrWhiteSpace(loginModel.Username) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                return BadRequest("Invalid login details");
            }

            var token = await _apiClient.LoginAsync(loginModel.Username, loginModel.Password);
            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized("Login failed");
            }

            await _dataSyncService.SyncDataAsync(token);
            return Ok("Data synchronization completed");
        }
    }
}
