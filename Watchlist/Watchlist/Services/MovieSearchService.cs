using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Watchlist.Models;


namespace Watchlist.Services
{
    class MovieSearchService
    {
        private const string api_key = "28a8bee0519768abe99e7e1303488518";
        private string Url;
        private HttpClient _client = new HttpClient();

        public static readonly int MinSearchLength = 5;

        public async Task<IEnumerable<Result>> GetListOfMovies(string _query)
        {
            Url = $"https://api.themoviedb.org/3/search/movie?api_key={api_key}&query={_query}&page=1&include_adult=false";

            if (_query.Length < MinSearchLength)
                return Enumerable.Empty<Result>();

            var response = await _client.GetAsync(Url);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return Enumerable.Empty<Result>();

            var content = await response.Content.ReadAsStringAsync();

            var movies = JsonConvert.DeserializeObject<MovieSearchResult>(content);

            return movies.Results;
        }
    }
}
