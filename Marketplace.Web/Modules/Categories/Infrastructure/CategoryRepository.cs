﻿using Marketplace.Web.Infrastructure;
using Marketplace.Web.Modules.Categories.Application.Interfaces;
using Marketplace.Web.Modules.Categories.Domain.Entities;
using Marketplace.Web.Modules.Categories.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Modules.Categories.Infrastructure
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
            => await _context.Categories.FindAsync(id);

        public async Task<Category?> GetByNameAsync(string name)
            => await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);

        public async Task<IEnumerable<Category>> GetAllAsync(CategoryStatusEnum? status)
            => status.HasValue
                ? await _context.Categories.Where(c => c.Status == status).ToListAsync()
                : await _context.Categories.ToListAsync();

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
