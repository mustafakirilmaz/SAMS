using SAMS.Infrastructure.Entities;
using SAMS.Infrastructure.Enums;
using SAMS.Infrastructure.WebToken;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAMS.Infrastructure.Models;

namespace SAMS.DataAccess
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext, IDisposable
    {
        private Dictionary<string, object> _repositories;
        private IJwtHelper jwtHelper;

        public UnitOfWork(TContext context, IJwtHelper jwtHelper)
        {
            this.jwtHelper = jwtHelper;
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = GetRepositoryType<TEntity>("Repository");
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(Context);
            return (IRepository<TEntity>)_repositories[type];
        }


        public TContext Context { get; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (jwtHelper != null)
                {
                    jwtHelper.GetCurrentUser();
                    var audits = AddAuditLog(Context.ChangeTracker);
                    Context.Set<Audit>().AddRange(audits);
                }
            }
            catch (Exception) { }


            return await Context.SaveChangesAsync(true, cancellationToken);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        private string GetRepositoryType<TEntity>(string repoType)
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            return string.Format("{0}-{1}", repoType, typeof(TEntity).ToString());
        }

        private List<Audit> AddAuditLog(ChangeTracker changeTracker)
        {
            long currentUserId = 0;
            long.TryParse(jwtHelper.GetValueFromToken("userId"), out currentUserId);

            List<Audit> auditLogList = new List<Audit>();
            var modifiedEntities = changeTracker.Entries().Where(d => d.Entity is BaseEntity && (d.State == EntityState.Added || d.State == EntityState.Modified || d.State == EntityState.Deleted));
            var roleId = jwtHelper.GetCurrentUser()?.SelectedRole?.RoleId;

            foreach (var change in modifiedEntities)
            {
                var auditableEntity = ((AuditableEntity)change.Entity);
                Audit auditLog = new Audit();
                auditLog.TableName = change.Entity.GetType().Name;
                auditLog.UserId = currentUserId;
                if (jwtHelper.GetCurrentUser() != null)
                {
                    auditLog.RoleId = roleId;
                }
                auditLog.CreatedDate = DateTime.Now;

                var oldValues = new StringBuilder();
                var newValues = new StringBuilder();

                if (change.State == EntityState.Modified || change.State == EntityState.Deleted)
                {
                    #region Entity Audits
                    auditableEntity.CreatedDate = change.OriginalValues["CreatedDate"] != null ? (DateTime)change.OriginalValues["CreatedDate"] : (DateTime?)null;
                    auditableEntity.CreatedBy = change.OriginalValues["CreatedBy"] != null ? (long)change.OriginalValues["CreatedBy"] : (long?)null;
                    auditableEntity.RowGuid = change.OriginalValues["RowGuid"]?.ToString();

                    auditableEntity.UpdatedDate = DateTime.Now;
                    auditableEntity.UpdatedBy = currentUserId;
                    #endregion

                    #region Audit Logs
                    auditLog.RowGuid = change.OriginalValues["RowGuid"]?.ToString();
                    auditLog.AuditType = AuditType.Update;

                    foreach (var propertyName in change.OriginalValues.Properties.Select(p => p.Name))
                    {
                        //var oldVal = change.GetDatabaseValues().GetValue<object>(propertyName);
                        var oldVal = change.OriginalValues[propertyName];
                        var newVal = change.CurrentValues[propertyName];

                        if (oldVal != null && newVal != null && !Equals(oldVal, newVal))
                        {
                            if (oldValues.Length > 2500 || newValues.Length > 2500)
                            {
                                auditLog.OldValues = oldValues.ToString();
                                auditLog.NewValues = newValues.ToString();
                                auditLogList.Add(auditLog);

                                newValues = new StringBuilder();
                                auditLog.NewValues = string.Empty;

                                oldValues = new StringBuilder();
                                auditLog.OldValues = string.Empty;
                            }
                            if (oldValues.Length > 0)
                            {
                                oldValues.Append(string.Format("{0}", "||"));
                            }

                            if (newValues.Length > 0)
                            {
                                newValues.Append(string.Format("{0}", "||"));
                            }

                            newValues.Append(string.Format("{0}={1}", propertyName, newVal?.ToString()));
                            oldValues.Append(string.Format("{0}={1}", propertyName, oldVal?.ToString()));
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(oldValues.ToString()) || !String.IsNullOrWhiteSpace(newValues.ToString()))
                    {
                        auditLog.OldValues = oldValues.ToString();
                        auditLog.NewValues = newValues.ToString();
                    }
                    #endregion
                }
                else if (change.State == EntityState.Added)
                {
                    #region Entity Audits
                    auditableEntity.CreatedDate = DateTime.Now;
                    auditableEntity.CreatedBy = currentUserId;
                    auditableEntity.RowGuid = string.IsNullOrEmpty(auditableEntity.RowGuid) ? Guid.NewGuid().ToString() : auditableEntity.RowGuid;
                    #endregion

                    #region Audit Logs
                    auditLog.RowGuid = auditableEntity.RowGuid;
                    auditLog.AuditType = AuditType.Insert;
                    auditLog.OldValues = "";

                    foreach (var propertyName in change.OriginalValues.Properties.Select(p => p.Name))
                    {
                        var newVal = propertyName != "Id" ? change.CurrentValues[propertyName] : 0;

                        if (newValues.Length > 2500)
                        {
                            auditLog.NewValues = newValues.ToString();
                            auditLogList.Add(auditLog);

                            newValues = new StringBuilder();
                            auditLog.NewValues = string.Empty;
                        }

                        if (newValues.Length > 0)
                        {
                            newValues.Append(string.Format("{0}", "||"));
                        }

                        newValues.Append(string.Format("{0}={1}", propertyName, newVal?.ToString()));
                    }

                    if (!String.IsNullOrWhiteSpace(newValues.ToString()))
                    {
                        auditLog.NewValues = newValues.ToString();
                    }
                    #endregion 
                }

                auditLogList.Add(auditLog);
            }
            return auditLogList;
        }
    }
}
