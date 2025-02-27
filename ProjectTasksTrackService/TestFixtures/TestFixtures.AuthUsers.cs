using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ProjectTasksTrackService.Core.Auth;

namespace TestFixtures
{
    public partial class TestFixtures
    {
        public static AuthUser GetAuthUserFixtureWithAllFields(
            bool generateId = false,
            bool generateLogin = true,
            bool generateName = true,
            bool generateEmail = true,
            bool generatePasswordHash = true,
            bool generateNick = true,
            bool generatePhone = true,
            bool generateRole = true,
            List<int> excludeIds = null,
            int setId = 0,
            int? setGranterId = null,
            string passwordHash = null)
        {
            var fixture = new Fixture();
            var id = generateId ? fixture.Create<int>() : setId;
            var granterId = setGranterId != null ? fixture.Create<int>() : setGranterId;
            if (generateId)
            {
                if (excludeIds != null && excludeIds.Any() && setId == 0)
                {
                    while (excludeIds.Contains(id))
                        id = fixture.Create<int>();
                }
                excludeIds ??= new List<int>();
                excludeIds.Add(id);
            }

            return new AuthUser(
                id: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                userName: GenerateStringIfTrueElseReturnNull(generateName),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                nick: GenerateStringIfTrueElseReturnNull(generateNick),
                phone: GenerateStringIfTrueElseReturnNull(generatePhone),
                role: GenerateStringIfTrueElseReturnNull(generateRole),
                granterId: granterId,
                createdDt: DateTime.Now,
                lastUpdateDt: DateTime.Now);

            //local
            string GenerateStringIfTrueElseReturnNull(bool flag) =>
                flag == true ? fixture.Build<string>().Create() : null;
        }

        public static AuthUser GetAuthUserFixtureWithRequiredFields(
            bool generateId = false,
            bool generateLogin = true,
            bool generateName = true,
            bool generateEmail = true,
            bool generatePasswordHash = true,
            string passwordHash = null)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : 0;
            
            return new AuthUser(
                id: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                userName: GenerateStringIfTrueElseReturnNull(generateName),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: passwordHash ?? GenerateStringIfTrueElseReturnNull(generatePasswordHash),
                nick: null,
                phone: null,
                role: null,
                granterId: null,
                createdDt: DateTime.Now,
                lastUpdateDt: null);

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
    }
}
