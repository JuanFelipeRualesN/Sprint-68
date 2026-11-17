using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition
{
    public class CompetitionResponseModel
    {
        private bool _userWon;
        private bool _rivalWon;
        private bool _draw;
        private int _userTotalPoints;
        private int _rivalTotalPoints;

        public bool UserWon { get => _userWon; set => _userWon = value; }
        public bool RivalWon { get => _rivalWon; set => _rivalWon = value; }
        public bool Draw { get => _draw; set => _draw = value; }
        public int UserTotalPoints { get => _userTotalPoints; set => _userTotalPoints = value; }
        public int RivalTotalPoints { get => _rivalTotalPoints; set => _rivalTotalPoints = value; }
    }
}
