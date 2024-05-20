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
    public class BuildingBusinessService : IBuildingBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Bina işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public BuildingBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Bina ekle
        /// </summary>
        /// <param name="buildingDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddBuilding(BuildingDto buildingDto)
        {
            var buildingRepo = UnitOfWork.GetRepository<Building>();
            var building = await buildingRepo.Get(d => d.Name == buildingDto.Name && !d.IsDeleted);

            if (building != null)
            {
                var message = string.Format("{0} isimli bina sistemde kayıtlıdır. Farklı isim deneyiniz.", buildingDto.Name); 
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            building = ReflectionHelper.CloneObject<BuildingDto, Building>(buildingDto);
            await buildingRepo.Add(building);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(building.Id);

        }

        /// <summary>
        /// Bina güncelle
        /// </summary>
        /// <param name="buildingDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateBuilding(BuildingDto buildingDto)
        {
            var buildingRepo = UnitOfWork.GetRepository<Building>();
            Building building = await buildingRepo.Get(d => d.Id == buildingDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (building == null)
            {
                return new ServiceResult<long?>(null, "Bina bulunamadı", ResultType.Warning);
            }

            building = building.CopyObject(buildingDto);
            buildingRepo.Update(building);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(building.Id);
        }

        /// <summary>
        /// Bina silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteBuilding(long id)
        {
            var buildingRepo = UnitOfWork.GetRepository<Building>();
            buildingRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre Bina getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<BuildingDto>> GetBuildingById(long id)
        {
            var building = await UnitOfWork.GetRepository<Building>().GetById(id);
            var buildingDto = ReflectionHelper.CloneObject<Building, BuildingDto>(building);
            return new ServiceResult<BuildingDto>(buildingDto);
        }

        /// <summary>
        /// Bina arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<BuildingGridDto>>> SearchBuildings(QueryFilter<BuildingFilterDto> queryFilter)
        {
            var buildingRepo = UnitOfWork.GetRepository<Building>().AsQueryable();
            var cityRepo = UnitOfWork.GetRepository<City>().AsQueryable();
            var townRepo = UnitOfWork.GetRepository<Town>().AsQueryable();
            var districtRepo = UnitOfWork.GetRepository<District>().AsQueryable();
            var siteRepo = UnitOfWork.GetRepository<Site>().AsQueryable();

            if (!string.IsNullOrEmpty(queryFilter.SearchFilter.Name))
            {
                buildingRepo = buildingRepo.Where(d => d.Name.ToLower().Contains(queryFilter.SearchFilter.Name.ToLower()));
            }
            if (queryFilter.SearchFilter.CityCode>0)
            {
                buildingRepo = buildingRepo.Where(d => d.CityCode == queryFilter.SearchFilter.CityCode);
            }
            if (queryFilter.SearchFilter.TownCode > 0)
            {
                buildingRepo = buildingRepo.Where(d => d.TownCode == queryFilter.SearchFilter.TownCode);
            }
            if (queryFilter.SearchFilter.DistrictCode > 0)
            {
                buildingRepo = buildingRepo.Where(d => d.DistrictCode == queryFilter.SearchFilter.DistrictCode);
            }

            var query = from building in buildingRepo

                        join site in siteRepo on building.SiteId equals site.Id into siteJoin
                        from site in siteJoin.DefaultIfEmpty()
                        
                        join city in cityRepo on building.CityCode equals city.Code into cityJoin
                        from city in cityJoin.DefaultIfEmpty()

                        join town in townRepo on building.TownCode equals town.Code into townJoin
                        from town in townJoin.DefaultIfEmpty()

                        join district in districtRepo on building.DistrictCode equals district.Code into districtJoin
                        from district in districtJoin.DefaultIfEmpty()

                        where building.IsDeleted != true
                        select new BuildingGridDto()
                        {
                            Id = building.Id,
                            SiteName = building.SiteId == 0 ? "-": site.Name, // siteID null ise "-", değilse site.Name yazar
                            Name = building.Name,
                            City = city.Name,
                            Town = town.Name,
                            District = district.Name,
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}