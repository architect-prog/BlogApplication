using BlogApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Repository
{
    public class PostRepository : BaseContextRepository<ApplicationContext, Post>
    {
        public PostRepository(ApplicationContext context) : base(context) { }
        public override async Task<Post> Add(Post entity)
        {
            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public override async Task<Post> Delete(int id)
        {
            Post removable = await _context.Posts.FindAsync(id);

            if (removable != null)
            {
                _context.Posts.Remove(removable);
                await _context.SaveChangesAsync();

                return removable;
            }
           
            return null;                      
        }

        public override List<Post> GetAll()
        {
            List<Post> posts = _context.Posts.ToList();
            return posts;
        }

        public override async Task<Post> GetById(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public override async Task<Post> Update(Post entity)
        {
            _context.Posts.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
