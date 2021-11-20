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
	/// Логика взаимодействия для SearchFormingWindow.xaml
	/// </summary>
	public partial class SearchFormingWindow : Window
	{
		public EmployeeFilter ResultObject { get; set; }
		public SearchFormingWindow()
		{
			InitializeComponent();
		}

		private void ApplyBtn_Click(object sender, RoutedEventArgs e)
		{
			if (DataContext != null)
			{
				ResultObject = (EmployeeFilter)DataContext;

				ResultObject.LastNameFilter = ResultObject.LastNameFilter == "" ? null : ResultObject.LastNameFilter;
				ResultObject.FirstNameFilter = ResultObject.FirstNameFilter == "" ? null : ResultObject.FirstNameFilter;
				ResultObject.PatronymicFilter = ResultObject.PatronymicFilter == "" ? null : ResultObject.PatronymicFilter;
			}
			else
			{
				ResultObject = FormNewEmployeeFilterByFields();
			}

			this.DialogResult = true;
			this.Close();
		}

		private EmployeeFilter FormNewEmployeeFilterByFields() 
		{
			EmployeeFilter resultObject = new EmployeeFilter();

			if (LastNameTextBox.Text != "")
				resultObject.LastNameFilter = LastNameTextBox.Text;
			if (FirstNameTextBox.Text != "")
				resultObject.FirstNameFilter = FirstNameTextBox.Text;
			if (PatronymicTextBox.Text != "")
				resultObject.PatronymicFilter = PatronymicTextBox.Text;
			if (BeginBirthDatePicker.SelectedDate != null)
				resultObject.BeginBirthDateFilter = (DateTime)BeginBirthDatePicker.SelectedDate;
			if (EndBirthDatePicker.SelectedDate != null)
				resultObject.EndBirthDateFilter = (DateTime)EndBirthDatePicker.SelectedDate;

			return resultObject;
		}

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			ResetFields();
		}

		private void ResetFields()
		{
			DataContext = new EmployeeFilter();
			BeginBirthDatePicker.SelectedDate = null;
			EndBirthDatePicker.SelectedDate = null;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			BeginBirthDatePicker.DisplayDateStart = new DateTime(1900, 1, 1);
			EndBirthDatePicker.DisplayDateStart = new DateTime(1900, 1, 1);

			if (DataContext != null)
			{
				if ((DataContext as EmployeeFilter).BeginBirthDateFilter == DateTime.MinValue)
					BeginBirthDatePicker.SelectedDate = null;
				if ((DataContext as EmployeeFilter).EndBirthDateFilter == DateTime.MinValue)
					EndBirthDatePicker.SelectedDate = null;
			}
			else
			{
				ResetFields();
			}
		}
	}
}
