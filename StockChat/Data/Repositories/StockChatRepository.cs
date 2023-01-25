namespace StockChat.Data.Repositories
{

    public class StockChatRepository : GenericRepository<Message>
    {
        public StockChatRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Message> All()
        {
            return _context.Messages
                .OrderByDescending(m => m.Date)
                .Take(50);
        }

    }

}


