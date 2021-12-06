namespace BikeScanner.Infrastructure.VK.Models
{
    public class WallSourceModel
    {
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is WallSourceModel item)
            {
                return OwnerId.Equals(item.OwnerId);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return OwnerId.GetHashCode();
        }
    }
}
