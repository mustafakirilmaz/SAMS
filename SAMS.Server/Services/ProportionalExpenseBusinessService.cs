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
    /// Oransal Gider işlemleri servisi
    /// </summary>
    public class ProportionalExpenseBusinessService : IProportionalExpenseBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Oransal Gider işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public ProportionalExpenseBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Oransal Gider ekle
        /// </summary>
        /// <param name="proportionalExpenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddProportionalExpense(ProportionalExpenseDto proportionalExpenseDto)
        {
            var proportionalExpenseRepo = UnitOfWork.GetRepository<ProportionalExpense>();
            var proportionalExpense = await proportionalExpenseRepo.Get(d => d.ProportionalExpenseType == proportionalExpenseDto.ProportionalExpenseType && d.BusinessProjectId == proportionalExpenseDto.BusinessProjectId && !d.IsDeleted);

            if (proportionalExpense != null)
            {
                var message = string.Format("Bu işletme projesinde {0} türüyle eşit gider vardır.", proportionalExpenseDto.ProportionalExpenseType.Description<ProportionalExpenseTypesEnum>());
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            proportionalExpense = ReflectionHelper.CloneObject<ProportionalExpenseDto, ProportionalExpense>(proportionalExpenseDto);
            await proportionalExpenseRepo.Add(proportionalExpense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(proportionalExpense.Id);

        }

        /// <summary>
        /// Oransal Gider güncelle
        /// </summary>
        /// <param name="proportionalExpenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateProportionalExpense(ProportionalExpenseDto proportionalExpenseDto)
        {
            var proportionalExpenseRepo = UnitOfWork.GetRepository<ProportionalExpense>();
            ProportionalExpense proportionalExpense = await proportionalExpenseRepo.Get(d => d.Id == proportionalExpenseDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (proportionalExpense == null)
            {
                return new ServiceResult<long?>(null, "Oransal Gider bulunamadı", ResultType.Warning);
            }

            proportionalExpense = proportionalExpense.CopyObject(proportionalExpenseDto);
            proportionalExpenseRepo.Update(proportionalExpense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(proportionalExpense.Id);
        }

        /// <summary>
        /// Oransal Gider silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteProportionalExpense(long id)
        {
            var proportionalExpenseRepo = UnitOfWork.GetRepository<ProportionalExpense>();
            proportionalExpenseRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre Oransal Gider getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ProportionalExpenseDto>> GetProportionalExpenseById(long id)
        {
            var proportionalExpense = await UnitOfWork.GetRepository<ProportionalExpense>().GetById(id);
            var proportionalExpenseDto = ReflectionHelper.CloneObject<ProportionalExpense, ProportionalExpenseDto>(proportionalExpense);
            return new ServiceResult<ProportionalExpenseDto>(proportionalExpenseDto);
        }

        /// <summary>
        /// Oransal Gider arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<ProportionalExpenseGridDto>>> SearchProportionalExpenses(QueryFilter<ProportionalExpenseFilterDto> queryFilter)
        {
            var proportionalExpenseRepo = UnitOfWork.GetRepository<ProportionalExpense>().AsQueryable();

            if (queryFilter.SearchFilter.BusinessProjectId > 0)
            {
                proportionalExpenseRepo = proportionalExpenseRepo.Where(d => d.BusinessProjectId == queryFilter.SearchFilter.BusinessProjectId);
            }

            var query = from proportionalExpense in proportionalExpenseRepo

                        where proportionalExpense.IsDeleted != true
                        select new ProportionalExpenseGridDto()
                        {
                            Id = proportionalExpense.Id,
                            ProportionalExpenseType = proportionalExpense.ProportionalExpenseType.Description<ProportionalExpenseTypesEnum>(),
                            Cost = proportionalExpense.Cost,
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}