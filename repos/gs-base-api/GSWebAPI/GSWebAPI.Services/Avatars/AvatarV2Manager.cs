using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GSWebAPI.GSWebAPI.Domain.Models.AvatarV2;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.AvatarV2;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;
using GSWebAPI.GSWebAPI.Services.GameUserRewards;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.CustomClasses;

namespace GSWebAPI.GSWebAPI.Services.Avatars
{
    /// <summary>
    /// To handle the 3D version (V2) of the avatar.
    /// </summary>
    public class AvatarV2Manager
    {
        private readonly string _connectionStringRead;
        private readonly string _connectionStringWrite;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="connectionStringWrite">The connection string (write).</param>
        /// <param name="langCode">The language code.</param>
        public AvatarV2Manager(string connectionStringRead, string connectionStringWrite, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _connectionStringWrite = connectionStringWrite;
            _langCode = langCode;
        }

        /// <summary>
        /// To buy item part.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="gender">The selected gender.</param>
        /// <param name="part">The selected part.</param>
        /// <param name="partIndex">The part index.</param>
        /// <returns>Returns the response with transaction details.</returns>
        public ResponseModel BuyItemPart(string userId, string gender, string part, int partIndex)
        {
            var response = new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
            };
            // First, read the part details. 
            var partDetails = new UserV2AvatarPartsCostDAO().Read(this._connectionStringRead,
                new UserV2AvatarPartsCostModel()
                {
                    Part = part,
                    PartIndex = partIndex.ToString(),
                }, true, true);

            if (partDetails == null || partDetails.Count() == 0)
            {
                // Return that the part does not exists.
                response.Valid = false;
                response.Messages.Add(new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_item_doesnt_exists"]));
                return response;
            }

            // Check if the user has the part already. 
            var userUnlockedParts = new UserV2AvatarUnlockDAO().Read(this._connectionStringRead,
                new UserV2AvatarUnlockModel()
                {
                    UserId = userId,
                    Part = part,
                    Gender = gender,
                    PartIndex = partIndex.ToString()
                }, true, true);

            if (userUnlockedParts != null && userUnlockedParts.Count > 0)
            {
                // Return that the user has unlocked the part already.
                response.Valid = false;
                response.Messages.Add(new MessageManager(this._connectionStringRead, this._langCode).
                    GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_item_already_exists_in_inventory"]));
                return response;
            }

            // Now check if the user has sufficient funds. 
            // Then, we check if the user has funds
            var gameUserRewardsManager = new GameUserRewardsService(this._connectionStringWrite,
                this._connectionStringRead, this._langCode);
            var userRewardModel = gameUserRewardsManager.GetUserRewards(userId);

            if (userRewardModel == null)
            {
                response.Valid = false;
                response.Messages.Add(new MessageManager(this._connectionStringRead, this._langCode)
                    .GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_not_enough_funds"]));
                return response;
            }

            if (int.Parse(userRewardModel.CoinsTotal) < int.Parse(partDetails[0].Cost))
            {
                response.Valid = false;
                response.Messages.Add(new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_not_enough_funds"]));
                return response;
            }

            new UserV2AvatarUnlockDAO().Create(this._connectionStringWrite, new UserV2AvatarUnlockModel()
            {
                ModifiedBy = userId,
                UserId = userId,
                Part = part,
                PartIndex = partIndex.ToString(),
                Gender = gender
            });

            // Remove the coins.
            gameUserRewardsManager.RemoveCoins(userId, int.Parse(partDetails[0].Cost));
            response.Valid = true;
            response.Messages.Add(new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_successful_trans"]));
            return response;
        }

        /// <summary>
        /// To get user avatar locked/unlocked parts.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="gender">The gender.</param>
        /// <returns>Returns the avatar v2 items model.</returns>
        public AvatarV2ItemsModel GetUserAvatarLockedAndUnlockedParts(string userId, string gender)
        {
            // First, read overall parts. 
            var overallParts = new UserV2AvatarPartsCostDAO().Read(this._connectionStringRead,
                new UserV2AvatarPartsCostModel(), false, false);

            // Read the parts that the user has unlocked.
            var userUnlockedParts = new UserV2AvatarUnlockDAO().Read(this._connectionStringRead,
                new UserV2AvatarUnlockModel()
                {
                    UserId = userId
                }, false, false);

            var avatarV2ItemsModelToReturn = new AvatarV2ItemsModel
            {
                Gender = gender,
                Parts = new AvatarV2Parts()
                {
                    Hairs = new List<AvatarV2PartDetails>(),
                    Legs = new List<AvatarV2PartDetails>(),
                    Chests = new List<AvatarV2PartDetails>(),
                    Feet = new List<AvatarV2PartDetails>()
                }
            };

            foreach (var overallPart in overallParts)
            {
                var avatarV2PartDetails = new AvatarV2PartDetails()
                {
                    Cost = int.Parse(overallPart.Cost),
                    PartIndex = int.Parse(overallPart.PartIndex),
                };

                if (overallPart.Cost == "0")
                {
                    avatarV2PartDetails.Unlocked = true;
                }
                else
                {
                    var userUnlockedPart = userUnlockedParts?.Find(
                        x => x.Part == overallPart.Part &&
                             x.PartIndex == overallPart.PartIndex &&
                             x.Gender == gender);
                    avatarV2PartDetails.Unlocked = userUnlockedPart != null;
                }

                switch (overallPart.Part)
                {
                    case "Hair":
                        // Check if the user unlocked.
                        avatarV2ItemsModelToReturn.Parts.Hairs.Add(avatarV2PartDetails);
                        break;
                    case "Chest":
                        avatarV2ItemsModelToReturn.Parts.Chests.Add(avatarV2PartDetails);
                        break;
                    case "Legs":
                        avatarV2ItemsModelToReturn.Parts.Legs.Add(avatarV2PartDetails);
                        break;
                    case "Feet":
                        avatarV2ItemsModelToReturn.Parts.Feet.Add(avatarV2PartDetails);
                        break;
                }
            }

            return avatarV2ItemsModelToReturn;
        }

        /// <summary>
        /// To get the user 3D avatar. If the user has not set the avatar, it will return the default avatar.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="isForRival">If it is for rival, it will return a null if empty.</param>
        /// <returns>Returns the Avatar JSON as expected in Unity 3D version.</returns>
        public AvatarV2JsonModel GetUser3dAvatar(string userId, bool isForRival = false)
        {
            // Read the user avatar v.2 
            var userAvatar = new UserV2AvatarDAO().Read(this._connectionStringRead,
                new UserV2AvatarModel()
                {
                    UserId = userId,
                }, true, true);

            if (userAvatar == null || !userAvatar.Any())
            {
                if (isForRival)
                {
                    return null;
                }
                // Return the default.
                var avatarJson = AppConfiguration.Configuration["AppSettings:avatar_3d_default"];
                return JsonConvert.DeserializeObject<AvatarV2JsonModel>(avatarJson);
            }

            // Convert to userAvatar[0].Avatar3dJson to AvatarV2JsonModel.
            return JsonConvert.DeserializeObject<AvatarV2JsonModel>(userAvatar[0].Avatar3dJson);
        }

        /// <summary>
        /// To create the user avatar 3D version.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="avatarV2JsonModel">The user json to save.</param>
        /// <returns>Returns the response model.</returns>
        public ResponseModel CreateOrUpdateUserV2AvatarModel(string userId, AvatarV2JsonModel avatarV2JsonModel)
        {
            var userV2AvatarDao = new UserV2AvatarDAO();
            // Check if the user has an avatar already.
            var avatarUser = userV2AvatarDao.Read(this._connectionStringRead, new UserV2AvatarModel()
            {
                UserId = userId
            }, true, true);

            // If avatarUser == null, create it.
            if (!avatarUser.Any())
            {
                userV2AvatarDao.Create(this._connectionStringWrite, new UserV2AvatarModel()
                {
                    UserId = userId,
                    Avatar3dJson = JsonConvert.SerializeObject(avatarV2JsonModel),
                    ModifiedBy = userId
                });
            }
            else
            {
                userV2AvatarDao.Update(this._connectionStringWrite, new UserV2AvatarModel()
                {
                    Avatar3dJson = JsonConvert.SerializeObject(avatarV2JsonModel),
                    ModifiedBy = userId
                }, new UserV2AvatarModel()
                {
                    UserId = userId
                });
            }

            return new ResponseModel()
            {
                Valid = true,
            };
        }
    }
}