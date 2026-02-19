using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Template.Application.Contracts.Api;
using Template.Domain.Common.Auditing;
using Template.Domain.Entities;
using Template.Infrastructure.Data.Identity.Entities;

namespace Template.Infrastructure.Data
{
    internal class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly ICurrentUserService _currentUserService;


        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            #region query filters
            // Apply global query filter for soft-deleted entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GenerateIsDeletedFilter(entityType.ClrType));
                }
            }
            #endregion
        }


        // Override SaveChangesAsync to automatically set audit properties  
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {

                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is IHasCreationTime cTime) cTime.CreatedAt = utcNow;
                    if (entry.Entity is IHasCreator cUser) cUser.CreatedBy = userId;
                }

                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is IHasModificationTime mTime) mTime.UpdatedAt = utcNow;
                    if (entry.Entity is IHasModifier mUser) mUser.UpdatedBy = userId;

                    //prevent modification of creation properties
                    if (entry.Entity is IHasCreationTime) entry.Property(nameof(IHasCreationTime.CreatedAt)).IsModified = false;
                    if (entry.Entity is IHasCreator) entry.Property(nameof(IHasCreator.CreatedBy)).IsModified = false;

                }

                else if (entry.Entity is ISoftDeletableEntity softDelete && entry.State == EntityState.Deleted)
                {
                    if (softDelete.IsDeleted)
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                        softDelete.IsDeleted = true;
                        softDelete.DeletedAt = utcNow;
                        softDelete.DeletedBy = userId;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #region Helper Functions

        // Generates a lambda expression for the global query filter to exclude soft-deleted entities
        private static LambdaExpression GenerateIsDeletedFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "it");
            var property = Expression.Property(parameter, nameof(IFullyAuditableEntity.IsDeleted));
            var falseConstant = Expression.Constant(false);
            var body = Expression.Equal(property, falseConstant);

            return Expression.Lambda(body, parameter);
        }

        #endregion
    }
}