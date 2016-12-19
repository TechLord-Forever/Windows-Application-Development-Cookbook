/*
    Exemplary file for Chapter 4 - Data Storage.
    Recipe: Reading a JSON file.
*/

using CH04.Models;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace CH04.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        public ObservableCollection<EmployeeViewModel> Employees { get; set; }
        public string FullName { get; set; }
        public List<string> Languages { get; set; }
        public ICommand CmdAdd { get; set; }
        public ICommand CmdSave { get; set; }
        public ICommand CmdLoad { get; set; }

        public MainViewModel()
        {
            Employees = new ObservableCollection<EmployeeViewModel>();
            CmdAdd = new RelayCommand(() => AddEmployee());
            CmdSave = new RelayCommand(async () => await Save());
            CmdLoad = new RelayCommand(async () => await Load());
        }

        private void AddEmployee()
        {
            EmployeeViewModel employee = new EmployeeViewModel()
            {
                FullName = FullName,
                Languages = Languages
            };
            Employees.Add(employee);
        }

        private async Task Save()
        {
            string json = JsonConvert.SerializeObject(Employees);
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("Employees.json", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, json);
        }

        private async Task Load()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = (StorageFile)await folder.TryGetItemAsync("Employees.json");
			if (file == null)
			{
				return;
			}
			
            string json = await FileIO.ReadTextAsync(file);
            Employees = JsonConvert.DeserializeObject<ObservableCollection<EmployeeViewModel>>(json);
        }
    }
}
