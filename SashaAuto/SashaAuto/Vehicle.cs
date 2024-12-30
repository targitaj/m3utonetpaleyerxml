using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SashaAuto
{
    public class Vehicle : MvxViewModel
    {
        private long? _id;
        private string _number;
        private string _notes = string.Empty;
        private bool _isEditing;

        public long? Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }
    }
}
