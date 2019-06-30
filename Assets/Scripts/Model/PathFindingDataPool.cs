using System.Collections.Generic;

public class PathFindingDataPool {
    System.Action<object> c = UnityEngine.Debug.Log;
    List<PathfindingData> pool;
    PathfindingData firstAvailable;
    const int POOL_SIZE = 250; // larger than we need
    public PathFindingDataPool () {
        // create a new list, fill it up, and create a linked list using SetNext
        pool = new List<PathfindingData> ();
        pool.Add (new PathfindingData (null, null));

        // set the next element to the next index in the array
        for (int i = 0; i < POOL_SIZE - 1; i++) {
            pool.Add (new PathfindingData (null, null));
            pool[i].SetNext (pool[i + 1]);
        }

        // for the last element, set it to null
        pool.Add (new PathfindingData (null, null));
        pool[POOL_SIZE - 1].SetNext (null);

        firstAvailable = pool[0];
    }

    public PathfindingData RetrieveItem () {
        try {
            // get the next available item in the pool
            var fetched = firstAvailable;
            firstAvailable = fetched.nextAvailableInPool;
            return fetched;
        } catch (System.Exception) {
            //throw an error if there are no available items in the pool
            c ("Ran out of memory, at the end of the pool");
            return null;
            throw;
        }
    }

    public void ReturnItem (PathfindingData item) {
        // clear out data and set it to the front of the list for available data
        item.tile = null;
        item.shadow = null;
        item.SetNext (firstAvailable);
        firstAvailable = item;
    }
}