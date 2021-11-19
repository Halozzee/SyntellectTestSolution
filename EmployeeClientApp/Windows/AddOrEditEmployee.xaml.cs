using Domain.EmployeeObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmployeeClientApp.Windows
{
	/// <summary>
	/// Логика взаимодействия для AddOrEditEmployee.xaml
	/// </summary>
	public partial class AddOrEditEmployee : Window
	{
		public Employee ResultObject { get; set; }
		public bool IsEditingExisting { get; set; }
		public AddOrEditEmployee()
		{
			InitializeComponent();
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			if (IsEditingExisting)
			{
				ResultObject = (Employee)DataContext;
			}
			else
			{
				ResultObject = new Employee(LastNameTextBox.Text, FirstNameTextBox.Text, PatronymicTextBox.Text, (DateTime)BirthDatePicker.SelectedDate);
			}
			DialogResult = true;
			this.Close();
		}

		private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
		{
		}
	}
}
