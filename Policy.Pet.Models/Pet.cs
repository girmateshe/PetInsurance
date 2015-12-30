using System;

namespace Policy.Pets.Models
{
    public class Pet : Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int PetOwnerId { get; set; }
        public string BreedName { get; set; }
        public bool Archived { get; set; }
    }
}