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
			WebServiceResponse response = httpRequester.SendGetAllRequest();

			if (response.IsSuccessful)
			{
				employees = new ObservableCollection<Employee>(response.CastRecievedContentToObject<IEnumerable<Employee>>());
				EmployeeTableListView.ItemsSource = employees;
			}
			else
			{
				DisplayUnSucessfulResultInformation(response);
			}
		}

		private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (EmployeeTableListView.SelectedItem != null)
			{
				WebServiceResponse response = httpRequester.SendDeleteRequest((Employee)EmployeeTableListView.SelectedItem);

				if (response.IsSuccessful)
				{
					employees.Remove((Employee)EmployeeTableListView.SelectedItem);
				}
				else
				{
					DisplayUnSucessfulResultInformation(response);
				}
			}
		}

		private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (EmployeeTableListView.SelectedItem != null)
			{
				int selectedItemIndex = EmployeeTableListView.SelectedIndex;
				AddOrEditEmployee addOrEdit = new AddOrEditEmployee();

				addOrEdit.Title = "Редактирование сотрудника";
				addOrEdit.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				addOrEdit.IsEditingExisting = true;
				addOrEdit.DataContext = (EmployeeTableListView.SelectedItem as Employee).Clone();

				if ((bool)addOrEdit.ShowDialog())
				{
					WebServiceResponse response = httpRequester.SendUpdateRequest(addOrEdit.ResultObject);

					if (response.IsSuccessful)
					{
						employees[selectedItemIndex] = addOrEdit.ResultObject;
					}
					else
					{
						DisplayUnSucessfulResultInformation(response);
					}
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

			addOrEdit.Title = "Добавление сотрудника";
			addOrEdit.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			if ((bool)addOrEdit.ShowDialog())
			{
				WebServiceResponse response = httpRequester.SendInsertRequest(addOrEdit.ResultObject);
				if (response.IsSuccessful)
				{
					employees.Add(response.CastRecievedContentToObject<Employee>());
				}
				else
				{
					DisplayUnSucessfulResultInformation(response);
				}
			}
		}

		private void DisplayUnSucessfulResultInformation(WebServiceResponse response) 
		{
			switch (response.ResponseStatus)
			{
				case Domain.ResponseStatus.None:
					MessageBox.Show("Формирование результата на стороне сервера происходит некорректно. Domain.ResponseStatus не инициализирован (выбран None).", "Ошибка инициализации");
					break;
				case Domain.ResponseStatus.Fail:
					MessageBox.Show(response.Content, "Действие на стороне сервера не было выполнено.");
					break;
				case Domain.ResponseStatus.Exception:
					MessageBox.Show($"Сообщение ошибки: {response.ExceptionString}", "Ошибка");
					break;
				default:
					break;
			}
		}
	}
}
