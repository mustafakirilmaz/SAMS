using SAMS.Common.Extensions;
using SAMS.Common.Helpers;
using SAMS.Data;
using SAMS.Data.Dtos;
using SAMS.DataAccess;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.Models;
using SAMS.Infrastructure.WebToken;
using SAMS.Server.ServiceContracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAMS.Server.Services
{
    /// <summary>
    /// Gider işlemleri servisi
    /// </summary>
    public class ExpenseBusinessService : IExpenseBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Gider işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public ExpenseBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Gider ekle
        /// </summary>
        /// <param name="expenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddExpense(ExpenseDto expenseDto)
        {
            var expenseRepo = UnitOfWork.GetRepository<Expense>();
            var expense = await expenseRepo.Get(d => d.Name == expenseDto.Name && !d.IsDeleted);

            if (expense != null)
            {
                var message = string.Format("{0} isimli gider sistemde kayıtlıdır. Farklı isim deneyiniz.", expenseDto.Name); 
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            expense = ReflectionHelper.CloneObject<ExpenseDto, Expense>(expenseDto);
            await expenseRepo.Add(expense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(expense.Id);

        }

        /// <summary>
        /// Gider güncelle
        /// </summary>
        /// <param name="expenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateExpense(ExpenseDto expenseDto)
        {
            var expenseRepo = UnitOfWork.GetRepository<Expense>();
            Expense expense = await expenseRepo.Get(d => d.Id == expenseDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (expense == null)
            {
                return new ServiceResult<long?>(null, "Gider bulunamadı", ResultType.Warning);
            }

            expense = expense.CopyObject(expenseDto);
            expenseRepo.Update(expense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(expense.Id);
        }

        /// <summary>
        /// Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteExpense(long id)
        {
            var expenseRepo = UnitOfWork.GetRepository<Expense>();
            expenseRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre Gider getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ExpenseDto>> GetExpenseById(long id)
        {
            var expense = await UnitOfWork.GetRepository<Expense>().GetById(id);
            var expenseDto = ReflectionHelper.CloneObject<Expense, ExpenseDto>(expense);
            return new ServiceResult<ExpenseDto>(expenseDto);
        }

        /// <summary>
        /// Gider arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<ExpenseGridDto>>> SearchExpenses(QueryFilter<ExpenseFilterDto> queryFilter)
        {
            var expenseRepo = UnitOfWork.GetRepository<Expense>().AsQueryable();
            var expenseTypeRepo = UnitOfWork.GetRepository<ExpenseType>().AsQueryable();

            if (queryFilter.SearchFilter.ExpenseTypeId > 0)
            {
                expenseRepo = expenseRepo.Where(d => d.ExpenseTypeId == queryFilter.SearchFilter.ExpenseTypeId);
            }
            if (!string.IsNullOrEmpty(queryFilter.SearchFilter.Name))
            {
                expenseRepo = expenseRepo.Where(d => d.Name.ToLower().Contains(queryFilter.SearchFilter.Name.ToLower()));
            }

            var query = from expense in expenseRepo

                        join ExpenseType in expenseTypeRepo on expense.ExpenseTypeId equals ExpenseType.Id into ExpenseTypeJoin
                        from ExpenseType in ExpenseTypeJoin.DefaultIfEmpty()
                        where expense.IsDeleted != true
                        select new ExpenseGridDto()
                        {
                            Id = expense.Id,
                            ExpenseTypeName = expense.ExpenseTypeId == 0 ? "-": ExpenseType.Name,
                            Name = expense.Name,
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}