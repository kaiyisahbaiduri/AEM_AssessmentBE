using AEMAssessment.WebAPI.DBContext;
using AEMAssessment.WebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AEMAssessment.WebAPI.Service
{
    public class SyncDataService
    {
        private readonly SyncDataContext _context;
        private readonly ApiClient _apiClient;

        public SyncDataService(SyncDataContext context, ApiClient apiClient)
        {
            _context = context;
            _apiClient = apiClient;
        }

        public async Task SyncDataAsync(string token)
        {
            var data = await _apiClient.GetPlatformWellActualAsync(token);

            foreach (var platformJson in data)
            {
                var platformId = (int)platformJson["id"];
                var platform = await _context.Platforms.Include(p => p.Wells).FirstOrDefaultAsync(p => p.Id == platformId);

                if (platform == null)
                {
                    platform = new Platform
                    {
                        Id = platformId,
                        UniqueName = platformJson["uniqueName"].ToString(),
                        Latitude = (double)platformJson["latitude"],
                        Longitude = (double)platformJson["longitude"],
                        CreatedAt = (DateTime)platformJson["createdAt"],
                        UpdatedAt = (DateTime)platformJson["updatedAt"],
                        Wells = new List<Well>()
                    };
                    _context.Platforms.Add(platform);
                }
                else
                {
                    UpdatePlatform(platform, platformJson);
                    _context.Platforms.Update(platform);
                }

                foreach (var wellJson in platformJson["well"])
                {
                    var wellId = (int)wellJson["id"];
                    var well = platform.Wells.FirstOrDefault(w => w.Id == wellId);

                    if (well == null)
                    {
                        well = new Well
                        {
                            Id = wellId,
                            PlatformId = platformId,
                            UniqueName = wellJson["uniqueName"].ToString(),
                            Latitude = (double)wellJson["latitude"],
                            Longitude = (double)wellJson["longitude"],
                            CreatedAt = (DateTime)wellJson["createdAt"],
                            UpdatedAt = (DateTime)wellJson["updatedAt"]
                        };
                        platform.Wells.Add(well);
                    }
                    else
                    {
                        UpdateWell(well, wellJson);
                        _context.Wells.Update(well);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        private void UpdatePlatform(Platform platform, dynamic platformJson)
        {
            platform.UniqueName = platformJson["uniqueName"].ToString();
            platform.Latitude = (double)platformJson["latitude"];
            platform.Longitude = (double)platformJson["longitude"];
            platform.CreatedAt = (DateTime)platformJson["createdAt"];
            platform.UpdatedAt = (DateTime)platformJson["updatedAt"];
        }

        private void UpdateWell(Well well, dynamic wellJson)
        {
            well.UniqueName = wellJson["uniqueName"].ToString();
            well.Latitude = (double)wellJson["latitude"];
            well.Longitude = (double)wellJson["longitude"];
            well.CreatedAt = (DateTime)wellJson["createdAt"];
            well.UpdatedAt = (DateTime)wellJson["updatedAt"];
        }
    }
}
