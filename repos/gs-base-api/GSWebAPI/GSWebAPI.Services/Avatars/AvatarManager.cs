using System;
using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.Avatar;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Avatars;
using GSWebAPI.GSWebAPI.Services.GameUserRewards;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.CustomClasses;

namespace GSWebAPI.GSWebAPI.Services.Avatars
{
    /// <summary>
    /// Class to handle avatar details as generating a random avatar, reading, creating, etc.
    /// </summary>
    public class AvatarManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string. (Writer).</param>
        /// <param name="connectionStringRead">The connection string (Reader).</param>
        /// <param name="langCode">The language code.</param>
        public AvatarManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Returns the rival's avatar. If it does not exist, it generates it.
        /// </summary>
        /// <param name="userId">The user id of the rival.</param>
        /// <param name="sex">The sex of the rival.</param>
        /// <returns>Return an user (rival) avatar object.</returns>
        public UserAvatarModel GetRivalAvatar(string userId, string sex)
        {
            // If doesn't exists, create the avatar
            UserAvatarModel userAvatarModel = new UserAvatarModel
            {
                UserId = userId,
            };

            List<UserAvatarModel> userAvatarModelList = new UserAvatarDAO().Read(_connectionStringRead, userAvatarModel, true, true);
            // If the user (rival) has an avatar, return the user's avatar.
            if (userAvatarModelList.Count > 0)
            {
                return userAvatarModelList[0];
            }
            else
            {
                // Generate a random avatar if does not exists.
                userAvatarModel = GenerateRandomAvatar(userId, sex);

                // Insert the avatar
                InsertUserAvatar(userAvatarModel, userId);
                return userAvatarModel;
            }
        }

        /// <summary>
        /// Returns the user's avatar if any. Else, return null.
        /// </summary>
        /// <param name="userId">The user id to find.</param>
        /// <returns>Returns the user's avatar object if exists. Else returns null.</returns>
        public UserAvatarModel GetUserAvatar(string userId)
        {
            List<UserAvatarModel> userAvatarModelList = new UserAvatarDAO().Read(_connectionStringRead, new UserAvatarModel
            {
                UserId = userId,
            }, true, true);
            if (userAvatarModelList == null || userAvatarModelList.Count == 0)
            {
                return new UserAvatarModel();
            }
            else
            {
                if (userAvatarModelList[0].UserId == userId)
                {
                    return userAvatarModelList[0];
                }
                else
                {
                    return new UserAvatarModel();
                }
            }
        }

        /// <summary>
        /// Inserts the user's avatar.
        /// </summary>
        /// <param name="userAvatarModel">User's avatar model to insert.</param>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the user avatar model with transaction details.</returns>
        public UserAvatarModel InsertUserAvatar(UserAvatarModel userAvatarModel, string userId)
        {
            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

            // Set to 0 the parts that are optional.
            if (string.IsNullOrWhiteSpace(userAvatarModel.BackAccesory))
            {
                userAvatarModel.BackAccesory = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Eyebrows))
            {
                userAvatarModel.Eyebrows = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.FacePaint))
            {
                userAvatarModel.FacePaint = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Glasses))
            {
                userAvatarModel.Glasses = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Hair))
            {
                userAvatarModel.Hair = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Hat))
            {
                userAvatarModel.Hat = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Jacket))
            {
                userAvatarModel.Jacket = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Mask))
            {
                userAvatarModel.Mask = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Mouth))
            {
                userAvatarModel.Mouth = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Nose))
            {
                userAvatarModel.Nose = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Pants))
            {
                userAvatarModel.Pants = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Shirt))
            {
                userAvatarModel.Shirt = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Shoes))
            {
                userAvatarModel.Shoes = "0";
            }

            if (string.IsNullOrWhiteSpace(userAvatarModel.Socks))
            {
                userAvatarModel.Socks = "0";
            }

            // First we check if the user avatar exists 
            UserAvatarDAO userAvatarDAO = new UserAvatarDAO();
            userAvatarModel.UserId = userId;

            UserAvatarModel userAvatarResponseModel = new UserAvatarModel();

            // if not exists, insert.
            List<UserAvatarModel> userAvatarCheckIfExistList = userAvatarDAO.Read(_connectionStringRead, new UserAvatarModel() { UserId = userId }, true, true);
            if (userAvatarCheckIfExistList == null || userAvatarCheckIfExistList.Count == 0)
            {
                userAvatarResponseModel = userAvatarDAO.Create(_connectionString, userAvatarModel)[0];
            }
            else
            {
                // Update
                userAvatarResponseModel = userAvatarModel;
                userAvatarResponseModel.UserId = userId;
                userAvatarDAO.Update(_connectionString, userAvatarModel, new UserAvatarModel
                {
                    UserId = userId,
                });
            }

            userAvatarResponseModel.Valid = true;
            userAvatarResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_successful_trans"]));
            return userAvatarResponseModel;
        }

        /// <summary>
        /// Returns the user's unlocked items for the avatar.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Return a list of UserUnlockedItemModel with user unlocked item details.</returns>
        public List<UserUnlockedItemModel> GetUserUnlockedItems(string userId)
        {
            return new UserUnlockedItemDAO().Read(_connectionStringRead, new UserUnlockedItemModel
            {
                UserId = userId,
            }, false, false);
        }

        /// <summary>
        /// Returns all the parts of the avatar. If you want to filter by sex, then send the avatar item model specifing the sex property. The same applies to the other properties. 
        /// </summary>
        /// <param name="avatarItemModelToFind">The avatar model with the filters to apply.</param>
        /// <returns>Returns a list of all avatar items.</returns>
        public List<AvatarItemModel> GetAllAvatarItems(AvatarItemModel avatarItemModelToFind)
        {
            return new AvatarItemDAO().Read(_connectionStringRead, avatarItemModelToFind, false, false);
        }

        /// <summary>
        /// Returns all the parts of the avatar with the option evaluating if the user unlocked the part or not. If you want to filter by sex, then send the avatar item model specifing the sex property. The same applies to the other properties. 
        /// </summary>
        /// <param name="avatarItemModelToFind">The avatar item model with the filters to apply.</param>
        /// <param name="userId">The user's id to get the unlocked items.</param>
        /// <returns>Returns a list of the avatar items with the option evaluating if it was unlocked by the user or not.</returns>
        public List<AvatarItemResponseModel> GetAllAvatarItems(AvatarItemModel avatarItemModelToFind, string userId)
        {
            List<AvatarItemResponseModel> avatarItemResponseModelList = new List<AvatarItemResponseModel>();
            List<AvatarItemModel> avatarItemModelList = new List<AvatarItemModel>();
            AvatarItemDAO avatarItemDAO = new AvatarItemDAO();
            avatarItemModelList = avatarItemDAO.Read(_connectionStringRead, avatarItemModelToFind, false, false);

            UserUnlockedItemModel userUnlockedItemModel = new UserUnlockedItemModel
            {
                UserId = userId,
            };
            UserUnlockedItemDAO userUnlockedItemDAO = new UserUnlockedItemDAO();
            List<UserUnlockedItemModel> userUnlockedItemModelList = new List<UserUnlockedItemModel>();

            userUnlockedItemModelList = userUnlockedItemDAO.Read(_connectionStringRead, userUnlockedItemModel, false, false);

            // Now Check the response
            if (userUnlockedItemModelList.Count == 0)
            {
                for (int i = 0; i < avatarItemModelList.Count; i++)
                {
                    AvatarItemResponseModel avatarItemResponseModel = new AvatarItemResponseModel
                    {
                        Code = avatarItemModelList[i].Code,
                        Gender = avatarItemModelList[i].Gender,
                        Name = avatarItemModelList[i].Name,
                        Part = avatarItemModelList[i].Part,
                        UnlockedByTheUser = false,
                        UnlockType = avatarItemModelList[i].UnlockType,
                        UnlockValue = avatarItemModelList[i].UnlockValue,
                        Url = avatarItemModelList[i].Url,
                    };

                    // If it doesn't cost anything
                    if (avatarItemResponseModel.UnlockValue == "0")
                    {
                        avatarItemResponseModel.UnlockedByTheUser = true;
                    }

                    avatarItemResponseModelList.Add(avatarItemResponseModel);

                }
            }
            else
            {
                for (int i = 0; i < avatarItemModelList.Count; i++)
                {
                    AvatarItemResponseModel avatarItemResponseModel = new AvatarItemResponseModel
                    {
                        Code = avatarItemModelList[i].Code,
                        Gender = avatarItemModelList[i].Gender,
                        Name = avatarItemModelList[i].Name,
                        Part = avatarItemModelList[i].Part,
                        UnlockType = avatarItemModelList[i].UnlockType,
                        UnlockValue = avatarItemModelList[i].UnlockValue,
                        Url = avatarItemModelList[i].Url,
                        UnlockedByTheUser = false,
                    };

                    // If it doesn't cost anything
                    if (avatarItemResponseModel.UnlockValue == "0")
                    {
                        avatarItemResponseModel.UnlockedByTheUser = true;
                    }
                    else
                    {
                        UserUnlockedItemModel userUnlockedItemModelToCheck = new UserUnlockedItemModel();
                        userUnlockedItemModelToCheck = userUnlockedItemModelList.Find(x => x.AvatarItemCode == avatarItemResponseModel.Code && x.AvatarPart == avatarItemResponseModel.Part);
                        if (userUnlockedItemModelToCheck != null)
                        {
                            avatarItemResponseModel.UnlockedByTheUser = true;
                        }
                    }
                    avatarItemResponseModelList.Add(avatarItemResponseModel);
                }
            }

            return avatarItemResponseModelList;

        }

        /// <summary>
        /// Insert the unlocked part for the user if applies.
        /// </summary>
        /// <param name="itemCode">The item code to unlock.</param>
        /// <param name="part">The item part name (if jacket, glasses, etc).</param>
        /// <param name="userId">The user's id.</param>
        /// <returns></returns>
        public UserUnlockedItemModel InsertUserUnlockedPart(string itemCode, string part, string userId)
        {
            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);
            UserUnlockedItemModel userUnlockedItemResponseModel = new UserUnlockedItemModel()
            {
                Valid = true,
            };

            // First, we check if the user has the item already.
            UserUnlockedItemDAO userUnlockedItemDAO = new UserUnlockedItemDAO();
            UserUnlockedItemModel userUnlockedItemModelToCheck = new UserUnlockedItemModel
            {
                UserId = userId,
                AvatarItemCode = itemCode,
                AvatarPart = part
            };
            List<UserUnlockedItemModel> userUnlockedItemModelsList = userUnlockedItemDAO.Read(_connectionStringRead, userUnlockedItemModelToCheck, false, false);
            if (userUnlockedItemModelsList.Count > 0)
            {
                userUnlockedItemResponseModel.Valid = false;
                userUnlockedItemResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_item_already_exists_in_inventory"]));
                return userUnlockedItemResponseModel;
            }

            // Then, we check if the user has funds
            GameUserRewardsService gameUserRewardsManager = new GameUserRewardsService(_connectionString, _connectionStringRead, _langCode);
            UserRewardModel userRewardModel = gameUserRewardsManager.GetUserRewards(userId);

            if (userRewardModel == null)
            {
                userUnlockedItemResponseModel.Valid = false;
                userUnlockedItemResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_not_enough_funds"]));
                return userUnlockedItemResponseModel;
            }

            // Now, we check if the item exists.
            AvatarItemModel avatarItemModelToCheck = new AvatarItemModel
            {
                Code = itemCode,
                Part = part,
            };
            AvatarItemDAO avatarItemDAO = new AvatarItemDAO();
            List<AvatarItemModel> avatarItemReturnedModelList = avatarItemDAO.Read(_connectionStringRead, avatarItemModelToCheck, false, false);

            // If the item doesn't exists, then return error
            if (avatarItemReturnedModelList.Count == 0)
            {
                userUnlockedItemResponseModel.Valid = false;
                userUnlockedItemResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_item_doesnt_exists"]));
                return userUnlockedItemResponseModel;
            }

            // Now check the if the user has the coins to buy it or the diamonds
            if (avatarItemReturnedModelList[0].UnlockType == AppConfiguration.Configuration["AppSettings:av_diamonds_code"])
            {
                if (int.Parse(userRewardModel.DiamondsTotal) < int.Parse(avatarItemReturnedModelList[0].UnlockValue))
                {
                    userUnlockedItemResponseModel.Valid = false;
                    userUnlockedItemResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_not_enough_funds"]));
                    return userUnlockedItemResponseModel;
                }
                else
                {
                    // Unlock the item 
                    UserUnlockedItemModel userUnlockedItem = new UserUnlockedItemModel
                    {
                        AvatarItemCode = itemCode,
                        AvatarPart = part,
                        UserId = userId,
                    };
                    userUnlockedItemDAO.Create(_connectionString, userUnlockedItem);

                    // Remove the diamonds 
                    gameUserRewardsManager.RemoveDiamonds(userId, int.Parse(avatarItemReturnedModelList[0].UnlockValue));
                    userUnlockedItemResponseModel.Valid = true;
                    userUnlockedItemResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_successful_trans"]));
                    return userUnlockedItemResponseModel;
                }
            }
            else
            {
                if (int.Parse(userRewardModel.CoinsTotal) < int.Parse(avatarItemReturnedModelList[0].UnlockValue))
                {
                    userUnlockedItemResponseModel.Valid = false;
                    userUnlockedItemResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_not_enough_funds"]));
                    return userUnlockedItemResponseModel;
                }
                else
                {
                    // Unlock the item 
                    UserUnlockedItemModel userUnlockedItem = new UserUnlockedItemModel
                    {
                        AvatarItemCode = itemCode,
                        AvatarPart = part,
                        UserId = userId,
                    };
                    userUnlockedItemResponseModel = userUnlockedItemDAO.Create(_connectionString, userUnlockedItem)[0];

                    // Remove the coins.
                    gameUserRewardsManager.RemoveCoins(userId, int.Parse(avatarItemReturnedModelList[0].UnlockValue));
                    userUnlockedItemResponseModel.Valid = true;
                    userUnlockedItemResponseModel.MessageList.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:av_message_code_successful_trans"]));
                    return userUnlockedItemResponseModel;
                }
            }
        }

        /// <summary>
        /// Generates a random avatar.
        /// </summary>
        /// <param name="userId">The id of the user to put it in the avatar model.</param>
        /// <param name="sex">The user's sex.</param>
        /// <returns>Return an user avatar object.</returns>
        public UserAvatarModel GenerateRandomAvatar(string userId, string sex)
        {
            // Generate the avatar.
            // First we select the avatar parts by sex.
            List<AvatarItemModel> avatarItemModelListBySex = GetAllAvatarItems(new AvatarItemModel
            {
                Gender = sex,
            });

            // Now retrieve each part.
            List<AvatarItemModel> baseList = new List<AvatarItemModel>();
            List<AvatarItemModel> eyebrowsList = new List<AvatarItemModel>();
            List<AvatarItemModel> eyesList = new List<AvatarItemModel>();
            List<AvatarItemModel> hairList = new List<AvatarItemModel>();
            List<AvatarItemModel> hatList = new List<AvatarItemModel>();
            List<AvatarItemModel> mouthList = new List<AvatarItemModel>();
            List<AvatarItemModel> noseList = new List<AvatarItemModel>();
            List<AvatarItemModel> pantList = new List<AvatarItemModel>();
            List<AvatarItemModel> shirtList = new List<AvatarItemModel>();
            List<AvatarItemModel> shoesList = new List<AvatarItemModel>();

            for (int i = 0; i < avatarItemModelListBySex.Count; i++)
            {

                switch (avatarItemModelListBySex[i].Part)
                {
                    case "base":
                        baseList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "eyebrows":
                        eyebrowsList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "eyes":
                        eyesList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "hair":
                        hairList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "hat":
                        hatList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "mouth":
                        mouthList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "nose":
                        noseList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "pants":
                        pantList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "shirt":
                        shirtList.Add(avatarItemModelListBySex[i]);
                        break;
                    case "shoes":
                        shoesList.Add(avatarItemModelListBySex[i]);
                        break;
                }
            }

            // Generate a random skin color.
            string[] hexSkinColos = { "ffe5c1", "ffebdd", "ffe0a8", "d8a17f", "5e4434", "bc8c6f", "ffefe5" };
            Random random = new Random();
            var color = string.Format("{0:X6}", random.Next(0x1000000));

            // Now get a random item of each part.
            UserAvatarModel userAvatarRandom = new UserAvatarModel
            {
                UserId = userId,
                Base = baseList[0].Code,
                BaseHex = hexSkinColos[random.Next(0, hexSkinColos.Length)],
                Sex = sex,
                Eyebrows = eyebrowsList[random.Next(0, eyebrowsList.Count)].Code,
                Eyes = eyesList[random.Next(0, eyesList.Count)].Code,
                Hair = hairList[random.Next(0, hairList.Count)].Code,
                HairHex = color.ToString(),
                Hat = hatList[random.Next(0, hatList.Count)].Code,
                Mouth = mouthList[random.Next(0, mouthList.Count)].Code,
                Nose = noseList[random.Next(0, noseList.Count)].Code,
                Pants = pantList[random.Next(0, pantList.Count)].Code,
                Shirt = shirtList[random.Next(0, shirtList.Count)].Code,
                Shoes = shoesList[random.Next(0, shoesList.Count)].Code,
            };

            return userAvatarRandom;
        }
    }
}
