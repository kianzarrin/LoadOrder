namespace LoadOrderTool.Data {
    using CO.IO;
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using LoadOrderTool.Helpers;
    using System.Linq;
    using System.Collections.Generic;
    using LoadOrderTool.Util;
    using CO.PlatformServices;

    public static class DownloadStatusExtension {
        public static bool IsBroken(this SteamCache.DownloadStatus status) {
            return (int)status > 2;
        }
    }

    public class SteamCache {
        public class Persona {
            public ulong ID;
            public string Name;
        }

        public enum DownloadStatus {
            None = 0,
            OK = 1,
            Unknown = 2,
            OutOfDate,
            NotDownloaded,
            PartiallyDownloaded,
            Removed,
        }

        public class Item {
            internal PublishedFileId PublishedFileId {
                get => new PublishedFileId(ID);
                set => ID = value.AsUInt64;
            }
            public ulong ID;
            public string Name;
            public string Description;
            public string Author;
            public ulong AuthorID;
            public DateTime DateUpdatedUTC;
            public DownloadStatus Status;
            public string DownloadFailureReason;
                        public string[] Tags;

            public void SetAuthor(ulong authorID) {
                AuthorID = authorID;
                UpdateAuthor();
            }

            public void UpdateAuthor() {
                var person = ConfigWrapper.instance.SteamCache.GetPersona(AuthorID);
                Author = person?.Name;
            }

            public virtual void Read(SteamUtil.PublishedFileDTO dto) {
                Name = dto.Title;
                DateUpdatedUTC = dto.UpdatedUTC;
                SetAuthor(dto.AuthorID);
                Status = ContentUtil.IsUGCUpToDate(dto, out DownloadFailureReason);
                Tags = dto.Tags.Select(tag => tag switch {
                        "Road" => "Network",
                        _ => tag,
                    })
                    .ToArray();
            }
        }


        // do not use! only for persistancy
        public Item[] Items = new Item[0];
        public Persona[] People = new Persona[0];

        private Hashtable<PublishedFileId, Item> itemTable_ = new Hashtable<PublishedFileId, Item>(15000);
        private Hashtable<ulong, Persona> peopleTable_ = new Hashtable<ulong, Persona>(1000);

        const string FILE_NAME = "SteamCache.xml";
        static string DIR => DataLocation.LocalLOMData;
        static string FilePath => Path.Combine(DIR, FILE_NAME);

        internal void Serialize() {
            XmlSerializer ser = new XmlSerializer(typeof(SteamCache));
            using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write)) {
                Items = itemTable_.Values.ToArray();
                People = peopleTable_.Values.ToArray();

                ser.Serialize(fs, this);
            }
        }

        internal static SteamCache Deserialize() {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(SteamCache));
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read)) {
                    var ret = ser.Deserialize(fs) as SteamCache;
                    ret.BuildIndeces();
                    return ret;
                }
            } catch {
                return null;
            }
        }

        private void BuildIndeces() {
            itemTable_ = new Hashtable<PublishedFileId, Item>(Items.ToDictionary(item => item.PublishedFileId));
            peopleTable_ = new Hashtable<ulong, Persona>(People.ToDictionary(persona => persona.ID));
        }

        public Item GetItem(PublishedFileId id) => itemTable_[id];
        public Item GetOrCreateItem(PublishedFileId id) {
            Assertion.Assert(id.IsValid, $"{id}.IsValid");
            return itemTable_[id] ??= new Item { PublishedFileId = id };
        }

        public Persona GetPersona(ulong ID) => peopleTable_[ID];
        public Persona AddPerson(ulong id, string name) {
            var p = peopleTable_[id];
            if (p != null) {
                p.Name = name;
            } else {
                p = peopleTable_[id] = new Persona { ID = id, Name = name };
            }
            return p;
        }
    }
}
