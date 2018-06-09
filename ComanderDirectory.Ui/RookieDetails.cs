using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;
using ClassLibrary1.Models;

namespace Ui
{
    public partial class RookieDetails : Form
    {
        private const string DefaultMissingValueLabel = "Значення відсутнє";

        private int rookieId = 0;

        private readonly CommanderDirectory _commanderDirectory = new CommanderDirectory();

        public RookieDetails(int rookieId)
        {
            InitializeComponent();

            InitSoldier(rookieId);
        }

        private void InitSoldier(int id)
        {
            try
            {
                var rookie = _commanderDirectory.GetRookie(id);
                InitRookieLabels(rookie);
            }
            catch (Exception e)
            {
                MessageBox.Show("Вибачте, сталася помилка", e.Message, MessageBoxButtons.OK);
            }
        }

        private void InitRookieLabels(Rookie rookie)
        {
            rookieId = rookie.Id;

            NameRookie.Text = rookie.Name;
            Surname.Text = rookie.Surname;
            Patronomic.Text = rookie.Patronymic;
            Age.Text = rookie.BirthDate.ToString();
            Character.Text = rookie.Character;

            ParentAddressCity.Text = rookie.ParentAdress?.City ?? DefaultMissingValueLabel;
            ParentAddressStreet.Text = rookie.ParentAdress?.Street ?? DefaultMissingValueLabel;
            ParentAddressNumberFlat.Text = rookie.ParentAdress?.NumberFlat.ToString() ?? DefaultMissingValueLabel;
            ParentAddressNumberHouse.Text = rookie.ParentAdress?.NumberHouse.ToString() ?? DefaultMissingValueLabel;

            EducationalCivilProfession.Text = rookie.Educational?.CivilProfession ?? DefaultMissingValueLabel;
            EducationalEducation.Text = rookie.Educational?.Education ?? DefaultMissingValueLabel;
            EducationalRank.Text = rookie.Educational?.Rank ?? DefaultMissingValueLabel;
            EducationalDataReceiveRank.Text = rookie.Educational?.DataReceiveRank.ToShortDateString() ?? DefaultMissingValueLabel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var editForm = new EditRookie(rookieId);
            editForm.Show();
            Close();
        }
    }
}
