using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ShopServices.Core.Auth;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;

namespace TestFixtures
{
    public partial class TestFixtures
    {
        public static Buyer GetBuyerFixtureWithAllFields(
                    bool generateId = false,
                    bool generateLogin = true,
                    bool generateName = true,
                    bool generateSurname = true,
                    bool generateAddress = true,
                    bool generateEmail = true,
                    bool generatePasswordHash = true,
                    bool generateNick = true,
                    bool generatePhone = true,
                    bool generateTelegramChatId = true,
                    bool generateDiscountGroups = true,
                    List<uint> excludeIds = null,
                    uint setId = 0,
                    uint? setGranterId = null,
                    string passwordHash = null)
        {
            var fixture = new Fixture();
            var id = generateId ? fixture.Create<uint>() : setId;
            var granterId = setGranterId != null ? fixture.Create<uint>() : setGranterId;
            if (generateId)
            {
                if (excludeIds != null && excludeIds.Any() && setId == 0)
                {
                    while (excludeIds.Contains(id))
                        id = fixture.Create<uint>();
                }
                excludeIds ??= new List<uint>();
                excludeIds.Add(id);
            }
            Random random = new Random();
            var discountGroupsCount = random.Next(1, 10);

            return new Buyer(
                id: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                name: GenerateStringIfTrueElseReturnNull(generateName),
                surname: GenerateStringIfTrueElseReturnNull(generateSurname),
                address: GenerateStringIfTrueElseReturnNull(generateAddress),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                nick: GenerateStringIfTrueElseReturnNull(generateNick),
                phones: GenerateStringIfTrueElseReturnNull(generatePhone),
                telegramChatId: generateTelegramChatId ? fixture.Create<long?>() : null,
                notificationMethods: new List<NotificationMethod> { NotificationMethod.Email, NotificationMethod.SMS },
                discountGroups: generateDiscountGroups ? GenerateIdsList(fixture, (uint)discountGroupsCount) : null,
                granterId: granterId,
                created: DateTime.Now,
                updated: DateTime.Now);

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }

        public static List<uint> GenerateIdsList(Fixture fixture, uint count)
        {
            List<uint> idsList = new List<uint>();
            for (int i = 0; i < count; i++)
                idsList.Add(fixture.Create<uint>());

            return idsList;
        }

        public static Buyer GetBuyerFixtureWithRequiredFields(
            bool generateId = false,
            bool generateLogin = true,
            bool generateName = true,
            bool generateSurname = true,
            bool generateAddress = true,
            bool generateEmail = true,
            bool generatePasswordHash = true,
            string passwordHash = null,
            string login = null)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<uint>() : 0;

            return new Buyer(
                id: id,
                login: login ?? GenerateStringIfTrueElseReturnNull(generateLogin),
                name: GenerateStringIfTrueElseReturnNull(generateName),
                surname: GenerateStringIfTrueElseReturnNull(generateSurname),
                address: GenerateStringIfTrueElseReturnNull(generateAddress),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                nick: null,
                phones: null,
                telegramChatId: null,
                notificationMethods: new List<NotificationMethod> { NotificationMethod.Email, NotificationMethod.SMS },
                discountGroups: null,
                granterId: null,
                created: DateTime.Now,
                updated: null);

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }

        public static ChangeDiscountGroupsData GetChangeDiscountGroupsDataFixture(
            bool generateId = true,
            bool generateLogin = true,
            bool generateGranterLogin = true,
            bool generatePasswordHash = true,
            bool generateDiscountGroups = true,
            List<uint> excludeIds = null,
            uint setId = 0,
            bool generateGranterId = true,
            string passwordHash = null)
        {
            var fixture = new Fixture();
            var id = generateId ? fixture.Create<uint>() : setId;
            var granterId = generateGranterId == true ? fixture.Create<uint>() : 0;
            if (generateId)
            {
                if (excludeIds != null && excludeIds.Any() && setId == 0)
                {
                    while (excludeIds.Contains(id))
                        id = fixture.Create<uint>();
                }
                excludeIds ??= new List<uint>();
                excludeIds.Add(id);
            }

            Random random = new Random();
            var discountGroupsCount = random.Next(1, 10);

            return new ChangeDiscountGroupsData(
                buyerId: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                discountGroups: generateDiscountGroups ? GenerateIdsList(fixture, (uint)discountGroupsCount) : null,
                granterId: granterId,
                granterLogin: GenerateStringIfTrueElseReturnNull(generateGranterLogin));

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }
    }
}
