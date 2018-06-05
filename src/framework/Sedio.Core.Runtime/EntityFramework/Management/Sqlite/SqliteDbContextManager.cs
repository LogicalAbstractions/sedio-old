using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages.Internal;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Runtime.EntityFramework.Management.Pools;

namespace Sedio.Core.Runtime.EntityFramework.Management.Sqlite
{
    public sealed class SqliteDbContextManager<T> : IDbContextManager<T>
        where T : DbContext
    {
        private const string FileExtension = ".db";
        private const string BranchSubDirectory = "branches";
        private const string RootDbFilename = "root.db";

        private readonly string rootPath;
        private readonly int rootPoolSize;
        private readonly int branchPoolSize;

        private StaticDbContextPool<T> rootPool;
        private ConcurrentDictionary<string, Lazy<StaticDbContextPool<T>>> branchPools;
        private ConcurrentDictionary<string, string> branchMarkers;

        public SqliteDbContextManager(string rootPath, int rootPoolSize, int branchPoolSize)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(rootPath));

            this.rootPath = rootPath;
            this.rootPoolSize = rootPoolSize;
            this.branchPoolSize = branchPoolSize;
        }      
        
        public void Initialize()
        {
            // Create all directories
            EnsureDirectoryHierarchy();
            
            // Enumerate branches and precreate pools
            branchPools = new ConcurrentDictionary<string, Lazy<StaticDbContextPool<T>>>(
                EnumerateBranches()
                .Select(branchId => new KeyValuePair<string, Lazy<StaticDbContextPool<T>>>(branchId,
                        new Lazy<StaticDbContextPool<T>>(() => CreatePool(branchId,branchPoolSize)))),StringComparer.InvariantCultureIgnoreCase
            );
            
            branchMarkers = new ConcurrentDictionary<string, string>(branchPools.Select(kv => new KeyValuePair<string, string>(kv.Key,kv.Key)),
                StringComparer.InvariantCultureIgnoreCase);
            
            // Create the main pool
            rootPool = CreatePool(CalculateRootPath(), rootPoolSize);
            
            // Initialize the root database
            using (var rootContext = rootPool.Aquire(CancellationToken.None).Result)
            {
                rootContext.Context.Database.Migrate();
            }
        }

        public ISet<string> BranchIds => branchPools.Keys.ToHashSet();
        
        public async Task CreateBranch(string sourceId, string targetId,CancellationToken cancellationToken)
        {
            if (targetId == null) throw new ArgumentNullException(nameof(targetId));

            if (sourceId == null || branchPools.ContainsKey(sourceId))
            {
                if (branchMarkers.TryAdd(targetId, targetId))
                {
                    var targetPool = CreatePool(CalculateBranchPath(targetId), branchPoolSize);

                    using (var targetContext = await targetPool.Aquire(cancellationToken).ConfigureAwait(false))
                    {
                        using (var sourceContext = await GetPool(sourceId).Aquire(cancellationToken).ConfigureAwait(false))
                        {
                            var sourceConnection = (SqliteConnection)sourceContext.Context.Database.GetDbConnection();
                            var targetConnection = (SqliteConnection)targetContext.Context.Database.GetDbConnection();

                            try
                            {
                                sourceConnection.Open();
                                targetConnection.Open();

                                await Task.Run(() => sourceConnection.BackupDatabase(targetConnection),
                                        cancellationToken)
                                    .ConfigureAwait(false);

                                if (!branchPools.TryAdd(targetId, new Lazy<StaticDbContextPool<T>>(() => targetPool)))
                                {
                                    throw new InvalidOperationException($"Branch already created");
                                }
                            }
                            finally
                            {
                                sourceConnection.Close();
                                targetConnection.Close();
                            }
                        }
                    }
                }
            }
        }

        public Task DeleteBranch(string id,CancellationToken cancellationToken)
        {
            if (branchPools.TryRemove(id, out var pool))
            {
                if (pool.IsValueCreated)
                {
                    pool.Value.Dispose();
                }

                File.Delete(CalculateBranchPath(id));
                
                branchMarkers.TryRemove(id, out var removedId);
            }

            return Task.CompletedTask;
        }

        public IDbContextPool<T> GetPool(string id)
        {
            if (id == null)
            {
                return rootPool;
            }

            if (branchPools.TryGetValue(id, out var branchPool))
            {
                return branchPool.Value;
            }

            throw new InvalidOperationException($"Cannot find branch {id}");
        }
        
        private StaticDbContextPool<T> CreatePool(string path,int size)
        {
            return new StaticDbContextPool<T>(new SqliteDbContextFactory<T>(path),size);
        }

        private IEnumerable<string> EnumerateBranches()
        {
            return Directory.EnumerateFiles(CalculateBranchDirectoryPath(), "*" + FileExtension,
                new EnumerationOptions()
                {
                    RecurseSubdirectories = false,
                    IgnoreInaccessible = true
                }).Select(Path.GetFileNameWithoutExtension);
        }

        private string CalculateRootPath()
        {
            return Path.Combine(rootPath, RootDbFilename);
        }
        
        private string CalculateBranchPath(string id)
        {
            return Path.Combine(rootPath, BranchSubDirectory, $"{id}{FileExtension}");
        }

        private string CalculateBranchDirectoryPath()
        {
            return Path.Combine(rootPath, BranchSubDirectory);
        }

        private void EnsureDirectoryHierarchy()
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var branchPath = CalculateBranchDirectoryPath();

            if (!Directory.Exists(branchPath))
            {
                Directory.CreateDirectory(branchPath);
            }
        }
    }
}