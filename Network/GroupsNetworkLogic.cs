using Pandora.DI;
using Pandora.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Pandora.Network
{
    class GroupsNetworkLogic : NetworkLogic
    {
        public static List<Group> GetGroups()
        {
            List<Group> groups = DownloadString<List<Group>>("api/v1/client/category/all") ?? new List<Group>();

            return groups;
        }

        public static List<Group> GetSubGroups(string id)
        {
            List<Group> groups = DownloadString<List<Group>>("api/v1/client/category/" + id + "/subcategories") ?? new List<Group>();

            return groups;
        }

        public List<Group> GetMockGroups()
        {
            return new List<Group>
            {
                new Group("1", "Построение"),
                new Group("2", "Корпусная мебель"),
                new Group("3", "Мягкая мебель"),
                new Group("4", "Кухня"),
                new Group("5", "Детская"),
                new Group("6", "Санузел"),
                new Group("7", "Освещение"),
                new Group("8", "Электроника")
            };
        }

        public List<Group> GetMockSubGroups(string id)
        {
            return new List<Group>
            {
                new Group("10", "Столы"),
                new Group("11", "Стулья"),
                new Group("12", "Шкафы"),
                new Group("13", "Стелажи"),
                new Group("14", "Комоды"),
                new Group("15", "Полки"),
                new Group("16", "Консоли"),
                new Group("17", "Мебель для медиа"),
                new Group("18", "Прихожие"),
                new Group("19", "Подставки"),
                new Group("20", "Тумбы"),
                new Group("21", "Модульная"),
                new Group("22", "Комплекты"),
                new Group("23", "Фурнитура")
            };
        }
    }
}
