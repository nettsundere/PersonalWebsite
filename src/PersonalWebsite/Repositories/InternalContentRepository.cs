using Microsoft.Data.Entity;
using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Repositories
{
    public class InternalContentRepository : IInternalContentRepository
    {
        private bool _isDisposed = false;

        private DataDbContext _context;

        public InternalContentRepository(DataDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                 _context.Dispose();
                _isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        ~InternalContentRepository()
        {
            Dispose();
        }

        public void DeleteContentsByInternalCaptions(IEnumerable<string> internalCaptions)
        {
            var contentsToRemove = from x in _context.Contents
                                   where internalCaptions.Contains(x.InternalCaption)
                                   select x;

            _context.Contents.RemoveRange(contentsToRemove);
            _context.SaveChanges();
        }

        public void EnsureContentsRangeAvailable(IEnumerable<Content> contentsRange)
        {
            var newContents = from x in contentsRange
                              where !_context.Contents.Contains(x)
                              select x;
            _context.Contents.AddRange(newContents);
            _context.SaveChanges();
        }
    }
}
