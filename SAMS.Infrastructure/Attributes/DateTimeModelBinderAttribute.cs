using Microsoft.AspNetCore.Mvc;
using SAMS.Infrastructure.Provider;

namespace SAMS.Infrastructure.Attributes
{
    public class DateTimeModelBinderAttribute : ModelBinderAttribute
    {
        public string DateFormat { get; set; }

        public DateTimeModelBinderAttribute() : base(typeof(DateTimeModelBinder)) { }
    }
}
