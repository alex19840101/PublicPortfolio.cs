﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ShopServices.Core.Auth;
using ShopServices.Core.Enums;

namespace TestFixtures
{
    public partial class TestFixtures
    {
        public static Employee GetEmployeeFixtureWithAllFields(
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
                    bool generateRole = true,
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

            return new Employee(
                id: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                name: GenerateStringIfTrueElseReturnNull(generateName),
                surname: GenerateStringIfTrueElseReturnNull(generateSurname),
                address: GenerateStringIfTrueElseReturnNull(generateAddress),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                nick: GenerateStringIfTrueElseReturnNull(generateNick),
                phone: GenerateStringIfTrueElseReturnNull(generatePhone),
                telegramChatId: generateTelegramChatId ? fixture.Create<long?>() : null,
                notificationMethods: new List<NotificationMethod> { NotificationMethod.Email, NotificationMethod.SMS },
                role: GenerateStringIfTrueElseReturnNull(generateRole),
                granterId: granterId,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now,
                shopId: fixture.Create<uint>(),
                warehouseId: fixture.Create<uint>());

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }

        public static Employee GetEmployeeFixtureWithRequiredFields(
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

            return new Employee(
                id: id,
                login: login ?? GenerateStringIfTrueElseReturnNull(generateLogin),
                name: GenerateStringIfTrueElseReturnNull(generateName),
                surname: GenerateStringIfTrueElseReturnNull(generateSurname),
                address: GenerateStringIfTrueElseReturnNull(generateAddress),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                nick: null,
                phone: null,
                telegramChatId: null,
                notificationMethods: new List<NotificationMethod> { NotificationMethod.Email, NotificationMethod.SMS },
                role: null,
                granterId: null,
                createdDt: DateTime.Now,
                lastUpdateDt: null,
                shopId: null,
                warehouseId: null);

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }

        public static LoginData GetLoginDataWithRequiredFields()
        {
            var fixture = new Fixture();

            return new LoginData(
                login: fixture.Build<string>().Create(),
                passwordHash: fixture.Build<string>().Create(),
                timeoutMinutes: null);
        }

        public static LoginData GetLoginDataWithoutLogin()
        {
            var fixture = new Fixture();

            return new LoginData(
                login: null,
                passwordHash: fixture.Build<string>().Create(),
                timeoutMinutes: fixture.Create<int>());
        }

        public static LoginData GetLoginDataWithoutPasswordHash()
        {
            var fixture = new Fixture();

            return new LoginData(
                login: fixture.Build<string>().Create(),
                passwordHash: null,
                timeoutMinutes: fixture.Create<int>());
        }

        public static GrantRoleData GetGrantRoleDataFixture(
            bool generateId = true,
            bool generateLogin = true,
            bool generateGranterLogin = true,
            bool generatePasswordHash = true,
            bool generateRole = true,
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

            return new GrantRoleData(
                id: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                newRole: GenerateStringIfTrueElseReturnNull(generateRole),
                granterId: granterId,
                granterLogin: GenerateStringIfTrueElseReturnNull(generateGranterLogin));

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }

        public static UpdateAccountData GetUpdateAccountDataFixture(
            bool generateId = true,
            bool generateLogin = true,
            bool generateName = true,
            bool generateSurname = true,
            bool generateAddress = true,
            bool generateEmail = true,
            bool generatePasswordHash = true,
            bool generateNewPasswordHash = true,
            string passwordHash = null,
            string newPasswordHash = null,
            string login = null)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<uint>() : 0;

            return new UpdateAccountData(
                id: id,
                login: login ?? GenerateStringIfTrueElseReturnNull(generateLogin),
                name: GenerateStringIfTrueElseReturnNull(generateName),
                surname: GenerateStringIfTrueElseReturnNull(generateSurname),
                address: GenerateStringIfTrueElseReturnNull(generateAddress),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                newPasswordHash: newPasswordHash ?? GenerateStringIfTrueElseReturnNull(generateNewPasswordHash),
                nick: null,
                phone: null,
                telegramChatId: null,
                requestedRole: null,
                shopId: fixture.Create<uint>(),
                warehouseId: fixture.Create<uint>());

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }


        public static DeleteAccountData GetDeleteAccountDataFixture(
            bool generateId = true,
            bool generateLogin = true,
            bool generateGranterLogin = false,
            bool generatePasswordHash = true,
            List<uint> excludeIds = null,
            uint setId = 0,
            bool generateGranterId = false,
            string passwordHash = null)
        {
            var fixture = new Fixture();
            var id = generateId ? fixture.Create<uint>() : setId;
            var granterId = generateGranterId == true ? fixture.Create<uint?>() : null;
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
            return new DeleteAccountData(
                id: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                granterId: granterId,
                granterLogin: GenerateStringIfTrueElseReturnNull(generateGranterLogin));

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }
    }
}
