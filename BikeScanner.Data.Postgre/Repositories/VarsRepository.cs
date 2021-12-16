using BikeScanner.Domain.Models;
using BikeScanner.Domain.Repositories;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class VarsRepository : IVarsRepository
    {
        private readonly string _indexingStampKey = "LastIndexEpoch";
        private readonly string _schedulingStampKey = "LastScheduleEpoch";

        private readonly BikeScannerContext _context;

        public VarsRepository(BikeScannerContext context)
        {
            _context = context;
        }

        public async Task<long?> GetLastIndexEpoch()
        {
            var value = await _context.Vars.FindAsync(_indexingStampKey);
            return value == null
                ? null
                : long.Parse(value.Value);
        }

        public async Task SetLastIndexEpoch(long stamp)
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

        public async Task<long?> GetLastScheduleEpoch()
        {
            var value = await _context.Vars.FindAsync(_schedulingStampKey);
            return value == null
                ? null
                : long.Parse(value.Value);
        }

        public async Task SetLastScheduleEpoch(long stamp)
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
