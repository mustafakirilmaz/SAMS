using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using SAMS.Infrastructure.Attributes;

namespace SAMS.Infrastructure.Enums
{
    [Serializable]
    [OpenApiParameterIgnore]
    public enum ResultType
    {
        [Description("Başarılı")]
        Success = 1,

        [Description("Hata")]
        Error = 2,

        [Description("Dikkat")]
        Warning = 3
    }
}
