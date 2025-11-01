namespace FarmManagement.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
    }
}
