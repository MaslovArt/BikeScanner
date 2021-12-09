using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class VarsRepository : IVarsRepository
    {
        private readonly string _indexingStampKey = "LastIndexingStamp";
        private readonly string _schedulingStampKey = "LastSchedulingStamp";

        private readonly BikeScannerContext _context;

        public VarsRepository(BikeScannerContext context)
        {
            _context = context;
        }

        public async Task<long?> GetLastIndexingStamp()
        {
            var value = await _context.Vars.FindAsync(_indexingStampKey);
            return value == null
                ? null
                : long.Parse(value.Value);
        }

        public async Task SetLastIndexingStamp(long stamp)
        {
            var indexingTimeVar = await _context.Vars.FindAsync(_indexingStampKey);

            if (indexingTimeVar != null)
            {
                indexingTimeVar.Value = stamp.ToString();
            }
            else
            {
                _context.Vars.Add(new VarEntity()
                {
                    Key = _indexingStampKey,
                    Value = stamp.ToString()
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<long?> GetLastSchedulingStamp()
        {
            var value = await _context.Vars.FindAsync(_schedulingStampKey);
            return value == null
                ? null
                : long.Parse(value.Value);
        }

        public async Task SetLastSchedulingStamp(long stamp)
        {
            var indexingTimeVar = await _context.Vars.FindAsync(_schedulingStampKey);

            if (indexingTimeVar != null)
            {
                indexingTimeVar.Value = stamp.ToString();
            }
            else
            {
                _context.Vars.Add(new VarEntity()
                {
                    Key = _schedulingStampKey,
                    Value = stamp.ToString()
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
