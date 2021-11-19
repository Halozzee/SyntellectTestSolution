using Domain.DataProtection;
using Domain.EmployeeObjects;
using Domain.DataProtection.Implementations;
using EmployeeClientApp.RequestSending;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using EmployeeClientApp.Windows;

namespace EmployeeClientApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		HttpRequester httpRequester;
		ObservableCollection<Employee> employees;
		public MainWindow()
		{
			InitializeComponent();

			httpRequester = new HttpRequester("http://localhost:17179/EmployeeApi", new SimpleCrypter());
			employees = new ObservableCollection<Employee>(httpRequester.SendGetAllRequest().ParseResponseMessageContentToObject<IEnumerable<Employee>>());
			EmployeeTableListView.ItemsSource = employees;
		}

		private void EmployeeTableListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var t = EmployeeTableListView.SelectedItem;
		}

		private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (EmployeeTableListView.SelectedItem != null)
			{
				WebServiceResponse response = httpRequester.SendDeleteRequest((Employee)EmployeeTableListView.SelectedItem);
				employees.Remove((Employee)EmployeeTableListView.SelectedItem);
			}
		}

		private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (EmployeeTableListView.SelectedItem != null)
			{
				int selectedItemIndex = EmployeeTableListView.SelectedIndex;
				AddOrEditEmployee addOrEdit = new AddOrEditEmployee();
				addOrEdit.IsEditingExisting = true;
				addOrEdit.DataContext = (EmployeeTableListView.SelectedItem as Employee).Clone();
				if ((bool)addOrEdit.ShowDialog())
				{
					WebServiceResponse response = httpRequester.SendUpdateRequest(addOrEdit.ResultObject);
					employees[selectedItemIndex] = addOrEdit.ResultObject;
				}
			}
			else
			{
				MessageBox.Show("Выберите сотрудника", "Не выбран сотрудник");
			}
		}

		private void AddEmployeeBtn_Click(object sender, RoutedEventArgs e)
		{
			AddOrEditEmployee addOrEdit = new AddOrEditEmployee();
			if ((bool)addOrEdit.ShowDialog())
			{
				WebServiceResponse response = httpRequester.SendInsertRequest(addOrEdit.ResultObject);
				employees.Add(response.ParseResponseMessageContentToObject<Employee>());
			}
		}
	}
}
