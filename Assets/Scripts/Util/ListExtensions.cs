using System;
using System.Collections.Generic;

public static class ListExtensions {
    
    /// <summary>
    /// Gets a random entry from the list, be careful to call as no items in the list will cause issues
    /// </summary>
    /// <param name="list">List to get random entry from</param>
    /// <typeparam name="T">The lists type</typeparam>
    /// <returns>A random item from the list</returns>
    public static T Random<T>(this List<T> list) {
        Random r = new Random();

        return list[r.Next(0, list.Count)];
    }
}
