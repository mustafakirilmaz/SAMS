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
    /// Demirbaş Gideri işlemleri servisi
    /// </summary>
    public class FixtureExpenseBusinessService : IFixtureExpenseBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Demirbaş Gideri işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public FixtureExpenseBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Demirbaş Gideri ekle
        /// </summary>
        /// <param name="fixtureExpenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddFixtureExpense(FixtureExpenseDto fixtureExpenseDto)
        {
            var fixtureExpenseRepo = UnitOfWork.GetRepository<FixtureExpense>();
            var fixtureExpense = await fixtureExpenseRepo.Get(d => d.FixtureExpenseType == fixtureExpenseDto.FixtureExpenseType && d.BusinessProjectId == fixtureExpenseDto.BusinessProjectId && !d.IsDeleted);

            if (fixtureExpense != null)
            {
                var message = string.Format("Bu işletme projesinde {0} türüyle demirbaş gideri vardır.", fixtureExpenseDto.FixtureExpenseType.Description<FixtureExpenseTypesEnum>());
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            fixtureExpense = ReflectionHelper.CloneObject<FixtureExpenseDto, FixtureExpense>(fixtureExpenseDto);
            await fixtureExpenseRepo.Add(fixtureExpense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(fixtureExpense.Id);

        }

        /// <summary>
        /// Demirbaş Gideri güncelle
        /// </summary>
        /// <param name="fixtureExpenseDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateFixtureExpense(FixtureExpenseDto fixtureExpenseDto)
        {
            var fixtureExpenseRepo = UnitOfWork.GetRepository<FixtureExpense>();
            FixtureExpense fixtureExpense = await fixtureExpenseRepo.Get(d => d.Id == fixtureExpenseDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (fixtureExpense == null)
            {
                return new ServiceResult<long?>(null, "Demirbaş Gideri bulunamadı", ResultType.Warning);
            }

            fixtureExpense = fixtureExpense.CopyObject(fixtureExpenseDto);
            fixtureExpenseRepo.Update(fixtureExpense);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(fixtureExpense.Id);
        }

        /// <summary>
        /// Demirbaş Gideri silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteFixtureExpense(long id)
        {
            var fixtureExpenseRepo = UnitOfWork.GetRepository<FixtureExpense>();
            fixtureExpenseRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre Demirbaş Gideri getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<FixtureExpenseDto>> GetFixtureExpenseById(long id)
        {
            var fixtureExpense = await UnitOfWork.GetRepository<FixtureExpense>().GetById(id);
            var fixtureExpenseDto = ReflectionHelper.CloneObject<FixtureExpense, FixtureExpenseDto>(fixtureExpense);
            return new ServiceResult<FixtureExpenseDto>(fixtureExpenseDto);
        }

        /// <summary>
        /// Demirbaş Gideri arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<FixtureExpenseGridDto>>> SearchFixtureExpenses(QueryFilter<FixtureExpenseFilterDto> queryFilter)
        {
            var fixtureExpenseRepo = UnitOfWork.GetRepository<FixtureExpense>().AsQueryable();

            if (queryFilter.SearchFilter.BusinessProjectId > 0)
            {
                fixtureExpenseRepo = fixtureExpenseRepo.Where(d => d.BusinessProjectId == queryFilter.SearchFilter.BusinessProjectId);
            }

            var query = from fixtureExpense in fixtureExpenseRepo

                        where fixtureExpense.IsDeleted != true
                        select new FixtureExpenseGridDto()
                        {
                            Id = fixtureExpense.Id,
                            FixtureExpenseType = fixtureExpense.FixtureExpenseType.Description<FixtureExpenseTypesEnum>(),
                            Description = fixtureExpense.Description,
                            Cost = fixtureExpense.Cost,
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}