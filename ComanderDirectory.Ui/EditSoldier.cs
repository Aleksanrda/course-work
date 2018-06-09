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
    public partial class EditSoldier : Form
    {
        private const string SaveType = "Save";
        private const string EditType = "Edit";

        private Soldier soldier;

        private readonly CommanderDirectory _commanderDirectory = new CommanderDirectory();
        private string type;

        public EditSoldier()
        {
            InitializeComponent();
            InitComponents();
            InitSoldierAdding();
        }

        public EditSoldier(int soldierId)
        {
            InitializeComponent();
            InitComponents();
            InitSoldierEditing(soldierId);
        }

        private void InitSoldierAdding()
        {
            soldier = new Soldier();

            soldier.BirthDate = birthDate.MinDate;
            soldier.ParentAdress.NumberHouse = (int)HouseNumberNnumericUpDown1.Minimum;
            soldier.ParentAdress.NumberFlat = (int)flatNumberNumericUpDown.Minimum;
            soldier.Educational.DataReceiveRank = receiveDateTimePicker.MinDate;

            type = SaveType;
            this.Text = "Додати солдата";
            saveEditSoldier.Text = "Додати";

            InitSoldierTextBoxes();
        }

        private void InitSoldierEditing(int id)
        {
            type = EditType;
            this.Text = "Редагувати солдата";
            saveEditSoldier.Text = "Редагувати";

            try
            {
                soldier = _commanderDirectory.GetSoldier(id);
                InitSoldierTextBoxes();
            }
            catch (Exception e)
            {
                MessageBox.Show("Вибачте, сталася помилка", e.Message, MessageBoxButtons.OK);
            }
        }


        private void InitSoldierTextBoxes()
        {
            this.nameTextBox.DataBindings.Add("Text", soldier, "Name");
            this.surname.DataBindings.Add("Text", soldier, "Surname");
            this.patronomic.DataBindings.Add("Text", soldier, "Patronymic");
            this.birthDate.DataBindings.Add("Value", soldier, "BirthDay");
            this.character.DataBindings.Add("Text", soldier, "Character");
            this.attitudeServiceTextBox.DataBindings.Add("Text", soldier, "ServiceAttitude");

            this.cityTextBox.DataBindings.Add("Text", soldier, "ParentAdress.City");
            this.streetTextBox.DataBindings.Add("Text", soldier, "ParentAdress.Street");
            this.flatNumberNumericUpDown.DataBindings.Add("Value", soldier, "ParentAdress.NumberFlat");
            this.HouseNumberNnumericUpDown1.DataBindings.Add("Value", soldier, "ParentAdress.NumberHouse");

            this.civilProfessionTextBox.DataBindings.Add("Text", soldier, "Educational.CivilProfession");
            this.educationTextBox.DataBindings.Add("Text", soldier, "Educational.Education");
            this.rankTextBox.DataBindings.Add("Text", soldier, "Educational.Rank");

            this.receiveDateTimePicker.DataBindings.Add("Value", soldier, "Educational.DataReceiveRank");

            this.postTextBox.DataBindings.Add("Text", soldier, "Service.Post");
            this.subdivisionTextBox.DataBindings.Add("Text", soldier, "Service.Subdivision");
            this.serviceFormTextBox.DataBindings.Add("Text", soldier, "Service.FormService");
            this.servicePeriodTextBox.DataBindings.Add("Text", soldier, "Service.PeriodService");


        }

        public void InitComponents()
        {
            receiveDateTimePicker.MaxDate = DateTime.Today;
            receiveDateTimePicker.MinDate = DateTime.Today.AddYears(-100);
        }

        private void saveEditSoldier_Click(object sender, EventArgs e)
        {
            var validationResult = soldier.Validate();
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
                    _commanderDirectory.AddSoldier(soldier);
                }
                else if (type == EditType)
                {
                    _commanderDirectory.UpdateSoldier(soldier.Id, soldier);
                }

                MessageBox.Show("Успіх", "Солдат був збережений.", MessageBoxButtons.OK);
                new SoldierDetails(soldier.Id).Show();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Вибачте, трапилася помилка", exception.Message, MessageBoxButtons.OK);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
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

        private void EditSoldier_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult response = MessageBox.Show("Ви впевнені, що хочете закрити вікно? Введена інформація не буде збережена.", "Закрити вікно?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (response == DialogResult.No)
                e.Cancel = true;
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void attitudeServiceText_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void receiveDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
