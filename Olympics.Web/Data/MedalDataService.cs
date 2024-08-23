using Olympics.Core.Entities;
using Newtonsoft.Json;

namespace Olympics.Web.Data {
    public class MedalDataService {
        public async Task<List<MedalData>> GetMedals() {
            var json = await File.ReadAllTextAsync("wwwroot/data/data.json");
            return JsonConvert.DeserializeObject<MedalList>(json)!.Medals.OrderByDescending(x => x.Year).ToList();
        }
    }
}
