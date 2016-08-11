using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockedCounter.Storage
{
    public class BaseRepository<T>
    {
        private string _fileName;
        protected IList<T> _collection;

        public virtual IEnumerable<T> Elements
        {
            get
            {
                return _collection;
            }
        }

        public async Task Add(T element)
        {
            _collection.Add(element);
            await SaveState();
        }

        protected BaseRepository() { } //Tests

        protected BaseRepository(string fileName)
        {
            _fileName = fileName;
        }

        internal async Task Initialize()
        {
            if (File.Exists(_fileName))
            {
                using (var reader = File.OpenText(_fileName))
                {
                    var content = await reader.ReadToEndAsync();
                    _collection = JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            else
            {
                _collection = new List<T>();
            }
        }

        protected async Task SaveState()
        {
            string content = JsonConvert.SerializeObject(_collection);
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            using (var writer = File.OpenWrite(_fileName))
            {
                await writer.WriteAsync(contentBytes, 0, contentBytes.Length);
            }
        }
    }
}
