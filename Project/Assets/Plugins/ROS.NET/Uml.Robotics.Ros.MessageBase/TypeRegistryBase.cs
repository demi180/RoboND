﻿using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Uml.Robotics.Ros
{
    public class TypeRegistryBase
    {
        public Dictionary<string, Type> TypeRegistry { get; } = new Dictionary<string, Type>();
        public List<string> PackageNames { get; } = new List<string>();
        protected ILogger Logger { get; set; }

        protected TypeRegistryBase(ILogger logger)
        {
            this.Logger = logger;
        }

        public IEnumerable<string> GetTypeNames()
        {
            return TypeRegistry.Keys;
        }

        protected T Create<T>(string rosType) where T : class, new()
        {
            T result = null;
			Type type;
            bool typeExist = TypeRegistry.TryGetValue(rosType, out type);
            if (typeExist)
            {
                result = Activator.CreateInstance(type) as T;
            }

            return result;
        }

        public static IEnumerable<Assembly> GetCandidateAssemblies(params string[] tagAssemblies)
        {
            if (tagAssemblies == null)
                throw new ArgumentNullException(nameof(tagAssemblies));
            if (tagAssemblies.Length == 0)
                throw new ArgumentException("At least one tag assembly name must be specified.", nameof(tagAssemblies));

            var context = DependencyContext.Load(Assembly.GetEntryAssembly());
            var loadContext = AssemblyLoadContext.Default;

            var referenceAssemblies = new HashSet<string>(tagAssemblies, StringComparer.OrdinalIgnoreCase);
            return context.RuntimeLibraries
                .Where(x => x.Dependencies.Any(d => referenceAssemblies.Contains(d.Name)))
                .SelectMany(x => x.GetDefaultAssemblyNames(context))
                .Select(x => loadContext.LoadFromAssemblyName(x));
        }
    }
}
