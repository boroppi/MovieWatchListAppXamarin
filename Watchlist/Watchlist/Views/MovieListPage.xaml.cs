using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchlist.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Watchlist.Models;

namespace Watchlist
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieListPage : ContentPage
    {
        private BindableProperty IsSearchingProperty =
              BindableProperty.Create("IsSearching", typeof(bool), typeof(MovieListPage), false);

        public bool IsSearching
        {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }

        private MovieSearchService _service = new MovieSearchService();

        public MovieListPage()
        {
            BindingContext = this;

            InitializeComponent();
        }

        private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null || e.NewTextValue.Length < MovieSearchService.MinSearchLength)
                return;

            await FindMovies(query: e.NewTextValue);
        }

        async Task FindMovies(string query)
        {
            try
            {
                IsSearching = true;
                SearchingIndicator.IsVisible = true;

                var movies = await _service.GetListOfMovies(query);
                listViewMovies.ItemsSource = movies;
                listViewMovies.IsVisible = movies.Any();
                notFound.IsVisible = !listViewMovies.IsVisible;
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Could not retrieve the list of movies.", "OK");
            }
            finally
            {
                IsSearching = false;
                SearchingIndicator.IsVisible = false;
                //var movie = listViewMovies.ItemsSource as List<Result>;

                //var _movie = movie.Find(c => c.Title.Contains("Fellowship")) as Result;

                //if (_movie != null)
                //    Log.Warning(nameof(Result), "Error loading Poster: {0}", _movie.Poster_Image_Url);

            }
        }


        private async void listViewMovies_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listViewMovies.SelectedItem == null)
                return;

            var movie = e.SelectedItem as Result;

            listViewMovies.SelectedItem = null;

            await Navigation.PushModalAsync(new MovieDetailPage(movie));
        }
    }
}