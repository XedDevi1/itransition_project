using course_project.Models;
using course_project.Persistence;
using Microsoft.EntityFrameworkCore;

namespace course_project.Services
{
    public class RefreshTokenService
    {
        private readonly CollectionContext _context;

        public RefreshTokenService(CollectionContext context)
        {
            _context = context;
        }

        public async Task<string> CreateRefreshTokenAsync(User user)
        {
            var token = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", "");

            var existsToken = await _context.RefreshTokens.SingleOrDefaultAsync(t => t.UserId == user.Id);

            if (existsToken != null)
            {
                _context.Remove(existsToken);
            }

            var newToken = new RefreshToken()
            {
                Token = token,
                UserId = user.Id
            };

            await _context.AddAsync(newToken);
            await _context.SaveChangesAsync();

            return token;
        }
    }
}
