using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;

namespace BugetPersonal.Models
{
    public class ChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CategorySummary> _categoryData;
        private decimal _totalAmount;
        private int _totalCount;
        private ObservableCollection<Brush> _categoryBrushes;

        public ObservableCollection<CategorySummary> CategoryData
        {
            get => _categoryData;
            set
            {
                _categoryData = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Brush> CategoryBrushes
        {
            get => _categoryBrushes;
            set
            {
                _categoryBrushes = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }

        public int TotalCount
        {
            get => _totalCount;
            set
            {
                _totalCount = value;
                OnPropertyChanged();
            }
        }

        // Dicționar pentru maparea categoriilor la culori
        private Dictionary<string, Color> CategoryColors = new Dictionary<string, Color>
        {
            { "Facturi", Color.FromArgb("#cd8b65") },          // Maro (Bills)
            { "Alimente", Color.FromArgb("#c6d63c") },         // Verde lime (Food)
            { "Haine", Color.FromArgb("#3a58a7") },            // Albastru (Clothing)
            { "Educație", Color.FromArgb("#9933CC") },         // Violet
            { "Gym", Color.FromArgb("#33CCCC") },              // Turcoaz
            { "Sanatate", Color.FromArgb("#6A5ACD") },         // Albastru-violet (Health Care)
            { "Altele", Color.FromArgb("#00CED1") }            // Cyan (Others)
        };

        public ChartViewModel()
        {
            CategoryData = new ObservableCollection<CategorySummary>();
            CategoryBrushes = new ObservableCollection<Brush>();
            LoadData();
        }

        public async void LoadData()
        {
            try
            {
                if (App.CheltuieliDatabase != null)
                {
                    // Obținem toate categoriile definite
                    List<CheltuieliCategory> predefinedCategories = CheltuieliCategory.GetCategories();

                    // Obținem toate cheltuielile din baza de date
                    var cheltuieli = await App.CheltuieliDatabase.GetCheltuieliAsync();

                    // Gruparea cheltuielilor după categorii
                    var groupedData = cheltuieli
                        .GroupBy(c => c.Category ?? "Altele")
                        .Select(g => new CategorySummary
                        {
                            Category = g.Key,
                            Amount = g.Sum(c => c.Amount),
                            Count = g.Count(),
                            Color = GetColorForCategory(g.Key)
                        })
                        .ToList();

                    // Calcularea totalurilor
                    TotalAmount = cheltuieli.Sum(c => c.Amount);
                    TotalCount = cheltuieli.Count;

                    // Ne asigurăm că avem toate categoriile predefinite reprezentate (chiar și cu valori zero)
                    foreach (var category in predefinedCategories)
                    {
                        if (!groupedData.Any(g => g.Category == category.Name))
                        {
                            groupedData.Add(new CategorySummary
                            {
                                Category = category.Name,
                                Amount = 0,
                                Count = 0,
                                Color = GetColorForCategory(category.Name)
                            });
                        }
                    }

                    // Adăugăm procentajele pentru afișare
                    foreach (var item in groupedData)
                    {
                        if (TotalAmount > 0)
                        {
                            item.Percentage = Math.Round((item.Amount / TotalAmount) * 100, 2);
                            // Păstrăm suma reală pentru a o afișa în legendă
                            item.RawAmount = item.Amount;
                        }
                    }

                    // Sortare după sumă descrescător
                    groupedData = groupedData.OrderByDescending(g => g.Amount).ToList();

                    // Actualizarea colecției de date
                    CategoryData.Clear();
                    foreach (var item in groupedData)
                    {
                        CategoryData.Add(item);
                    }

                    // Actualizarea colecției de culori pentru grafic
                    CategoryBrushes.Clear();
                    foreach (var item in groupedData)
                    {
                        CategoryBrushes.Add(new SolidColorBrush(item.Color));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la încărcarea datelor pentru grafic: {ex.Message}");
                // Opțional: poți afișa un mesaj de eroare utilizatorului
                // await Application.Current.MainPage.DisplayAlert("Eroare", "Nu s-au putut încărca datele pentru grafic", "OK");
            }
        }

        // Obține culoarea pentru o categorie
        private Color GetColorForCategory(string category)
        {
            if (CategoryColors.ContainsKey(category))
                return CategoryColors[category];

            return Color.FromArgb("#CCCCCC"); // Gri implicit
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Restul codului pentru CategorySummary rămâne neschimbat
}

public class CategorySummary : INotifyPropertyChanged
    {
        private string _category;
        private decimal _amount;
        private decimal _rawAmount; // Pentru a păstra suma reală
        private int _count;
        private decimal _percentage;
        private Color _color;

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public decimal RawAmount
        {
            get => _rawAmount;
            set
            {
                _rawAmount = value;
                OnPropertyChanged();
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        public decimal Percentage
        {
            get => _percentage;
            set
            {
                _percentage = value;
                OnPropertyChanged();
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
