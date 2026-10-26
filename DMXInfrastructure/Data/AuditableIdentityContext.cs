

using DMXDomain.Models;
using DMXInfrastructure.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DMXInfrastructure.Data
{
    public abstract class AuditableIdentityContext
        : IdentityDbContext<AppUser, AppRole, string>
    {
        protected AuditableIdentityContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Audit> AuditLogs { get; set; } = null!;

        public virtual async Task<int> SaveChangesAsync(string userId = "")
        {
            OnBeforeSaveChanges(userId);

            return await base.SaveChangesAsync();
        }

        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();

            var now = DateTime.UtcNow;
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                // Populate audit properties on auditable entities.
                if (entry.Entity is TableAudit entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.CreatedBy = userId;
                            entity.CreatedDate = now;
                            entity.IsDeleted = false;
                            break;

                        case EntityState.Modified:
                            entity.ModifiedBy = userId;
                            entity.ModifiedDate = now;

                            // Preserve original creation information.
                            entry.Property(nameof(TableAudit.CreatedBy))
                                .IsModified = false;

                            entry.Property(nameof(TableAudit.CreatedDate))
                                .IsModified = false;
                            break;

                        case EntityState.Deleted:
                            entity.ModifiedBy = userId;
                            entity.ModifiedDate = now;
                            break;
                    }
                }

                // Preserve the existing audit-log functionality.
                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] =
                            property.CurrentValue;

                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] =
                                property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] =
                                property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;

                                auditEntry.OldValues[propertyName] =
                                    property.OriginalValue;

                                auditEntry.NewValues[propertyName] =
                                    property.CurrentValue;
                            }
                            break;
                    }
                }

                // Only retain entries representing actual changes.
                if (auditEntry.AuditType != null)
                {
                    auditEntries.Add(auditEntry);
                }
            }

            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}

