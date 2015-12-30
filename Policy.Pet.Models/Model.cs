
namespace Policy.Pets.Models
{
    public abstract class Model
    {
    }

    public class Country : Model
    {
        public string IsoCode { get; set; }
        public string Name { get; set; }
    }

    public class Breed : Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
