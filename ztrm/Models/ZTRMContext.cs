using Microsoft.EntityFrameworkCore;
using ztrm.Models.RandomThoughts;


namespace ztrm.Models
{
    public class ZTRMContext : DbContext
    {
        public ZTRMContext(DbContextOptions<ZTRMContext> options) : base(options) { }
        

        //Define DbSets from POCO classes
        public DbSet<RandomThought> RandomThoughts { get; set; }
        public DbSet<RandomThoughtsCategory> RandomThoughtsCategories { get; set; }
        public DbSet<RandomThoughtsCategoryLookup> RandomThoughtsCategoriesLookup { get; set; }



        //Stored Procedure Container entites - these are not actual tables in the DB



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the many-to-many relationship
            modelBuilder.Entity<RandomThoughtsCategory>()
                .HasKey(rtc => new { rtc.postid, rtc.categoryid });

            modelBuilder.Entity<RandomThoughtsCategory>()
                .HasOne<RandomThought>(rtc => rtc.RandomThought)
                .WithMany(rt => rt.RandomThoughtsCategories)
                .HasForeignKey(rtc => rtc.postid);

            modelBuilder.Entity<RandomThoughtsCategory>()
                .HasOne<RandomThoughtsCategoryLookup>(rtc => rtc.RandomThoughtsCategoryLookup)
                .WithMany(rtl => rtl.RandomThoughtsCategories)
                .HasForeignKey(rtc => rtc.categoryid);
        }


    }
}
