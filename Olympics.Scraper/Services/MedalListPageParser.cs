using HtmlAgilityPack;
using Olympics.Core.Entities;
using System.Web;

namespace Olympics.Scraper.Services {
    public class MedalListPageParser {
        public List<MedalData> ParseMedalPage(string html, int year, Season season) {
            List<MedalData> results = new List<MedalData>();

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNodeCollection h2 = htmlDoc.DocumentNode.SelectNodes("//h2[@id[contains(., 'Érmesek')]]");
            if (h2 == null) {
                h2 = htmlDoc.DocumentNode.SelectNodes("//h2[@id[contains(., 'érmesek')]]");
            }
            if (h2 == null) {
                return results;
            }
            HtmlNode table = h2.FirstOrDefault().ParentNode;
            while (table != null) {
                if (table.NodeType == HtmlNodeType.Element) { 
                    if (table.Name == "table") {
                        break;
                    }
                }
                table = table.NextSibling;
            }
            if (table == null) {
                return results;
            }
            var hasDateColumn = table.ChildNodes[1].ChildNodes.Take(1).FirstOrDefault().InnerHtml.Contains("Dátum");
            var rows = table.ChildNodes[1].ChildNodes.Skip(1).Where(row => row.Name == "tr").ToList();
            foreach(var row in rows) {
                var result = new MedalData() { 
                    Year = year,
                    Season = season
                };
                results.Add(result);

                var colNum = 0;
                foreach (var col in row.ChildNodes) {
                    try {
                        if (col.Name == "#text") {
                            continue;
                        }
                        if (colNum == 0) {
                            var medal = col.ChildNodes[0].InnerText;
                            Rank rank = Rank.Gold;
                            if (medal == "2") {
                                rank = Rank.Silver;
                            }
                            else if (medal == "3") {
                                rank = Rank.Bronz;
                            }
                            result.Rank = rank;
                        }
                        if (colNum == 1) {
                            foreach(var node in col.ChildNodes) {
                                if (node.Name == "a") {
                                    var name = HttpUtility.HtmlDecode(node.InnerText);
                                    result.Names.Add(name);
                                }
                            }
                        }
                        if (colNum == 2) {
                            var sport = HttpUtility.HtmlDecode(col.ChildNodes[0].InnerText);
                            result.Sport = sport;
                        }
                        if (colNum == 3) {
                            foreach (var node in col.ChildNodes) {
                                if (node.Name == "a") {
                                    var competition = HttpUtility.HtmlDecode(node.InnerText);
                                    result.Competition = competition;
                                }
                            }
                        }
                        if (colNum == 4) {
                            if (hasDateColumn) {
                                var date = col.ChildNodes[1].InnerText;
                                result.Date = date;
                            }
                        }
                        colNum++;
                    }
                    catch { }
                }
            }
            return results;
        }
    }
}
