using AlqoGreetBot.Common;
using AlqoGreetBot.Models.Explorer;
using AlqoGreetBot.Services;
using AlqoGreetBot.Services.Interfaces;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlqoGreetBot.Modules
{
    public class ExplorerModule : ModuleBase<SocketCommandContext>
    {
        private readonly IExplorerModuleService _explorerService;

        public ExplorerModule(IExplorerModuleService explorerService)
        {
            this._explorerService = explorerService;
        }

        [Command("price")]
        public async Task Price()
        {
            var result = await this._explorerService.GetLastPrice();
            var resultString = string.Empty;

            if (result != null)
            {
                resultString = $"```Price USD: {result.Summary.AlqoUSD}\nPrice BTC: {result.Summary.AlqoBTC}\n24 Hours Average BTC: {result.History.AlqoBTC24H}\n24 Hours Average USD: {result.History.AlqoUSD24H}```";
            }
            else
            {
                resultString = "Something went wrong";
            }

            await Context.Message.Author.SendMessageAsync(resultString);
        }

        [Command("mninfo")]
        public async Task MnInfo(string ip)
        {
            var ipDashed = ip.Replace('.', '-');
            var result = await this._explorerService.GetMasternodeInfo(ipDashed);
            var resultString = this.CreateMasternodeInfoMessage(result);

            await Context.Message.Author.SendMessageAsync(resultString);
        }

        private string CreateMasternodeInfoMessage(MasternodeInfoModel model)
        {
            var result =
                $"```" +
                $"Status: {model.Status}\nIP: {model.IP}\nTX: {model.TX}\nPublic Key:{model.Pubkey}\nLast Seen: {model.LastSeenString}\nLast Paid: {model.LastPaidString}\nOverall Payouts: {model.OverallPayoutsInt}" +
                $"\n\nGeographical Data:" +
                $"\n\tCountry Code: {model.GeoData.CountryCode}" +
                $"\n\tCountry Name: {model.GeoData.CountryName}" +
                $"{(!string.IsNullOrEmpty(model.GeoData.RegionCode) ? $"\n\tRegion Code: {model.GeoData.RegionCode}" : string.Empty)}" +
                $"{(!string.IsNullOrEmpty(model.GeoData.RegionName) ? $"\n\tRegion Name: {model.GeoData.RegionName}" : string.Empty)}" +
                $"{(!string.IsNullOrEmpty(model.GeoData.City) ? $"\n\tCity: {model.GeoData.City}" : string.Empty)}" +
                $"{(!string.IsNullOrEmpty(model.GeoData.ZipCode) ? $"\n\tZip Code: {model.GeoData.ZipCode}" : string.Empty)}" +
                $"{(model.GeoData.Latitude != 0d ? $"\n\tLatitude: {model.GeoData.Latitude}" : string.Empty)}" +
                $"{(model.GeoData.Longitude != 0d ? $"\n\tLatitude: {model.GeoData.Longitude}" : string.Empty)}" +
                $"{(!string.IsNullOrEmpty(model.GeoData.MetroCode) ? $"\n\tMetro Code: {model.GeoData.MetroCode}" : string.Empty)}" +
                $"```";

            return result;
        }
    }
}
