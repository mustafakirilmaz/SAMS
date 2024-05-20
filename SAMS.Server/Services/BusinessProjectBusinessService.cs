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
    /// İşletme Projesi işlemleri servisi
    /// </summary>
    public class BusinessProjectBusinessService : IBusinessProjectBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// İşletme Projesi işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public BusinessProjectBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// İşletme Projesi ekle
        /// </summary>
        /// <param name="businessProjectDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddBusinessProject(BusinessProjectDto businessProjectDto)
        {
            var businessProjectRepo = UnitOfWork.GetRepository<BusinessProject>();
            var businessProject = await businessProjectRepo.Get(d => d.Name == businessProjectDto.Name && !d.IsDeleted);

            if (businessProject != null)
            {
                var message = string.Format("{0} isimli işletme projesi sistemde kayıtlıdır. Farklı isim deneyiniz.", businessProjectDto.Name);
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            businessProject = ReflectionHelper.CloneObject<BusinessProjectDto, BusinessProject>(businessProjectDto);
            await businessProjectRepo.Add(businessProject);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(businessProject.Id);

        }

        /// <summary>
        /// İşletme Projesi güncelle
        /// </summary>
        /// <param name="businessProjectDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateBusinessProject(BusinessProjectDto businessProjectDto)
        {
            var businessProjectRepo = UnitOfWork.GetRepository<BusinessProject>();
            BusinessProject businessProject = await businessProjectRepo.Get(d => d.Id == businessProjectDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (businessProject == null)
            {
                return new ServiceResult<long?>(null, "İşletme Projesi bulunamadı", ResultType.Warning);
            }

            businessProject = businessProject.CopyObject(businessProjectDto);
            businessProjectRepo.Update(businessProject);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(businessProject.Id);
        }

        /// <summary>
        /// İşletme Projesi silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteBusinessProject(long id)
        {
            var businessProjectRepo = UnitOfWork.GetRepository<BusinessProject>();
            businessProjectRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre İşletme Projesi getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<BusinessProjectDto>> GetBusinessProjectById(long id)
        {
            var businessProject = await UnitOfWork.GetRepository<BusinessProject>().GetById(id);
            var businessProjectDto = ReflectionHelper.CloneObject<BusinessProject, BusinessProjectDto>(businessProject);
            return new ServiceResult<BusinessProjectDto>(businessProjectDto);
        }

        /// <summary>
        /// İşletme Projesi arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<BusinessProjectGridDto>>> SearchBusinessProjects(QueryFilter<BusinessProjectFilterDto> queryFilter)
        {
            var businessProjectRepo = UnitOfWork.GetRepository<BusinessProject>().AsQueryable();
            var buildinRepo = UnitOfWork.GetRepository<Building>().AsQueryable();

            if (queryFilter.SearchFilter.BuildingId > 0)
            {
                businessProjectRepo = businessProjectRepo.Where(d => d.BuildingId == queryFilter.SearchFilter.BuildingId);
            }

            var query = from businessProject in businessProjectRepo

                        join building in buildinRepo on businessProject.BuildingId equals building.Id into buildingJoin
                        from building in buildingJoin.DefaultIfEmpty()

                        where businessProject.IsDeleted != true
                        select new BusinessProjectGridDto()
                        {
                            Id = businessProject.Id,
                            BuildingName = building.Name,
                            Name = businessProject.Name,
                            StartDate = businessProject.StartDate,
                            EndDate= businessProject.EndDate,
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}