using Microsoft.EntityFrameworkCore;
using NewProjectAPI.Data;
using NewProjectAPI.Helpers;
using NewProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Repo
{
  public class DatingRepo : IDatingRepo
  {
    private readonly NewProjectAPIContext _context;
    public DatingRepo(NewProjectAPIContext context)
    {
      _context = context;
    }
    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public async Task<Like> GetLike(int userId, int recipientId)
    {
      return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
    }

    public async Task<Photo> GetMainPhotoForUser(int userId)
    {
      return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
    }

    public async Task<Message> GetMessage(int id)
    {
      return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<PagedList<Message>> GetMessageForUser(MessageParams messageParams)
    {
      //We want to show the sender reciver and photo
      var messages = _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
        .Include(u => u.Recipient).ThenInclude(p => p.Photos).AsQueryable();

      switch(messageParams.MessageContainer)
      {
        case "Inbox":
          messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted ==false);
          break;
        case "Outbox":
          messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted ==false);
          break;
        default:
          messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted ==false && u.IsRead == false);
          break;
      }
      //ordered
      messages = messages.OrderByDescending(d => d.MessageSent);

      //return
      return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

    }

    public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
    {

      //We want to show the sender reciver and photo
      var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
        .Include(u => u.Recipient).ThenInclude(p => p.Photos)
        .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
        && m.SenderId == recipientId
        || m.RecipientId == recipientId && m.SenderDeleted ==false
        && m.SenderId == userId)
        .OrderByDescending(m => m.MessageSent)
        .ToListAsync();

      return messages;

    }

    public async Task<Photo> GetPhoto(int id)
    {
      var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
      return photo;
    }

    public async Task<Users> GetUser(int id)
    {
      var user =await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
      return user;
    }

    //public async Task<IEnumerable<Users>> GetUsers()
    //{
    //  var users = await _context.Users.Include(p => p.Photos).ToListAsync();
    //  return users;
    //}
    //This method for the paginations
    public async Task<PagedList<Users>> GetUsers(UserParams userParams)
    {

      var users = _context.Users.Include(p => p.Photos)
        .OrderByDescending(u => u.LastActive).AsQueryable();//We added asquerable for filter

      //Filter for Gender
      users = users.Where(u => u.Id != userParams.UserId);
      users = users.Where(u => u.Gender == userParams.Gender);
      //like
      if (userParams.Likers)
      {
        var userLikers = await GetUserLike(userParams.UserId, userParams.Likers);
        users = users.Where(u => userLikers.Contains(u.Id));
      }
      if (userParams.Likees)
      {
        var userLikees = await GetUserLike(userParams.UserId, userParams.Likers);
        users = users.Where(u => userLikees.Contains(u.Id));
      }
      //Filter for country
      // users = users.Where(u => u.Country == userParams.Country);
      //users = users.Where(u => u.City == userParams.City);
      //Filter for Age
      if (userParams.MinAge != 18 || userParams.MaxAge != 99)
      {
        var minDOB = DateTime.Today.AddYears(-userParams.MaxAge - 1);
        var maxDOB = DateTime.Today.AddYears(-userParams.MinAge);
        users = users.Where(u => u.DateOfBirth >= minDOB && u.DateOfBirth <= maxDOB);
      }

      //orderby
      if (!string.IsNullOrEmpty(userParams.OrderBy))
      {
        switch (userParams.OrderBy)
        {

          case "created":
            users = users.OrderByDescending(u => u.Created);
            break;

          default:
            users = users.OrderByDescending(u => u.LastActive);
            break;
        }
      }

      return await PagedList<Users>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<bool> SaveAll()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    private async Task<IEnumerable<int>>GetUserLike(int id, bool likers)
    {
      var user = await _context.Users
        .Include(x => x.Likers)
        .Include(x => x.Likees)
        .FirstOrDefaultAsync(u => u.Id == id);
      if (likers)
      {
        return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
      }
      else
      {
        return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
      }
    }
  }
}
