﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatelAPI.Areas.Data
{
    public interface ISageData
    {
        Task<Dictionary<string, string>> GetDBLoginDetails(string systemID);

        Task<Dictionary<string, string>> GetDBLoginDetailsLive(string systemID);

        Task<bool> StockAllocateAllowed(string order_no);

    }
}
