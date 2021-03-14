using System;

namespace TRRP_Lab1
{
    class Program
    {

		static public void PrintFriends(FriendsCRUD api)
        {
			var friends = api.GetFriends();
			Console.WriteLine("У Вас " + friends.Count + " друзей\nСписок друзей: ");
			for (int i = 1; i < friends.Count + 1; i++)
			{
				var friend = friends[i - 1];
				Console.WriteLine(i + ". " + friend.FirstName + " " + friend.LastName);
			}
			Console.WriteLine();
		}

		static void Main(string[] args)
		{
			var decr = SecureManager.Decrypt();
			FriendsCRUD api = null;
			if (decr == null)
			{
				var logined = false;
				while (!logined)
				{
					try
					{
						string login = "";
						string pass = "";
						Console.WriteLine("Введите логин:");
						login = Console.ReadLine();
						Console.WriteLine("Введите пароль:");
						pass = Console.ReadLine();
						api = new FriendsCRUD(login, pass);
						SecureManager.Encrypt(api.GetToken());
						logined = true;
						Console.Clear();
					}
					catch
					{
						Console.WriteLine("Неверное сочетание логина и пароля, попробуйте еще раз");
					}
				}
			}
			else
				api = new FriendsCRUD(decr);
			Console.WriteLine("Вход выполнен успешно");

			var friends = api.GetFriends();
			long bestFriendsId = api.GetBestFriendListId();
		
			var b = true;
			while (b)
			{
				Console.WriteLine("Выберите действие: ");
				Console.WriteLine("1. Получить список друзей");
				Console.WriteLine("2. Добавить в друзья");
				Console.WriteLine("3. Удалить из друзей");
				Console.WriteLine("4. Добавить в 'лучшие друзья'");
				Console.WriteLine("5. Удалить из 'лучших друзей'");
				Console.WriteLine("6. Опубликовать запись о 'лучших друзьях' на стене");
				Console.WriteLine("7. Выход\n");
				int select = Convert.ToInt32(Console.ReadLine());
				
				switch (select)
                {
					case 1:
						PrintFriends(api);
						break;

					case 2:
						Console.WriteLine("Введите id пользователя");
						long id = Convert.ToInt64(Console.ReadLine());
						var user = api.GetUserById(id);
						Console.WriteLine($"Это {user.FirstName} {user.LastName}? (y/n)");
						var result = Console.ReadLine();
						if (result == "y")
						{
							api.AddFriend(id);
							Console.WriteLine("Друг добавлен");
						}
						else
						{
							if (result == "n")
								Console.WriteLine("Вы отказались добавлять друга");
							else 
								Console.WriteLine("Вы ввели некорректное значение");
						}
						Console.WriteLine();
						break;

					case 3:
						PrintFriends(api);
						Console.WriteLine("Введите номер друга в списке");
						var number = Convert.ToInt32(Console.ReadLine());
						if (number > friends.Count || number < 1)
						{
							Console.WriteLine("Вы ввели недопустимое значение");
							break;
						}
						var friend3 = friends[number - 1];
						var res3 = api.DeleteFriend(friend3.Id);
						if (res3)
							Console.WriteLine("Пользователь успешно удален из друзей");
						else
							Console.WriteLine("Пользователь уже не ваш друг");

						break;

					case 4:
						PrintFriends(api);
						Console.WriteLine("Введите номер друга в списке");
						var number4 = Convert.ToInt32(Console.ReadLine());
						if (number4 > friends.Count || number4 < 1)
                        {
							Console.WriteLine("Вы ввели недопустимое значение");
							break;
                        }
						var friend4 = friends[number4 - 1];
						var res4 = api.AddUserIds(friend4.Id, new long[] { bestFriendsId });
						if (res4)
							Console.WriteLine("Пользователь успешно добавлен в 'лучшие друзья'");
						else
							Console.WriteLine("Пользователь уже в 'лучших друзьях'");

						break;

					case 5:
						Console.WriteLine("Список 'лучших друзей':");
						var bestFriends5 = api.GetBestFriends();
						for (int i = 1; i < bestFriends5.Count + 1; i++)
							Console.WriteLine($"{i}. {bestFriends5[i - 1].FirstName} {bestFriends5[i - 1].LastName}");
						Console.WriteLine("Введите номер друга в списке");
						var number5 = Convert.ToInt32(Console.ReadLine());
						if (number5 > bestFriends5.Count || number5 < 1)
						{
							Console.WriteLine("Вы ввели недопустимое значение");
							break;
						}
						var friend5 = bestFriends5[number5 - 1];
						var res5 = api.RemoveUserIds(friend5.Id);
						if (res5)
							Console.WriteLine("Друг успешно убран из 'лучших друзей'");
						else
							Console.WriteLine("Друг уже не в 'лучших друзьях'");

						break;

					case 6:
						Console.WriteLine("Список 'лучших друзей':");
						var bestFriends6 = api.GetBestFriends();
						for (int i = 1; i < bestFriends6.Count + 1; i++)
							Console.WriteLine($"{i}. {bestFriends6[i - 1].FirstName} {bestFriends6[i - 1].LastName}");
						Console.WriteLine("Введите номер друга в списке");
						var number6 = Convert.ToInt32(Console.ReadLine());
						if (number6 > bestFriends6.Count || number6 < 1)
						{
							Console.WriteLine("Вы ввели недопустимое значение");
							break;
						}
						var friend6 = bestFriends6[number6 - 1];
						var res6 = api.PostOnTheWall(friend6);
						if (res6)
							Console.WriteLine("Запись создана");
						else
							Console.WriteLine("Произошла ошибка");

						break;

					case 7:
						b = false;
						break;

					default:
						Console.WriteLine("Вашего значения нет в списке");
						break;
                }
			}			
		}
	}
}
