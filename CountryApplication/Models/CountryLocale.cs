
using BuildingBlock.DataAccess;

namespace CountryApplication.Models
{
    public class CountryLocale : Entity
    {

        protected CountryLocale()
        {
            
        }

        public CountryLocale(bool @default = false)
        {
            Default = @default;
        }
        
        public bool Default { get; }
        
        public int CountryId { get; }
        
        public Country Country { get; }
        
        public int LocaleId { get; }
        
        public Locale Locale { get; }
        
    }
}