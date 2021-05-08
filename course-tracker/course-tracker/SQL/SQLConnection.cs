using System;
using System.IO;
using SQLite;

namespace course_tracker.SQL
{
    public class SQLConnection
    {
        private static SQLiteAsyncConnection connection;
        public SQLiteAsyncConnection GetAsyncConnection()
        {
            if (connection == null)
            {
                var docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var path = Path.Combine(docPath, "CourseTracker.db3");
                connection = new SQLiteAsyncConnection(path);
            }
            return connection;
        }
    }
}
