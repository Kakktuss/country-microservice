namespace CountryApplication.Dtos.Request
{
    public class CreateCountryDto
    {
        public string Name { get; }

        public string Code { get; }

        public CreateCountryDto(string name, string code)
        {
            Name = name;

            Code = code;
        }
    }
}