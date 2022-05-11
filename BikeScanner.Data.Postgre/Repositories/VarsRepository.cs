using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class VarsRepository : IVarsRepository
    {
        private readonly string _indexingStampKey = "LastCrawlingTime";
        private readonly string _autoSearchStampKey = "LastAutoSearchTime";

        private readonly BikeScannerContext _context;

        public VarsRepository(BikeScannerContext context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastCrawlingTime()
        {
            var value = await _context.Vars.FindAsync(_indexingStampKey);
            return value == null
                ? null
                : DateTime.Parse(value.Value);
        }

        public async Task SetLastCrawlingTime(DateTime time)
        {
            var indexingTimeVar = await _context.Vars.FindAsync(_indexingStampKey);

            if (indexingTimeVar != null)
            {
                indexingTimeVar.Value = time.ToString();
            }
            else
            {
                _context.Vars.Add(new VarEntity()
                {
                    Key = _indexingStampKey,
                    Value = time.ToString()
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DateTime?> GetLastAutoSearchTime()
        {
            var value = await _context.Vars.FindAsync(_autoSearchStampKey);
            return value == null
                ? null
                : DateTime.Parse(value.Value);
        }

        public async Task SetLastAutoSearchTime(DateTime time)
        {
            var indexingTimeVar = await _context.Vars.FindAsync(_autoSearchStampKey);

            if (indexingTimeVar != null)
            {
                indexingTimeVar.Value = time.ToString();
            }
            else
            {
                _context.Vars.Add(new VarEntity()
                {
                    Key = _autoSearchStampKey,
                    Value = time.ToString()
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
