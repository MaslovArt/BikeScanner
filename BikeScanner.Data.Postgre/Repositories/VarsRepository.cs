using BikeScanner.Domain.Vars;
using System;
using System.Threading.Tasks;

namespace BikeScanner.Data.Postgre.Repositories
{
    public class VarsRepository : IVarsRepository
    {
        private readonly string _indexingTimeKey = "LastIndexingTime";

        private readonly BikeScannerContext _context;

        public VarsRepository(BikeScannerContext context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastIndexingTime()
        {
            var value = await _context.Vars.FindAsync(_indexingTimeKey);
            return value == null
                ? null
                : DateTime.Parse(value.Value);
        }

        public async Task SetLastIndexingTime(DateTime time)
        {
            var timeStr = time.ToString("u");
            var indexingTimeVar = await _context.Vars.FindAsync(_indexingTimeKey);

            if (indexingTimeVar != null)
            {
                indexingTimeVar.Value = timeStr;
            }
            else
            {
                _context.Vars.Add(new VarEntity()
                {
                    Key = _indexingTimeKey,
                    Value = timeStr
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
