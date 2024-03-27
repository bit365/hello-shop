using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Microsoft.Extensions.Caching.Distributed;

public static class CustomDistributedCacheExtensions
{
    /// <summary>
    /// Sets a string in the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
    public static void SetObject<TItem>(this IDistributedCache cache, string key, TItem value)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        cache.Set(key, bytes);
    }

    /// <summary>
    /// Sets a string in the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
    public static void SetObject<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        cache.Set(key, bytes, options);
    }

    /// <summary>
    /// Asynchronously sets a string in the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous set operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
    public static Task SetObjectAsync<TItem>(this IDistributedCache cache, string key, TItem value, CancellationToken token = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        return cache.SetAsync(key, bytes, new DistributedCacheEntryOptions(), token);
    }

    /// <summary>
    /// Asynchronously sets a string in the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous set operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/> or <paramref name="value"/> is null.</exception>
    public static Task SetObjectAsync<TItem>(this IDistributedCache cache, string key, TItem value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        return cache.SetAsync(key, bytes, options, token);
    }

    /// <summary>
    /// Gets a string from the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to get the stored data for.</param>
    /// <returns>The T value from the stored cache key.</returns>
    public static TItem? GetObject<TItem>(this IDistributedCache cache, string key)
    {
        byte[]? data = cache.Get(key);

        if (data == null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<TItem>(data);
    }

    /// <summary>
    /// Asynchronously gets a string from the specified cache with the specified key.
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to get the stored data for.</param>
    /// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
    /// <returns>A task that gets the T value from the stored cache key.</returns>
    public static async Task<TItem?> GetObjectAsync<TItem>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        byte[]? data = await cache.GetAsync(key, token).ConfigureAwait(false);

        if (data == null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<TItem>(data);
    }

    /// <summary>
    /// Try to get the value associated with the given key.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">The value associated with the given key.</param>
    /// <returns><c>true</c> if the key was found. <c>false</c> otherwise.</returns>
    public static bool TryGetValue<TItem>(this IDistributedCache cache, string key, out TItem? value)
    {
        var data = cache.Get(key);

        value = default;

        try
        {
            value = JsonSerializer.Deserialize<TItem>(data);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="createOptions">The options to be applied to the <see cref="ICacheEntry"/> if the key does not exist in the cache.</param>
    /// <returns>The value associated with this key.</returns>
    public static TItem? GetOrCreate<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, TItem> factory, DistributedCacheEntryOptions? createOptions = null)
    {
        if (!cache.TryGetValue(key, out object? result))
        {
            createOptions ??= new DistributedCacheEntryOptions();

            result = factory(createOptions);

            cache.SetObject(key, result, createOptions);
        }

        return (TItem?)result;
    }

    /// <summary>
    /// Asynchronously gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="TItem">The type of the object to get.</typeparam>
    /// <param name="cache">The <see cref="IDistributedCache"/> instance this method extends.</param>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory task that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="createOptions">The options to be applied to the <see cref="ICacheEntry"/> if the key does not exist in the cache.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<TItem?> GetOrCreateAsync<TItem>(this IDistributedCache cache, string key, Func<DistributedCacheEntryOptions, Task<TItem>> factory, DistributedCacheEntryOptions? createOptions = null)
    {
        if (!cache.TryGetValue(key, out object? result))
        {
            createOptions ??= new DistributedCacheEntryOptions();

            result = await factory(createOptions).ConfigureAwait(false);

            await cache.SetObjectAsync(key, result, createOptions);
        }

        return (TItem?)result;
    }
}
