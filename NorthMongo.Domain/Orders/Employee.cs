namespace NorthMongo.Domain.Orders
{
    public class Employee : BaseEntity
    {
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
    }
}
