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
		private HttpRequester _httpRequester;
		private ObservableCollection<Employee> _employees;
		private EmployeeFilter _diplayFilter = new EmployeeFilter();

		private int _currentPaginationIndex = 0;
		private int _currentPaginationCount = 0;
		public MainWindow()
		{
			InitializeComponent();

			_httpRequester = new HttpRequester("http://localhost:17179/EmployeeApi", new SimpleCrypter());

			PaginationElementCountComboBox.SelectedIndex = 0;
			Paginate(0);
		}

		private bool RefreshEmployeeList()
		{
			if (_diplayFilter.HasAtleastOneFieldToFilterWith)
			{
				WebServiceResponse response = _httpRequester.SendGetByConditionsRequest(_diplayFilter);

				if (response.IsSuccessful)
				{
					_employees = new ObservableCollection<Employee>(response.CastRecievedContentToObject<IEnumerable<Employee>>());
					EmployeeTableListView.ItemsSource = _employees;
					return true;
				}
				else
				{
					DisplayUnSucessfulResultInformation(response);
					return false;
				}
			}
			else
			{
				WebServiceResponse response = _httpRequester.SendGetAllRequest();

				if (response.IsSuccessful)
				{
					_employees = new ObservableCollection<Employee>(response.CastRecievedContentToObject<IEnumerable<Employee>>());
					EmployeeTableListView.ItemsSource = _employees;
					return true;
				}
				else
				{
					DisplayUnSucessfulResultInformation(response);
					return false;
				}
			}
		}

		private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (EmployeeTableListView.SelectedItem != null)
			{
				var messageBoxAnswer = MessageBox.Show("Удалить этого сотрудника?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

				if (messageBoxAnswer == MessageBoxResult.Yes)
				{
					WebServiceResponse response = _httpRequester.SendDeleteRequest((Employee)EmployeeTableListView.SelectedItem);

					if (response.IsSuccessful)
					{
						_employees.Remove((Employee)EmployeeTableListView.SelectedItem);
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

		private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (EmployeeTableListView.SelectedItem != null)
			{
				int selectedItemIndex = EmployeeTableListView.SelectedIndex;
				AddOrEditEmployee addOrEdit = new AddOrEditEmployee();

				addOrEdit.Title = "Редактирование сотрудника";
				addOrEdit.Owner = this;
				addOrEdit.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				addOrEdit.IsEditingExisting = true;
				addOrEdit.DataContext = (EmployeeTableListView.SelectedItem as Employee).Clone();

				if ((bool)addOrEdit.ShowDialog())
				{
					WebServiceResponse response = _httpRequester.SendUpdateRequest(addOrEdit.ResultObject);

					if (response.IsSuccessful)
					{
						_employees[selectedItemIndex] = addOrEdit.ResultObject;
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
			addOrEdit.Owner = this;
			addOrEdit.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			if ((bool)addOrEdit.ShowDialog())
			{
				WebServiceResponse response = _httpRequester.SendInsertRequest(addOrEdit.ResultObject);
				if (response.IsSuccessful)
				{
					_employees.Add(response.CastRecievedContentToObject<Employee>());
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

		private void RefreshEmployeesBtn_Click(object sender, RoutedEventArgs e)
		{
			RefreshEmployeeList();
		}

		private void FilterEmployeesBtn_Click(object sender, RoutedEventArgs e)
		{
			SearchFormingWindow searchFormingWindow = new SearchFormingWindow();

			searchFormingWindow.DataContext = _diplayFilter;
			searchFormingWindow.Owner = this;
			searchFormingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			if ((bool)searchFormingWindow.ShowDialog())
			{
				_diplayFilter = searchFormingWindow.ResultObject;
				Paginate(0);
			}
		}

		private void Paginate(int index) 
		{
			if (index > -1)
			{
				_diplayFilter.PaginationData.PaginationIndex = index;
				bool succesRefreshing = RefreshEmployeeList();

				if (succesRefreshing)
				{
					if (_employees.Count == 0)
					{
						Paginate(index - 1);
					}
					else
					{
						_currentPaginationIndex = index;
						PaginationLabel.Content = $"Страница {index + 1}";
					}
				}
			}
		}

		private void PrevPaginationBtn_Click(object sender, RoutedEventArgs e)
		{
			Paginate(_currentPaginationIndex - 1);
		}

		private void NextPaginationBtn_Click(object sender, RoutedEventArgs e)
		{
			Paginate(_currentPaginationIndex + 1);
		}

		private void PaginationElementCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_currentPaginationCount = (PaginationElementCountComboBox.SelectedIndex + 1) * 10;
			_diplayFilter.PaginationData.PaginationCount = _currentPaginationCount;
			RefreshEmployeeList();
		}

		public static List<Employee> MakeRandomEmployees(int count)
		{
			List<Employee> result = new List<Employee>();
			Random random = new Random();
			for (int i = 0; i < count; i++)
			{
				result.Add(new Employee(GenerateName(random.Next(0, 10)), GenerateName(random.Next(0, 10)), GenerateName(random.Next(0, 10)), new DateTime(random.Next(1990, 2020), random.Next(1, 12), random.Next(1, 27))));
			}
			return result;
		}
		public static string GenerateName(int len)
		{
			Random r = new Random();
			string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
			string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
			string Name = "";
			Name += consonants[r.Next(consonants.Length)].ToUpper();
			Name += vowels[r.Next(vowels.Length)];
			return Name;
		}

    }
}

