using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Models;
using ClassLibrary1.Infrastrcture;

namespace ClassLibrary1
{
    public class CommanderDirectory
    {
        private List<Soldier> _soldiers;
        private List<Rookie> _rookies;
        private const string PathToSoldiersFile = "soldiers.txt";
        private const string PathToRookiesFile = "rookies.txt";

        public CommanderDirectory()
        {
            LoadSoldiers();
            LoadRookies();
        }

        public static string RookieType { get; } = "Новобранець";

        public static string SoldierType { get; } = "Солдат";

        public Rookie GetRookie(int id)
        {
            try
            {
                var rookie = GetAllRookies().SingleOrDefault(s => s.Id == id);

                if (rookie == null)
                {
                    throw new ComanderException($"Жоден новобранець з айдішіком {id} не був знайдений.");
                }

                return rookie;
            }
            catch (InvalidOperationException ex)
            {
                throw new ComanderException(
                    $"Більш ніж один новобранець має аналогчний айдішник - {id}. Видаліть зайвих новобранців з таким же айдішником чи виберіть іншого новобранця.",
                    ex);
            }
        }

        public Soldier GetSoldier(int id)
        {
            try
            {
                var soldier = GetAllSoldiers().SingleOrDefault(s => s.Id == id);

                if (soldier == null)
                {
                    throw new ComanderException($"Жоден солдат з айдішіком {id} не був знайдений.");
                }

                return soldier;
            }
            catch (InvalidOperationException ex)
            {
                throw new ComanderException(
                    $"Більш ніж один солдат має аналогчний айдішник - {id}. Видаліть зайвих солдатыв з таким же айдішником чи виберіть іншого новобранця.",
                    ex);
            }
        }

        public void LoadSoldiers()
        {
            _soldiers = new List<Soldier>();

            using (TextReader reader = new StreamReader(PathToSoldiersFile))
            {
                var n = Convert.ToInt32(reader.ReadLine());
                while (n-- > 0)
                {
                    var dateTest = DateTime.Now.ToString();
                    var dateTest2 = DateTime.Now.ToShortDateString();
                    var dateTest3 = DateTime.Now.ToLongDateString();

                    var soldier = new Soldier
                    {
                        Id = Convert.ToInt32(reader.ReadLine()),
                        Name = reader.ReadLine(),
                        Surname = reader.ReadLine(),
                        Patronymic = reader.ReadLine(),
                        BirthDate = DateTime.Parse(reader.ReadLine()),
                        Character = reader.ReadLine(),
                        ServiceAttitude = reader.ReadLine()
                    };

                    soldier.ParentAdress = new Adress
                    {
                        City = reader.ReadLine(),
                        Street = reader.ReadLine(),
                        NumberHouse = Convert.ToInt32(reader.ReadLine()),
                        NumberFlat = Convert.ToInt32(reader.ReadLine())
                    };
                    soldier.Educational = new Educational
                    {
                        CivilProfession = reader.ReadLine(),
                        Education = reader.ReadLine(),
                        Rank = reader.ReadLine(),
                        DataReceiveRank = DateTime.Parse(reader.ReadLine())
                    };
                    soldier.Service = new Service
                    {
                        Post = reader.ReadLine(),
                        Subdivision = reader.ReadLine(),
                        FormService = reader.ReadLine(),
                        PeriodService = reader.ReadLine()
                    };

                    _soldiers.Add(soldier);
                }
            }
        }

        public List<Soldier> GetAllSoldiers()
        {
            LoadSoldiers();

            return _soldiers;
        }

        public void AddSoldier(Soldier soldier)
        {
            var id = 1;
            if (_soldiers.Count > 0)
            {
                id = _soldiers.Last().Id + 1;
            }

            soldier.Id = id;

            _soldiers.Add(soldier);

            using (var writer = File.AppendText(PathToSoldiersFile))
            {
                writer.WriteLine(soldier.Id);
                writer.WriteLine(soldier.Name);
                writer.WriteLine(soldier.Surname);
                writer.WriteLine(soldier.Patronymic);
                writer.WriteLine(soldier.BirthDate);
                writer.WriteLine(soldier.Character);
                writer.WriteLine(soldier.ServiceAttitude);
                writer.WriteLine(soldier.ParentAdress?.City);
                writer.WriteLine(soldier.ParentAdress?.Street);
                writer.WriteLine(soldier.ParentAdress?.NumberHouse);
                writer.WriteLine(soldier.ParentAdress?.NumberFlat);
                writer.WriteLine(soldier.Educational?.CivilProfession);
                writer.WriteLine(soldier.Educational?.Education);
                writer.WriteLine(soldier.Educational?.Rank);
                writer.WriteLine(soldier.Educational?.DataReceiveRank);
                writer.WriteLine(soldier.Service?.Post);
                writer.WriteLine(soldier.Service?.Subdivision);
                writer.WriteLine(soldier.Service?.FormService);
                writer.WriteLine(soldier.Service?.PeriodService);
            }

            UpdateCount(_soldiers.Count);
        }

        public void RemoveSoldier(int soldierId)
        {
            // TODO: Додати трай кетч для обробки ситуації, коли не знайдено жодного солдата з таким айдішніком чи знайдено більше одного солдату
            try
            {
                if (soldierId < 0)
                {
                    throw new Exception("Айдішник не може бути від`ємним");
                }
                var soldierToRemove = _soldiers.SingleOrDefault(s => s.Id == soldierId);
                _soldiers.Remove(soldierToRemove);

                var fileLines = File.ReadAllLines(PathToSoldiersFile);

                if (fileLines == null || fileLines.Length <= 0)
                {
                    throw new Exception("File is empty. Can not delete soldier in file.");
                }

                int tempIndex = 0;

                for (int i = 1; i < fileLines.Length; i++)
                {
                    if (string.Equals(fileLines[i], soldierId.ToString()))
                    {
                        tempIndex = i;
                        break;
                    }
                }

                var myListString = fileLines.ToList();

                myListString.RemoveRange(tempIndex, 19);

                var myArrayString = myListString.ToArray();

                File.WriteAllLines(PathToSoldiersFile, myArrayString);

                UpdateCount(_soldiers.Count);
            }
            catch (InvalidOperationException ex)
            {
                throw new ComanderException($"Більш ніж один солдат має аналогчний айдішник - {soldierId} чи такого айдішнику не існує. Видаліть зайвих солдат з таким же айдішником чи виберіть іншого солдата.", ex);
            }

        }

        public void UpdateSoldier(int soldierId, Soldier soldierToUpdate)
        {
            Soldier soldier = null;

            try
            {
                soldier = _soldiers.SingleOrDefault(s => s.Id == soldierId);

                if (soldier == null)
                {
                    AddSoldier(soldierToUpdate);

                    throw new ComanderException(
                        $"Жоден солдат з айдішіком {soldierId} не був знайдений, тому був створений новий солдат з айдішником {soldier.Id}.");
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new ComanderException(
                    $"Більш ніж один солдат має аналогчний айдішник - {soldierId}. Видаліть зайвих солдат з таким же айдішником чи виберіть іншого солдата.",
                    ex);
            }

            soldier.Name = soldierToUpdate.Name;
            soldier.Surname = soldierToUpdate.Surname;
            soldier.Service = soldierToUpdate.Service;
            soldier.Patronymic = soldierToUpdate.Patronymic;
            soldier.BirthDate = soldierToUpdate.BirthDate;
            soldier.Character = soldierToUpdate.Character;
            soldier.ServiceAttitude = soldierToUpdate.ServiceAttitude;

            if (soldier.ParentAdress == null)
            {
                soldier.ParentAdress = new Adress();
            }

            soldier.ParentAdress.City = soldierToUpdate.ParentAdress.City;
            soldier.ParentAdress.Street = soldierToUpdate.ParentAdress.Street;
            soldier.ParentAdress.NumberHouse = soldierToUpdate.ParentAdress.NumberHouse;
            soldier.ParentAdress.NumberFlat = soldierToUpdate.ParentAdress.NumberFlat;

            if (soldier.Educational == null)
            {
                soldier.Educational = new Educational();
            }

            soldier.Educational.CivilProfession = soldierToUpdate.Educational.CivilProfession;
            soldier.Educational.Education = soldierToUpdate.Educational.Education;
            soldier.Educational.Rank = soldierToUpdate.Educational.Rank;
            soldier.Educational.DataReceiveRank = soldier.Educational.DataReceiveRank;

            if (soldier.Service == null)
            {
                soldier.Service = new Service();
            }

            soldier.Service.Post = soldierToUpdate.Service.Post;
            soldier.Service.Subdivision = soldierToUpdate.Service.Subdivision;
            soldier.Service.FormService = soldierToUpdate.Service.FormService;
            soldier.Service.PeriodService = soldierToUpdate.Service.PeriodService;

            var fileLines = File.ReadAllLines(PathToSoldiersFile);
            if (fileLines == null || fileLines.Length <= 0)
            {
                throw new Exception("File is empty. Can not update soldier in file.");
            }

            for (int i = 1; i < fileLines.Length; i++)
            {
                if (string.Equals(fileLines[i], soldier.Id.ToString()))
                {
                    fileLines[i + 1] = soldier.Name;
                    fileLines[i + 2] = soldier.Surname;
                    fileLines[i + 3] = soldier.Patronymic;
                    fileLines[i + 4] = soldier.BirthDate.ToString();
                    fileLines[i + 5] = soldier.Character;
                    fileLines[i + 6] = soldier.ServiceAttitude;

                    fileLines[i + 7] = soldier.ParentAdress.City;
                    fileLines[i + 8] = soldier.ParentAdress.Street;
                    fileLines[i + 9] = soldier.ParentAdress.NumberHouse.ToString();
                    fileLines[i + 10] = soldier.ParentAdress.NumberFlat.ToString();

                    fileLines[i + 11] = soldier.Educational.CivilProfession;
                    fileLines[i + 12] = soldier.Educational.Education;
                    fileLines[i + 13] = soldier.Educational.Rank;
                    fileLines[i + 14] = soldier.Educational.DataReceiveRank.ToString();

                    fileLines[i + 15] = soldier.Service.Post;
                    fileLines[i + 16] = soldier.Service.Subdivision;
                    fileLines[i + 17] = soldier.Service.FormService;
                    fileLines[i + 18] = soldier.Service.PeriodService;

                    break;
                }
            }

            File.WriteAllLines(PathToSoldiersFile, fileLines);
        }

        private void UpdateCount(int count)
        {
            var fileLines = File.ReadAllLines(PathToSoldiersFile);
            if (fileLines == null || fileLines.Length <= 0)
            {
                throw new Exception("File is empty. Can not update soldier in file.");
            }

            fileLines[0] = count.ToString();

            File.WriteAllLines(PathToSoldiersFile, fileLines);

        }

        public void LoadRookies()
        {
            _rookies = new List<Rookie>();

            using (TextReader reader = new StreamReader(PathToRookiesFile))
            {
                var n = Convert.ToInt32(reader.ReadLine());
                while (n-- > 0)
                {
                    var rookie = new Rookie()
                    {
                        Id = Convert.ToInt32(reader.ReadLine()),
                        Name = reader.ReadLine(),
                        Surname = reader.ReadLine(),
                        Patronymic = reader.ReadLine(),
                        BirthDate = DateTime.Parse(reader.ReadLine()),
                        Character = reader.ReadLine(),
                    };

                    rookie.ParentAdress = new Adress
                    {
                        City = reader.ReadLine(),
                        Street = reader.ReadLine(),
                        NumberHouse = Convert.ToInt32(reader.ReadLine()),
                        NumberFlat = Convert.ToInt32(reader.ReadLine())
                    };
                    rookie.Educational = new Educational
                    {
                        CivilProfession = reader.ReadLine(),
                        Education = reader.ReadLine(),
                        Rank = reader.ReadLine(),
                        DataReceiveRank = DateTime.Parse(reader.ReadLine())
                    };

                    _rookies.Add(rookie);
                }
            }
        }

        public List<Rookie> GetAllRookies()
        {
            LoadRookies();

            return _rookies;
        }

        public void AddRookie(Rookie rookie)
        {
            var id = 1;
            if (_rookies.Count > 0)
            {
                id = _rookies.Last().Id + 1;
            }

            rookie.Id = id;

            _rookies.Add(rookie);

            using (var writer = File.AppendText(PathToRookiesFile))
            {
                writer.WriteLine(rookie.Id);
                writer.WriteLine(rookie.Name);
                writer.WriteLine(rookie.Surname);
                writer.WriteLine(rookie.Patronymic);
                writer.WriteLine(rookie.BirthDate);
                writer.WriteLine(rookie.Character);
                writer.WriteLine(rookie.ParentAdress?.City);
                writer.WriteLine(rookie.ParentAdress?.Street);
                writer.WriteLine(rookie.ParentAdress?.NumberHouse);
                writer.WriteLine(rookie.ParentAdress?.NumberFlat);
                writer.WriteLine(rookie.Educational?.CivilProfession);
                writer.WriteLine(rookie.Educational?.Education);
                writer.WriteLine(rookie.Educational?.Rank);
                writer.WriteLine(rookie.Educational?.DataReceiveRank);
            }

            UpdaterRookieCount(_rookies.Count);
        }

        public List<Rookie> SearchRookieByFullName(string stringRookieToSearchBy)
        {

            var sequenceRookies = _rookies.Where(p => p.Name.Contains(stringRookieToSearchBy)
                || p.Surname.Contains(stringRookieToSearchBy)
                || p.Patronymic.Contains(stringRookieToSearchBy));

            return sequenceRookies.ToList();
        }

        public void RemoveRookie(int rookieId)
        {
            // TODO: Додати трай кетч для обробки ситуації, коли не знайдено жодного солдата з таким айдішніком чи знайдено більше одного солдату
            //Rookie rookieToRemove = null;
            try
            {
                if (rookieId < 0)
                {
                    throw new Exception("Айдішник не може бути від`ємним");
                }

                var rookieToRemove = _rookies.SingleOrDefault(s => s.Id == rookieId);
                _rookies.Remove(rookieToRemove);

                var fileLines = File.ReadAllLines(PathToRookiesFile);

                if (fileLines == null || fileLines.Length <= 0)
                {
                    throw new Exception("File is empty. Can not delete rookie in file.");
                }

                int tempIndex = 0;

                for (int i = 1; i < fileLines.Length; i++)
                {
                    if (string.Equals(fileLines[i], rookieId.ToString()))
                    {
                        tempIndex = i;
                        break;
                    }
                }

                var myListString = fileLines.ToList();

                myListString.RemoveRange(tempIndex, 14);

                var myArrayString = myListString.ToArray();

                File.WriteAllLines(PathToRookiesFile, myArrayString);

                UpdaterRookieCount(_rookies.Count);
            }
            catch (InvalidOperationException ex)
            {
                throw new ComanderException($"Більш ніж один новобранець має аналогчний айдішник - {rookieId} чи такого айдішнику не існує. Видаліть зайвих новобранців з таким же айдішником чи виберіть іншого новобранця.", ex);
            }

        }

        public void UpdateRookie(int rookieId, Rookie rookieToUpdate)
        {
            Rookie rookie = null;

            try
            {
                rookie = _rookies.SingleOrDefault(s => s.Id == rookieId);

                if (rookie == null)
                {
                    AddRookie(rookieToUpdate);

                    throw new ComanderException(
                        $"Жоден новобранець з айдішіком {rookieId} не був знайдений, тому був створений новий новобранець з айдішником {rookie.Id}.");
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new ComanderException(
                    $"Більш ніж один новобранець має аналогчний айдішник - {rookieId}. Видаліть зайвих новобранців з таким же айдішником чи виберіть іншого новобранця.",
                    ex);
            }

            rookie.Name = rookieToUpdate.Name;
            rookie.Surname = rookieToUpdate.Surname;
            rookie.Patronymic = rookieToUpdate.Patronymic;
            rookie.BirthDate = rookieToUpdate.BirthDate;
            rookie.Character = rookieToUpdate.Character;

            if (rookie.ParentAdress == null)
            {
                rookie.ParentAdress = new Adress();
            }

            rookie.ParentAdress.City = rookieToUpdate.ParentAdress.City;
            rookie.ParentAdress.Street = rookieToUpdate.ParentAdress.Street;
            rookie.ParentAdress.NumberHouse = rookieToUpdate.ParentAdress.NumberHouse;
            rookie.ParentAdress.NumberFlat = rookieToUpdate.ParentAdress.NumberFlat;

            if (rookie.Educational == null)
            {
                rookie.Educational = new Educational();
            }

            rookie.Educational.CivilProfession = rookieToUpdate.Educational.CivilProfession;
            rookie.Educational.Education = rookieToUpdate.Educational.Education;
            rookie.Educational.Rank = rookieToUpdate.Educational.Rank;
            rookie.Educational.DataReceiveRank = rookieToUpdate.Educational.DataReceiveRank;

            var fileLines = File.ReadAllLines(PathToRookiesFile);
            if (fileLines == null || fileLines.Length <= 0)
            {
                throw new Exception("File is empty. Can not update rookie in file.");
            }

            for (int i = 1; i < fileLines.Length; i++)
            {
                if (string.Equals(fileLines[i], rookie.Id.ToString()))
                {
                    fileLines[i + 1] = rookie.Name;
                    fileLines[i + 2] = rookie.Surname;
                    fileLines[i + 3] = rookie.Patronymic;
                    fileLines[i + 4] = rookie.BirthDate.ToString();
                    fileLines[i + 5] = rookie.Character;

                    fileLines[i + 6] = rookie.ParentAdress.City;
                    fileLines[i + 7] = rookie.ParentAdress.Street;
                    fileLines[i + 8] = rookie.ParentAdress.NumberHouse.ToString();
                    fileLines[i + 9] = rookie.ParentAdress.NumberFlat.ToString();

                    fileLines[i + 10] = rookie.Educational.CivilProfession;
                    fileLines[i + 11] = rookie.Educational.Education;
                    fileLines[i + 12] = rookie.Educational.Rank;
                    fileLines[i + 13] = rookie.Educational.DataReceiveRank.ToString();

                    break;
                }
            }

            File.WriteAllLines(PathToRookiesFile, fileLines);
        }

        private void UpdaterRookieCount(int count)
        {
            var fileLines = File.ReadAllLines(PathToRookiesFile);
            if (fileLines == null || fileLines.Length <= 0)
            {
                throw new Exception("File is empty. Can not update rookie in file.");
            }

            fileLines[0] = count.ToString();

            File.WriteAllLines(PathToRookiesFile, fileLines);

        }
        
        
    }
}
