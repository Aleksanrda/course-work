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
    public partial class EditRookie : Form
    {
        private const string SaveType = "Save";
        private const string EditType = "Edit";

        private Rookie rookie;

        private readonly CommanderDirectory _commanderDirectory = new CommanderDirectory();
        private string type;

        public EditRookie()
        {
            InitializeComponent();
            InitComponents();

            InitRookieAdding();
        }

        public EditRookie(int rookieId)
        {
            InitializeComponent();
            InitComponents();

            InitRookieEditing(rookieId);
        }

        public void InitComponents()
        {
            RankReceivingDate.MaxDate = DateTime.Today;
            RankReceivingDate.MinDate = DateTime.Today.AddYears(-100);
        }

        private void InitRookieAdding()
        {
            rookie = new Rookie();

            rookie.BirthDate = birthData.MinDate;
            rookie.ParentAdress.NumberHouse = (int) HouseNumberNumeric.Minimum;
            rookie.ParentAdress.NumberFlat = (int) FlatNumberNumeric.Minimum;
            rookie.Educational.DataReceiveRank = RankReceivingDate.MinDate;

            type = SaveType;
            this.Text = "Додати новобранця";
            SaveButton.Text = "Додати";
            InitRookieTextBoxes();
        }

        private void InitRookieEditing(int id)
        {
            type = EditType;
            this.Text = "Редагувати новобранця";
            SaveButton.Text = "Редагувати";

            try
            {
                rookie = _commanderDirectory.GetRookie(id);
                InitRookieTextBoxes();
            }
            catch (Exception e)
            {
                MessageBox.Show("Вибачте, сталася помилка", e.Message, MessageBoxButtons.OK);
            }
        }

        private void InitRookieTextBoxes()
        {
            this.nameTextBox.DataBindings.Add("Text", rookie, "Name");
            this.Surname.DataBindings.Add("Text", rookie, "Surname");
            this.Patronic.DataBindings.Add("Text", rookie, "Patronymic");
            this.birthData.DataBindings.Add("Value", rookie, "BirthDate");
            this.CharacterTextbox.DataBindings.Add("Text", rookie, "Character");

            this.CityTextBox.DataBindings.Add("Text", rookie, "ParentAdress.City");
            this.StreetTextBox.DataBindings.Add("Text", rookie, "ParentAdress.Street");
            this.FlatNumberNumeric.DataBindings.Add("Value", rookie, "ParentAdress.NumberFlat");
            this.HouseNumberNumeric.DataBindings.Add("Value", rookie, "ParentAdress.NumberHouse");

            this.CivilProfessionTextBox.DataBindings.Add("Text", rookie, "Educational.CivilProfession");
            this.EducationTextBox.DataBindings.Add("Text", rookie, "Educational.Education");
            this.RankTextBox.DataBindings.Add("Text", rookie, "Educational.Rank");

            this.RankReceivingDate.DataBindings.Add("Value", rookie, "Educational.DataReceiveRank");
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var validationResult = rookie.Validate();
            ErrorMessage.Text = validationResult.ErrorMessage;

            if (!validationResult.IsSuccess)
            {
                MessageBox.Show("Помилка валідації", "Ви ввели невірні дані. Виправте помилки перед збереженням.", MessageBoxButtons.OK);

                return;
            }

            try
            {
                if (type == SaveType)
                {
                    _commanderDirectory.AddRookie(rookie);
                }
                else if (type == EditType)
                {
                    _commanderDirectory.UpdateRookie(rookie.Id, rookie);
                }

                MessageBox.Show("Успіх", "Новобранець був збережений.", MessageBoxButtons.OK);
                new RookieDetails(rookie.Id).Show();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Вибачте, трапилася помилка", exception.Message, MessageBoxButtons.OK);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult response = MessageBox.Show("Ви впевнені, що хочете закрити вікно? Введена інформація не буде збережена.", "Закрити вікно?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (response == DialogResult.No)
                return;

            if (response == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void EditRookie_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult response = MessageBox.Show("Ви впевнені, що хочете закрити вікно? Введена інформація не буде збережена.", "Закрити вікно?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (response == DialogResult.No)
                e.Cancel = true;
        }

        private void ErrorMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
