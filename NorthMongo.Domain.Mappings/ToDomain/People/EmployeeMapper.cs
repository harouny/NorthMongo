using System.Linq;
using NorthMongo.Domain.People;

namespace NorthMongo.Domain.Mappings.ToDomain.People
{
    public class EmployeeMapper : IMapToNew<EF.Employee, Employee>
    {
        public Employee Map(EF.Employee source)
        {
            if (source == null) return null;
            var territoryMapper = new TerritoryMapper(); 
            return new Employee
            {
                Address = source.Address,
                BirthDate = source.BirthDate,
                HomePhone = source.HomePhone,
                HireDate = source.HireDate,
                FirstName = source.FirstName,
                Extension = source.Extension,
                EmployeeId = source.EmployeeID,
                Country = source.Country,
                City = source.City,
                LastName = source.LastName,
                Title = source.Title,
                Notes = source.Notes,
                Photo = source.Photo,
                PhotoPath = source.PhotoPath,
                PostalCode = source.PostalCode,
                TitleOfCourtesy = source.PostalCode,
                Region = source.Region,
                ReportsTo = source.ReportsTo,
                Territories = source.Territories
                    .Select(obj => territoryMapper.Map(obj))
                    .ToList()
            };
        }
    }
}
