using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ui
{
    public partial class SoldierDetails : Form
    {
        private const string DefaultMissingValueLabel = "Значення відсутнє";

        private int soldierId = 0;

        private readonly CommanderDirectory _commanderDirectory = new CommanderDirectory();

        public SoldierDetails(int soldierId)
        {
            InitializeComponent();
            InitSoldier(soldierId);
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void InitSoldier(int id)
        {
            try
            {

                var soldier = _commanderDirectory.GetSoldier(id);
                InitSoldierLabels(soldier);
            }
            catch (Exception e)
            {
                MessageBox.Show("Вибачте, сталася помилка", e.Message, MessageBoxButtons.OK);
            }
        }

        private void InitSoldierLabels(Soldier soldier)
        {
            soldierId = soldier.Id;

            name.Text = soldier.Name;
            Surname.Text = soldier.Surname;
            Patronomic.Text = soldier.Patronymic;
            Age.Text = soldier.BirthDate.ToString();
            Character.Text = soldier.Character;
            serviceAttitudeText.Text = soldier.ServiceAttitude;

            ParentAddressCity.Text = soldier.ParentAdress?.City ?? DefaultMissingValueLabel;
            ParentAddressStreet.Text = soldier.ParentAdress?.Street ?? DefaultMissingValueLabel;
            ParentAddressNumberFlat.Text = soldier.ParentAdress?.NumberFlat.ToString() ?? DefaultMissingValueLabel;
            ParentAddressNumberHouse.Text = soldier.ParentAdress?.NumberHouse.ToString() ?? DefaultMissingValueLabel;

            EducationalCivilProfession.Text = soldier.Educational?.CivilProfession ?? DefaultMissingValueLabel;
            EducationalEducation.Text = soldier.Educational?.Education ?? DefaultMissingValueLabel;
            EducationalRank.Text = soldier.Educational?.Rank ?? DefaultMissingValueLabel;
            EducationalDataReceiveRank.Text = soldier.Educational?.DataReceiveRank.ToShortDateString() ?? DefaultMissingValueLabel;

            postText.Text = soldier.Service?.Post ?? DefaultMissingValueLabel;
            subdivisionText.Text = soldier.Service?.Subdivision ?? DefaultMissingValueLabel;
            formServiceText.Text = soldier.Service?.FormService ?? DefaultMissingValueLabel;
            periodServiceText.Text = soldier.Service?.PeriodService ?? DefaultMissingValueLabel;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var editForm = new EditSoldier(soldierId);
            editForm.Show();
            Close();
        }


        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void EditRookie_Click(object sender, EventArgs e)
        {
           var editForm = new EditSoldier(soldierId);
            editForm.Show();
            Close();
        }
    }
}
