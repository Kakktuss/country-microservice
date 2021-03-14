
namespace CountryApplication.Models
{
    public class CountryLocale
    {

        protected CountryLocale()
        {
            
        }
        
        public int CountryId { get; }
        
        public Country Country { get; }
        
        public int LocaleId { get; }
        
        public Locale Locale { get; }
        
    }
}