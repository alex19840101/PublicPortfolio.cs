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
            int? setGranterId = null)
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
                passwordHash: GenerateStringIfTrueElseReturnNull(generatePasswordHash),
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
            bool generatePasswordHash = true)
        {
            var fixture = new Fixture();

            var id = generateId ? fixture.Create<int>() : 0;
            var projectId = fixture.Create<int>();
            
            return new AuthUser(
                id: id,
                login: GenerateStringIfTrueElseReturnNull(generateLogin),
                userName: GenerateStringIfTrueElseReturnNull(generateName),
                email: GenerateStringIfTrueElseReturnNull(generateEmail),
                passwordHash: GenerateStringIfTrueElseReturnNull(generatePasswordHash),
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
    }
}
