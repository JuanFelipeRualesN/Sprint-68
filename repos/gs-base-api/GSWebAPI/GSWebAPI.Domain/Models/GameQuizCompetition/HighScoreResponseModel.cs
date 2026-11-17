using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition
{
    /// <summary>
    /// Get the high score for the top 100
    /// </summary>
    public class HighScoreResponseModel : HighScoreModel
    {
        private string _fullName;
        private string _username;
        private string _schoolName;
        private string _city;
        private string _country;
        private string _state;

        public string FullName { get => _fullName; set => _fullName = value; }
        public string Username { get => _username; set => _username = value; }
        public string SchoolName { get => _schoolName; set => _schoolName = value; }
        public string City { get => _city; set => _city = value; }
        public string State { get => _state; set => _state = value; }
        public string Country { get => _country; set => _country = value; }
    }
}
