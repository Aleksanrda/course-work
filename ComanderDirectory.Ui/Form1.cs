using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ClassLibrary1;
using ClassLibrary1.Models;
using Ui.ViewModels;

namespace Ui
{
    public partial class Form1 : Form
    {
        private List<string> sourceTypes = new List<string>{ "Усі", CommanderDirectory.RookieType, CommanderDirectory.SoldierType};
        private Dictionary<string, Func<SoldierViewModel, string>> rookieGroupByDictionary = new Dictionary<string, Func<SoldierViewModel, string>>
        {
            {"Характер", r => r.Character },
            {"Вік", r => r.Age.ToString() },
            {"Місто батьків", r => r.ParentAddressStreet },
            {"Цивільна професія", r => r.EducationalCivilProfession},
            {"Освіта", r => r.EducationalEducation}
        };

        private Dictionary<string, Func<SoldierViewModel, string>> soldierGroupByDictionary =
            new Dictionary<string, Func<SoldierViewModel, string>>();

        private Dictionary<string, List<SoldierViewModel>> groupingResult;

        private CommanderDirectory comanderDirectory;
        private BindingSource soldiersSource;

        public Form1()
        {
            InitializeComponent();
            InitServiceTooltips();
            InitGrouping();

            comanderDirectory = new CommanderDirectory();

            sourceType.DataSource = sourceTypes;
            sourceType.SelectedIndexChanged += this.sourceType_SelectedIndexChanged;
        }

        private void InitGrouping()
        {
            foreach (var pair in rookieGroupByDictionary)
            {
                soldierGroupByDictionary.Add(pair.Key, pair.Value);
            }

            soldierGroupByDictionary.Add("Форма служби", s => s.SoldierServiceFormService);
            soldierGroupByDictionary.Add("Підрозділ", s => s.SoldierServiceSubdivision);
            soldierGroupByDictionary.Add("Пост", s => s.SoldierServicePost);
        }

        private List<SoldierViewModel> GetAll()
        {
            var rookies = GetRookies();
            var soldiers = GetSoldiers();

            rookies.AddRange(soldiers);

            return rookies;
        }

        private List<SoldierViewModel> GetRookies()
        {
            var rookies = comanderDirectory.GetAllRookies().Select(s => new SoldierViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname,
                Patronymic = s.Patronymic,
                Age = s.Age,
                Character = s.Character,
                Type = s.Type,

                EducationalCivilProfession = s.Educational?.CivilProfession,
                EducationalDataReceiveRank = s.Educational?.DataReceiveRank.ToShortDateString(),
                EducationalEducation = s.Educational?.Education,
                EducationalRank = s.Educational?.Rank,

                ParentAddressCity = s.ParentAdress?.City,
                ParentAddressNumberFlat = s.ParentAdress?.NumberFlat,
                ParentAddressNumberHouse = s.ParentAdress?.NumberHouse,
                ParentAddressStreet = s.ParentAdress?.Street
            }).ToList();

            return rookies;
        }

        private List<SoldierViewModel> GetSoldiers()
        {
            var soldiers = comanderDirectory.GetAllSoldiers().Select(s => new SoldierViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname,
                Patronymic = s.Patronymic,
                Age = s.Age,
                Character = s.Character,
                Type = s.Type,

                EducationalCivilProfession = s.Educational?.CivilProfession,
                EducationalDataReceiveRank = s.Educational?.DataReceiveRank.ToShortDateString(),
                EducationalEducation = s.Educational?.Education,
                EducationalRank = s.Educational?.Rank,

                ParentAddressCity = s.ParentAdress?.City,
                ParentAddressNumberFlat = s.ParentAdress?.NumberFlat,
                ParentAddressNumberHouse = s.ParentAdress?.NumberHouse,
                ParentAddressStreet = s.ParentAdress?.Street,

                SoldierServiceAttitude = s.ServiceAttitude,

                SoldierServiceFormService = s.Service?.FormService,
                SoldierServicePeriodService = s.Service?.PeriodService,
                SoldierServicePost = s.Service?.Post,
                SoldierServiceSubdivision = s.Service?.Subdivision
            });

            return soldiers.ToList();
        }

        private void soldiers_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            
        }

        private void soldiers_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!e.Row.IsNewRow)
            {
                var response = MessageBox.Show("Ви впевнені, що хочете видалити?", "Видалити рядок?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (response == DialogResult.No)
                    e.Cancel = true;

                if (response == DialogResult.Yes)
                {
                    if (e.Row.Cells[1].Value.ToString() == CommanderDirectory.RookieType)
                        comanderDirectory.RemoveRookie((int)e.Row.Cells[0].Value);
                    if (e.Row.Cells[1].Value.ToString() == CommanderDirectory.SoldierType)
                        comanderDirectory.RemoveSoldier((int)e.Row.Cells[0].Value);
                }
            }
        }

        private void soldiers_CellClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void AddRookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EditRookie().Show();
        }

        private void LoadSoldiersbutton1_Click(object sender, EventArgs e)
        {
            LoadSoldiers();
        }

        private void InfaProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Програма \"Довідник командира\" була розроблена студенткою гр. ПЗПІ-17-1 Кривко Олександрою. Всі права захищені відповідно до закону про авторське право.", "Про програму", MessageBoxButtons.OK);
        }

        private void sourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSourceType = sourceType.SelectedItem.ToString(); 
            if (selectedSourceType == "Усі")
            {
                EnableServiceSearchTextboxes(true);

                groupByComboBox.DataSource = new BindingSource(soldierGroupByDictionary, null);
                InitGroupByComboboxMembers();
            }
            else if (selectedSourceType == CommanderDirectory.RookieType)
            {
                EnableServiceSearchTextboxes(false);

                groupByComboBox.DataSource = new BindingSource(rookieGroupByDictionary, null);
                InitGroupByComboboxMembers();
            }
            else if (selectedSourceType == CommanderDirectory.SoldierType)
            {
                EnableServiceSearchTextboxes(true);

                groupByComboBox.DataSource = new BindingSource(soldierGroupByDictionary, null);
                InitGroupByComboboxMembers();
            }
            LoadSoldiers();
        }

        private void InitGroupByComboboxMembers()
        {
            groupByComboBox.DisplayMember = "Key";
            groupByComboBox.ValueMember = "Value";
        }

        private List<SoldierViewModel> GetSoldiersBySourceType()
        {
            var soldiersList = new List<SoldierViewModel>();

            var selectedSourceType = sourceType.SelectedItem.ToString();

            if (selectedSourceType == "Усі")
                soldiersList = GetAll();
            else if (selectedSourceType == CommanderDirectory.RookieType)
                soldiersList = GetRookies();
            else if (selectedSourceType == CommanderDirectory.SoldierType)
                soldiersList = GetSoldiers();

            return soldiersList;
        }

        private void EnableServiceSearchTextboxes(bool enabled)
        {
            searchFormServiceTextBox.Enabled = enabled;
            searchSubdivisionTextBox.Enabled = enabled;
            searchPostTextBox.Enabled = enabled;
        }

        private void LoadSoldiers()
        {
            var soldiersList = GetSoldiersBySourceType();

            BindSoldiers(soldiersList);
        }

        private void BindSoldiers(List<SoldierViewModel> soldiersList)
        {
            soldiersSource = new BindingSource(new BindingList<SoldierViewModel>(soldiersList), null);

            soldiers.DataSource = soldiersSource;
        }

        private void soldiers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 && e.ColumnIndex < 0) return;

            var id = (int)soldiers.Rows[e.RowIndex].Cells[0].Value;
            var type = soldiers.Rows[e.RowIndex].Cells[1].Value.ToString();

            if (type == CommanderDirectory.RookieType)
            {
                var form = new RookieDetails(id);
                form.Show();
            }

            if (type == CommanderDirectory.SoldierType)
            {
                var form = new SoldierDetails(id);
                form.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var soldiersList = SearchSoldiers();

            //BindSoldiers(soldiersList.ToList());

            //selectGroupComboBox.Enabled = false;
        }

        private List<SoldierViewModel> SearchSoldiers()
        {
            var soldiersList = GetSoldiersBySourceType();

            if (!string.IsNullOrEmpty(searchNameTextBox.Text))
                soldiersList = soldiersList.Where(s => s.Name.ToLower().Contains(searchNameTextBox.Text.ToLower())).ToList();

            if (!string.IsNullOrEmpty(searchSurnameTextBox.Text))
                soldiersList = soldiersList.Where(s => s.Surname.ToLower().Contains(searchSurnameTextBox.Text.ToLower())).ToList();

            if (!string.IsNullOrEmpty(searchPatronomicTextBox.Text))
                soldiersList = soldiersList.Where(s => s.Patronymic.ToLower().Contains(searchPatronomicTextBox.Text.ToLower())).ToList();

            if (!string.IsNullOrEmpty(searchCharacterTextBox.Text))
                soldiersList = soldiersList.Where(s => s.Character.ToLower().Contains(searchCharacterTextBox.Text.ToLower())).ToList();

            if (!string.IsNullOrEmpty(searchCivilProfessionTextBox.Text))
                soldiersList = soldiersList.Where(s => s.EducationalCivilProfession.ToLower().Contains(searchCivilProfessionTextBox.Text.ToLower())).ToList();

            if (!string.IsNullOrEmpty(searchEducationalTextBox.Text))
                soldiersList = soldiersList.Where(s => s.EducationalEducation.ToLower().Contains(searchEducationalTextBox.Text.ToLower())).ToList();

            soldiersList = soldiersList.Where(s => s.Age >= searchAgeFromNumeric.Value && s.Age <= searchAgeToNumeric.Value).ToList();

            var selectedSourceType = sourceType.SelectedItem.ToString();

            if (selectedSourceType == "Усі" || selectedSourceType == CommanderDirectory.SoldierType)
            {
                if (!string.IsNullOrEmpty(searchFormServiceTextBox.Text))
                soldiersList = soldiersList.Where(s => (s.SoldierServiceFormService != null) && s.SoldierServiceFormService.ToLower().Contains(searchFormServiceTextBox.Text.ToLower())).ToList();
                
                if (!string.IsNullOrEmpty(searchSubdivisionTextBox.Text))
                    soldiersList = soldiersList.Where(s => (s.SoldierServiceSubdivision != null) && s.SoldierServiceSubdivision.ToLower().Contains(searchSubdivisionTextBox.Text.ToLower())).ToList();

                if (!string.IsNullOrEmpty(searchPostTextBox.Text))
                    soldiersList = soldiersList.Where(s => (s.SoldierServicePost != null) && (s.SoldierServicePost).ToLower().Contains(searchPostTextBox.Text.ToLower())).ToList();
            }

            return soldiersList;
        }

        private void InitServiceTooltips()
        {
            var toolTip1 = new ToolTip();

            toolTip1.Show("Пошук доступний тільки для солдатів", searchFormServiceTextBox, 1000);
            toolTip1.SetToolTip(searchPostTextBox, "Пошук доступний тільки для солдатів");
            toolTip1.SetToolTip(searchSubdivisionTextBox, "Пошук доступний тільки для солдатів");
        }

        private void searchPostTextBox_MouseHover(object sender, EventArgs e) ///??????????
        {
            var tt = new ToolTip();

            tt.Show("Пошук по посту доступний тільки для солдатів", searchFormServiceTextBox, 1000);
        }

        private void searchSubdivisionTextBox_MouseHover(object sender, EventArgs e)
        {
            var tt = new ToolTip();

            tt.Show("Пошук по підрозділу доступний тільки для солдатів", searchFormServiceTextBox, 1000);
        }

        private void searchFormServiceTextBox_MouseHover(object sender, EventArgs e)
        {
            var tt = new ToolTip();

            tt.Show("Пошук по формі служби доступний тільки для солдатів", searchFormServiceTextBox, 1000);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedGroupFunc = (KeyValuePair<string, Func<SoldierViewModel, string>>) groupByComboBox.SelectedItem;

            groupingResult = GroupSoldiersBy(selectedGroupFunc.Value);

            selectGroupComboBox.DataSource = groupingResult.Keys.ToList();
            selectGroupComboBox.Enabled = true;
        }

        public Dictionary<string, List<SoldierViewModel>> GroupSoldiersBy(Func<SoldierViewModel, string> fieldtoGroup)
        {
            var groupedSoldiers = new Dictionary<string, List<SoldierViewModel>>();

            foreach (var soldier in SearchSoldiers())
            {
                if (string.IsNullOrEmpty(fieldtoGroup(soldier)))
                {
                    Add(groupedSoldiers, soldier, "Значення відсутнє");

                    continue;
                }

                Add(groupedSoldiers, soldier, fieldtoGroup(soldier));
            }

            return groupedSoldiers;
        }

        private void Add<T>(Dictionary<string, List<T>> dictionary, T soldier, string key)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key].Add(soldier);
            else
                dictionary[key] = new List<T> { soldier };
        }

        private void selectGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedGrouping = selectGroupComboBox.SelectedItem.ToString();

            var soldiers = groupingResult[selectedGrouping];

            BindSoldiers(soldiers);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HelpForm().Show();
        }

        private void AddSoldierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EditSoldier().Show();
        }

        private void soldiers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void soldiers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
