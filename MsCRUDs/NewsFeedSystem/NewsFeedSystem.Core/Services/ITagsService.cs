using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NewsFeedSystem.Core.Results;

namespace NewsFeedSystem.Core.Services
{
    public interface ITagsService
    {
        Task<CreateResult> Create(Tag request);
    }
}
