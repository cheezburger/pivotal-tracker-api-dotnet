using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using PivotalTrackerAPI.Domain.Services;
using PivotalTrackerAPI.Util;

namespace PivotalTrackerAPI.Domain.Model
{
    [XmlRoot("note")]
    public class PivotalNote
    {
        public PivotalNote() { }

        #region Private Properties

        private string _creationDateString;
        private DateTime _creationDate;

        #endregion

        #region Public Properties

        /// <summary>
        /// The id of the note
        /// </summary>
        [XmlElement("id", IsNullable = true)]
        public Nullable<int> NoteId { get; set; }

        /// <summary>
        /// The content of the note
        /// </summary>
        [XmlElement("text")]
        public string Text { get; set; }

        /// <summary>
        /// The author of the note
        /// </summary>
        [XmlElement("author")]
        public string Author { get; set; }

        /// <summary>
        /// The string representing the date the note was created (as returned by Pivotal).  See CreationDate for the value
        /// </summary>
        [XmlElement("noted_at", IsNullable = true)]
        public string CreationDateString
        {
            get
            {
                return _creationDateString;
            }
            set
            {
                _creationDateString = value;
                if (value != null && value.Length > 4)
                {
                    try
                    {
                        CreationDate = DateTime.ParseExact(value.Substring(0, value.Length - 4), "yyyy/MM/dd hh:mm:ss", new System.Globalization.CultureInfo("en-US", true), System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    }
                    catch
                    {
                        CreationDate = new DateTime();
                    }
                }
                else
                    CreationDate = new DateTime();
            }
        }

        #endregion

        #region Non-Pivotal Properties (helpers)

        /// <summary>
        /// The date the task was created
        /// </summary>
        [XmlIgnore]
        public DateTime CreationDate
        {
            get
            {
                return _creationDate;
            }
            set
            {
                _creationDate = value;
                _creationDateString = _creationDate.ToString("yyyy/MM/dd hh:mm:ss") + " UTC";
            }
        }

        #endregion

        public static IList<PivotalNote> FetchNotes(PivotalUser user, int projectId, int storyId, string filter)
        {
            var url = String.Format("{0}/projects/{1}/stories/{2}/notes?token={3}", PivotalService.BaseUrl, projectId, storyId, user.ApiToken);
            if (!string.IsNullOrEmpty(filter))
                url += "&" + filter;
            var xmlDoc = PivotalService.GetData(url);
            var noteList = SerializationHelper.DeserializeFromXmlDocument<PivotalNoteList>(xmlDoc);
            return noteList.Notes;
        }
    }
}
