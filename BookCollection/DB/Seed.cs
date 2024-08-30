namespace BookCollection.DB
{
    public class Seed(AppDBContext context)
    {
        private readonly AppDBContext _context = context;

        public void SeedTestDB()
        {
            _context.Database.EnsureCreated();
        }
    }
}
