using ERecruitmentBE.Interfaces;
using ERecruitmentBE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ERecruitmentBE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<ApplicantSpecification> ApplicantSpecifications { get; set; }
        public DbSet<ApplicantSpecificationItem> ApplicantSpecificationItems { get; set; }
        public Task<int> SaveChangesAsync()
        {
            SaveChangesHelper();
            return base.SaveChangesAsync();
        }

        private void SaveChangesHelper()
        {
            var addedEntitiesMs = ChangeTracker.Entries().Where(a => a.State == EntityState.Added).AsEnumerable().Select(a => a.Entity).OfType<IPublicPropModel>().ToList();
            var modifiedEntitiesMs = ChangeTracker.Entries().Where(a => a.State == EntityState.Modified).AsEnumerable().Select(a => a.Entity).OfType<IPublicPropModel>().ToList();
            var deletedEntitiesMs = ChangeTracker.Entries().Where(a => a.State == EntityState.Deleted).AsEnumerable().Select(a => a.Entity).OfType<IPublicPropModel>().ToList();

            foreach (var item in addedEntitiesMs)
            {
                var now = DateTimeOffset.Now;
                item.CreatedAt = now;
                item.UpdatedAt = now;
            }

            foreach (var item in modifiedEntitiesMs)
            {
                item.UpdatedAt = DateTimeOffset.Now;
            }

            foreach (var item in deletedEntitiesMs)
            {
                item.UpdatedAt = DateTimeOffset.Now;
            }
        }
    }
}
