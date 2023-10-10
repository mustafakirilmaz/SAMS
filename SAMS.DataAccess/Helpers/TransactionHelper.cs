using System.Data;
using SAMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace SAMS.DataAccess
{
    public static class TransactionHelper
    {
        /// <summary>
        /// Her request başladığında tüm işlemlerin tek bir transactionda yapılmasını sağlar
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="level"></param>
        public static void UseOneTransactionPerHttpCall(this IServiceCollection serviceCollection, IsolationLevel level = IsolationLevel.ReadUncommitted)
        {
            serviceCollection.AddScoped<IDbContextTransaction>(serviceProvider =>
            {
                var context = serviceProvider.GetService<SAMSDbContext>();
                return context.Database.BeginTransaction(level);
            });

            serviceCollection.AddScoped(typeof(UnitOfWorkFilter), typeof(UnitOfWorkFilter));
            serviceCollection.AddMvc(setup => { setup.Filters.AddService<UnitOfWorkFilter>(1); });
        }
    }
}
