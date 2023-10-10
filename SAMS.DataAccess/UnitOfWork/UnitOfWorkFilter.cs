using SAMS.Data;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace SAMS.DataAccess
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private readonly IDbContextTransaction transaction;
        private readonly SAMSDbContext context;

        public UnitOfWorkFilter(IDbContextTransaction transaction, SAMSDbContext context)
        {
            this.transaction = transaction;
            this.context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next.Invoke();
            bool hasError = executedContext.Exception != null;

            var result = executedContext.Result;
            if (result is ObjectResult objectResult)
            {
                var objectResultValue = objectResult.Value;
                if (objectResultValue is ServiceResult serviceResult)
                {
                    if (!serviceResult.IsSuccess())
                    {
                        hasError = true;
                    }
                }
            }

            if (hasError)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }
        }

        public bool HasUnsavedChanges()
        {
            var entries = context.ChangeTracker.Entries();
            return entries.Any(e => e.State == EntityState.Added|| e.State == EntityState.Modified|| e.State == EntityState.Deleted);
        }
    }
}

