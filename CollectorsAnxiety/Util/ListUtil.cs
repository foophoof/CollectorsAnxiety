using System.Collections.Generic;

namespace CollectorsAnxiety.Util;

public static class ListUtil {

    // https://stackoverflow.com/a/20036379
    public static IEnumerable<IList<T>> ChunksOf<T>(this IEnumerable<T> sequence, int size) {
        var chunk = new List<T>(size);

        foreach (var element in sequence) {
            chunk.Add(element);
            if (chunk.Count != size) continue;
            yield return chunk;
            chunk = new List<T>(size);
        }

        if (chunk.Count > 0) yield return chunk;
    }

}
