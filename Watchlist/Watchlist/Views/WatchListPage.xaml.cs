using System.Collections.ObjectModel;
using System.Linq;
using SQLite.Net.Async;
using Watchlist.Models;
using Watchlist.Persistance;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Watchlist
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WatchListPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Movie> _Movie;
        private bool _IsDataLoaded;


        public WatchListPage()
        {
            InitializeComponent();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            BindingContext = this;

            MessagingCenter.Subscribe<MovieDetailPage, Movie>(this, "Movie", (sender, args) =>
            {
                _Movie.Insert(0, args);
            });

            MessagingCenter.Subscribe<MovieDetailPage, Movie>(this, "MovieUpdate", (sender, args) =>
            {
                _Movie.Remove(_Movie.Single(c => c.Id == args.Id));
            });

            LoadMovies();
        }

        public async void LoadMovies()
        {
            if (_IsDataLoaded)
                return;

            await _connection.CreateTableAsync<Movie>();

            var movies = await _connection.Table<Movie>().ToListAsync();
            var orderedMovies = movies.Where(s => s.InWatchList == true).OrderByDescending(c => c.DateCreated);

            _Movie = new ObservableCollection<Movie>(orderedMovies);

            listWatchList.ItemsSource = _Movie;
            _IsDataLoaded = true;
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listWatchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }


        private void searchBar_Focused(object sender, FocusEventArgs e)
        {
            //var IsFocused = await isSearchBarEmpty(e.IsFocused);

            //if (IsFocused)
            //    searchBar.Text = "";
            //else
            //    searchBar.Text = "My Watch List";

            //await Task.Delay(3000);
            //await isSearchBarEmpty(searchBar.IsFocused);
            //if (IsFocused)
            //    return;
            //else
            //    searchBar.Text = "My Watch List";

            searchBar.HorizontalTextAlignment = TextAlignment.Start;
            searchBar.Text = "";
        }

        //private async Task<Boolean> isSearchBarEmpty(bool IsFocused)
        //{
        //    return IsFocused;
        //}

        private void searchBar_Unfocused(object sender, FocusEventArgs e)
        {
            searchBar.HorizontalTextAlignment = TextAlignment.Center;
            searchBar.Text = "My Watch List";
        }
    }
}