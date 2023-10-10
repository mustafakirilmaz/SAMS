﻿using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SAMS.Infrastructure.Provider
{
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DateTime?) || context.Metadata.ModelType == typeof(DateTime))
            {
                return new DateTimeModelBinder();
            }

            return null;
        }
    }
}
