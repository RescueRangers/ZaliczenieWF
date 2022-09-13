using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MvvmCross;

namespace ZaliczenieWF.Core.Models
{

    public class Participant : INotifyDataErrorInfo, IEquatable<Participant>
    {
        private void ValidateProperty(string property)
        {
            ClearErrors(property);
            switch (property)
            {
                case nameof(Name):
                    if (string.IsNullOrWhiteSpace(Name))
                        AddError(nameof(Name), "Nazwisko i imię nie mogą być puste");
                    if (!Name.Contains(' '))
                        AddError(nameof(Name), "Pole powinno zawierać nazwisko i imię oddzielone spacją");
                    break;
                case nameof(Stopien):
                    if (string.IsNullOrWhiteSpace(Stopien))
                        AddError(nameof(Stopien), "Stopień nie może być pusty");
                    break;
                case nameof(PESEL):
                    if (string.IsNullOrWhiteSpace(PESEL))
                        AddError(nameof(PESEL), "PESEL nie może być pusty");
                    break;
                case nameof(Kolumna):
                    if (string.IsNullOrWhiteSpace(Kolumna))
                        AddError(nameof(Kolumna), "Kolumna nie może być pusta");
                    break;
                case nameof(JednostkaWojskowa):
                    if (string.IsNullOrWhiteSpace(JednostkaWojskowa))
                        AddError(nameof(JednostkaWojskowa), "Jednostka wojskowa nie może być pusta");
                    break;
                default:
                    break;
            }
        }

        private void AddError(string property, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(property))
                _errorsByPropertyName[property] = new List<string>();

            if (!_errorsByPropertyName[property].Contains(error))
            {
                _errorsByPropertyName[property].Add(error);
                OnErrorsChanged(property);
            }
        }

        private void ClearErrors(string property)
        {
            if (_errorsByPropertyName.ContainsKey(property))
            {
                _errorsByPropertyName.Remove(property);
                OnErrorsChanged(property);
            }
        }

        public Participant()
        {
            Name += "";
            JednostkaWojskowa += "";
            Stopien += "";
            Kolumna += "";
            JednostkaWojskowa += "";
        }

        public string Stopien
        {
            get => _stopien;
            set
            {
                _stopien = value;
                ValidateProperty(nameof(Stopien));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                ValidateProperty(nameof(Name));
            }
        }
        public string PESEL
        {
            get => _pESEL;
            set
            {
                _pESEL = value;
                ValidateProperty(nameof(PESEL));
            }
        }
        public string Kolumna
        {
            get => _kolumna;
            set
            {
                _kolumna = value;
                ValidateProperty(nameof(Kolumna));
            }
        }
        public double Time { get; set; }
        public double Score { get; set; }
        public string JednostkaWojskowa
        {
            get => _jednostkaWojskowa;
            set
            {
                _jednostkaWojskowa = value;
                ValidateProperty(nameof(JednostkaWojskowa));
            }
        }
        public string Ocena { get; set; }
        public List<Score> Scores { get; set; } = new List<Score>();
        public string Errors => GetErrors();

        private string GetErrors()
        {
            var result = "";
            foreach (KeyValuePair<string, List<string>> item in _errorsByPropertyName)
            {
                foreach (var error in item.Value)
                {
                    result += error + " ";
                }
            }
            return result;
        }

        public List<string> Kolumny { get; set; } = new List<string> { "I", "II", "III", "IV", "V" };

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        private string _stopien;
        private string _name;
        private string _pESEL;
        private string _kolumna;
        private string _jednostkaWojskowa;

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            return obj is Participant part && Equals(part);
        }

        public override int GetHashCode()
        {
            return PESEL.GetHashCode();
        }

        public bool Equals(Participant other)
        {
            return string.Equals(other.PESEL, PESEL, StringComparison.OrdinalIgnoreCase);
        }
    }
}

