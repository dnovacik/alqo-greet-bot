using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlqoGreetBot.Models.Explorer
{
    public class SummaryModel
    {
        [JsonProperty("data")]
        public List<SummaryInfo> Summaries { get; set; }
    }

    public class SummaryInfo
    {
        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("supply")]
        public double Supply { get; set; }

        [JsonProperty("hashrate")]
        public string Hashrate { get; set; }

        [JsonProperty("lastPrice")]
        public double LastPrice { get; set; }

        [JsonProperty("connections")]
        public int Connections { get; set; }

        [JsonProperty("blockcount")]
        public int BlockCount { get; set; }

        [JsonProperty("alqoUSD")]
        public string AlqoUSDString { get; set; }

        public double AlqoUSD => double.Parse(this.AlqoUSDString);

        public double AlqoBTC => double.Parse(this.AlqoBTCString);

        [JsonProperty("alqoBTC")]
        public string AlqoBTCString { get; set; }

        [JsonProperty("marketCap")]
        public double MarketCap { get; set; }

        public DateTime TimeStamp => DateTime.UtcNow;
    }
}
