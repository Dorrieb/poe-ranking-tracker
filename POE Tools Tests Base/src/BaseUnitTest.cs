using Castle.MicroKernel;
using Castle.Windsor;
using Newtonsoft.Json;
using PoeApiClient.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POEToolsTestsBase
{
    public abstract class BaseUnitTest
    {
        protected static JsonSerializerSettings GetJsonSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            List<JsonConverter> converters = new List<JsonConverter>
            {
                new LeagueConverter(),
                new LeagueRuleConverter(),
                new LadderConverter(),
                new EntryConverter(),
                new CharacterConverter(),
                new AccountConverter(),
                new ChallengesConverter(),
            };

            foreach (var converter in converters)
            {
                settings.Converters.Add(converter);
            }
            return settings;
        }

        protected static IHandler[] GetAllHandlers(IWindsorContainer container)
        {
            return GetHandlersFor(typeof(object), container);
        }

        protected static IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
        {
            return container.Kernel.GetAssignableHandlers(type);
        }

        protected static Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        protected static Type[] GetPublicClassesFromApplicationAssembly<T>(Predicate<Type> where)
        {
            return typeof(T).Assembly.GetExportedTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsAbstract == false)
                .Where(where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        protected static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
