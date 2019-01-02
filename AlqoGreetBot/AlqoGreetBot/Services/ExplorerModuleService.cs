using AlqoGreetBot.Common;
using AlqoGreetBot.Models.Explorer;
using AlqoGreetBot.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AlqoGreetBot.Services
{
    public class ExplorerModuleService : IExplorerModuleService
    {
        private static Timer PriceTimer { get; set; }
        private static int LastHour { get; set; } = DateTime.UtcNow.Hour;
        private static HttpClient _client = new HttpClient();

        public static CustomStack<SummaryInfo> _priceData { get; set; }
            = new CustomStack<SummaryInfo>(24);

        public PriceHistoryModel PriceHistory { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public ExplorerModuleService()
        {
            PriceTimer = new Timer(60 * 60 * 1000)
            {
                Enabled = true
            };

            PriceTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            _priceData.CollectionChanged += new NotifyCollectionChangedEventHandler(OnPriceHistoryChanged);
        }

        private void OnPriceHistoryChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var nonNull = _priceData.Where(x => x != null).ToList();

            if (nonNull.Count != 0)
            {
                this.PriceHistory = new PriceHistoryModel
                {
                    AlqoBTC24H = nonNull.Sum(x => x.AlqoBTC) / nonNull.Count,
                    AlqoUSD24H = nonNull.Sum(x => x.AlqoUSD) / nonNull.Count
                };
            }
        }

        /// <summary>
        /// Loads the price if necessary
        /// </summary>
        /// <returns>async Task</returns>
        private async Task LoadPrice()
        {
            _priceData.TryPeek(out SummaryInfo last);

            if (last == null)
            {
                await GetSummary();
            }
        }

        /// <summary>
        /// Event handler triggered every hour
        /// </summary>
        /// <param name="source">object source</param>
        /// <param name="e">event args</param>
        private static async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (LastHour < DateTime.UtcNow.Hour || (LastHour == 23 && DateTime.UtcNow.Hour == 0))
            {
                await GetSummary();

                var lastItem = _priceData.Peek();

                if (lastItem != null)
                {
                    LastHour = lastItem.TimeStamp.Hour;
                }
            }
        }

        /// <summary>
        /// Gets last price from the API, if there is no price in the stack, forces to get one
        /// </summary>
        /// <returns>SummaryInfo Task</returns>
        public async Task<PriceModel> GetLastPrice()
        {
            await this.LoadPrice();
            return await Task.Run(() => new PriceModel { Summary = _priceData.Peek(), History = this.PriceHistory });
        }

        /// <summary>
        /// Pushes the summary to stack
        /// </summary>
        /// <returns>a Task</returns>
        public static async Task GetSummary()
        {
            var summaryResponse = await _client.GetStringAsync($"https://explorer.alqo.org/api/summary");
            var summary = JsonConvert.DeserializeObject<SummaryModel>(summaryResponse);

            var lastEntry = summary.Summaries.FirstOrDefault();

            if (lastEntry != null)
            {
                LastHour = lastEntry.TimeStamp.Hour;
                _priceData.Push(lastEntry);
            }
        }

        /// <summary>
        /// Gets a masternode info
        /// </summary>
        /// <param name="ip">[string] ip string parameter</param>
        /// <returns>MasternodeInfoModel Task</returns>
        public async Task<MasternodeInfoModel> GetMasternodeInfo(string ip)
        {
            var mnInfoResponse = await _client.GetStringAsync($"https://explorer.alqo.org/api/masternode/IP/{ip}");
            var result = JsonConvert.DeserializeObject<MasternodeInfoModel>(mnInfoResponse);

            return result;
        }
    }
}
