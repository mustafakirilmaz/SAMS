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
using System.Numerics;

namespace SAMS.Server.Services
{
    /// <summary>
    /// Gider Türü işlemleri servisi
    /// </summary>
    public class ExpenseTypeBusinessService : IExpenseTypeBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Gider Türü işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public ExpenseTypeBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Gider Türü ekle
        /// </summary>
        /// <param name="expenseTypeDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddExpenseType(ExpenseTypeDto expenseTypeDto)
        {
            var expenseTypeRepo = UnitOfWork.GetRepository<ExpenseType>();
            var expenseType = await expenseTypeRepo.Get(d => d.Name == expenseTypeDto.Name && !d.IsDeleted);

            if (expenseType != null)
            {
                var message = string.Format("{0} gider türü sistemde kayıtlıdır. Farklı isim deneyiniz.", expenseTypeDto.Name); 
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            expenseType = ReflectionHelper.CloneObject<ExpenseTypeDto, ExpenseType>(expenseTypeDto);
            await expenseTypeRepo.Add(expenseType);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(expenseType.Id);

        }

        /// <summary>
        /// Gider Türü güncelle
        /// </summary>
        /// <param name="expenseTypeDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateExpenseType(ExpenseTypeDto expenseTypeDto)
        {
            var expenseTypeRepo = UnitOfWork.GetRepository<ExpenseType>();
            ExpenseType expenseType = await expenseTypeRepo.Get(d => d.Id == expenseTypeDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (expenseType == null)
            {
                return new ServiceResult<long?>(null, "Gider Türü bulunamadı", ResultType.Warning);
            }

            expenseType = expenseType.CopyObject(expenseTypeDto);
            expenseTypeRepo.Update(expenseType);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(expenseType.Id);
        }

        /// <summary>
        /// Gider Türü silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteExpenseType(long id)
        {
            var expenseTypeRepo = UnitOfWork.GetRepository<ExpenseType>();
            expenseTypeRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre Gider Türü getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ExpenseTypeDto>> GetExpenseTypeById(long id)
        {
            var expenseType = await UnitOfWork.GetRepository<ExpenseType>().GetById(id);
            var expenseTypeDto = ReflectionHelper.CloneObject<ExpenseType, ExpenseTypeDto>(expenseType);
            return new ServiceResult<ExpenseTypeDto>(expenseTypeDto);
        }

        /// <summary>
        /// Gider Türü arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<ExpenseTypeGridDto>>> SearchExpenseTypes(QueryFilter<ExpenseTypeFilterDto> queryFilter)
        {
            var expenseTypeRepo = UnitOfWork.GetRepository<ExpenseType>().AsQueryable();
            if (!string.IsNullOrEmpty(queryFilter.SearchFilter.Name))
            {
                expenseTypeRepo = expenseTypeRepo.Where(d => d.Name.ToLower().Contains(queryFilter.SearchFilter.Name.ToLower()));
            }
            var query = from expenseType in expenseTypeRepo
                        where expenseType.IsDeleted != true
                        select new ExpenseTypeGridDto()
                        {
                            Id = expenseType.Id,
                            Name = expenseType.Name
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}