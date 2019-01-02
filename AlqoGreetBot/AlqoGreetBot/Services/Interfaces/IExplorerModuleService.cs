using AlqoGreetBot.Models.Explorer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlqoGreetBot.Services.Interfaces
{
    public interface IExplorerModuleService
    {
        Task<PriceModel> GetLastPrice();
        Task<MasternodeInfoModel> GetMasternodeInfo(string ip);
    }
}
