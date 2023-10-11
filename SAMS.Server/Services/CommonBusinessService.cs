using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAMS.Common.Extensions;
using SAMS.Common.Helpers;
using SAMS.Data;
using SAMS.Data.Dtos;
using SAMS.DataAccess;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.Models;
using SAMS.Server.ServiceContracts;

namespace SAMS.Server.Services
{    /// <summary>
     /// Ortak kullanılan servis
     /// </summary>
    public class CommonBusinessService : ICommonBusinessService
    {
        private readonly IUnitOfWork UnitOfWork;
        /// <summary>
        /// Ortak kullanılan servis ctor
        /// </summary>
        public CommonBusinessService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Rolleri getir
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<List<SelectItem>>> GetRoles()
        {
            var roles = await UnitOfWork.GetRepository<Role>().GetAll();
            var result = roles.Select(p => new SelectItem
            {
                value = p.Id,
                label = p.Description
            }).ToList();

            return new ServiceResult<List<SelectItem>>(result);
        }

        /// <summary>
        /// Örnek enumları Getir
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<List<SelectItem>>> GetSampleEnum()
        {
            var deviceTypes = EnumHelper<SampleEnum>.GetSelectListAsDescription().OrderBy(x => x.value).ToList();
            var result = new ServiceResult<List<SelectItem>>
            {
                ResultType = ResultType.Success,
                Data = deviceTypes
            };

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Ad soyad'a göre kullanıcı arama
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<ServiceResult<List<SelectItem>>> GetUsers(string searchTerm)
        {
            var userRepo = UnitOfWork.GetRepository<User>().AsQueryable().Where(d => !d.IsDeleted);

            var nameSurname = searchTerm.SplitNameSurname().Where(d => d != "").ToArray();
            if (nameSurname.Length == 1)
            {
                userRepo = userRepo.Where(d => d.Name.ToLower().Contains(nameSurname[0].ToLower()) || d.Surname.ToLower().Contains(nameSurname[0].ToLower()));
            }
            else
            {
                userRepo = userRepo.Where(d => d.Name.ToLower().Contains(nameSurname[0].ToLower()) && d.Surname.ToLower().Contains(nameSurname[1].ToLower()));
            }
            var users = await userRepo.Skip(0).Take(50).ToListAsync();
            var result = users.Select(d => new SelectItem
            {
                value = d.Id,
                label = d.Name + ' ' + d.Surname
            }).ToList();

            return new ServiceResult<List<SelectItem>>(result);
        }

        /// <summary>
        /// İlleri getir
        /// </summary>
        /// <returns></returns>
		public async Task<ServiceResult<List<SelectItem>>> GetCities()
		{
			var cities = (await UnitOfWork.GetRepository<City>().GetAll()).OrderBy(d => d.Name).ToList();
			var result = cities.Select(d => new SelectItem
			{
				value = d.Code,
				label = d.Name
			}).ToList();

			return new ServiceResult<List<SelectItem>>(result);
		}

        /// <summary>
        /// İlçeleri getir
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
		public async Task<ServiceResult<List<SelectItem>>> GetTowns(int cityCode)
		{
			var towns = (await UnitOfWork.GetRepository<Town>().GetAll(d => d.CityCode == cityCode)).OrderBy(d => d.Name).ToList();
			var result = towns.Select(d => new SelectItem
			{
				value = d.Code,
				label = d.Name
			}).ToList();

			return new ServiceResult<List<SelectItem>>(result);
		}

        /// <summary>
        /// Mahalleleri getir
        /// </summary>
        /// <param name="townCode"></param>
        /// <returns></returns>
		public async Task<ServiceResult<List<SelectItem>>> GetDistricts(int townCode)
		{
			var towns = (await UnitOfWork.GetRepository<District>().GetAll(d => d.TownCode == townCode)).OrderBy(d => d.Name).ToList();
			var result = towns.Select(d => new SelectItem
			{
				value = d.Code,
				label = d.Name
			}).ToList();

			return new ServiceResult<List<SelectItem>>(result);
		}

		#region privates
		private async Task<ServiceResult<List<SelectItem>>> GetEnumsAsSelectList<T>()
        {
            var items = EnumHelper<T>.GetSelectListAsDescription().OrderBy(x => x.value).ToList();
            var result = new ServiceResult<List<SelectItem>>
            {
                ResultType = ResultType.Success,
                Data = items
            };

            return await Task.FromResult(result);
        }
        #endregion
    }
}
