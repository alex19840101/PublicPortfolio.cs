using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFeedSystem.Core.Services
{
    public interface ITopicsService
    {
        Task<CreateResult> Create(Topic request);
    }
}
