using System;
using BuildingBlock.DataAccess;
using BuildingBlock.DataAccess.Abstractions;

namespace CountryApplication.Models
{
    public class Country : Entity, IAggregateRoot
    {
        public Country(string name,
            string code)
        {
            Uuid = Guid.NewGuid();

            Name = name;

            Code = code;
        }

        public Guid Uuid { get; }

        public string Name { get; }

        public string Code { get; }
    }
}