using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using System.Collections;
using System.Data.OleDb;
using System.Data;

namespace SashaAuto
{
    public class MainWindowModel : MvxViewModel
    {
        private readonly MvxCommand _addNewCommand;

        private List<Vehicle> _allVehicles = new List<Vehicle>();
        private ObservableCollection<Vehicle> _vehicles = new ObservableCollection<Vehicle>();
        private Vehicle _selectedVehicle;
        private readonly MvxCommand _saveCommand;
        private readonly MvxCommand _deleteCommand;
        private string _notes;
        private string _search;

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                foreach (var vehicle in _allVehicles)
                {
                    vehicle.IsEditing = false;
                }

                if (value != null)
                {
                    value.IsEditing = true;
                }
                SetProperty(ref _selectedVehicle, value);
                RaisePropertyChanged(nameof(Notes));
            }
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                DoSearch();
            }
        }

        public string Notes
        {
            get => SelectedVehicle?.Notes;
            set
            {
                if (SelectedVehicle != null)
                {
                    SelectedVehicle.Notes = value;
                }
            }
        }

        public MvxCommand AddNewCommand => _addNewCommand;

        public MvxCommand SaveCommand => _saveCommand;

        public MvxCommand DeleteCommand => _deleteCommand;

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }


        public MainWindowModel()
        {
            _addNewCommand = new MvxCommand(AddNew);
            _saveCommand = new MvxCommand(Save);
            _deleteCommand = new MvxCommand(Delete);

            LoadData();
        }

        public void DoSearch()
        {
            if (string.IsNullOrWhiteSpace(_search))
            {
                Vehicles = new ObservableCollection<Vehicle>(_allVehicles.OrderBy(o=>o.Number));
            }
            else
            {
                Vehicles = new ObservableCollection<Vehicle>(_allVehicles.Where(w =>
                    w.Number.ToLower().Contains(_search.ToLower())).OrderBy(o => o.Number));
            }
        }

        private void LoadData()
        {
            _allVehicles.Clear();

            string databasePath = "SashaAuto.mdb";
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};";
            string query = "SELECT * FROM Vehicle";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                connection.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    _allVehicles.Add(new Vehicle()
                    {
                        Id = Convert.ToInt64(row["ID"]),
                        Number = row["Number"].ToString(),
                        Notes = row["Notes"].ToString()
                    });
                }
            }

            DoSearch();
        }

        public void AddNew()
        {
            _allVehicles.Add(new Vehicle()
            {
                Number = "New Number"
            });

            DoSearch();
        }

        public void Delete()
        {
            if (SelectedVehicle != null)
            {
                _allVehicles.Remove(SelectedVehicle);
            }
            
            DoSearch();
        }

        public void Save()
        {
            string databasePath = "SashaAuto.mdb";
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};";
            string query = "SELECT * FROM Vehicle";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                connection.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                
                var rowsToRemove = new List<DataRow>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var vehicle = _allVehicles.FirstOrDefault(f => f.Id == Convert.ToInt64(row["ID"]));

                    if (vehicle != null)
                    {
                        row["Number"] = vehicle.Number;
                        row["Notes"] = vehicle.Notes;
                    }
                    else
                    {
                        rowsToRemove.Add(row);
                    }
                }

                foreach (var dataRow in rowsToRemove)
                {
                    dataRow.Delete();
                }

                foreach (var vehicle in _allVehicles)
                {
                    if (!vehicle.Id.HasValue)
                    {
                        var row = dataTable.NewRow();
                        row["Number"] = vehicle.Number;
                        row["Notes"] = vehicle.Notes;

                        dataTable.Rows.Add(row);
                    }
                }

                adapter.InsertCommand = new OleDbCommand("INSERT INTO Vehicle ([Number], [Notes]) VALUES (?, ?)", connection);
                adapter.InsertCommand.Parameters.Add("Number", OleDbType.VarChar, 255, "Number");
                adapter.InsertCommand.Parameters.Add("Notes", OleDbType.LongVarWChar, -1, "Notes");

                adapter.UpdateCommand = new OleDbCommand("UPDATE Vehicle SET [Number] = ?, [Notes] = ? WHERE [ID] = ?", connection);
                adapter.UpdateCommand.Parameters.Add("Number", OleDbType.VarChar, 255, "Number");
                adapter.UpdateCommand.Parameters.Add("Notes", OleDbType.LongVarWChar, -1, "Notes");
                adapter.UpdateCommand.Parameters.Add("Id", OleDbType.Integer, 0, "ID");

                adapter.DeleteCommand = new OleDbCommand("DELETE FROM Vehicle WHERE [ID] = ?", connection);
                adapter.DeleteCommand.Parameters.Add("Id", OleDbType.Integer, 0, "ID");


                adapter.Update(dataTable);
            }

            LoadData();
        }
    }
}
