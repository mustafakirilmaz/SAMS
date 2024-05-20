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
    /// Site işlemleri servisi
    /// </summary>
    public class SiteBusinessService : ISiteBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IJwtHelper jwtHelper;

        /// <summary>
        /// Site işlemleri servisi ctor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="jwtHelper"></param>
        public SiteBusinessService(IUnitOfWork unitOfWork, IJwtHelper jwtHelper)
        {
            this.UnitOfWork = unitOfWork;
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Site ekle
        /// </summary>
        /// <param name="siteDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> AddSite(SiteDto siteDto)
        {
            var siteRepo = UnitOfWork.GetRepository<Site>();
            var site = await siteRepo.Get(d => d.Name == siteDto.Name && !d.IsDeleted);

            if (site != null)
            {
                var message = string.Format("{0} sitesi sistemde kayıtlıdır. Farklı isim deneyiniz.", siteDto.Name); 
                return new ServiceResult<long?>(null, message, ResultType.Error);
            }

            site = ReflectionHelper.CloneObject<SiteDto, Site>(siteDto);
            await siteRepo.Add(site);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(site.Id);

        }

        /// <summary>
        /// Site güncelle
        /// </summary>
        /// <param name="siteDto"></param>
        /// <returns></returns>
        public async Task<ServiceResult<long?>> UpdateSite(SiteDto siteDto)
        {
            var siteRepo = UnitOfWork.GetRepository<Site>();
            Site site = await siteRepo.Get(d => d.Id == siteDto.Id && !d.IsDeleted, trackingEnabled: true);

            if (site == null)
            {
                return new ServiceResult<long?>(null, "Site bulunamadı", ResultType.Warning);
            }

            site = site.CopyObject(siteDto);
            siteRepo.Update(site);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<long?>(site.Id);
        }

        /// <summary>
        /// Site silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> DeleteSite(long id)
        {
            var siteRepo = UnitOfWork.GetRepository<Site>();
            siteRepo.DeleteById(id);
            await UnitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true);
        }

        /// <summary>
        /// Id'ye göre site getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<SiteDto>> GetSiteById(long id)
        {
            var site = await UnitOfWork.GetRepository<Site>().GetById(id);
            var siteDto = ReflectionHelper.CloneObject<Site, SiteDto>(site);
            return new ServiceResult<SiteDto>(siteDto);
        }

        /// <summary>
        /// Site arama
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<SiteGridDto>>> SearchSites(QueryFilter<SiteFilterDto> queryFilter)
        {
            var siteRepo = UnitOfWork.GetRepository<Site>().AsQueryable();
            if (!string.IsNullOrEmpty(queryFilter.SearchFilter.Name))
            {
                siteRepo = siteRepo.Where(d => d.Name.ToLower().Contains(queryFilter.SearchFilter.Name.ToLower()));
            }
            var query = from site in siteRepo
                        where site.IsDeleted != true
                        select new SiteGridDto()
                        {
                            Id = site.Id,
                            Name = site.Name
                        };

            var result = await query.GetGridResult(queryFilter);
            return result.ToGridServiceResult();
        }
    }
}