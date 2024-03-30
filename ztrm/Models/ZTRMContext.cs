using Microsoft.EntityFrameworkCore;


namespace ztrm.Models
{
    public class ZTRMContext : DbContext
    {
        public ZTRMContext(DbContextOptions<ZTRMContext> options) : base(options) { }

        //Define DbSets from POCO classes











        //Stored Procedure Container entites - these are not actual tables in the DB







    }
}
