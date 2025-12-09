using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class WasenderService
    {
        private readonly ApplicationDbContext _context;

        public WasenderService(
            ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
