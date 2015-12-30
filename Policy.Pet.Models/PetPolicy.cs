
namespace Policy.Pets.Models
{
    public class PetPolicy : Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
    }
}