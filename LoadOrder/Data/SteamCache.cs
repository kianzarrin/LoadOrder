namespace LoadOrderTool.Data {
    using CO.IO;
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using LoadOrderTool.Helpers;
    using System.Linq;
    using System.Collections.Generic;
    using LoadOrderTool.Util;

    public class SteamCache {
        public class Persona {
            public ulong ID;
            public string Name;
        }

        public enum DownloadStatus {
            None,
            OK,
            OutOfDate,
            NotDownloaded,
            PartiallyDownloaded,
            Removed,
            Unknown,
        }

        public class Item {
            public string Path; // included path
            public string Name;
            public string Description;
            public string Author;
            public ulong AuthorID;
            public DateTime DateUpdatedUTC;
            public DownloadStatus Status;
            public string DownloadFailureReason;

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
            }
        }

        public class Mod : Item {
            // public string AssemblyName;
        }

        public class Asset : Item {
            public string[] Tags;

            public override void Read(SteamUtil.PublishedFileDTO dto) {
                base.Read(dto);
                Tags = dto.Tags;
            }
        }

        // do not use! only for persistancy
        public Mod[] Mods = new Mod[0];
        public Asset[] Assets = new Asset[0];
        public Persona[] People = new Persona[0];

        private Hashtable<string, Mod> modTable_ = new Hashtable<string, Mod>(1000);
        private Hashtable<string, Asset> assetTable_ = new Hashtable<string, Asset>(10000);
        private Hashtable<ulong, Persona> peopleTable_ = new Hashtable<ulong, Persona>(1000);
        private System.Collections.Hashtable x = new System.Collections.Hashtable();



        const string FILE_NAME = "SteamCache.xml";
        static string DIR => DataLocation.LocalLOMData;
        static string FilePath => Path.Combine(DIR, FILE_NAME);

        internal void Serialize() {
            XmlSerializer ser = new XmlSerializer(typeof(SteamCache));
            using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write)) {
                Mods = modTable_.Values.ToArray();
                Assets = assetTable_.Values.ToArray();
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
            modTable_ = new Hashtable<string, Mod>(Mods.ToDictionary(mod => mod.Path));
            assetTable_ = new Hashtable<string, Asset>(Assets.ToDictionary(asset => asset.Path));
            peopleTable_ = new Hashtable<ulong, Persona>(People.ToDictionary(persona => persona.ID));
        }

        public Mod GetMod(string includedPath) => modTable_[includedPath];
        public Mod GetOrCreateMod(string includedPath) {
            var ret = modTable_[includedPath];
            if(ret == null){
                ret = modTable_[includedPath] = new Mod { Path = includedPath };
            }
            return ret;
        }

        public Asset GetAsset(string includedPath) => assetTable_[includedPath];
        public Asset GetOrCreateAsset(string includedPath) {
            var ret = assetTable_[includedPath];
            if (ret == null) {
                ret = assetTable_[includedPath] = new Asset { Path = includedPath };
            }
            return ret;
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
