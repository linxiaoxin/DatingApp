using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helper;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            this._context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity); //no async as this is not updating the database yet
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int receiptientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikeeId ==receiptientId && u.LikerId == userId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.UserId == userId && p.isMain);
        }

        public async Task<Photo> GetPhoto(int Id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == Id);
            return photo;
        }

        public async Task<User> GetUser(int Id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == Id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable() ;
            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.gender == userParams.Gender);

            if(userParams.isLiker) 
            {
                var liker = await _GetUserLikes(userParams.UserId, userParams.isLiker);
                users = users.Where(u => liker.Contains(u.Id));
            }
            if(userParams.isLikee) 
            {
                var likee = await _GetUserLikes(userParams.UserId, userParams.isLiker);
                users = users.Where(u => likee.Contains(u.Id));
            }
            if(userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge + 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge );
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }
            if(!string.IsNullOrEmpty(userParams.OrderBy)) {
                switch(userParams.OrderBy)
                {
                    case "created" :
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> _GetUserLikes(int id, bool likers) {
            var user = await this._context.Users
                .Include(x => x.Liker)
                .Include(x => x.Likee)
                .FirstOrDefaultAsync(u => u.Id == id);

            if(likers)
            {
                // the likers of this user
                return user.Liker.Where(u => u.LikeeId == id).Select(u => u.LikerId);
            }
            else
            {   // the likees of users
                return user.Likee.Where(u => u.LikerId == id).Select(u => u.LikeeId);
            }
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() >0;    
        }
    }
}