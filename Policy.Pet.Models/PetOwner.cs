using System;
using System.Collections.Generic;

namespace Policy.Pets.Models
{
    public class PetOwner : Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime PolicyDate { get; set; }
        public string CountryIsoCode { get; set; }
        public string Email { get; set; }
        public bool Archived { get; set; }
        public List<Pet> Pets { get; set; }
    }
}