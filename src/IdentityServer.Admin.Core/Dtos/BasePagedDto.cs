using System.Collections.Generic;

namespace IdentityServer.Admin.Core.Dtos
{
    public class BasePagedDto<T>
    {
        public BasePagedDto()
        {
            DataPagedList = new List<T>();
        }

        public IList<T> DataPagedList { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
    }
}
