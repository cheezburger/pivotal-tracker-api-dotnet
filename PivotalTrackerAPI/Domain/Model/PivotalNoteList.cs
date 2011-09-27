using System.Collections.Generic;
using System.Xml.Serialization;

namespace PivotalTrackerAPI.Domain.Model
{
    ///<summary>
    /// Container for a note list
    ///</summary>
    [XmlRoot("notes")]
    public class PivotalNoteList
    {
        ///<summary>
        /// List of notes
        ///</summary>
        [XmlElement("note")]
        public List<PivotalNote> Notes { get; set; }
    }
}
