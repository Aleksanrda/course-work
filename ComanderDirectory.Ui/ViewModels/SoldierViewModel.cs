using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ui.ViewModels
{
    public class SoldierViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; } 

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public int Age { get; set; }

        public string Character { get; set; }

        public string ParentAddressCity { get; set; }

        public string ParentAddressStreet { get; set; }

        public int? ParentAddressNumberHouse { get; set; }

        public int? ParentAddressNumberFlat { get; set; }

        public string EducationalCivilProfession { get; set; }

        public string EducationalEducation { get; set; }

        public string EducationalRank { get; set; }

        public string EducationalDataReceiveRank { get; set; }

        public string SoldierServiceAttitude { get; set; }

        public string SoldierServicePost { get; set; }

        public string SoldierServiceSubdivision { get; set; }

        public string SoldierServiceFormService { get; set; }

        public string SoldierServicePeriodService { get; set; }
    }
}
