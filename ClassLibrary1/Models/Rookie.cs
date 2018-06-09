using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Models
{
    public class Rookie
    {
        private const int MinNameLength = 2;
        //public static DateTime Today { get; }
        private DateTime today = DateTime.Today;

        public int Id { get; set; }

        public virtual string Type { get; } = CommanderDirectory.RookieType;

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public DateTime BirthDate { get; set; } = DateTime.MinValue;

        private int age;
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = today.Year - BirthDate.Year;

                if (today.Month < BirthDate.Month || (today.Month == BirthDate.Month && today.Day < BirthDate.Day))
                {
                    age--;
                }
             
                   age = value;
            }

        }
        

        public string Character { get; set; }

        public Adress ParentAdress { get; set; } = new Adress();

        public Educational Educational { get; set; } = new Educational();

        public virtual ValidationResult Validate()
        {
            var result = string.Empty;

            result += ValidateStringProperty(Name, "ім'я");
            result += ValidateStringProperty(Surname, "прізвище");
            result += ValidateStringProperty(Patronymic, "по-батькові");
            result += ValidateStringProperty(Character, "характер");

            if (Age < 18 || Age > 125)
            {
                result += "Вік не може бути менше 18 років чи більше 125 років.";
            }

            result += ValidateStringProperty(ParentAdress?.City, "місто проживання батьків");
            result += ValidateStringProperty(ParentAdress?.Street, "вулиця проживання батьків");
            result += ValidatePositiveNumberProperty(ParentAdress?.NumberHouse, "номер будинку проживаня батьків");
            result += ValidatePositiveNumberProperty(ParentAdress?.NumberFlat, "номер квартири проживання батьків");

            return new ValidationResult
            {
                IsSuccess = string.IsNullOrEmpty(result),
                ErrorMessage = result
            };
        }

        protected string ValidateStringProperty(string propertyValue, string propertyName)
        {
            var errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(propertyValue))
            {
                errorMessage += $"{propertyName} не може бути пустим. Додайте, будь-ласка, {propertyName}.";
            }

            if (propertyValue?.Length < MinNameLength)
            {
                errorMessage += $"Довжина {propertyName} повинна бути не меншою ніж два символи.";
            }

            return errorMessage;
        }

        protected string ValidatePositiveNumberProperty(int? propertyValue, string propertyName)
        {
            return propertyValue < 1 
                ? $"{propertyName} не може мати значення менше одного." 
                : string.Empty;
        }
    }
}
