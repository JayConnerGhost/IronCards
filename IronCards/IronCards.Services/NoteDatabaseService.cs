using System.Collections.Generic;
using System.Linq;

namespace IronCards.Services
{
    public class NoteDatabaseService : BaseDatabaseService, INotesDatabaseService
    {
        public int Insert(string title, string description, int projectId)
        {
            int id;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var notes = database.GetCollection<NoteDocument>();
                id = notes.Insert(new NoteDocument()
                {
                    Title = title,
                    Text = description,
                    ProjectId = projectId
                });
            }

            return id;
        }

        public IList<NoteDocument> GetAllForProject(int projectId)
        {
            List<NoteDocument> notes;

            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var collection = database.GetCollection<NoteDocument>();
                collection.EnsureIndex(x => x.ProjectId);
                notes = collection.Find(x => x.ProjectId == projectId).ToList();
            }

            return notes;
        }

        public string FindNoteTextByNoteId(string noteId)
        {
            var numericNoteId = int.Parse(noteId.Trim());
            var result = string.Empty;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var collection = database.GetCollection<NoteDocument>();
                collection.EnsureIndex(x => x.Id);
                result = collection.Find(x => x.Id == numericNoteId).First().Text;
            }

            return result;
        }
    }
}