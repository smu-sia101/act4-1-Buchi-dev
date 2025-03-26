using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace PetNameGenerator.Controllers
{
    [ApiController]
    [Route("api/petnames")]
    public class PetNameController : ControllerBase
    {
        private string[] dogNames = { "Buddy", "Max", "Charlie", "Rocky", "Rex" };
        private string[] catNames = { "Whiskers", "Mittens", "Luna", "Simba", "Tiger" };
        private string[] birdNames = { "Tweety", "Sky", "Chirpy", "Raven", "Sunny" };

        [HttpGet("generate")]
        public IActionResult Get(string animalType, bool twoPart = false)
        {
            if (string.IsNullOrEmpty(animalType))
            {
                return BadRequest(new { error = "The 'animalType' field is required." });
            }

            string[] selectedNames;
            var lowerAnimalType = animalType.ToLower();

            switch (lowerAnimalType)
            {
                case "dog":
                    selectedNames = dogNames;
                    break;
                case "cat":
                    selectedNames = catNames;
                    break;
                case "bird":
                    selectedNames = birdNames;
                    break;
                default:
                    return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });
            }

            var randomPicker = new Random();
            string finalName;

            if (twoPart)
            {
                var firstPart = selectedNames[randomPicker.Next(selectedNames.Length)];
                var secondPart = selectedNames[randomPicker.Next(selectedNames.Length)];
                finalName = firstPart + secondPart;
            }
            else
            {
                finalName = selectedNames[randomPicker.Next(selectedNames.Length)];
            }

            return Ok(new { name = finalName });
        }

        [HttpPost]
        public IActionResult Post(string animalType, string name)
        {
            if (string.IsNullOrEmpty(animalType) || string.IsNullOrEmpty(name))
            {
                return BadRequest(new { error = "Please provide both an animal type and a name." });
            }

            var lowerAnimalType = animalType.ToLower();

            switch (lowerAnimalType)
            {
                case "dog":
                    dogNames = dogNames.Concat(new[] { name }).ToArray();
                    break;
                case "cat":
                    catNames = catNames.Concat(new[] { name }).ToArray();
                    break;
                case "bird":
                    birdNames = birdNames.Concat(new[] { name }).ToArray();
                    break;
                default:
                    return BadRequest(new { error = "Sorry, I only support adding names for dogs, cats, and birds!" });
            }

            return Ok(new { message = $"Name '{name}' added to {animalType} names." });
        }

       
    }
}