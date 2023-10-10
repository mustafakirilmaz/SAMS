using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.Models;
using System.Collections.Generic;

namespace SAMS.Common.Helpers
{
    public static class GridHelper
    {
        //public static ServiceResult ToGridServiceResult<T>(this GridResult<List<T>> gridData)
        //{
        //    return new ServiceResult<List<T>>(gridData.Result, gridData.TotalCount.ToString(), ResultType.Success);
        //}
        public static ServiceResult<List<T>> ToGridServiceResult<T>(this GridResult<List<T>> gridData)
        {
            return new ServiceResult<List<T>>(gridData.Result, gridData.TotalCount.ToString(), ResultType.Success);
        }
    }
}
