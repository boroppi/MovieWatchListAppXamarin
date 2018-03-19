using System;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace Watchlist.Models
{
    public class MovieSearchResult
    {
        public int Page { get; set; }
        public List<Result> Results { get; set; }
        public int Total_Results { get; set; }
        public int Total_Pages { get; set; }
    }

    public class Result
    {
        public string Poster_Path { get; set; }
        public bool Adult { get; set; }
        public string Overview { get; set; }
        public string Release_Date { get; set; }

        private DateTime _dateResult;

        public string ReleaseDate
        {
            get
            {
                return DateTime.TryParse(Release_Date, out _dateResult)
                    ? _dateResult.ToString("MMM dd, yyyy")
                    : "Unknown";
            }
        }

        public int Id { get; set; }
        public string original_title { get; set; }
        public string original_language { get; set; }
        public string Title { get; set; }
        public string Tagline { get; set; }
        public string Backdrop_Path { get; set; }
        public float Popularity { get; set; }
        public int vote_count { get; set; }
        public float Vote_Average { get; set; }

        public bool InWatchList { get; set; }

        public string Release_Year
        {
            get
            {
                if (Release_Date.Trim() != null && Release_Date.Length == 10)
                    return $"({(Release_Date.Substring(0, 4))})";
                else
                    return "(Unknown)";
            }
        }

        public string Backdrop_Image_Url
        {
            get { return $"http://image.tmdb.org/t/p/w150{Backdrop_Path}"; }
        }

        public Uri Poster_Image_Url
        {
            get
            {
                var _Url = string.Concat("http://image.tmdb.org/t/p/w150", @Poster_Path);
                var _Uri = new Uri(_Url);
                if (Title == "Mr. Nobody")
                {
                    Log.Warning(nameof(Title), _Uri.ToString());
                }

                return _Uri;
            }
        }

        public string Large_Poster_Image_Url
        {
            get { return "http://image.tmdb.org/t/p/w1000" + Poster_Path; }
        }
    }
}