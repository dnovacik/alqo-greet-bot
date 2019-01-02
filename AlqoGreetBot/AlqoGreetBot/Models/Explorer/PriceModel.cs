using System;
using System.Collections.Generic;
using System.Text;

namespace AlqoGreetBot.Models.Explorer
{
    public class PriceModel
    {
        public SummaryInfo Summary { get; set; }
        public PriceHistoryModel History { get; set; }
    }
}
