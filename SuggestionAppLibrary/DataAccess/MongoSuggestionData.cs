using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess
{
    public class MongoSuggestionData
    {
        private readonly IDbConnection _db;
        private readonly IMemoryCache _cache;
        private readonly IUserData _userData;
        private readonly IMongoCollection<SuggestionModel> _suggestions;
        private const string CacheName = "suggestionData";

        public MongoSuggestionData(IDbConnection db, IMemoryCache cache, IUserData userData)
        {
            _db = db;
            _cache = cache;
            _userData = userData;
            _cache = cache;
            _suggestions = db.SuggestionCollection;
        }

        public async Task<List<SuggestionModel>> GetAllSuggestion()
        {
            var output = _cache.Get<List<SuggestionModel>>(CacheName);
            if (output == null)
            {
                var results = await _suggestions.FindAsync(s => s.IsArchived == false);
                output = results.ToList();
                _cache.Set(CacheName, output, TimeSpan.FromDays(1));
            }

            return output;
        }
    }
}
