using SAMS.Infrastructure.Enums;
using System.Collections.Generic;

namespace SAMS.Infrastructure.Models
{
    public class ServiceResult
    {
        public ServiceResult(string message = "", ResultType state = ResultType.Success)
        {
            Messages = new List<string>() { message };
            ResultType = state;
        }

        public ServiceResult(List<string> messages = null, ResultType state = ResultType.Success)
        {
            Messages = messages;
            ResultType = state;
        }

        public ResultType ResultType { get; set; }

        public List<string> Messages { get; set; }

        public bool IsSuccess()
        {
            return ResultType == ResultType.Success;
        }

        public bool IsError()
        {
            return ResultType == ResultType.Error;
        }

        public bool IsWarning()
        {
            return ResultType == ResultType.Warning;
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }

        public ServiceResult(T result = default(T), string message = "İşlem başarılı", ResultType state = ResultType.Success) : base(message, state)
        {
            Data = result;
            Messages = new List<string>() { message };
            ResultType = state;
        }
    }
}
