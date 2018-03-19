using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinAndroid;
using Watchlist.Droid.Persistance;
using Watchlist.Persistance;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace Watchlist.Droid.Persistance
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "WatchListDb.db3");

            var connectionFactory = new Func<SQLiteConnectionWithLock>(() =>
                new SQLiteConnectionWithLock(new SQLitePlatformAndroid(), new SQLiteConnectionString(path, storeDateTimeAsTicks: false)));

            return new SQLiteAsyncConnection(connectionFactory);
        }
    }
}