using System.Collections.Generic;

public class ShadowTilePool {
    System.Action<object> c = UnityEngine.Debug.Log;
    List<ShadowTile> pool;
    ShadowTile firstAvailable;
    const int POOL_SIZE = 250; // larger than we need

    // not used at the moment
    public ShadowTilePool () {
        // create a new list, fill it up, and create a linked list using SetNext
        pool = new List<ShadowTile> ();
        pool.Add (new ShadowTile (int.MaxValue, new Point (-99, -99), null, null));

        // set the next element to the next index in the array
        for (int i = 0; i < POOL_SIZE - 1; i++) {
            pool.Add (new ShadowTile (int.MaxValue, new Point (-99, -99), null, null));
            pool[i].SetNext (pool[i + 1]);
        }

        // for the last element, set it to null
        pool.Add (new ShadowTile (int.MaxValue, new Point (-99, -99), null, null));
        pool[POOL_SIZE - 1].SetNext (null);

        firstAvailable = pool[0];
    }

    public ShadowTile RetrieveItem () {
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

    public void ReturnItem (ShadowTile item) {
        // clear out data and set it to the front of the list for available data
        item.ClearLocalData ();
        item.SetNext (firstAvailable);
        firstAvailable = item;
    }
}