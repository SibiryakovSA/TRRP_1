using System;
using System.Collections.Generic;
using System.Text;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace TRRP_Lab1
{
    public class FriendsCRUD
    {
        VkApi api = new VkApi(null, new CaptchaResolver());

        public FriendsCRUD(string login, string pass)
        {
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = 7788641,
                Login = login,
                Password = pass,
                Settings = Settings.All
            });
        }

        public FriendsCRUD(string token)
        {

            api.Authorize(new ApiAuthParams
            {
                AccessToken = token,//"9ac0d5d0d786d6648386456fe7dc0485151ea62021f1cdde7081f06770f6d96220558fcedb03f6afaf177",
                Settings = Settings.All
            }) ;
        }

        //получить список друзей
        public VkCollection<User> GetFriends()
        {
            var friends = api.Friends.Get(new FriendsGetParams
            {
                Fields = ProfileFields.All
            });

            return friends;
        }

        //добавить в друзья
        public void AddFriend(long userId)
        {
            api.Friends.Add(userId, "", null);
        }

        //удалить из друзей
        public bool DeleteFriend(long userId)
        {
            var result = api.Friends.Delete(userId);
            return result.Success == true;
        }

        //добавляет пользователя в группы (лучшие друзья и тд)
        public bool AddUserIds(long userId, long[] Ids)
        {
            return api.Friends.Edit(userId, Ids);
        }

        //отчистить все группы пользователя по идентификатору
        public bool RemoveUserIds(long userId)
        {
            return api.Friends.Edit(userId, new long[] { });
        }

        //список групп пользователя
        public VkCollection<FriendList> GetListIds()
        {
            return api.Friends.GetLists(null, true);
        }

        //получить пользователя по идентификатору
        public User GetUserById(long userId)
        {
            var result = api.Users.Get(new List<long>() { userId });
            if (result.Count == 0) return null;
            return result[0];
        }

        //публикует запись на стене
        public bool PostOnTheWall(User user)
        {
             var i = api.Wall.Post(new WallPostParams() { 
                Message = $"{user.FirstName + " " + user.LastName} в списке моих лучших друзей!"
            });

            return i > 0;
        }

        //получает идентификатор списка лучших друзей
        public long GetBestFriendListId()
        {
            var listIds = GetListIds();
            long index = -1;
            foreach (var item in listIds)
            {
                if (item.Name == "Лучшие друзья")
                {
                    index = item.Id;
                    break;
                }
            }
            return index;
        }

        //получить список лучших друзей
        public VkCollection<User> GetBestFriends()
        {
            var bestFiendId = GetBestFriendListId();
            var friends = api.Friends.Get(new FriendsGetParams { 
                Fields = ProfileFields.All,
                ListId = bestFiendId 
            });
            

            return friends;
        }

        public string GetToken()
        {
            return api.Token;
        }
    }
}
