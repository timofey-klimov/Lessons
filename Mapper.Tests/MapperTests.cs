using Mapper.Expressions;
using NUnit.Framework;
using System.Reflection.Emit;

namespace Mapper.Tests
{
    public class Tests
    {
        class Person
        {
            public string Name { get; set; }

            public string Surname { get; set; }

            public Address Address { get; set; }
        }

        class PersonDto
        {
            public string FullName { get; set; }
            public string Name { get; set; }

            public string Surname { get; set; }

            public AddressDto Address { get; set; }
        }
        class Address
        {
            public string Country { get; set; }
            public City City { get; set; }

            public string Street { get; set; }
        }

        class AddressDto
        {
            public string Country { get; set; }
            public CityDto City { get; set; }

            public string Street { get; set; }
        }

        class City
        {
            public string CityName { get; set; }
        }
        class CityDto
        {
            public string CityName { get; set; }
        }


        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ExpressionMapper()
        {
            var person = new Person() { Name = "test", Surname = "test" };

            var mapper = new Mapper.Expressions.Mapper();
            var result = mapper.Map<PersonDto>(person);

            Assert.AreEqual(person.Name, result.Name);
            Assert.AreEqual(person.Surname, result.Surname);
        }

        [Test]
        public void Configuration()
        {
            var person = new Person() { Name = "test", Surname = "test" };
            var fullName = person.Name + " " + person.Surname;
            var address = new Address() { Country = "Russia", City = new City { CityName = "Moscow" }, Street = "SomeStreet" };
            person.Address = address;


            var configuration = new MemberMappingConfigurationBuilder();
            var personTypeConfiguration = configuration.CreateMap<Person, PersonDto>()
                .ForMember(x => x.FullName, x => x.Map(x => x.Name + " " + x.Surname))
                .ForMember(x => x.Name, x => x.Ignore())
                .Build();

            var adressTypeConfiguration = configuration.CreateMap<Address, AddressDto>().Build();
            var cityTypeConfiguration = configuration.CreateMap<City, CityDto>().Build();

            var mapper = new Expressions.Mapper(
                new MappingConfiguration(new List<TypeMappingConfiguration> { personTypeConfiguration, adressTypeConfiguration, cityTypeConfiguration }));
            var result = mapper.Map<PersonDto>(person);
            Assert.AreEqual(fullName, result.FullName);
            Assert.IsNull(result.Name);
            Assert.AreEqual(person.Address.Country, result.Address.Country);
        }
    }
}