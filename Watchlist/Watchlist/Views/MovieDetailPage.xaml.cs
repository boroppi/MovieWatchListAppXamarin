using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Async;
using Watchlist.Models;
using Watchlist.Persistance;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Watchlist
{
    [XamlCompilation(XamlCompilationOptions.Compile)]   
    public partial class MovieDetailPage : ContentPage
    {

        //public event EventHandler<Movie> AddedToWatchListEvent;
        //public event EventHandler<Movie> RemovedfromWatchListEvent;

        private SQLiteAsyncConnection _connection2;

        public const string watchListFalseIcon = "favoritef.png";
        public const string WatchListTrueIcon = "favoritet.png";

        public List<Movie> _MovieList;
        public bool _InWatchList;
        public bool _IdExistsInWatchList;


        public MovieDetailPage(Result result)
        {
            InitializeComponent();

            _connection2 = DependencyService.Get<ISQLiteDb>().GetConnection();

            BindingContext = result ?? throw new ArgumentNullException(nameof(result));

            setWatchListIcon(result.Id);
        }

        public async void setWatchListIcon(int _Id)
        {
            _MovieList = await _connection2.Table<Movie>().ToListAsync();
            _InWatchList = _MovieList.Exists(c => c.Id == _Id && c.InWatchList == true);
            _IdExistsInWatchList = _MovieList.Exists(c => c.Id == _Id);

            if (_InWatchList)
            {
                iconAddWatchList.Source = WatchListTrueIcon;
            }
            else
                iconAddWatchList.Source = watchListFalseIcon;
        }

        private async void favoriteTapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            var movie = BindingContext as Result;
            int _Id = movie.Id;
            var _Movie = new Movie();

            if (!_InWatchList && !_IdExistsInWatchList)
            {
                image.Source = "favoritet.png";

                _Movie.Id = movie.Id;
                _Movie.Title = movie.Title;
                _Movie.Poster_Path = movie.Poster_Path;
                _Movie.InWatchList = true;
                _Movie.Overview = movie.Overview;
                _Movie.Release_Date = movie.Release_Date;
                _Movie.DateCreated = DateTime.Now;

                await _connection2.InsertAsync(_Movie);
                MessagingCenter.Send<MovieDetailPage, Movie>(this, "Movie", _Movie);

            }
            else if (_InWatchList)
            {
                image.Source = "favoritef.png";
                _Movie = _MovieList.Single(c => c.Id == movie.Id);
                _Movie.InWatchList = false;

                await _connection2.UpdateAsync(_Movie);
                MessagingCenter.Send<MovieDetailPage, Movie>(this, "MovieUpdate", _Movie);
            }
            else
            {
                image.Source = "favoritet.png";
                _Movie = _MovieList.Single(c => c.Id == movie.Id);
                _Movie.InWatchList = true;

                await _connection2.UpdateAsync(_Movie);
                MessagingCenter.Send<MovieDetailPage, Movie>(this, "Movie", _Movie);
            }
            setWatchListIcon(_Id);
        }
    }
}