using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Core.Configs;
using Core.Extensions;
using Core.Types.RanobeOvh;
using HtmlAgilityPack;

namespace Core.Logic.Getters.RanobeOvh; 

public class MangaOvhGetter(BookGetterConfig config) : RanobeOvhGetterBase(config) {
    protected override Uri SystemUrl => new("https://manga.ovh/");
    
    public override Task Init() {
        Config.Client.DefaultRequestVersion = HttpVersion.Version20;
        Config.Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Safari/605.1.15");
        Config.Client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        Config.Client.DefaultRequestHeaders.Add("Accept-Language", "ru");
        Config.Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        Config.Client.DefaultRequestHeaders.Add("Referrer", SystemUrl.ToString());
        
        return Task.CompletedTask;
    }

    protected override async Task<HtmlDocument> GetChapter(RanobeOvhChapterShort ranobeOvhChapterFull)
    {
        var data = await Config.Client.GetFromJsonAsync<RanobeOvhChapterFull>($"https://api.{SystemUrl.Host}/chapter/{ranobeOvhChapterFull.Id}");
        var sb = new StringBuilder();

        foreach (var page in data.Pages)
        {
            sb.Append($"<img src='{page.Image}'/>");
        }

        return sb.AsHtmlDoc();
    }
}