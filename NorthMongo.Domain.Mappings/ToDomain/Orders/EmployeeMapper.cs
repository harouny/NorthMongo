using NorthMongo.Domain.Orders;

namespace NorthMongo.Domain.Mappings.ToDomain.Orders
{
    internal class EmployeeMapper : IMapToNew<EF.Employee, Employee>
    {
        public Employee Map(EF.Employee source)
        {
            if (source == null) return null;
            return new Employee
            {
                EmployeeId = source.EmployeeID,
                LastName = source.LastName,
                FirstName = source.FirstName,
                Title = source.Title
            };
        }
    }
}
