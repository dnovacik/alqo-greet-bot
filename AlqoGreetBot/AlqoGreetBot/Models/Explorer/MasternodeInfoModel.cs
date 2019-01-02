using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AlqoGreetBot.Common.Extensions;

namespace AlqoGreetBot.Models.Explorer
{
    public class MasternodeInfoModel
    {
        [JsonProperty("STATUS")]
        public string Status { get; set; }

        [JsonProperty("IP")]
        public string IP { get; set; }

        [JsonProperty("TX")]
        public string TX { get; set; }

        [JsonProperty("PUBKEY")]
        public string Pubkey { get; set; }

        [JsonProperty("FIRSTSEEN")]
        public int FirstSeen { get; set; }

        public string FirstSeenString => Extensions.ParseUnixTimeToDateTimeString(this.FirstSeen);

        [JsonProperty("LASTSEEN")]
        public int LastSeen { get; set; }

        public string LastSeenString => Extensions.ParseUnixTimeToDateTimeString(this.LastSeen);

        [JsonProperty("GEODATA")]
        public GeoData GeoData { get; set; }

        [JsonProperty("LASTPAID")]
        public int LastPaid { get; set; }

        public string LastPaidString => Extensions.ParseUnixTimeToDateTimeString(this.LastPaid);

        [JsonProperty("OVERALLPAYOUTS")]
        public string OverallPayouts { get; set; }

        public int OverallPayoutsInt => int.Parse(this.OverallPayouts);
    }

    public class GeoData
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("metro_code")]
        public string MetroCode { get; set; }
    }
}
