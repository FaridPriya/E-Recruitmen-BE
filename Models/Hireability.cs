using Newtonsoft.Json;
using static ERecruitmentBE.Models.Education;

namespace ERecruitmentBE.Models
{
    public class RootHireability
    {
        public Hireability hireability { get; set; }
    }
    public class Hireability
    {
        public string status { get; set; }
        public ExtractedData extracted_data { get; set; }
    }

    public class ExtractedData
    {
        public PersonalInfos personal_infos { get; set; }
        public Education education { get; set; }
        public WorkExperience work_experience { get; set; }
        //public List<Language> languages { get; set; }
        public List<Skill> skills { get; set; }
        //public List<object> certifications { get; set; }
        //public List<object> courses { get; set; }
        //public List<object> publications { get; set; }
        //public List<object> interests { get; set; }
    }

    public class Skill
    {
        public string name { get; set; }
        public string type { get; set; }
    }

    public class WorkExperience
    {
        public List<Entry> entries { get; set; }
    }

    public class Education
    {
        public List<Entry> entries { get; set; }

        public class Entry
        {
            public string title { get; set; }
            public object start_date { get; set; }
            public string end_date { get; set; }
            public string description { get; set; }
            public string company { get; set; }
            public string industry { get; set; }
        }
    }

    public class PersonalInfos
    {
        public PersonalInfoName name { get; set; }
        public PersonalInfoAddress address { get; set; }
        public List<string> phones { get; set; }
        public List<string> mails { get; set; }

        public class PersonalInfoName
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string raw_name { get; set; }
        }

        public class PersonalInfoAddress
        {
            public string formatted_location { get; set; }
            public string region { get; set; }
            public string country { get; set; }
            public string street { get; set; }
        }
    }
}
