﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;

namespace Chem4Word
{
    public class AssemblyReflectionProxy : MarshalByRefObject
    {
        private string _assemblyPath;

        public void LoadAssembly(String assemblyPath)
        {
            try
            {
                _assemblyPath = assemblyPath;
                Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }
            catch (FileNotFoundException)
            {
                // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain.
            }
        }

        public TResult Reflect<TResult>(Func<Assembly, TResult> func)
        {
            DirectoryInfo directory = new FileInfo(_assemblyPath).Directory;

            // Extract filename as files will be loaded from a random dl3 cache location.
            FileInfo fileInfo = new FileInfo(_assemblyPath);
            string fileName = fileInfo.Name;

            ResolveEventHandler resolveEventHandler =
                (s, e) =>
                {
                    return OnReflectionOnlyResolve(
                     e, directory);
                };

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;

            var assembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(a => a.Location.EndsWith(fileName));

            var result = func(assembly);

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;

            return result;
        }

        private Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
        {
            Assembly loadedAssembly =
                AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                    .FirstOrDefault(
                      asm => string.Equals(asm.FullName, args.Name,
                          StringComparison.OrdinalIgnoreCase));

            if (loadedAssembly != null)
            {
                return loadedAssembly;
            }

            AssemblyName assemblyName =
                new AssemblyName(args.Name);
            string dependentAssemblyFilename =
                Path.Combine(directory.FullName,
                assemblyName.Name + ".dll");

            if (File.Exists(dependentAssemblyFilename))
            {
                return Assembly.ReflectionOnlyLoadFrom(
                    dependentAssemblyFilename);
            }
            return Assembly.ReflectionOnlyLoad(args.Name);
        }
    }

    public class AssemblyReflectionManager : IDisposable
    {
        private Dictionary<string, AppDomain> _mapDomains = new Dictionary<string, AppDomain>();
        private Dictionary<string, AppDomain> _loadedAssemblies = new Dictionary<string, AppDomain>();
        private Dictionary<string, AssemblyReflectionProxy> _proxies = new Dictionary<string, AssemblyReflectionProxy>();

        public bool LoadAssembly(string assemblyPath, string domainName)
        {
            // if the assembly file does not exist then fail
            if (!File.Exists(assemblyPath))
            {
                return false;
            }

            // if the assembly was already loaded then fail
            if (_loadedAssemblies.ContainsKey(assemblyPath))
            {
                return false;
            }

            // check if the appdomain exists, and if not create a new one
            AppDomain appDomain = null;
            if (_mapDomains.ContainsKey(domainName))
            {
                appDomain = _mapDomains[domainName];
            }
            else
            {
                appDomain = CreateChildDomain(AppDomain.CurrentDomain, domainName);
                _mapDomains[domainName] = appDomain;
            }

            // load the assembly in the specified app domain
            try
            {
                Type proxyType = typeof(AssemblyReflectionProxy);
                if (proxyType.Assembly != null)
                {
                    var proxy =
                        (AssemblyReflectionProxy)appDomain.
                            CreateInstanceFrom(
                            proxyType.Assembly.Location,
                            proxyType.FullName).Unwrap();

                    proxy.LoadAssembly(assemblyPath);

                    _loadedAssemblies[assemblyPath] = appDomain;
                    _proxies[assemblyPath] = proxy;

                    return true;
                }
            }
            catch
            {
                // Do Nothing
            }

            return false;
        }

        public bool UnloadAssembly(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                return false;
            }

            // check if the assembly is found in the internal dictionaries
            if (_loadedAssemblies.ContainsKey(assemblyPath) &&
               _proxies.ContainsKey(assemblyPath))
            {
                // check if there are more assemblies loaded in the same app domain; in this case fail
                AppDomain appDomain = _loadedAssemblies[assemblyPath];
                int count = _loadedAssemblies.Values.Count(a => a == appDomain);
                if (count != 1)
                    return false;

                try
                {
                    // remove the appdomain from the dictionary and unload it from the process
                    _mapDomains.Remove(appDomain.FriendlyName);
                    AppDomain.Unload(appDomain);

                    // remove the assembly from the dictionaries
                    _loadedAssemblies.Remove(assemblyPath);
                    _proxies.Remove(assemblyPath);

                    return true;
                }
                catch
                {
                    // Do Nothing
                }
            }

            return false;
        }

        public bool UnloadDomain(string domainName)
        {
            // check the appdomain name is valid
            if (string.IsNullOrEmpty(domainName))
            {
                return false;
            }

            // check we have an instance of the domain
            if (_mapDomains.ContainsKey(domainName))
            {
                try
                {
                    var appDomain = _mapDomains[domainName];

                    // check the assemblies that are loaded in this app domain
                    var assemblies = new List<string>();
                    foreach (var kvp in _loadedAssemblies)
                    {
                        if (kvp.Value == appDomain)
                            assemblies.Add(kvp.Key);
                    }

                    // remove these assemblies from the internal dictionaries
                    foreach (var assemblyName in assemblies)
                    {
                        _loadedAssemblies.Remove(assemblyName);
                        _proxies.Remove(assemblyName);
                    }

                    // remove the appdomain from the dictionary
                    _mapDomains.Remove(domainName);

                    // unload the appdomain
                    AppDomain.Unload(appDomain);

                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public TResult Reflect<TResult>(string assemblyPath, Func<Assembly, TResult> func)
        {
            // check if the assembly is found in the internal dictionaries
            if (_loadedAssemblies.ContainsKey(assemblyPath) &&
               _proxies.ContainsKey(assemblyPath))
            {
                return _proxies[assemblyPath].Reflect(func);
            }

            return default(TResult);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AssemblyReflectionManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var appDomain in _mapDomains.Values)
                {
                    AppDomain.Unload(appDomain);
                }

                _loadedAssemblies.Clear();
                _proxies.Clear();
                _mapDomains.Clear();
            }
        }

        private AppDomain CreateChildDomain(AppDomain parentDomain, string domainName)
        {
            Evidence evidence = new Evidence(parentDomain.Evidence);
            AppDomainSetup setup = parentDomain.SetupInformation;
            return AppDomain.CreateDomain(domainName, evidence, setup);
        }
    }
}