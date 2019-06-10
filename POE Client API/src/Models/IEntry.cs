namespace PoeApiClient.Models
{
    public interface IEntry
    {
        int Rank { set; get; }
        bool Dead { set; get; }
        bool Retired { set; get; }
        bool Online { set; get; }
        ICharacter Character { set; get; }
        IAccount Account { get; set; }
    }

    public class Entry : IEntry
    {
        public int Rank { set; get; }
        public bool Dead { set; get; }
        public bool Retired { set; get; }
        public bool Online { set; get; }
        public ICharacter Character { set; get; }
        public IAccount Account { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (this.GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Entry e = (Entry)obj;
                return (this.Character.Id == e.Character.Id);
            }
        }

        public override int GetHashCode()
        {
            var hashCode = 584682452;
            hashCode = hashCode * -1425578 + Character.Id.GetHashCode();
            return hashCode;
        }
    }
}
