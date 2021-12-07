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
    }
}
