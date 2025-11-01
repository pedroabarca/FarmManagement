namespace FarmManagement.Domain.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
