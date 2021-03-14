using System;
using System.Collections.Generic;
using BuildingBlock.DataAccess;
using BuildingBlock.DataAccess.Abstractions;

namespace CountryApplication.Models
{
    public class Locale : Entity, IAggregateRoot
    {

        protected Locale()
        {
            
        }

        public Locale(string name)
        {
            Uuid = Guid.NewGuid();

            Name = name;
            
            Countries = new List<CountryLocale>();
        }
        
        public Guid Uuid { get; }
        
        public string Name { get; }
        
        public List<CountryLocale> Countries { get; }
        
    }
}