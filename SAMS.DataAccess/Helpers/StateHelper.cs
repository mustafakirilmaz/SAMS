using System;
using SAMS.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace SAMS.DataAccess
{
    public class StateHelper
    {
        public static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Added:
                    return EntityState.Added;

                case ObjectState.Modified:
                    return EntityState.Modified;

                case ObjectState.Deleted:
                    return EntityState.Deleted;

                default:
                    return EntityState.Unchanged;
            }
        }

        public static ObjectState ConvertState(EntityState state)
        {
            switch (state)
            {
                case EntityState.Added:
                    return ObjectState.Added;

                case EntityState.Deleted:
                    return ObjectState.Deleted;

                case EntityState.Detached:
                    return ObjectState.Unchanged;

                case EntityState.Modified:
                    return ObjectState.Modified;

                case EntityState.Unchanged:
                    return ObjectState.Unchanged;

                default:
                    throw new ArgumentOutOfRangeException("Improper Entity State");
            }
        }
    }
}
