using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;

namespace BugetPersonal.Models
{
    public class VenituriChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MonthlyDataPoint> _monthlyData;
        private decimal _totalIncome;
        private decimal _totalExpense;
        private decimal _monthlyAverage;
        private ObservableCollection<Venit> _recentVenituri;
        private int _selectedSlideIndex;

        public ObservableCollection<MonthlyDataPoint> MonthlyData
        {
            get => _monthlyData;
            set
            {
                _monthlyData = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalIncome
        {
            get => _totalIncome;
            set
            {
                _totalIncome = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalExpense
        {
            get => _totalExpense;
            set
            {
                _totalExpense = value;
                OnPropertyChanged();
            }
        }

        public decimal MonthlyAverage
        {
            get => _monthlyAverage;
            set
            {
                _monthlyAverage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Venit> RecentVenituri
        {
            get => _recentVenituri;
            set
            {
                _recentVenituri = value;
                OnPropertyChanged();
            }
        }

        public int SelectedSlideIndex
        {
            get => _selectedSlideIndex;
            set
            {
                _selectedSlideIndex = value;
                OnPropertyChanged();
            }
        }

        public VenituriChartViewModel()
        {
            MonthlyData = new ObservableCollection<MonthlyDataPoint>();
            RecentVenituri = new ObservableCollection<Venit>();
            LoadData();
        }

        public async void LoadData()
        {
            try
            {
                if (App.VenituriDatabase != null && App.CheltuieliDatabase != null)
                {
                    // Obținem toate veniturile din baza de date
                    var venituri = await App.VenituriDatabase.GetVenituriAsync();

                    // Obținem toate cheltuielile din baza de date
                    var cheltuieli = await App.CheltuieliDatabase.GetCheltuieliAsync();

                    // Calculăm totalurile
                    TotalIncome = venituri.Sum(v => v.Amount);
                    TotalExpense = cheltuieli.Sum(c => c.Amount);

                    // Grupăm datele pe luni pentru ultimele 6 luni
                    DateTime currentDate = DateTime.Today;
                    var last6Months = Enumerable.Range(0, 6)
                        .Select(i => currentDate.AddMonths(-i))
                        .OrderBy(d => d)
                        .ToList();

                    var monthlyDataPoints = new List<MonthlyDataPoint>();

                    foreach (var month in last6Months)
                    {
                        var startOfMonth = new DateTime(month.Year, month.Month, 1);
                        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                        var monthlyIncome = venituri
                            .Where(v => v.Date >= startOfMonth && v.Date <= endOfMonth)
                            .Sum(v => v.Amount);

                        var monthlyExpense = cheltuieli
                            .Where(c => c.Date >= startOfMonth && c.Date <= endOfMonth)
                            .Sum(c => c.Amount);

                        monthlyDataPoints.Add(new MonthlyDataPoint
                        {
                            Month = month.ToString("MMM"),
                            Income = monthlyIncome,
                            Expense = monthlyExpense
                        });
                    }

                    // Actualizăm colecția de date lunare
                    MonthlyData.Clear();
                    foreach (var point in monthlyDataPoints)
                    {
                        MonthlyData.Add(point);
                    }

                    // Calculăm media lunară a veniturilor
                    if (monthlyDataPoints.Count > 0)
                    {
                        MonthlyAverage = monthlyDataPoints.Average(p => p.Income);
                    }

                    // Încărcăm veniturile recente pentru slider
                    RecentVenituri.Clear();
                    foreach (var venit in venituri.OrderByDescending(v => v.Date).Take(5))
                    {
                        RecentVenituri.Add(venit);
                    }

                    // Resetăm indexul slidului selectat
                    SelectedSlideIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la încărcarea datelor pentru grafic: {ex.Message}");
                // Opțional: poți afișa un mesaj de eroare utilizatorului
                // await Application.Current.MainPage.DisplayAlert("Eroare", "Nu s-au putut încărca datele pentru grafic", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MonthlyDataPoint : INotifyPropertyChanged
    {
        private string _month;
        private decimal _income;
        private decimal _expense;

        public string Month
        {
            get => _month;
            set
            {
                _month = value;
                OnPropertyChanged();
            }
        }

        public decimal Income
        {
            get => _income;
            set
            {
                _income = value;
                OnPropertyChanged();
            }
        }

        public decimal Expense
        {
            get => _expense;
            set
            {
                _expense = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}