using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.MiniGame
{
    /// <summary>
    /// This class use many common properties of GameLangModel, which 
    /// we could used some interfaces
    /// However, due to structure and to ease the JSON objects we sent back to the user
    /// we decided to repeat the title and description properties
    /// </summary>
    public class GameResponseModel : GameModel
    {
        private string _title;
        private string _description;
        private bool _unlockedByTheUser;

        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public bool UnlockedByTheUser { get => _unlockedByTheUser; set => _unlockedByTheUser = value; }

    }
}
