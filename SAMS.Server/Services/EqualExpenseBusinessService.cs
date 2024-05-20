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
    /// Eşit Gider işlemleri servisi
    /// </summary>
    public class EqualExpenseBusinessService : IEqualExpenseBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Eşit Gider işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public EqualExpenseBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Eşit Gider ekle
        /// </summary>
        /// <param name="equalExpenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddEqualExpense(EqualExpenseDto equalExpenseDto)
        {
            var equalExpenseRepo = UnitOfWork.GetRepository<EqualExpense>();
            var equalExpense = await equalExpenseRepo.Get(d => d.EqualExpenseType == equalExpenseDto.EqualExpenseType && d.BusinessProjectId==equalExpenseDto.BusinessProjectId && !d.IsDeleted);

            if (equalExpense != null)
            {
                var message = string.Format("Bu işletme projesinde {0} türüyle eşit gider vardır.", equalExpenseDto.EqualExpenseType.Description<EqualExpenseTypesEnum>());
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            equalExpense = ReflectionHelper.CloneObject<EqualExpenseDto, EqualExpense>(equalExpenseDto);
            await equalExpenseRepo.Add(equalExpense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(equalExpense.Id);

        }

        /// <summary>
        /// Eşit Gider güncelle
        /// </summary>
        /// <param name="equalExpenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateEqualExpense(EqualExpenseDto equalExpenseDto)
        {
            var equalExpenseRepo = UnitOfWork.GetRepository<EqualExpense>();
            EqualExpense equalExpense = await equalExpenseRepo.Get(d => d.Id == equalExpenseDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (equalExpense == null)
            {
                return new ServiceResult<long?>(null, "Eşit Gider bulunamadı", ResultType.Warning);
            }

            equalExpense = equalExpense.CopyObject(equalExpenseDto);
            equalExpenseRepo.Update(equalExpense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(equalExpense.Id);
        }

        /// <summary>
        /// Eşit Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteEqualExpense(long id)
        {
            var equalExpenseRepo = UnitOfWork.GetRepository<EqualExpense>();
            equalExpenseRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre Eşit Gider getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<EqualExpenseDto>> GetEqualExpenseById(long id)
        {
            var equalExpense = await UnitOfWork.GetRepository<EqualExpense>().GetById(id);
            var equalExpenseDto = ReflectionHelper.CloneObject<EqualExpense, EqualExpenseDto>(equalExpense);
            return new ServiceResult<EqualExpenseDto>(equalExpenseDto);
        }

        /// <summary>
        /// Eşit Gider arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<EqualExpenseGridDto>>> SearchEqualExpenses(QueryFilter<EqualExpenseFilterDto> queryFilter)
        {
            var equalExpenseRepo = UnitOfWork.GetRepository<EqualExpense>().AsQueryable();

            if (queryFilter.SearchFilter.BusinessProjectId > 0)
            {
                equalExpenseRepo = equalExpenseRepo.Where(d => d.BusinessProjectId == queryFilter.SearchFilter.BusinessProjectId);
            }

            var query = from equalExpense in equalExpenseRepo

                        where equalExpense.IsDeleted != true
                        select new EqualExpenseGridDto()
                        {
                            Id = equalExpense.Id,
                            EqualExpenseType = equalExpense.EqualExpenseType.Description<EqualExpenseTypesEnum>(),
                            Cost = equalExpense.Cost,
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}