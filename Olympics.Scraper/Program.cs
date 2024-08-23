using Newtonsoft.Json;
using Olympics.Core.Entities;
using Olympics.Scraper.Services;

namespace Olympics.Scraper {
    public class Program {
        [STAThread]
        public static void Main(string[] args) {
            try {
                var provider = new MedalListPageProvider();
                provider.ProcessMedalPages();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error! - {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
