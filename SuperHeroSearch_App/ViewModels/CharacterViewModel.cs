using System.Collections.Generic;

namespace SuperHeroSearch_App.ViewModels
{
    public class CharacterViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public CharacterPowerstats Powerstats { get; set; }
        public CharacterBiography Biography { get; set; }
        public CharacterAppearance Appearance { get; set; }
        public CharacterWork Work { get; set; }
        public CharacterConnections Connections { get; set; }

        public class CharacterPowerstats
        {
            public string Intelligence { get; set; }
            public string Strength { get; set; }
            public string Speed { get; set; }
            public string Durability { get; set; }
            public string Power { get; set; }
            public string Combat { get; set; }
        }

        public class CharacterBiography
        {
            public string Fullname { get; set; }
            public string Alteregos { get; set; }
            public IEnumerable<string> Aliases { get; set; }
            public string PlaceOfBirth { get; set; }
            public string FirstAppearance { get; set; }
            public string Publisher { get; set; }
            public string Alignment { get; set; }
        }

        public class CharacterAppearance
        {
            public string Gender { get; set; }
            public string Race { get; set; }
            public IEnumerable<string> Height { get; set; }
            public IEnumerable<string> Weight { get; set; }
            public string EyeColor { get; set; }
            public string HairColor { get; set; }
        }

        public class CharacterWork
        {
            public string Occupation { get; set; }
            public string Base { get; set; }
        }

        public class CharacterConnections
        {
            public string GroupAffiliation { get; set; }
            public string Relatives { get; set; }
        }
    }
}
