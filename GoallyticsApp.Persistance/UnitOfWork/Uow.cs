using GoallyticsApp.Application.Interfaces;
using GoallyticsApp.Domain.Entities;
using GoallyticsApp.Persistance.Context;
using GoallyticsApp.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.UnitOfWork
{
    public class Uow : IUow
    {
        private readonly MatchContext _context;

        public Uow(MatchContext context)
        {
            _context = context;
        }
        public IRepository<T> GetRepository<T>() where T: BaseEntity
        {
            return new Repository<T>(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
