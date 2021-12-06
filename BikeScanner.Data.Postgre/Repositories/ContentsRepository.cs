using BikeScanner.Domain.Content;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class ContentsRepository : BaseRepository<ContentEntity>, IContentsRepository
    {
        public ContentsRepository(BikeScannerContext context)
            : base(context)
        { }

        public async Task<int> Refresh(IEnumerable<ContentEntity> contents, DateTime saveSince)
        {
            var existingModels = await Context.Contents
                .Select(c => new { c.Id, c.Url, c.Published })
                .ToArrayAsync();

            var existingUrls = existingModels.Select(m => m.Url);
            var inputUrls = contents.Select(c => c.Url);

            var newContents = inputUrls.Except(existingUrls);
            var deletedContents = existingUrls.Except(inputUrls);

            var newItems = contents.Where(c => newContents.Contains(c.Url));
            var deletedItems = existingModels
                .Where(m => deletedContents.Contains(m.Url) || m.Published < saveSince)
                .Select(m => new ContentEntity() { Id = m.Id });

            Context.Contents.RemoveRange(deletedItems);
            Context.Contents.AddRange(newItems);

            return await Context.SaveChangesAsync();
        }
    }
}
