﻿using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SAMS.Infrastructure.Attributes
{
    public class TransactionFilterAttribute : IAsyncActionFilter
    {
        private readonly DbTransaction transaction;

        public TransactionFilterAttribute(DbTransaction transaction)
        {
            this.transaction = transaction;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var connection = transaction.Connection;
            if (connection.State != ConnectionState.Open)
            {
                throw new NotSupportedException("The provided connection was not open!");
            }

            var executedContext = await next.Invoke();
            if (executedContext.Exception == null)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }
        }
    }
}
