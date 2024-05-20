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
    /// Bina işlemleri servisi
    /// </summary>
    public class UnitBusinessService : IUnitBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Bina işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public UnitBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Bina ekle
        /// </summary>
        /// <param name="unitDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddUnit(UnitDto unitDto)
        {
            var unitRepo = UnitOfWork.GetRepository<Unit>();
            var unit = await unitRepo.Get(d => d.Name == unitDto.Name && !d.IsDeleted);

            if (unit != null)
            {
                var message = string.Format("{0} bağımsız bölüm sistemde kayıtlıdır. Farklı isim deneyiniz.", unitDto.Name); 
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            unit = ReflectionHelper.CloneObject<UnitDto, Unit>(unitDto);
            await unitRepo.Add(unit);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(unit.Id);

        }

        /// <summary>
        /// Bina güncelle
        /// </summary>
        /// <param name="unitDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateUnit(UnitDto unitDto)
        {
            var unitRepo = UnitOfWork.GetRepository<Unit>();
            Unit unit = await unitRepo.Get(d => d.Id == unitDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (unit == null)
            {
                return new ServiceResult<long?>(null, "Bağımsız bölüm bulunamadı", ResultType.Warning);
            }

            unit = unit.CopyObject(unitDto);
            unitRepo.Update(unit);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(unit.Id);
        }

        /// <summary>
        /// Bina silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteUnit(long id)
        {
            var unitRepo = UnitOfWork.GetRepository<Unit>();
            unitRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre Bina getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UnitDto>> GetUnitById(long id)
        {
            var unit = await UnitOfWork.GetRepository<Unit>().GetById(id);
            var unitDto = ReflectionHelper.CloneObject<Unit, UnitDto>(unit);
            return new ServiceResult<UnitDto>(unitDto);
        }

        /// <summary>
        /// Bina arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<UnitGridDto>>> SearchUnits(QueryFilter<UnitFilterDto> queryFilter)
        {
            var unitRepo = UnitOfWork.GetRepository<Unit>().AsQueryable();
            var buildingRepo = UnitOfWork.GetRepository<Building>().AsQueryable();
            var siteRepo = UnitOfWork.GetRepository<Site>().AsQueryable();

            if (!string.IsNullOrEmpty(queryFilter.SearchFilter.Name))
            {
                unitRepo = unitRepo.Where(d => d.Name.ToLower().Contains(queryFilter.SearchFilter.Name.ToLower()));
            }
            if (queryFilter.SearchFilter.SiteId > 0)
            {
                var buildingIds = buildingRepo.Where(b => b.SiteId == queryFilter.SearchFilter.SiteId)
                    .Select(b=>b.Id);
                unitRepo = unitRepo.Where(u => buildingIds.Contains(u.BuildingId));
            }
            if (queryFilter.SearchFilter.BuildingId > 0)
            {
                unitRepo = unitRepo.Where(d => d.BuildingId == queryFilter.SearchFilter.BuildingId);
            }

            var query = from unit in unitRepo
                        join building in buildingRepo on unit.BuildingId equals building.Id into buildingJoin
                        from building in buildingJoin.DefaultIfEmpty()
                        join site in siteRepo on building.SiteId equals site.Id into siteJoin
                        from site in siteJoin.DefaultIfEmpty()
                        where unit.IsDeleted != true
                        select new UnitGridDto()
                        {
                            Id = unit.Id,
                            SiteName = building == null ? "Apartman" : site.Name,
                            BuildingName = building == null ? "" : building.Name,
                            UnitName = unit.Name,
                            GrossSquareMeter = unit.GrossSquareMeter.ToString(),
                            NetSquareMeter = unit.NetSquareMeter.ToString(),
                            ShareOfLand = unit.ShareOfLand.ToString(),
                        };

            var result = await query.Distinct().GetGridResult(queryFilter); // Distinct kullanarak tekrarlı sonuçları ele
            return result.ToGridServiceResult();
        }
    }
}