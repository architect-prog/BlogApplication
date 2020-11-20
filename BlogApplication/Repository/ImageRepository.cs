using BlogApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Repository
{
    public class ImageRepository : BaseContextRepository<ApplicationContext, Image>
    {
        public ImageRepository(ApplicationContext context) : base(context) { }
        public override async Task<Image> Add(Image entity)
        {
            await _context.Images.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public override async Task<Image> Delete(int id)
        {
            Image removable = await _context.Images.FindAsync(id);

            if (removable != null)
            {
                _context.Images.Remove(removable);
                await _context.SaveChangesAsync();

                return removable;
            }

            return null;
        }

        public override List<Image> GetAll()
        {
            List<Image> posts = _context.Images.ToList();
            return posts;
        }

        public override async Task<Image> GetById(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public override async Task<Image> Update(Image entity)
        {
            _context.Images.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
