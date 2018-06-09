using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Models;

namespace ClassLibrary1
{
    public class Soldier : Rookie
    {
        public override string Type { get; } = CommanderDirectory.SoldierType;

        public string ServiceAttitude { get; set; }
    
        public Service Service { get; set; } = new Service();

        public override ValidationResult Validate()
        {
            var result =  base.Validate();

            result.ErrorMessage += ValidateStringProperty(ServiceAttitude, "відношеня до служби");

            // TODO: добавить валиацию для сервиса 
            return result;
        }
    }
}
