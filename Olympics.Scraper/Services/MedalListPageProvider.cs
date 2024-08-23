using Newtonsoft.Json;
using Olympics.Core.Entities;

namespace Olympics.Scraper.Services {
    public class MedalListPageProvider {
        private readonly string MedalsPageUrl = "https://hu.wikipedia.org/wiki/Magyarorsz%C3%A1g_{0}_{1}._%C3%A9vi_ny%C3%A1ri_olimpiai_j%C3%A1t%C3%A9kokon";
        
        public IEnumerable<int> GetYears(Season season) {
            var startYear = season == Season.Summer ? 1896 : 1924;
            var isOlympicYear = true;
            var yearCounter = 0;
            for (int i = startYear; i <= DateTime.Now.Year; i++) { 
                if (isOlympicYear) {
                    yield return i;
                    isOlympicYear = false;
                }
                yearCounter++;
                if (yearCounter == 4) {
                    isOlympicYear = true;
                    yearCounter = 0;
                }
            }    
        }

        public string GetMedalsPageLink(int year) {
            var prefix = year >= 2000 ? "a" : "az";
            return string.Format(MedalsPageUrl, prefix, year);
        }

        public string GetMedalsPage(string link) {
            string html = null;
            try {
                HttpClient client = new HttpClient();
                html = client.GetAsync(link).Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException ex) { 
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    return html;
                }
            }
            return html;
        }

        public void ProcessMedalPages() {
            var medalList = new MedalList();
            var parser = new MedalListPageParser();
            foreach (var year in GetYears(Season.Summer)) {
                var html = GetMedalsPage(GetMedalsPageLink(year));
                if (html != null) {
                    Console.WriteLine($"Processing: {year} - summer");
                    var result = parser.ParseMedalPage(html, year, Season.Summer);
                    medalList.Medals.AddRange(result);
                }
            }
            foreach (var year in GetYears(Season.Winter)) {
                var html = GetMedalsPage(GetMedalsPageLink(year));
                if (html != null) {
                    Console.WriteLine($"Processing: {year} - winter");
                    var result = parser.ParseMedalPage(html, year, Season.Winter);
                    medalList.Medals.AddRange(result);
                }
            }
            File.WriteAllText(@"data.json", JsonConvert.SerializeObject(medalList, Formatting.Indented));
        }
    }
}
